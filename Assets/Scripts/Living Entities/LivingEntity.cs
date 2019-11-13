using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivingEntity : MonoBehaviour
{        
    
    [Header("Component References")]
    public Slider myHealthBar;
    public CanvasGroup myCG;
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
    public int baseWisdom;
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
    public int currentWisdom;
    public int currentDexterity;
    public int currentInitiative;
    public int currentBlock;

    [Header("Pathing + Location Related ")]
    public Tile tile;
    public Point gridPosition;
    public LivingEntity myCurrentTarget;
    public Stack<Node> path;
    public Vector3 destination;

    [Header("Miscealaneous Properties ")]
    public float speed;
    public int currentInitiativeRoll;
    public int moveActionsTakenThisTurn;
    public int timesAttackedThisTurn;
    public bool inDeathProcess;
    public bool facingRight;
    public bool spriteImportedFacingRight;   
   

    // Initialization / Setup
    #region
    public virtual void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {
        // Create Portal vfx
        //ParticleManager.Instance.CreateParticleEffect(startingTile.WorldPosition, ParticleManager.Instance.portalParticlePrefab);
        Debug.Log("Calling LivingEntity.InitializeSetup...");
        // Get component references        
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCG = GetComponent<CanvasGroup>();
        myAnimator = GetComponent<Animator>();
        defender = GetComponent<Defender>();
        enemy = GetComponent<Enemy>();
        myPassiveManager = GetComponent<PassiveManager>();
        myPassiveManager.InitializeSetup();
        mySpellBook = GetComponent<SpellBook>();
        mySpellBook.InitializeSetup();
        //StartCoroutine(PlaySpawnVFX());
        // Set grid position 'Point'
        gridPosition = startingGridPosition;
        // Set our current tile
        tile = startingTile;
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
        if(enemy && ArtifactManager.Instance.HasArtifact("Black Star"))
        {
            baseStartingHealth = (int) (baseMaxHealth * 0.8f);
        }
        currentHealth = baseStartingHealth;
        currentMaxAP = baseMaxAP;
        currentEnergy = baseEnergy;              
        currentMeleeRange = baseMeleeRange;         
        ModifyCurrentStrength(baseStrength);
        ModifyCurrentWisdom(baseWisdom);
        ModifyCurrentDexterity(baseDexterity);
        ModifyCurrentInitiative(baseInitiative);
        myHealthBar.value = CalculateHealthBarPosition();
        ModifyCurrentBlock(baseStartingBlock);
        ModifyCurrentAP(baseStartingAP);
        UpdateCurrentHealthText();
        UpdateCurrentMaxHealthText(currentMaxHealth);
    }
    #endregion    

    // Stat and property modifiers
    #region
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
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "AP +" + APGainedOrLost, false, "Blue"));
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
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strength +" + strengthGainedOrLost, false, "Blue"));
        }

        if (strengthGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strength " + strengthGainedOrLost, false, "Red"));
        }

    }
    public virtual void ModifyCurrentWisdom(int wisdomGainedOrLost)
    {
        int previousWisdom = currentWisdom;
        currentWisdom += wisdomGainedOrLost;

        if (currentWisdom != 0 ||
            (currentWisdom == 0 && (previousWisdom > 0 || previousWisdom < 0))
            )
        {
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Wisdom"), wisdomGainedOrLost);
        }


        if (wisdomGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Wisdom +" + wisdomGainedOrLost, false, "Blue"));
        }

        if (wisdomGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Wisdom " + wisdomGainedOrLost, false, "Red"));
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
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dexterity +" + dexterityGainedOrLost, false, "Blue"));
        }

        if (dexterityGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dexterity " + dexterityGainedOrLost, false, "Red"));
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
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Energy +" + energyGainedOrLost, false, "Blue"));
        }

        if (energyGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Energy " + energyGainedOrLost, false, "Red"));
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

        if (blockGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Block +" + (blockGainedOrLost + currentDexterity).ToString(), false, "Blue"));
        }

        if (currentBlock <= 0)
        {
            currentBlock = 0;
            myBlockIcon.SetActive(false);
        }

        else if(currentBlock > 0)
        {
            myBlockIcon.SetActive(true);
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
    #endregion
    
    // Damage + Death related and events and VFX
    #region
    public virtual IEnumerator HandleDeath()
    {
        LevelManager.Instance.SetTileAsUnoccupied(tile);
        LivingEntityManager.Instance.allLivingEntities.Remove(this);

        Defender defender = GetComponent<Defender>();
        Enemy enemy = GetComponent<Enemy>();        

        if (myPassiveManager.Volatile)
        {
            CombatLogic.Instance.CreateAoEAttackEvent(this, myPassiveManager.volatileStacks, tile, 1, true, true,AbilityDataSO.DamageType.Physical);
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
                    EventManager.Instance.StartNewEndEliteEncounterEvent();
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
        PositionLogic.Instance.CalculateWhichDirectionToFace(this, entityMovedTowards.tile);

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
    public Action StartQuickReflexesMove()
    {
        Action action = new Action();
        StartCoroutine(StartQuickReflexesMoveCoroutine(action));
        return action;
    }
    #endregion

    // Turn + activation related
    #region
    public Action OnActivationStart()
    {
        Action action = new Action();
        StartCoroutine(OnActivationStartCoroutine(action));
        return action;
    }
    public IEnumerator OnActivationStartCoroutine(Action action)
    {
        moveActionsTakenThisTurn = 0;
        timesAttackedThisTurn = 0;
        GainEnergyOnActivationStart();
        ReduceCooldownsOnActivationStart();
        ModifyBlockOnActivationStart();        

        if (myPassiveManager.growing)
        {
            ModifyCurrentStrength(myPassiveManager.growingStacks);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.lightningShield)
        {
            myPassiveManager.lightningShield = false;
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Lightning Shield"), -myPassiveManager.lightningShieldStacks);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.thickOfTheFight)
        {
            int charactersInMyMeleeRange = 0;
            List<Tile> tilesInMyMeleeRange = LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, tile);

            if (defender)
            {
                foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInMyMeleeRange.Contains(enemy.tile))
                    {
                        charactersInMyMeleeRange++;
                    }
                }
            }

            else if (enemy)
            {
                foreach (Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInMyMeleeRange.Contains(defender.tile))
                    {
                        charactersInMyMeleeRange++;
                    }
                }
            }

            if(charactersInMyMeleeRange > 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Thick Of The Fight", false, "Blue");
                yield return new WaitForSeconds(0.5f);
                ModifyCurrentAP(myPassiveManager.thickOfTheFightStacks);
            }
            
        }

        action.actionResolved = true;
    }
    public Action OnActivationEnd()
    {
        Action action = new Action();
        StartCoroutine(OnActivationEndCoroutine(action));
        return action;
    }
    public virtual IEnumerator OnActivationEndCoroutine(Action action)
    {
        Debug.Log("OnActivationEndCoroutine() called...");

        // Remove/apply relevant status effects and passives
        if (myPassiveManager.exposed)
        {
            myPassiveManager.ModifyExposed(-1);
        }

        if (myPassiveManager.exhausted)
        {
            myPassiveManager.ModifyExhausted(-1);
        }

        if (myPassiveManager.pinned)
        {
            myPassiveManager.ModifyPinned(-myPassiveManager.pinnedStacks);
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Pinned Removed", false, "Blue"));
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.stunned)
        {
            myPassiveManager.ModifyStunned(-myPassiveManager.stunnedStacks);
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Stunned Removed", false, "Blue"));
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.sleeping)
        {
            Debug.Log("Removing sleep on turn end...");
            myPassiveManager.ModifySleeping(-1);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.temporaryStrength)
        {
            myPassiveManager.ModifyTemporaryStrength(-myPassiveManager.temporaryStrengthStacks);            
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.temporaryInitiative)
        {
            myPassiveManager.ModifyTemporaryInitiative(-myPassiveManager.temporaryInitiativeStacks);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.temporaryMobility)
        {
            myPassiveManager.ModifyTemporaryMobility(-myPassiveManager.temporaryMobilityStacks);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.cautious)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Cautious", false, "Blue"));
            yield return new WaitForSeconds(0.5f);
            ModifyCurrentBlock(myPassiveManager.cautiousStacks);
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.encouragingPresence)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Encouraging Presence", false, "Blue"));
            yield return new WaitForSeconds(0.5f);

            if (defender)
            {
                List<Tile> tilesInEncouragingPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, tile);
                foreach(Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInEncouragingPresenceRange.Contains(defender.tile))
                    {
                        Debug.Log("Character within range of Encouraging presence, granting bonus AP...");
                        defender.ModifyCurrentAP(myPassiveManager.encouragingPresenceStacks);
                    }
                }                
            }

            else if (enemy)
            {
                List<Tile> tilesInEncouragingPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, tile);
                foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInEncouragingPresenceRange.Contains(enemy.tile))
                    {
                        enemy.ModifyCurrentAP(myPassiveManager.encouragingPresenceStacks);
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        if (myPassiveManager.soulDrainAura)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Soul Drain Aura", false));
            yield return new WaitForSeconds(0.5f);

            if (enemy)
            {
                List<Tile> tilesInSoulDrainAuraRange = LevelManager.Instance.GetTilesWithinRange(1, tile);
                foreach (Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInSoulDrainAuraRange.Contains(defender.tile))
                    {
                        Debug.Log("Character within range of Sould Drain Aura, stealing Strength...");
                        defender.ModifyCurrentStrength(-myPassiveManager.soulDrainAuraStacks);
                        ModifyCurrentStrength(myPassiveManager.soulDrainAuraStacks);
                    }
                }
            }

            else if (defender)
            {
                List<Tile> tilesInSoulDrainAuraRange = LevelManager.Instance.GetTilesWithinRange(1, tile);
                foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInSoulDrainAuraRange.Contains(enemy.tile))
                    {
                        Debug.Log("Character within range of Sould Drain Aura, stealing Strength...");
                        enemy.ModifyCurrentStrength(-myPassiveManager.soulDrainAuraStacks);
                        ModifyCurrentStrength(myPassiveManager.soulDrainAuraStacks);
                    }
                }
            }
        }

        if (myPassiveManager.hatefulPresence)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Hateful Presence", false, "Blue"));
            yield return new WaitForSeconds(0.5f);

            if (defender)
            {
                List<Tile> tilesInHatefulPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, tile);
                foreach (Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInHatefulPresenceRange.Contains(defender.tile))
                    {
                        Debug.Log("Character within range of Hateful Presence, granting bonus Strength...");
                        defender.ModifyCurrentStrength(myPassiveManager.hatefulPresenceStacks);
                    }
                }
            }

            else if (enemy)
            {
                List<Tile> tilesInHatefulPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, tile);
                foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInHatefulPresenceRange.Contains(enemy.tile))
                    {
                        enemy.ModifyCurrentStrength(myPassiveManager.hatefulPresenceStacks);
                    }
                }
            }
        }
        if (myPassiveManager.fieryPresence)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fiery Presence", false, "Blue"));
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInFieryPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, tile);

            foreach(LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if(tilesInFieryPresenceRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity) == false)
                {
                    CombatLogic.Instance.HandleDamage(myPassiveManager.fieryPresenceStacks, this, entity, false, AbilityDataSO.AttackType.None, AbilityDataSO.DamageType.Magic);
                }
            }
            
        }
        if (myPassiveManager.guardianPresence)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Guardian Presence", false, "Blue"));
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInGuardianPresenceRange = LevelManager.Instance.GetTilesWithinRange(1, tile);

            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (tilesInGuardianPresenceRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity))
                {
                    entity.ModifyCurrentBlock(myPassiveManager.guardianPresenceStacks);
                }
            }

        }

        if (myPassiveManager.unhygienic)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Unhygienic", false, "Blue"));
            yield return new WaitForSeconds(0.5f);
            List<Tile> tilesInUnhygienicAuraRange = LevelManager.Instance.GetTilesWithinRange(1, tile);
            if (defender)
            {
                foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
                {
                    if (tilesInUnhygienicAuraRange.Contains(enemy.tile))
                    {
                        enemy.myPassiveManager.ModifyPoison(myPassiveManager.unhygienicStacks,this);
                    }
                }
            }

            if (enemy)
            {
                foreach (Defender defender in DefenderManager.Instance.allDefenders)
                {
                    if (tilesInUnhygienicAuraRange.Contains(defender.tile))
                    {
                        defender.myPassiveManager.ModifyPoison(myPassiveManager.unhygienicStacks, this);
                    }
                }
            }
        }

        if (myPassiveManager.regeneration)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Regeneration", false, "Blue"));
            yield return new WaitForSeconds(0.5f);
            ModifyCurrentHealth(myPassiveManager.regenerationStacks);
        }

        // Take damage from poison, bleed etc

        if (myPassiveManager.poison)
        {
            Action poisonDamage = CombatLogic.Instance.HandleDamage(myPassiveManager.poisonousStacks, this, this, false, AbilityDataSO.AttackType.None, AbilityDataSO.DamageType.Poison);
            yield return new WaitUntil(() => poisonDamage.ActionResolved() == true);
            yield return new WaitForSeconds(0.5f);
        }

        // Tile related events

        if(tile.myTileType == Tile.TileType.Grass)
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

        if (tile.myTileType == Tile.TileType.Rock)
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
        action.actionResolved = true;
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
        if (myPassiveManager.unwavering)
        {
            return;
        }

        // prevent removing block from characters that start combat with block
        if(TurnManager.Instance.currentTurnCount != 1)
        {
            if (ArtifactManager.Instance.HasArtifact("Calipers"))
            {
                ModifyCurrentBlock(-5);
            }
            else
            {
                SetCurrentBlock(0);
            }
        }      
        
    }
    #endregion

    // Misc
    #region
    
    public IEnumerator StartQuickReflexesMoveCoroutine(Action action)
    {
        // TO DO: prevent quick reflex movements from occuring on a characters own turn (only triggered during the enemies turn)
        if (ActivationManager.Instance.entityActivated != this)
        {
            Tile destinationTile = null;
            List<Tile> availableTiles = LevelManager.Instance.GetValidMoveableTilesWithinRange(currentMobility, tile);

            destinationTile = availableTiles[Random.Range(0, availableTiles.Count)];

            if (destination != null)
            {
                Action moveAction = MovementLogic.Instance.MoveEntity(defender, destinationTile);
                yield return new WaitUntil(() => moveAction.ActionResolved() == true);
                /*
                if (enemy)
                {
                    //enemy.SetPath(AStar.GetPath(TileCurrentlyOn.GridPosition, destinationTile.GridPosition));
                    //StartCoroutine(enemy.Move(currentMobility, 3));
                }

                else if (defender)
                {
                    MovementLogic.Instance.MoveEntity(defender, destinationTile);
                }
                */
            }

        }

        action.actionResolved = true;

    }
    #endregion

    // Text component modifiers
    #region
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
            defender.myCurrentMaxHealthTextStatBar.text = currentMaxHealth.ToString();
        }
    }
    public float CalculateHealthBarPosition()
    {
        float currentHealthFloat = currentHealth;
        float currentMaxHealthFloat = currentMaxHealth;

        return currentHealthFloat / currentMaxHealthFloat;
    }
    #endregion

}
