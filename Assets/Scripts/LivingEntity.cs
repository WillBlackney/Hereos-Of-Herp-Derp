using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivingEntity : MonoBehaviour
{   
    [SerializeField] public float speed;    
    public enum Class { None, Warrior, Mage, Ranger, Priest, Rogue, Shaman, Warlock };
    public Class myClass;

    [Header("Component References")]
    public Slider myHealthBar;
    public GameObject myWorldSpaceCanvasParent;
    public GameObject myBlockIcon;
    public TextMeshProUGUI myBlockText;
    public TextMeshProUGUI myCurrentHealthText;
    public TextMeshProUGUI myCurrentMaxHealthText;
    public Animator myAnimator;
    public SpriteRenderer mySpriteRenderer;
    public StatusManager myStatusManager;
    public SpellBook mySpellBook;
    public PassiveManager myPassiveManager;
    public ActivationWindow myActivationWindow;
    public Defender defender;
    public Enemy enemy;


    [Header("Base Trait Properties")]
    public int baseMobility;
    public int baseMaxHealth;
    public int baseStartingHealth;
    public int baseEnergy;
    public int baseStartingAP;
    public int baseMaxAP;
    public int baseMeleeRange;
    public int baseStrength;
    public int baseDexterity;
    public int baseInitiative;
    public int baseStartingBlock;

    [Header("Current Trait Properties")]
    public int currentMobility;
    public int currentMaxHealth;
    public int currentHealth;
    public int currentMaxAP;
    public int currentEnergy;
    public int currentAP;
    public int currentMeleeRange;
    public int currentStrength;
    public int currentDexterity;
    public int currentInitiative;
    public int currentBlock;

    // TO DO: below properties should be moved into a new script in the futre (MyStatusManager);
    [Header("Buff/Debuff Related Properties ")]
    public bool isKnockedDown;
    public bool isPinned;
    public bool isStunned;
    public bool hasBarrier;
    public int currentBarrierStacks;
    public bool isSleeping;
    public int currentSleepingStacks;
    public bool isCamoflaged;
    public bool isPoisoned;
    public int poisonStacks;

    [Header("Miscealaneous Properties ")]
    public int currentInitiativeRoll;
    public int moveActionsTakenThisTurn;
    public int timesAttackedThisTurn;


    [Header("Pathing + Location Related ")]

    public TileScript TileCurrentlyOn;

    public Point GridPosition;

    public LivingEntity myCurrentTarget;

    public Stack<Node> path;

    public Vector3 destination;

    public bool facingRight;
    // bool used for correctly flipping sprites and changing the direction of sprites that are not imported facing left.
    public bool spriteImportedFacingRight;

    public virtual void InitializeSetup(Point startingGridPosition, TileScript startingTile)
    {
        Debug.Log("Calling LivingEntity.InitializeSetup...");
        // Get component references        
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        defender = GetComponent<Defender>();
        enemy = GetComponent<Enemy>();
        myPassiveManager = GetComponent<PassiveManager>();
        myPassiveManager.InitializeSetup();
        mySpellBook = GetComponent<SpellBook>();
        mySpellBook.InitializeSetup();
        // Set grid position 'Point'
        GridPosition = startingGridPosition;
        // Set our current tile
        TileCurrentlyOn = startingTile;
        // Set its tile to 'occupied' state
        LevelManager.Instance.SetTileAsOccupied(startingTile);
        // Place tower in the centre point of the tile
        transform.position = startingTile.WorldPosition;
        // Add this to the list of all active enemy and defender characters
        LivingEntityManager.Instance.allLivingEntities.Add(this);
        // Create Activation Window
        ActivationManager.Instance.CreateActivationWindow(this);
        // Face towards the opponents
        if (defender)
        {
            PositionLogic.Instance.SetDirection(this, "Right");
        }
        else if (enemy)
        {
            PositionLogic.Instance.SetDirection(this, "Left");
        }
        // Set up all base properties and values (damage, mobility etc)
        SetBaseProperties();
    }
    
    public virtual void SetBaseProperties()
    {
        currentMobility = baseMobility;
        currentMaxHealth = baseMaxHealth;
        currentHealth = baseStartingHealth;
        currentMaxAP = baseMaxAP;
        currentEnergy = baseEnergy;              
        currentMeleeRange = baseMeleeRange;         
        ModifyCurrentStrength(baseStrength);
        ModifyCurrentDexterity(baseDexterity);
        ModifyCurrentInitiative(baseInitiative);
        myHealthBar.value = CalculateHealthBarPosition();
        ModifyCurrentBlock(baseStartingBlock);
        ModifyCurrentAP(baseStartingAP);
        UpdateCurrentHealthText();
        UpdateCurrentMaxHealthText(currentMaxHealth);
    }

    // Apply + Remove debuffs, buffs, effects
    public virtual void ApplyKnockDown()
    {
        if (!CombatLogic.Instance.IsProtectedByRune(this))
        {
            isKnockedDown = true;
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Knock Down"), 1);
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Knocked Down!", false));
        }
        
    }
    
    public virtual void RemoveKnockDown()
    {
        if (isKnockedDown)
        {
            isKnockedDown = false;
            myStatusManager.RemoveStatusIcon(myStatusManager.GetStatusIconByName("Knock Down"));
        }        
    }

    public virtual void ApplyPinned()
    {
        if (!CombatLogic.Instance.IsProtectedByRune(this))
        {
            isPinned = true;
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Pinned"), 1);
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Pinned!", false));
        }
            
    }

    public virtual void RemovePinned()
    {
        if (isPinned)
        {
            Debug.Log("RemovePinned() called");
            isPinned = false;
            myStatusManager.RemoveStatusIcon(myStatusManager.GetStatusIconByName("Pinned"));
        }
        
    }

    public virtual void ApplyStunned()
    {
        if (!CombatLogic.Instance.IsProtectedByRune(this))
        {
            isStunned = true;
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Stunned"), 1);
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Stunned!", false));
        }
            
    }

    public virtual void RemoveStunned()
    {
        if (isStunned)
        {
            isStunned = false;
            myStatusManager.RemoveStatusIcon(myStatusManager.GetStatusIconByName("Stunned"));
        }        
    }

    public void ApplyCamoflage()
    {
        isCamoflaged = true;
        myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Camoflage"), 1);
        StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Camoflage!", false));

    }

    public void RemoveCamoflage()
    {
        if (isCamoflaged)
        {
            isCamoflaged = false;
            myStatusManager.RemoveStatusIcon(myStatusManager.GetStatusIconByName("Camoflage"));
        }
    }

    public void ModifyPoison(int stacks)
    {
        if (myPassiveManager.PoisonImmunity)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Poison Immunity", true));
            return;
        }

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(this))
            {
                poisonStacks += stacks;
                if (poisonStacks > 0)
                {
                    isPoisoned = true;
                    StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Poison + " + stacks.ToString(), false));
                    myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Poison"), stacks);
                }
            }
        }

        // TO DO: implement a way to reduce poison stacks when we have spells that do that
        // Maybe a new method called (ClearPoison()) 


    }

    public virtual void ModifySleeping(int stacks)
    {
        // Increae Sleep
        if (!CombatLogic.Instance.IsProtectedByRune(this) && stacks > 0)
        {
            currentSleepingStacks += stacks;
            if (currentSleepingStacks > 0)
            {
                isSleeping = true;
            }
        }

        // Reduce sleep
        else
        {
            currentSleepingStacks += stacks;
            if (currentSleepingStacks <= 0)
            {
                currentSleepingStacks = 0;
                isSleeping = false;
            }
        }     
        
        myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Sleeping"), stacks);
    }

    public float CalculateHealthBarPosition()
    {
        float currentHealthFloat = currentHealth;
        float currentMaxHealthFloat = currentMaxHealth;

        return currentHealthFloat / currentMaxHealthFloat;
    }

    // State related booleans and conditional checks.
    public bool IsAbleToMove()
    {
        Enemy enemy = GetComponent<Enemy>();
        Defender defender = GetComponent<Defender>();

        if (defender)
        {
            if (isPinned == false && defender.currentMobility > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if (enemy)
        {
            if (isPinned == false && enemy.currentMobility > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else
        {
            Debug.Log("AbleToMove() unable to find 'Defender' or 'Enemy' component");
            return false;
        }
    }

    public bool HasEnoughAP(int currentAP, int APCostOfAction)
    {
        if (currentAP >= APCostOfAction)
        {
            return true;
        }

        else if (myPassiveManager.Preparation)
        {
            return true;
        }

        else
        {
            Debug.Log("Action failed: Not enough AP");
            return false;
        }
    }

    public bool IsAbleToTakeActions()
    {
        if (isStunned)
        {
            Debug.Log("Action failed. Unable to take actions while stunned");
            return false;
        }
        if (isSleeping)
        {
            Debug.Log("Action failed. Unable to take actions while sleeping");
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsTargetInRange(LivingEntity target, int range)
    {
        List<TileScript> tilesWithinMyRange = LevelManager.Instance.GetTilesWithinRange(range, TileCurrentlyOn, false);
        

        if(target == null)
        {
            Debug.Log("IsTargetInRange() target value is null...");
        }        

        TileScript targetsTile = target.TileCurrentlyOn;

        if (tilesWithinMyRange.Contains(targetsTile) && IsTargetValid(target))
        {
            Debug.Log("Target enemy is range");
            return true;
        }
        else
        {
            Debug.Log("Target enemy is NOT range");
            return false;
        }
    }

    public bool IsTargetValid(LivingEntity target)
    {
        List<TileScript> tilesWithinStealthSight = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);

        if (tilesWithinStealthSight.Contains(target.TileCurrentlyOn) == false && 
            (target.isCamoflaged || target.myPassiveManager.Stealth)
            )
        {
            Debug.Log("Invalid target: Target is in stealth/camoflague and more than 1 tile away...");
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsAbilityOffCooldown(int currentCooldownTimer)
    {
        if (currentCooldownTimer == 0)
        {
            return true;
        }
        else
        {
            Debug.Log("Cannot use ability: Ability is on cooldown");
            return false;
        }
    }

    // Stat and property modifiers
    public virtual void ModifyCurrentMobility(int mobilityGainedOrLost)
    {
        currentMobility += mobilityGainedOrLost;
        if (currentMobility < 0)
        {
            currentMobility = 0;
        }        
    }

    public virtual void ModifyCurrentAP(int APGainedOrLost, bool showVFX = true)
    {
        currentAP += APGainedOrLost;

        if (currentAP > currentMaxAP)
        {
            currentAP = currentMaxAP;
        }

        if (currentAP < 0)
        {
            currentAP = 0;
        }

        if(APGainedOrLost > 0 && showVFX == true)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "AP +" + APGainedOrLost, false));
        }
    }

    public virtual void ModifyCurrentHealth(int healthGainedOrLost)
    {
        int originalHealth = currentHealth;
        currentHealth += healthGainedOrLost;
        if(currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }

        if(currentHealth > originalHealth)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateHealingEffect(transform.position, healthGainedOrLost, false));
        }
        myHealthBar.value = CalculateHealthBarPosition();
        UpdateCurrentHealthText();

    }

    public virtual void ModifyCurrentStrength(int strengthGainedOrLost)
    {
        int previousStrength = currentStrength;
        currentStrength += strengthGainedOrLost;

        if (currentStrength != 0 ||
            (currentStrength == 0 && (previousStrength > 0 || previousStrength < 0))
            )
        {
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Strength"), strengthGainedOrLost);
        }
        
       
        if (strengthGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strength +" + strengthGainedOrLost, false));
        }

        if (strengthGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strength " + strengthGainedOrLost, false));
        }

    }

    public virtual void ModifyCurrentDexterity(int dexterityGainedOrLost)
    {
        currentDexterity += dexterityGainedOrLost;
        if (currentDexterity != 0)
        {
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Dexterity"), dexterityGainedOrLost);
        }

        if (dexterityGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dexterity +" + dexterityGainedOrLost, false));
        }

        if (dexterityGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dexterity " + dexterityGainedOrLost, false));
        }

    }
    public virtual void ModifyCurrentInitiative(int initiativeGainedOrLost)
    {
        currentInitiative += initiativeGainedOrLost;     
    }
    public virtual void ModifyCurrentEnergy(int energyGainedOrLost)
    {
        currentEnergy += energyGainedOrLost;
        if (currentDexterity != 0)
        {
           // myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Dexterity"), energyGainedOrLost);
        }

        if (energyGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Energy +" + energyGainedOrLost, false));
        }

        if (energyGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Energy " + energyGainedOrLost, false));
        }

    }

    public virtual void ModifyCurrentBarrierStacks(int barrierGainedOrLost)
    {
        currentBarrierStacks += barrierGainedOrLost;
        if (currentBarrierStacks >= 1)
        {
            hasBarrier = true;
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Barrier"), barrierGainedOrLost);
        }

        else if (currentBarrierStacks <= 0)
        {
            hasBarrier = false;
            myStatusManager.RemoveStatusIcon(myStatusManager.GetStatusIconByName("Barrier"));
        }

        if(barrierGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Barrier +" + barrierGainedOrLost, false));
        }
    }

    public virtual void ModifyCurrentBlock(int blockGainedOrLost)
    {
        // if block is being gained
        if(blockGainedOrLost >= 0)
        {
            currentBlock += blockGainedOrLost + currentDexterity;
        }
        // else if block is being reduced
        else if(blockGainedOrLost < 0)
        {
            currentBlock += blockGainedOrLost;
        }
        

        if(currentBlock <= 0)
        {
            currentBlock = 0;
            myBlockIcon.SetActive(false);
        }

        else if(currentBlock > 0)
        {
            myBlockIcon.SetActive(true);
        }

        if(blockGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Block +" + (blockGainedOrLost + currentDexterity).ToString(), false));
        }

        UpdateBlockAmountText(currentBlock);
    }

    public virtual void SetCurrentBlock(int newBlockValue)
    {
        currentBlock = newBlockValue;
        if (currentBlock <= 0)
        {
            currentBlock = 0;
            myBlockIcon.SetActive(false);
        }

        else if (currentBlock > 0)
        {
            myBlockIcon.SetActive(true);
        }

        UpdateBlockAmountText(currentBlock);
    }

    // Abilities
    public void OnAbilityUsed(Ability ability, LivingEntity livingEntity)
    {
        Debug.Log("OnAbilityUsed() called for " + livingEntity.gameObject.name + " using " + ability.abilityName);
        // Set ability on cooldown
        ability.ModifyCurrentCooldown(ability.abilityBaseCooldownTime);

        // Reduce AP by cost of the ability
        // check for preparation here
        if (myPassiveManager.Preparation && ability.abilityName != "Preparation" && ability.abilityName != "Slice And Dice")
        {
            myPassiveManager.Preparation = false;
            myPassiveManager.preparationStacks = 0;
            myStatusManager.RemoveStatusIcon(myStatusManager.GetStatusIconByName("Preparation"));
        }
        else
        {
            ModifyCurrentAP(-ability.abilityAPCost);
        }
        

        // remove camoflage if the ability is not move
        /*
        if(ability.abilityName != "Move")
        {
            if (isCamoflaged)
            {
                RemoveCamoflage();
            }
        }
        */

        // TO DO: re-do fleetfooted pasive bonus logic: move ability should be free, not paid for then refunded with AP
        if(ability.abilityName == "Move")
        {
            // if character has a free move available
            if(moveActionsTakenThisTurn == 0 && myPassiveManager.FleetFooted)
            {
                StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fleet Footed", true));
                ModifyCurrentAP(ability.abilityAPCost, false);
            }
            moveActionsTakenThisTurn++;
        }        

        LevelManager.Instance.UnhighlightAllTiles();
    }

    
    // Damage related and events
    
    public void HandlePoisonDamage(int damageAmount)
    {        
        int healthAfter = currentHealth;
        healthAfter = currentHealth - damageAmount;

        if (hasBarrier && healthAfter < currentHealth)
        {
            damageAmount = 0;
            healthAfter = currentHealth;
            ModifyCurrentBarrierStacks(-1);
        }

        // Enrage
        if (myPassiveManager.Enrage && healthAfter < currentHealth)
        {
            Debug.Log("Enrage triggered, gaining +2 strength");
            ModifyCurrentStrength(myPassiveManager.enrageStacks);
        }

        // Remove Sleeping
        if(isSleeping && healthAfter < currentHealth)
        {
            ModifySleeping(-currentSleepingStacks);
        }

        if (myPassiveManager.Adaptive && healthAfter < currentHealth)
        {
            Debug.Log("Adaptive triggered, gaining block");
            ModifyCurrentBlock(myPassiveManager.adaptiveStacks);
        }

        // remove camoflage if damaged
        /*
        if (isCamoflaged && healthAfter < currentHealth)
        {
            RemoveCamoflage();
        }
        */

        currentHealth = healthAfter;        
        myHealthBar.value = CalculateHealthBarPosition();
        if (defender != null)
        {
            defender.myCharacterData.SetCurrentHealth(currentHealth);
        }
        

        if (damageAmount > 0)
        {
            // TO DO: the damage effect from poison should look different to the regular damage vfx
            StartCoroutine(VisualEffectManager.Instance.CreateDamageEffect(transform.position, damageAmount, true));
        }

        UpdateCurrentHealthText();

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

   
    public virtual IEnumerator HandleDeath()
    {
        LevelManager.Instance.SetTileAsUnoccupied(TileCurrentlyOn);
        LivingEntityManager.Instance.allLivingEntities.Remove(this);

        Defender defender = GetComponent<Defender>();
        Enemy enemy = GetComponent<Enemy>();        

        if (myPassiveManager.Volatile)
        {
            CombatLogic.Instance.CreateAoEAttackEvent(this, myPassiveManager.volatileStacks, TileCurrentlyOn, 1, true, true,AbilityDataSO.DamageType.None);
        }

        // Check if the player has lost all characters and thus the game
        if (defender)
        {
            DefenderManager.Instance.allDefenders.Remove(defender);
        }

        DisableWorldSpaceCanvas();
        PlayDeathAnimation();
        yield return new WaitUntil(() => MyDeathAnimationFinished() == true);

        // Check all enemies are dead, end combat event
        if (enemy)
        {
            enemy.currentlyActivated = false;
            EnemyManager.Instance.allEnemies.Remove(enemy);
            // check if this was the last enemy in the encounter
            if (EnemyManager.Instance.allEnemies.Count == 0 &&
                DefenderManager.Instance.allDefenders.Count >= 1)
            {
                // End combat event, loot screen etc
                if(EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.EliteEnemy)
                {
                    StartCoroutine(EventManager.Instance.StartNewEndEliteEncounterEvent());
                }
                else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.BasicEnemy)
                {
                    //StartCoroutine(EventManager.Instance.StartNewEndBasicEncounterEvent());
                    EventManager.Instance.StartNewEndBasicEncounterEvent();
                }                

            }
        }

        // end turn and activation triggers just incase        
        myOnActivationEndEffectsFinished = true;
        ActivationManager.Instance.activationOrder.Remove(this);
        Destroy(myActivationWindow.gameObject);
        ActivationManager.Instance.MoveArrowTowardsTargetPanelPos(ActivationManager.Instance.entityActivated.myActivationWindow);
        Destroy(gameObject,0.1f);
    }

    public void PlayDeathAnimation()
    {
        myAnimator.SetTrigger("Die");
    }

    public bool myDeathAnimationFinished = false;
    public bool MyDeathAnimationFinished()
    {
        if (myDeathAnimationFinished == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetDeathAnimAsFinished()
    {
        myDeathAnimationFinished = true;
        mySpriteRenderer.enabled = false;
    }

    public void DisableWorldSpaceCanvas()
    {
        myWorldSpaceCanvasParent.SetActive(false);
    }

    public virtual IEnumerator AttackMovement(LivingEntity entityMovedTowards)
    {
        PositionLogic.Instance.CalculateWhichDirectionToFace(this, entityMovedTowards.TileCurrentlyOn);

        Vector3 startingPos = transform.position;
        Vector3 targetPos = Vector3.MoveTowards(startingPos, entityMovedTowards.transform.position, 0.5f);

        bool hasCompletedMovement = false;
        bool hasReachedTarget = false;

        while (hasCompletedMovement == false)   
        {
            if(hasReachedTarget == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, 10f * Time.deltaTime);
                if(transform.position == targetPos)
                {
                    hasReachedTarget = true;
                }
                
            }

            else if(hasReachedTarget == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, startingPos, 7f * Time.deltaTime);
                if(transform.position == startingPos)
                {
                    hasCompletedMovement = true;
                }
            }

            yield return new WaitForEndOfFrame();           

        }
    }

    public IEnumerator StartQuickReflexesMove()
    {
        // TO DO: prevent quick reflex movements from occuring on a characters own turn (only triggered during the enemies turn)

        TileScript destinationTile = null;
        List<TileScript> availableTiles = LevelManager.Instance.GetValidMoveableTilesWithinRange(currentMobility, TileCurrentlyOn);

        destinationTile = availableTiles[Random.Range(0, availableTiles.Count)];

        if (destination != null)
        {
            if (enemy)
            {
                enemy.SetPath(AStar.GetPath(TileCurrentlyOn.GridPosition, destinationTile.GridPosition));
                StartCoroutine(enemy.Move(currentMobility, 3));
            }

            else if (defender)
            {                
                MovementLogic.Instance.MoveEntity(defender, destinationTile);                
            }
        }

        yield return null;

    }

    // Debuff + buff related
    public virtual IEnumerator StandUp()
    {
        RemoveKnockDown();
        //ModifyCurrentAP(-2);
        yield return new WaitForSeconds(0.5f);
    }

    // Turn + activation related
    public virtual IEnumerator OnActivationStart()
    {
        moveActionsTakenThisTurn = 0;
        timesAttackedThisTurn = 0;
        GainEnergyOnActivationStart();
        ReduceCooldownsOnActivationStart();
        ModifyBlockOnActivationStart();
        if (isKnockedDown)
        {
            Debug.Log("Removing knockdown");
            StartCoroutine(StandUp());
        }

        if (myPassiveManager.Growing)
        {
            ModifyCurrentStrength(myPassiveManager.growingStacks);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.LightningShield)
        {
            myPassiveManager.LightningShield = false;
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Lightning Shield"), -myPassiveManager.lightningShieldStacks);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.ThickOfTheFight)
        {
            int charactersInMyMeleeRange = 0;
            List<TileScript> tilesInMyMeleeRange = LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, TileCurrentlyOn);

            if (defender)
            {
                foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInMyMeleeRange.Contains(enemy.TileCurrentlyOn))
                    {
                        charactersInMyMeleeRange++;
                    }
                }
            }

            else if (enemy)
            {
                foreach (Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInMyMeleeRange.Contains(defender.TileCurrentlyOn))
                    {
                        charactersInMyMeleeRange++;
                    }
                }
            }

            if(charactersInMyMeleeRange > 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Thick Of The Fight", false);
                yield return new WaitForSeconds(0.5f);
                ModifyCurrentAP(myPassiveManager.thickOfTheFightStacks);
            }
            
        }
    }

    public virtual void OnActivationEnd()
    {
        StartCoroutine(OnActivationEndCoroutine());
    }
    public virtual IEnumerator OnActivationEndCoroutine()
    {
        Debug.Log("OnActivationEndCoroutine() called...");

        // Remove/apply relevant status effects and passives
        if (myPassiveManager.Exposed)
        {
            myPassiveManager.ModifyExposed(-1);
        }

        if (myPassiveManager.Exhausted)
        {
            myPassiveManager.ModifyExhausted(-1);
        }

        if (isPinned)
        {
            RemovePinned();
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Pinned Removed", false));
            yield return new WaitForSeconds(0.5f);
        }

        if (isStunned)
        {
            RemoveStunned();
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Stunned Removed", false));
            yield return new WaitForSeconds(0.5f);
        }

        if (isSleeping)
        {
            Debug.Log("Removing sleep on turn end...");
            ModifySleeping(-1);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.TemporaryStrength)
        {
            myPassiveManager.ModifyTemporaryStrength(-myPassiveManager.temporaryStrengthStacks);
            /*
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength"), -myPassiveManager.temporaryStrengthStacks);
            ModifyCurrentStrength(-myPassiveManager.temporaryStrengthStacks);
            myPassiveManager.temporaryStrengthStacks = 0;
            myPassiveManager.TemporaryStrength = false;
            */
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.Cautious)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Cautious", false));
            yield return new WaitForSeconds(0.5f);
            ModifyCurrentBlock(myPassiveManager.cautiousStacks);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.EncouragingPresence)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Encouraging Presence", false));
            yield return new WaitForSeconds(0.5f);

            if (defender)
            {
                List<TileScript> tilesInEncouragingPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);
                foreach(Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInEncouragingPresenceRange.Contains(defender.TileCurrentlyOn))
                    {
                        Debug.Log("Character within range of Encouraging presence, granting bonus AP...");
                        defender.ModifyCurrentAP(myPassiveManager.encouragingPresenceStacks);
                    }
                }
            }

            else if (enemy)
            {
                List<TileScript> tilesInEncouragingPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);
                foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInEncouragingPresenceRange.Contains(enemy.TileCurrentlyOn))
                    {
                        enemy.ModifyCurrentAP(myPassiveManager.encouragingPresenceStacks);
                    }
                }
            }
        }

        if (myPassiveManager.SoulDrainAura)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Soul Drain Aura", false));
            yield return new WaitForSeconds(0.5f);

            if (enemy)
            {
                List<TileScript> tilesInSoulDrainAuraRange = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);
                foreach (Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInSoulDrainAuraRange.Contains(defender.TileCurrentlyOn))
                    {
                        Debug.Log("Character within range of Sould Drain Aura, stealing Strength...");
                        defender.ModifyCurrentStrength(-myPassiveManager.soulDrainAuraStacks);
                        ModifyCurrentStrength(myPassiveManager.soulDrainAuraStacks);
                    }
                }
            }

            else if (defender)
            {
                List<TileScript> tilesInSoulDrainAuraRange = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);
                foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInSoulDrainAuraRange.Contains(enemy.TileCurrentlyOn))
                    {
                        Debug.Log("Character within range of Sould Drain Aura, stealing Strength...");
                        enemy.ModifyCurrentStrength(-myPassiveManager.soulDrainAuraStacks);
                        ModifyCurrentStrength(myPassiveManager.soulDrainAuraStacks);
                    }
                }
            }
        }

        if (myPassiveManager.HatefulPresence)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Hateful Presence", false));
            yield return new WaitForSeconds(0.5f);

            if (defender)
            {
                List<TileScript> tilesInHatefulPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);
                foreach (Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInHatefulPresenceRange.Contains(defender.TileCurrentlyOn))
                    {
                        Debug.Log("Character within range of Hateful Presence, granting bonus Strength...");
                        defender.ModifyCurrentStrength(myPassiveManager.hatefulPresenceStacks);
                    }
                }
            }

            else if (enemy)
            {
                List<TileScript> tilesInHatefulPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);
                foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInHatefulPresenceRange.Contains(enemy.TileCurrentlyOn))
                    {
                        enemy.ModifyCurrentStrength(myPassiveManager.hatefulPresenceStacks);
                    }
                }
            }
        }

        if (myPassiveManager.Unhygienic)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Unhygienic", false));
            yield return new WaitForSeconds(0.5f);
            List<TileScript> tilesInUnhygienicAuraRange = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);
            if (defender)
            {
                foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInUnhygienicAuraRange.Contains(enemy.TileCurrentlyOn))
                    {
                        enemy.ModifyPoison(myPassiveManager.unhygienicStacks);
                    }
                }
            }

            if (enemy)
            {
                foreach (Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInUnhygienicAuraRange.Contains(defender.TileCurrentlyOn))
                    {
                        defender.ModifyPoison(myPassiveManager.unhygienicStacks);
                    }
                }
            }
        }

        if (myPassiveManager.Regeneration)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Regeneration", false));
            yield return new WaitForSeconds(0.5f);
            ModifyCurrentHealth(myPassiveManager.regenerationStacks);
        }

        // Take damage from poison, bleed etc

        if (isPoisoned)
        {
            HandlePoisonDamage(poisonStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Tile related events

        if(TileCurrentlyOn.myTileType == TileScript.TileType.Grass)
        {
            Debug.Log("Turn ended on grass: applying stealth...");
            // Gain stealth
            //ApplyCamoflage();
            //yield return new WaitForSeconds(0.5f);

            // Snake ring 
            if (defender &&
                ArtifactManager.Instance.HasArtifact("Snake Ring")
                )
            {
                ModifyCurrentStrength(1);
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (TileCurrentlyOn.myTileType == TileScript.TileType.Rock)
        {
            Debug.Log("Turn ended on grass: applying stealth...");
            // Gain 5 block
            ModifyCurrentBlock(5);
            yield return new WaitForSeconds(0.5f);            
        }

        // Artifact related events

        if( defender && 
            ArtifactManager.Instance.HasArtifact("Wind Up Boots") &&
            TurnManager.Instance.currentTurnCount == 1)
        {
            ModifyCurrentMobility(-1);
            yield return new WaitForSeconds(0.5f);
        }

        if (defender &&
           ArtifactManager.Instance.HasArtifact("Crown Of Kings") &&
           currentAP == currentMaxAP)
        {
            ModifyCurrentStrength(1);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        myOnActivationEndEffectsFinished = true;

    }

    public bool myOnActivationEndEffectsFinished = false;
    public bool MyOnTurnEndEffectsFinished()
    {
        if (myOnActivationEndEffectsFinished == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void GainEnergyOnActivationStart()
    {
        
        currentAP += currentEnergy;

        if (currentAP > currentMaxAP)
        {
            currentAP = currentMaxAP;
        }

        if (currentAP < 0)
        {
            currentAP = 0;
        }

        if (defender)
        {
            defender.UpdateCurrentAPText(currentAP);
        }
    }

    public void ReduceCooldownsOnActivationStart()
    {
        foreach (Ability ability in mySpellBook.myActiveAbilities)
        {
            ability.ReduceCooldownOnTurnStart();
        }          
    }

    public void ModifyBlockOnActivationStart()
    {
        if (ArtifactManager.Instance.HasArtifact("Calipers"))
        {
            ModifyCurrentBlock(-10);
        }
        else
        {
            SetCurrentBlock(0);
        }
        
    }

    // Visual
    /*
    public void FlipMySprite(bool faceRight)
    {
        if (faceRight == true)
        {
            mySpriteRenderer.flipX = true;
        }

        else
        {
            mySpriteRenderer.flipX = false;
        }
    }

    public void ChangeDirection(string leftOrRight)
    {
        if(leftOrRight == "Left")
        {
            FlipMySprite(false);
        }
        else if(leftOrRight == "Right")
        {
            FlipMySprite(true);
        }
    }

    public void CalculateWhichDirectionToFace(TileScript myCurrentPosition, TileScript tileToFace)
    {
        // flip the sprite's x axis depending on the direction of movement
        if (LevelManager.Instance.IsDestinationTileToTheRight(myCurrentPosition, tileToFace))
        {
            ChangeDirection("Left");
            //FlipMySprite(false);
        }

        else if (LevelManager.Instance.IsDestinationTileToTheRight(myCurrentPosition, tileToFace) == false)
        {
            ChangeDirection("Right");
            //FlipMySprite(true);
        }

    }
    */

    public void UpdateBlockAmountText(int newBlockValue)
    {
        myBlockText.text = newBlockValue.ToString();
    }

    public void UpdateCurrentHealthText()
    {
        myCurrentHealthText.text = currentHealth.ToString();
        if (defender)
        {
            defender.myCurrentHealthTextStatBar.text = currentHealth.ToString();
        }
    }

    public void UpdateCurrentMaxHealthText(int newMaxHealthValue)
    {
        myCurrentMaxHealthText.text = newMaxHealthValue.ToString();
        if (defender)
        {
            defender.myCurrentMaxHealthTextStatBar.text = currentHealth.ToString();
        }
    }

    // Targeting and tiles

    public LivingEntity GetClosestFriendlyTarget()
    {
        Defender myDefender = GetComponent<Defender>();
        Enemy myEnemy = GetComponent<Enemy>();

        LivingEntity closestTarget = null;
        float minimumDistance = Mathf.Infinity;

        if (myDefender)
        {
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                float distancefromThisCharacter = Vector2.Distance(defender.gameObject.transform.position, transform.position);
                if (distancefromThisCharacter < minimumDistance && defender != this)
                {
                    closestTarget = defender;
                    minimumDistance = distancefromThisCharacter;
                }
            }
            
        }

        else if (myEnemy)
        {
            foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
            {
                float distancefromThisCharacter = Vector2.Distance(enemy.gameObject.transform.position, transform.position);
                if (distancefromThisCharacter < minimumDistance && enemy != this)
                {
                    closestTarget = enemy;
                    minimumDistance = distancefromThisCharacter;
                }
            }
            
        }

        if(closestTarget == null)
        {
            closestTarget = this;
        }

        return closestTarget;
    }

    public TileScript GetValidGrassTileWithinRange(int range)
    {
        TileScript closestGrassTile = null;
        List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinMovementRange(range, TileCurrentlyOn);
        foreach (TileScript tile in adjacentTiles)
        {
            if (tile.myTileType == TileScript.TileType.Grass && tile.IsEmpty && tile.IsWalkable)
            {
                closestGrassTile = tile;
            }
        }

        return closestGrassTile;
    }

    // method used to enemy AI move away from a defender logically to safety
    public TileScript GetFurthestTileFromTargetWithinRange(LivingEntity target, int range)
    {
        List<TileScript> tilesWithinRangeOfOriginCharacter = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, TileCurrentlyOn);
        return LevelManager.Instance.GetFurthestTileFromTargetFromList(tilesWithinRangeOfOriginCharacter, target.TileCurrentlyOn);        
    }
}
