using Spriter2UnityDX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour
{        
    
    [Header("Component References")]
    public Slider myHealthBar;
    public GameObject myWorldSpaceCanvasParent;
    public GameObject mySpriteParent;
    public GameObject myModelParent;
    public EntityRenderer myEntityRenderer;
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
    public int baseStamina;    
    public int baseMaxEnergy;
    public int baseMeleeRange;
    public int baseStrength;
    public int baseWisdom;
    public int baseDexterity;
    public int baseInitiative;
    public int baseCriticalChance;
    public int baseDodgeChance;
    public int baseParryChance;
    public int baseAuraSize;
    public int baseStartingBlock;
    public int baseStartingEnergyBonus;
    public int baseMaxPowersCount;

    public int basePhysicalResistance;
    public int baseFireResistance;
    public int baseFrostResistance;
    public int baseShadowResistance;
    public int basePoisonResistance;
    public int baseAirResistance;

    [Header("Current Trait Properties")]
    public int currentMobility;
    public int currentMaxHealth;
    public int currentHealth;
    public int currentMaxEnergy;
    public int currentStamina;
    public int currentEnergy;
    public int currentMeleeRange;
    public int currentStrength;
    public int currentWisdom;
    public int currentDexterity;
    public int currentInitiative;
    public int currentCriticalChance;
    public int currentDodgeChance;
    public int currentParryChance;
    public int currentAuraSize;
    public int currentMaxPowersCount;
    public int currentBlock;
    public int currentPhysicalResistance;
    public int currentFireResistance;
    public int currentFrostResistance;
    public int currentShadowResistance;
    public int currentPoisonResistance;
    public int currentAirResistance;
    public ItemDataSO myMainHandWeapon;
    public ItemDataSO myOffHandWeapon;

    [Header("Pathing + Location Related ")]
    public Tile tile;
    public Point gridPosition;
    public LivingEntity myCurrentTarget;
    public Stack<Node> path;
    public Vector3 destination;

    [Header("Other Properties ")]
    public List<Ability> activePowers;

    [Header("Miscealaneous Properties ")]
    public float speed;   
    public bool mouseIsOverStatusIconPanel;
    public bool mouseIsOverCharacter;
    public int currentInitiativeRoll;
    public int moveActionsTakenThisActivation;
    public int timesAttackedThisTurnCycle;
    public bool inDeathProcess;
    public bool facingRight;
    public bool spriteImportedFacingRight;
    public Color normalColour;
    public Color highlightColour;
    public bool myRangedAttackFinished;
    public bool hasActivatedThisTurn;
    public LivingEntity myTaunter;

    // Initialization / Setup
    #region
    public virtual void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {
        Debug.Log("Calling LivingEntity.InitializeSetup...");

        // Get component references        
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        if (myAnimator == null)
        {
            myAnimator = GetComponent<Animator>();
        }
        
        defender = GetComponent<Defender>();
        enemy = GetComponent<Enemy>();
        myPassiveManager = GetComponent<PassiveManager>();
        myPassiveManager.InitializeSetup();
        mySpellBook = GetComponent<SpellBook>();
        mySpellBook.InitializeSetup();

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
        
        myEntityRenderer = GetComponentInChildren<EntityRenderer>();
        myStatusManager.SetPanelViewState(true);
        MovementLogic.Instance.OnNewTileSet(this);

        // Set up all base properties and values (damage, mobility etc)
        SetBaseProperties();
    }    
    public virtual void SetBaseProperties()
    {
        //currentMobility = baseMobility;
        currentMaxHealth = baseMaxHealth;    
        currentHealth = baseStartingHealth;
        currentMaxEnergy = baseMaxEnergy;
        //currentStamina = baseStamina;              
        currentMeleeRange = baseMeleeRange;

        // remove later when we've made characters base power = 1 in the inspector
        baseMaxPowersCount++;

        // Set Weapons
        ItemManager.Instance.SetUpLivingEntityWeapons(this);        

        // Set up core stats
        ModifyCurrentStrength(baseStrength);
        ModifyCurrentWisdom(baseWisdom);
        ModifyCurrentDexterity(baseDexterity);
        ModifyCurrentInitiative(baseInitiative);
        ModifyCurrentStamina(baseStamina);
        ModifyCurrentMobility(baseMobility);

        // Set up secondary stats
        ModifyCurrentCriticalChance(baseCriticalChance);
        ModifyCurrentDodgeChance(baseDodgeChance);
        ModifyCurrentParryChance(baseParryChance);
        ModifyCurrentAuraSize(baseAuraSize);
        ModifyMaxPowersLimit(baseMaxPowersCount);

        // Set up misc stats
        ModifyCurrentBlock(baseStartingBlock);
        ModifyCurrentEnergy(baseStartingEnergyBonus);        

        // Refresh GUI's
        UpdateHealthGUIElements();
        SetColor(normalColour);

        // TESTING CODE FOR ITEMS/PASSIVES/ABILITIES ETC!! Remove in future

        // Modify Stats
        //ModifyCurrentStrength(2);
        //ModifyCurrentWisdom(2);
        //ModifyCurrentDexterity(2);
        // ModifyCurrentAuraSize(1);
        //ModifyCurrentParryChance(100);
        //ModifyCurrentDodgeChance(100);

        // Modify items
        //ItemManager.Instance.AssignWeaponToLivingEntity(this, ItemLibrary.Instance.GetItemByName("Simple Sword"));
        //ItemManager.Instance.AssignWeaponToCharacter(this, ItemLibrary.Instance.GetItemByName("Simple Bow"));
        //ItemManager.Instance.AssignShieldToCharacter(this, ItemLibrary.Instance.GetItemByName("Simple Shield"));

        // Modify Passives

        // Debuffs
      //  myPassiveManager.ModifyBlind(1);
      //  myPassiveManager.ModifyDisarmed(1);
        //myPassiveManager.ModifyStunned(1);
      //  myPassiveManager.ModifySilenced(1);
        // myPassiveManager.ModifySleep(1);
       //  myPassiveManager.ModifyTerrified(1);
       //  myPassiveManager.ModifyImmobilized(1);
        //myPassiveManager.ModifyVulnerable(1);
        // myPassiveManager.ModifyWeakened(1);
        // myPassiveManager.ModifyChilled(1);
        // myPassiveManager.ModifyShocked(1);
        // myPassiveManager.ModifyPoisoned(1);
        // myPassiveManager.ModifyBurning(1);
        //myPassiveManager.ModifyFading(10);

        // Core stat buffs
        // myPassiveManager.ModifyBonusStrength(2);
        // myPassiveManager.ModifyBonusWisdom(2);
        // myPassiveManager.ModifyBonusStamina(20);
        //myPassiveManager.ModifyBonusInitiative(2);
        // myPassiveManager.ModifyBonusMobility(2);
        // myPassiveManager.ModifyBonusDexterity(2);

        // Core stat temp buffs
        //  myPassiveManager.ModifyTemporaryStrength(2);
        // myPassiveManager.ModifyTemporaryWisdom(2);
        // myPassiveManager.ModifyTemporaryStamina(20);
        // myPassiveManager.ModifyTemporaryInitiative(2);
        // myPassiveManager.ModifyTemporaryMobility(2);
        // myPassiveManager.ModifyTemporaryDexterity(2);
        // myPassiveManager.ModifyTemporaryParry(50);
        //  myPassiveManager.ModifyTemporaryDodge(50);

        // Temp Buffs
        //  myPassiveManager.ModifySharpenedBlade(1);
        // myPassiveManager.ModifyCamoflage(1);
        // myPassiveManager.ModifyPreparation(1);
        // myPassiveManager.ModifyTemporaryTrueSight(1);
        // myPassiveManager.ModifyTemporaryHawkEyeBonus(2);
        // myPassiveManager.ModifyTranscendence(1);
        //  myPassiveManager.ModifyOverwatch(1);
        //myPassiveManager.ModifyBarrier(1);
        //myPassiveManager.ModifyTimeWarp(1);

        // Permanent Buffs (Non Stacking)       
        // myPassiveManager.ModifyStealth(1);
        // myPassiveManager.ModifyPatientStalker(1);
        // myPassiveManager.ModifyPredator(1);
       // myPassiveManager.ModifyUndead();
        // myPassiveManager.ModifyPoisonImmunity();
        // myPassiveManager.ModifyNimble(1);
        //  myPassiveManager.ModifyPerfectReflexes(1);
        // myPassiveManager.ModifyMasochist(1);
        // myPassiveManager.ModifySlippery(1);
        // myPassiveManager.ModifyLastStand(1);
        // myPassiveManager.ModifyRiposte(1);
       // myPassiveManager.ModifyLifeSteal(1);
       // myPassiveManager.ModifyShatter(1);
       // myPassiveManager.ModifyEtherealBeing(1);
       // myPassiveManager.ModifyFlux(1);
       // myPassiveManager.ModifyPhasing(1);
      //  myPassiveManager.ModifyTrueSight(1);
       // myPassiveManager.ModifyPerfectAim(1);

        // Permanent Buffs (Stacking)
       // myPassiveManager.ModifyEnrage(2);
       // myPassiveManager.ModifyThorns(5);
       // myPassiveManager.ModifyOpportunist(50);
        //myPassiveManager.ModifyVolatile(5);
       // myPassiveManager.ModifyCautious(5);
       // myPassiveManager.ModifyGrowing(2);        
       // myPassiveManager.ModifyPoisonous(1);
      //  myPassiveManager.ModifyImmolation(1);
      //  myPassiveManager.ModifyRegeneration(3);
       // myPassiveManager.ModifyTenacious(5);
       // myPassiveManager.ModifyHawkEye(2);
       // myPassiveManager.ModifyVenomous(1);
        //myPassiveManager.ModifyRune(1);

        // Immunity buffs
       // myPassiveManager.ModifyUnleashed(1);
       // myPassiveManager.ModifyUnstoppable(1);
       // myPassiveManager.ModifyInfallible(1);
        // myPassiveManager.ModifyIncorruptable(1);
        // myPassiveManager.ModifyUnwavering(1);

       // myPassiveManager.ModifyVulnerable(1);
         //myPassiveManager.ModifyWeakened(1);


        // Permanent Imbuements
        //myPassiveManager.ModifyAirImbuement(1);
        // myPassiveManager.ModifyFireImbuement(1);
        // myPassiveManager.ModifyPoisonImbuement(1);
        // myPassiveManager.ModifyShadowImbuement(1);
        //  myPassiveManager.ModifyFrostImbuement(1);

        // Temp Imbuements
        // myPassiveManager.ModifyAirImbuement(1);
        // myPassiveManager.ModifyFireImbuement(1);
        // myPassiveManager.ModifyPoisonImbuement(1);
        // myPassiveManager.ModifyShadowImbuement(1);
        //  myPassiveManager.ModifyFrostImbuement(1);

        // Power passives
        // myPassiveManager.ModifyPurity(1);
        // myPassiveManager.ModifyRecklessness(1);
        // myPassiveManager.ModifyTestudo(1);
        // myPassiveManager.ModifyConcentration(1);
        // myPassiveManager.ModifyInfuse(1);
        //  myPassiveManager.ModifyRapidCloaking(1);

        // Auras
       //  myPassiveManager.ModifyEncouragingAura(10);
        // myPassiveManager.ModifyStormAura(3);
        // myPassiveManager.ModifyToxicAura(5);
        // myPassiveManager.ModifyHatefulAura(2);
        // myPassiveManager.ModifyFieryAura(3);
        // myPassiveManager.ModifyGuardianAura(3);
        // myPassiveManager.ModifySacredAura(1);
        // myPassiveManager.ModifyShadowAura(1);
        // myPassiveManager.ModifySoulDrainAura(1);

        // Forms
        // myPassiveManager.ModifyShadowForm(1);
        // myPassiveManager.ModifyDemon(1);
        // myPassiveManager.ModifyToxicity(1);
        // myPassiveManager.ModifyStormLord(1);
        // myPassiveManager.ModifyFrozenHeart(1);

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
    public virtual void ModifyCurrentEnergy(int energyGainedOrLost, bool showVFX = true)
    {
        currentEnergy += energyGainedOrLost;

        if (currentEnergy > currentMaxEnergy)
        {
            currentEnergy = currentMaxEnergy;
        }

        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }

        /*
        if(energyGainedOrLost > 0 && showVFX == true)
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Energy +" + energyGainedOrLost);
            //StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        }
        */

        if (defender)
        {
            defender.UpdateEnergyBarPosition();
        }
    }
    public virtual void ModifyCurrentHealth(int healthGainedOrLost)
    {
        int originalHealth = currentHealth;
        currentHealth += healthGainedOrLost;

        // prevent health increasing over maximum
        if(currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }

        // prevent health going less then 0
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        if(currentHealth > originalHealth)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateHealEffect(transform.position, healthGainedOrLost));
        }
        UpdateHealthGUIElements();

    }
    public virtual void ModifyCurrentStrength(int strengthGainedOrLost)
    {
        currentStrength += strengthGainedOrLost;
        /*
        if(strengthGainedOrLost == 0)
        {
            return;
        }

        int previousStrength = currentStrength;
        currentStrength += strengthGainedOrLost;

        if (currentStrength != 0 ||
            (currentStrength == 0 && (previousStrength > 0 || previousStrength < 0))
            )
        {
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Bonus Strength"), strengthGainedOrLost);
        }
        
       
        if (strengthGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strength +" + strengthGainedOrLost, false));
            StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        }

        if (strengthGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strength " + strengthGainedOrLost, false));
            StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
        }
        */

    }
    public virtual void ModifyCurrentWisdom(int wisdomGainedOrLost)
    {
        currentWisdom += wisdomGainedOrLost;
        /*
        int previousWisdom = currentWisdom;
        currentWisdom += wisdomGainedOrLost;

        if (currentWisdom != 0 ||
            (currentWisdom == 0 && (previousWisdom > 0 || previousWisdom < 0))
            )
        {
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Bonus Wisdom"), wisdomGainedOrLost);
        }


        if (wisdomGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Wisdom +" + wisdomGainedOrLost, false));
            StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        }

        if (wisdomGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Wisdom " + wisdomGainedOrLost, false));
            StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
        }
        */

    }
    public virtual void ModifyCurrentDexterity(int dexterityGainedOrLost)
    {
        currentDexterity += dexterityGainedOrLost;
        /*
        if (currentDexterity != 0)
        {
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Dexterity"), dexterityGainedOrLost);
        }

        if (dexterityGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dexterity +" + dexterityGainedOrLost, false, "Blue"));
            StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        }

        if (dexterityGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dexterity " + dexterityGainedOrLost, false));
            StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
        }
        */

    }
    public virtual void ModifyCurrentInitiative(int initiativeGainedOrLost)
    {
        currentInitiative += initiativeGainedOrLost;     
    }
    public virtual void ModifyCurrentStamina(int staminaGainedOrLost)
    {
        currentStamina += staminaGainedOrLost;

        /*
        if (energyGainedOrLost > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Energy +" + energyGainedOrLost, false, "Blue"));
            StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        }

        if (energyGainedOrLost < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Energy " + energyGainedOrLost, false));
            StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
        }
        */

    }
    public virtual void ModifyCurrentCriticalChance(int criticalGainedOrLost)
    {
        currentCriticalChance += criticalGainedOrLost;
    }
    public virtual void ModifyCurrentDodgeChance(int dodgeGainedOrLost)
    {
        currentDodgeChance += dodgeGainedOrLost;
    }
    public virtual void ModifyCurrentParryChance(int parryGainedOrLost)
    {
        currentParryChance += parryGainedOrLost;
    }
    public virtual void ModifyCurrentAuraSize(int auraSizeGainedOrLost)
    {
        currentAuraSize += auraSizeGainedOrLost;
    }
    public virtual void ModifyMaxPowersLimit(int maxPowersLimitGainedOrLost)
    {
        currentMaxPowersCount += maxPowersLimitGainedOrLost;
    }
    public virtual void ModifyCurrentBlock(int blockGainedOrLost)
    {
        Debug.Log("LivingEntity.ModifyCurrentBlock() called for " + name);

        if (!myPassiveManager.terrified && blockGainedOrLost > 0)
        {
            currentBlock += blockGainedOrLost;

            if (blockGainedOrLost > 0)
            {
                StartCoroutine(VisualEffectManager.Instance.CreateGainBlockEffect(transform.position, blockGainedOrLost));
            }
        }

        if (myPassiveManager.terrified)
        {
            Debug.Log("Unable to gain block: " + name + " is 'Terrified'");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Terrified!");
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

    // Resistances
    public void ModifyFireResistance(int fireResistanceGainedOrLost)
    {
        currentFireResistance += fireResistanceGainedOrLost;
    }
    public void ModifyShadowResistance(int shadowResistanceGainedOrLost)
    {
        currentShadowResistance += shadowResistanceGainedOrLost;
    }
    public void ModifyAirResistance(int airResistanceGainedOrLost)
    {
        currentAirResistance += airResistanceGainedOrLost;
    }
    public void ModifyPoisonResistance(int poisonResistanceGainedOrLost)
    {
        currentPoisonResistance += poisonResistanceGainedOrLost;
    }
    public void ModifyFrostResistance(int frostResistanceGainedOrLost)
    {
        currentFrostResistance += frostResistanceGainedOrLost;
    }
    public void ModifyPhysicalResistance(int physicalResistanceGainedOrLost)
    {
        currentPhysicalResistance += physicalResistanceGainedOrLost;
    }
   
    #endregion

    // Damage + Death related events and VFX
    #region
    public IEnumerator HandleDeath()
    {
        LevelManager.Instance.SetTileAsUnoccupied(tile);
        LivingEntityManager.Instance.allLivingEntities.Remove(this);

        Defender defender = GetComponent<Defender>();
        Enemy enemy = GetComponent<Enemy>();

       // PlayDeathAnimation();

        if (myPassiveManager.Volatile)
        {
            // Notification
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Volatile");

            // Calculate which characters are hit by the aoe
            List<LivingEntity> targetsInRange = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(this, tile, 1, true, true);

            // Damage all targets hit
            foreach (LivingEntity entity in targetsInRange)
            {
                if(entity.inDeathProcess == false)
                {
                    int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(this, entity, null, "Physical", false, myPassiveManager.volatileStacks);
                    Action volatileExplosion = CombatLogic.Instance.HandleDamage(finalDamageValue, this, entity, "Physical");
                    //yield return new WaitForSeconds(0.1f);
                    yield return new WaitUntil(() => volatileExplosion.ActionResolved() == true);
                }               
            }

            yield return new WaitForSeconds(1);
        }

        // check for soul link and damage allies if they have it
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (entity.myPassiveManager.soulLink && CombatLogic.Instance.IsTargetFriendly(this, entity))
            {
                //Action soulLinkDamage = CombatLogic.Instance.HandleDamage(10, this, entity);
            }
        }


        // Check if the player has lost all characters and thus the game
        if (defender)
        {
            DefenderManager.Instance.allDefenders.Remove(defender);
            if (DefenderManager.Instance.allDefenders.Count == 0)
            {
                //LivingEntityManager.Instance.StopAllEntityCoroutines();
                EventManager.Instance.StartNewGameOverDefeatedEvent();

            }
        }

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
                if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.EliteEnemy)
                {
                    EventManager.Instance.StartNewEndEliteEncounterEvent();
                }
                else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.BasicEnemy)
                {
                    //StartCoroutine(EventManager.Instance.StartNewEndBasicEncounterEvent());
                    EventManager.Instance.StartNewEndBasicEncounterEvent();
                }
                else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.Boss)
                {
                    //StartCoroutine(EventManager.Instance.StartNewEndBasicEncounterEvent());
                    EventManager.Instance.StartNewEndBossEncounterEvent();
                }

            }

        }

        DisableWorldSpaceCanvas();
        Action destroyWindowAction = myActivationWindow.FadeOutWindow();
        PlayDeathAnimation();
        yield return new WaitUntil(() => MyDeathAnimationFinished() == true);
        Debug.Log("LivingEntity.HandleDeath() finished waiting for death anim to finish");
        myAnimator.enabled = false;
        yield return new WaitUntil(() => destroyWindowAction.ActionResolved() == true);
        Debug.Log("LivingEntity.HandleDeath() finished waiting for activation window to be destroyed");          

        // end turn and activation triggers just incase        
        myOnActivationEndEffectsFinished = true;

        if (ActivationManager.Instance.entityActivated == this)
        {
            ActivationManager.Instance.ActivateNextEntity();
        }
        ActivationManager.Instance.activationOrder.Remove(this);
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
        //mySpriteRenderer.enabled = false;
    }
    public void PlayRangedAttackAnimation()
    {
        myAnimator.SetTrigger("Ranged Attack");
    }
    public void SetRangedAttackAnimAsFinished()
    {
        myRangedAttackFinished = true;
    }
    public void RefreshRangedAttackBool()
    {
        myRangedAttackFinished = false;
    }
    public void DisableWorldSpaceCanvas()
    {
        myWorldSpaceCanvasParent.SetActive(false);
    }
    public virtual IEnumerator PlayMeleeAttackAnimation(LivingEntity entityMovedTowards, float speed = 3)
    {
        PositionLogic.Instance.CalculateWhichDirectionToFace(this, entityMovedTowards.tile);

        Vector3 startingPos = transform.position;
        Vector3 targetPos = Vector3.MoveTowards(startingPos, entityMovedTowards.transform.position, 0.5f);

        bool hasCompletedMovement = false;
        bool hasReachedTarget = false;

        myAnimator.SetTrigger("Melee Attack");
        while (hasCompletedMovement == false)   
        {
            if(hasReachedTarget == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                if(transform.position == targetPos)
                {
                    hasReachedTarget = true;
                }
                
            }

            else if(hasReachedTarget == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, startingPos, speed * Time.deltaTime);
                if(transform.position == startingPos)
                {
                    hasCompletedMovement = true;
                }
            }

            yield return new WaitForEndOfFrame();           

        }
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
    protected IEnumerator OnActivationStartCoroutine(Action action)
    {
        moveActionsTakenThisActivation = 0;
        timesAttackedThisTurnCycle = 0;
        GainEnergyOnActivationStart();
        ReduceCooldownsOnActivationStart();
        ModifyBlockOnActivationStart();

        // Remove time warp
        if (myPassiveManager.timeWarp && hasActivatedThisTurn)
        {
            myPassiveManager.ModifyTimeWarp(-myPassiveManager.timeWarpStacks);
        }

        // Growing
        if (myPassiveManager.growing)
        {
            myPassiveManager.ModifyBonusStrength(myPassiveManager.growingStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Testudo
        if (myPassiveManager.testudo)
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Testudo");
            yield return new WaitForSeconds(0.5f);
            ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(5, this));
            yield return new WaitForSeconds(0.5f);
        }

        // Lightning Shield
        if (myPassiveManager.lightningShield)
        {
            myPassiveManager.lightningShield = false;
            myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Lightning Shield"), -myPassiveManager.lightningShieldStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Thick of the Fight
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
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Thick Of The Fight");
                yield return new WaitForSeconds(0.5f);
                ModifyCurrentEnergy(myPassiveManager.thickOfTheFightStacks);
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
    protected virtual IEnumerator OnActivationEndCoroutine(Action action)
    {
        Debug.Log("OnActivationEndCoroutine() called...");

        // Remove/apply relevant status effects and passives
        if (myPassiveManager.vulnerable)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Vulnerable...");
            myPassiveManager.ModifyVulnerable(-1);
            yield return new WaitForSeconds(1f);
        }

        // Remove Weakened
        if (myPassiveManager.weakened)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Weakened...");
            myPassiveManager.ModifyWeakened(-1);
            yield return new WaitForSeconds(1f);
        }

        // Remove Immobilized
        if (myPassiveManager.immobilized)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Immobilized...");
            myPassiveManager.ModifyImmobilized(-myPassiveManager.immobilizedStacks);
            yield return new WaitForSeconds(1f);
        }

        // Remove Blind
        if (myPassiveManager.blind)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Blind...");
            myPassiveManager.ModifyBlind(-myPassiveManager.blindStacks);
            yield return new WaitForSeconds(1f);
        }

        // Remove Disarmed
        if (myPassiveManager.disarmed)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Disarmed...");
            myPassiveManager.ModifyDisarmed(-myPassiveManager.disarmedStacks);
            yield return new WaitForSeconds(1f);
        }

        // Remove Silenced
        if (myPassiveManager.silenced)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Silenced...");
            myPassiveManager.ModifySilenced(-myPassiveManager.silencedStacks);
            yield return new WaitForSeconds(1f);
        }

        // Remove Stunned
        if (myPassiveManager.stunned)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Stunned..");
            myPassiveManager.ModifyStunned(-myPassiveManager.stunnedStacks);
            yield return new WaitForSeconds(1f);
        }

        // Remove Sleep
        if (myPassiveManager.sleep)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Sleep...");
            myPassiveManager.ModifySleep(-myPassiveManager.sleepStacks);
            yield return new WaitForSeconds(1f);
        }

        // Remove Taunted
        if (myPassiveManager.taunted)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Taunted...");
            myPassiveManager.ModifyTaunted(-myPassiveManager.tauntedStacks, null);
            yield return new WaitForSeconds(1f);
        }

        // Remove Chilled
        if (myPassiveManager.chilled)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Chilled...");
            myPassiveManager.ModifyChilled(-myPassiveManager.chilledStacks);
            yield return new WaitForSeconds(1f);
        }

        // Remove Shocked
        if (myPassiveManager.shocked)
        {
            Debug.Log("OnActivationEndCoroutine() clearing Shocked...");
            myPassiveManager.ModifyShocked(-myPassiveManager.shockedStacks);
            yield return new WaitForSeconds(1f);
        }       

        // Cautious
        if (myPassiveManager.cautious && currentBlock == 0)
        {
            Debug.Log("OnActivationEndCoroutine() checking Cautious...");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Cautious");
            yield return new WaitForSeconds(0.5f);
            ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(myPassiveManager.cautiousStacks, this));
            yield return new WaitForSeconds(1f);
        }

        // Encouraging Aura
        if (myPassiveManager.encouragingAura)
        {
            Debug.Log("OnActivationEndCoroutine() checking Encouraging Aura..");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Encouraging Aura");
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInEncouragingPresenceRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(this), tile);
            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (tilesInEncouragingPresenceRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity))
                {
                    Debug.Log("Character " + entity.name + " is within range of Encouraging presence, granting bonus Energy...");
                    StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(entity.transform.position));
                    entity.ModifyCurrentEnergy(myPassiveManager.encouragingAuraStacks);
                }
            }            

            yield return new WaitForSeconds(1f);
        }

        // Soul Drain Aura
        if (myPassiveManager.soulDrainAura)
        {
            Debug.Log("OnActivationEndCoroutine() checking Soul Drain Aura...");

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Soul Drain Aura");
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInSoulDrainAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(this), tile);
            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (tilesInSoulDrainAuraRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity) == false)
                {
                    Debug.Log("Character " + entity.name + " is within range of Sould Drain Aura, stealing Strength...");
                    entity.ModifyCurrentStrength(-myPassiveManager.soulDrainAuraStacks);
                    ModifyCurrentStrength(myPassiveManager.soulDrainAuraStacks);
                }
            }

            yield return new WaitForSeconds(1f);
        }

        // Hateful Aura
        if (myPassiveManager.hatefulAura)
        {
            Debug.Log("OnActivationEndCoroutine() checking Hateful Aura...");

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Hateful Aura");
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInHatefulPresenceRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(this), tile);
            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (tilesInHatefulPresenceRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity))
                {
                    Debug.Log("Character " + entity.name + " is within range of Hateful Aura, granting bonus Strength...");
                    entity.myPassiveManager.ModifyBonusStrength(myPassiveManager.hatefulAuraStacks);
                }
            }

            yield return new WaitForSeconds(1f);
        }

        // Fiery Aura
        if (myPassiveManager.fieryAura)
        {
            Debug.Log("OnActivationEndCoroutine() checking Fiery Aura...");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fiery Aura");
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInFieryAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(this), tile);

            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (tilesInFieryAuraRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity) == false)
                {
                    int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(this, entity, null, "Fire", false, myPassiveManager.fieryAuraStacks);
                    Action damageAction = CombatLogic.Instance.HandleDamage(finalDamageValue, this, entity, "Fire");
                    yield return new WaitUntil(() => damageAction.ActionResolved() == true);
                }
            }

            yield return new WaitForSeconds(1f);

        }

        // Shadow Aura
        if (myPassiveManager.shadowAura)
        {
            Debug.Log("OnActivationEndCoroutine() checking Shadow Aura...");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shadow Aura");
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInShadowAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(this), tile);

            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (tilesInShadowAuraRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity) == false)
                {
                    entity.myPassiveManager.ModifyWeakened(1);
                }
            }
            yield return new WaitForSeconds(1f);

        }

        // Storm Aura
        if (myPassiveManager.stormAura)
        {
            Debug.Log("OnActivationEndCoroutine() checking Storm Aura...");

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Storm Aura");
            yield return new WaitForSeconds(0.5f);

            List<LivingEntity> stormAuraRange = EntityLogic.GetAllEnemiesWithinRange(this, EntityLogic.GetTotalAuraSize(this));
            List<LivingEntity> targetsHit = new List<LivingEntity>();

            // are there even enemies within aura range?
            if(stormAuraRange.Count > 0)
            {
                // get a random target 2 times
                for (int i = 0; i < 2; i++)
                {
                    targetsHit.Add(stormAuraRange[Random.Range(0, stormAuraRange.Count)]);
                }

                // Resolve hits against targets
                foreach (LivingEntity entity in targetsHit)
                {
                    if (entity.inDeathProcess == false)
                    {
                        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(this, entity, null, "Air", false, myPassiveManager.stormAuraStacks);

                        Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, this, entity, "Air");
                        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                    }
                }

                yield return new WaitForSeconds(1f);
            }           

        }

        // Guardian Aura
        if (myPassiveManager.guardianAura)
        {
            Debug.Log("OnActivationEndCoroutine() checking Guardian Aura...");

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Guardian Aura");
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInGuardianAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(this), tile);

            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (tilesInGuardianAuraRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity))
                {
                    // Give target block
                    entity.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(myPassiveManager.guardianAuraStacks, this));
                }
            }
            yield return new WaitForSeconds(1f);
        }

        // Toxic Aura
        if (myPassiveManager.toxicAura)
        {
            Debug.Log("OnActivationEndCoroutine() checking Toxic Aura...");

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Toxic Aura");
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInToxicAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(this), tile);

            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (tilesInToxicAuraRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity) == false)
                {
                    // Modify Poisoned
                    entity.myPassiveManager.ModifyPoisoned(myPassiveManager.toxicAuraStacks, this);
                }
            }
            yield return new WaitForSeconds(1f);
        }

        // Sacred Aura
        if (myPassiveManager.sacredAura)
        {
            Debug.Log("OnActivationEndCoroutine() checking Sacred Aura...");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Sacred Aura");
            yield return new WaitForSeconds(0.5f);

            List<Tile> tilesInEncouragingPresenceRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(this), tile);
            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (tilesInEncouragingPresenceRange.Contains(entity.tile) &&
                    CombatLogic.Instance.IsTargetFriendly(this, entity))
                {
                    Debug.Log("Character " + entity.name + " is within range of Sacred Aura, removing debuffs...");
                    StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(entity.transform.position));
                    
                    // Remove Immobilized
                    if (entity.myPassiveManager.immobilized)
                    {
                        entity.myPassiveManager.ModifyImmobilized(-entity.myPassiveManager.immobilizedStacks);
                        yield return new WaitForSeconds(0.5f);
                    }

                    // Remove Blind
                    if (entity.myPassiveManager.blind)
                    {
                        entity.myPassiveManager.ModifyBlind(-entity.myPassiveManager.blindStacks);
                        yield return new WaitForSeconds(0.5f);
                    }

                    // Remove Disarmed
                    if (entity.myPassiveManager.disarmed)
                    {
                        entity.myPassiveManager.ModifyDisarmed(-entity.myPassiveManager.disarmedStacks);
                        yield return new WaitForSeconds(0.5f);
                    }

                    // Remove Silenced
                    if (entity.myPassiveManager.silenced)
                    {
                        entity.myPassiveManager.ModifySilenced(-entity.myPassiveManager.silencedStacks);
                        yield return new WaitForSeconds(0.5f);
                    }

                    // Remove Terrified
                    if (entity.myPassiveManager.terrified)
                    {
                        entity.myPassiveManager.ModifyTerrified(-entity.myPassiveManager.terrifiedStacks);
                        yield return new WaitForSeconds(0.5f);
                    }
                }
            }

            yield return new WaitForSeconds(1f);
        }

        // Regeneration
        if (myPassiveManager.regeneration)
        {
            Debug.Log("OnActivationEndCoroutine() checking Regeneration...");

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Regeneration");            
            ModifyCurrentHealth(myPassiveManager.regenerationStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Poisoned
        if (myPassiveManager.poisoned)
        {
            Debug.Log("OnActivationEndCoroutine() checking Poisoned...");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Poisoned");
            yield return new WaitForSeconds(0.5f);
            Action poisonDamage = CombatLogic.Instance.HandleDamage(myPassiveManager.poisonedStacks, this, this, "None", null, true);
            yield return new WaitUntil(() => poisonDamage.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
        }

        // Burning
        if (myPassiveManager.burning)
        {
            Debug.Log("OnActivationEndCoroutine() checking Burning...");

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Burning");
            yield return new WaitForSeconds(0.5f);
            Action burningDamage = CombatLogic.Instance.HandleDamage(myPassiveManager.burningStacks, this, this, "None", null, true);
            yield return new WaitUntil(() => burningDamage.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
        }

        // Fading
        if (myPassiveManager.fading)
        {
            Debug.Log("OnActivationEndCoroutine() checking Fading...");

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fading");
            yield return new WaitForSeconds(0.5f);
            Action fadingDamage = CombatLogic.Instance.HandleDamage(myPassiveManager.fadingStacks, this, this, "None", null, true);
            yield return new WaitUntil(() => fadingDamage.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
        }

        // Remove Temporary Imbuements

        // Air Imbuement
        if (myPassiveManager.temporaryAirImbuement)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Air Imbuement...");
            myPassiveManager.ModifyTemporaryAirImbuement(-myPassiveManager.temporaryAirImbuementStacks);
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Air Imbuement Removed");
            yield return new WaitForSeconds(1f);            
        }

        // Fire Imbuement
        if (myPassiveManager.temporaryFireImbuement)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Fire Imbuement...");
            myPassiveManager.ModifyTemporaryFireImbuement(-myPassiveManager.temporaryFireImbuementStacks);
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fire Imbuement Removed");
            yield return new WaitForSeconds(1f);
        }

        // Shadow Imbuement
        if (myPassiveManager.temporaryShadowImbuement)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Shadow Imbuement...");
            myPassiveManager.ModifyTemporaryShadowImbuement(-myPassiveManager.temporaryShadowImbuementStacks);            
            yield return new WaitForSeconds(1f);
        }

        // Frost Imbuement
        if (myPassiveManager.temporaryFrostImbuement)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Frost Imbuement...");
            myPassiveManager.ModifyTemporaryFrostImbuement(-myPassiveManager.temporaryFrostImbuementStacks);
            yield return new WaitForSeconds(1f);
        }

        // Poison Imbuement
        if (myPassiveManager.temporaryPoisonImbuement)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Poison Imbuement...");
            myPassiveManager.ModifyTemporaryPoisonImbuement(-myPassiveManager.temporaryPoisonImbuementStacks);
            yield return new WaitForSeconds(1f);
        }

        // Rapid Cloaking
        if (myPassiveManager.rapidCloaking)
        {
            Debug.Log("OnActivationEndCoroutine() checking Rapid Cloaking...");
            myPassiveManager.ModifyCamoflage(1);
            yield return new WaitForSeconds(1f);
        }

        // Tile related events
        if (tile.myTileType == Tile.TileType.Grass)
        {
            Debug.Log("OnActivationEndCoroutine() checking Grass Tile (Camoflage)...");
            if (myPassiveManager.camoflage == false)
            {
                myPassiveManager.ModifyCamoflage(1);
            }

            yield return new WaitForSeconds(1);
        }

        // Remove Temporary Core + Secondary Stats

        // Bonus Strength
        if (myPassiveManager.temporaryBonusStrength)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Strength...");
            myPassiveManager.ModifyTemporaryStrength(-myPassiveManager.temporaryBonusStrengthStacks);            
            yield return new WaitForSeconds(1f);
        }

        // Bonus Dexterity
        if (myPassiveManager.temporaryBonusDexterity)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Dexterity...");
            myPassiveManager.ModifyTemporaryDexterity(-myPassiveManager.temporaryBonusDexterityStacks);
            yield return new WaitForSeconds(1f);
        }

        // Bonus Stamina
        if (myPassiveManager.temporaryBonusStamina)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Stamina...");
            myPassiveManager.ModifyTemporaryStamina(-myPassiveManager.temporaryBonusStaminaStacks);            
            yield return new WaitForSeconds(1);
        }

        // Bonus Wisdom
        if (myPassiveManager.temporaryBonusWisdom)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Wisdom...");
            myPassiveManager.ModifyTemporaryWisdom(-myPassiveManager.temporaryBonusWisdomStacks);
            yield return new WaitForSeconds(1f);
        }

        // Bonus Initiative
        if (myPassiveManager.temporaryBonusInitiative)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Initiative...");
            myPassiveManager.ModifyTemporaryInitiative(-myPassiveManager.temporaryBonusInitiativeStacks);
            yield return new WaitForSeconds(1f);
        }

        // Bonus Mobility
        if (myPassiveManager.temporaryBonusMobility)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Mobility...");
            myPassiveManager.ModifyTemporaryMobility(-myPassiveManager.temporaryBonusMobilityStacks);
            yield return new WaitForSeconds(1f);
        }

        // Bonus Dodge
        if (myPassiveManager.temporaryBonusDodge)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Dodge...");
            myPassiveManager.ModifyTemporaryDodge(-myPassiveManager.temporaryBonusDodgeStacks);
            yield return new WaitForSeconds(1f);
        }

        // Bonus Parry
        if (myPassiveManager.temporaryBonusParry)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Parry...");
            myPassiveManager.ModifyTemporaryParry(-myPassiveManager.temporaryBonusParryStacks);
            yield return new WaitForSeconds(1f);
        }

        // Remove Terrified
        if (myPassiveManager.terrified)
        {
            Debug.Log("OnActivationEndCoroutine() checking Terrified...");
            myPassiveManager.ModifyTerrified(-myPassiveManager.terrifiedStacks);
            yield return new WaitForSeconds(1f);
        }

        // Remove Temporary True Sight
        if (myPassiveManager.temporaryTrueSight)
        {
            Debug.Log("OnActivationEndCoroutine() checking Temporary True Sight...");
            myPassiveManager.ModifyTemporaryTrueSight(-myPassiveManager.temporaryTrueSightStacks);
            yield return new WaitForSeconds(1f);
        }

        // Resolve
        Debug.Log("OnActivationEndCoroutine() finished and resolving...");
        yield return new WaitForSeconds(1f);
        myOnActivationEndEffectsFinished = true;
        action.actionResolved = true;

    }
    public Action OnNewTurnCycleStarted()
    {
        Action action = new Action();
        StartCoroutine(OnNewTurnCycleStartedCoroutine(action));
        return action;
    }
    public IEnumerator OnNewTurnCycleStartedCoroutine(Action action)
    {
        timesAttackedThisTurnCycle = 0;
        hasActivatedThisTurn = false;

        // Remove Temporary Parry 
        if (myPassiveManager.temporaryBonusParry)
        {
            myPassiveManager.ModifyTemporaryParry(myPassiveManager.temporaryBonusParryStacks);
            yield return new WaitForSeconds(0.5f);
        }

        action.actionResolved = true;
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
        currentEnergy += EntityLogic.GetTotalStamina(this);

        if (currentEnergy > currentMaxEnergy)
        {
            currentEnergy = currentMaxEnergy;
        }

        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }

        if (defender)
        {
            defender.UpdateEnergyBarPosition();
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
        else
        {
            // Remove all block
            ModifyCurrentBlock(-currentBlock);
        }
        
    }
    #endregion

    // Misc
    #region
    public Action StartPhasingMove()
    {
        Action action = new Action();
        StartCoroutine(StartPhasingMoveCoroutine(action));
        return action;
    }
    public IEnumerator StartPhasingMoveCoroutine(Action action)
    {
        Debug.Log("StartPhasingMoveCoroutine() could not find a valid adjacent tile to teleport to...");
        if (ActivationManager.Instance.entityActivated != this)
        {           
            List<Tile> availableTiles = LevelManager.Instance.GetTilesWithinRange(2, tile);
            Tile destinationTile = LevelManager.Instance.GetRandomValidMoveableTile(availableTiles);

            if (destinationTile != null)
            {
                Action teleportAction = MovementLogic.Instance.TeleportEntity(this, destinationTile);
                yield return new WaitUntil(() => teleportAction.ActionResolved() == true);                
            }
            else
            {
                Debug.Log("StartQuickReflexesMoveCoroutine() could not find a valid adjacent tile to teleport to...");
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
    public float CalculateHealthBarPosition()
    {
        float currentHealthFloat = currentHealth;
        float currentMaxHealthFloat = currentMaxHealth;

        return currentHealthFloat / currentMaxHealthFloat;
    }
    public void UpdateHealthGUIElements()
    {
        float finalValue = CalculateHealthBarPosition();
        myHealthBar.value = finalValue;
        myActivationWindow.myHealthBar.value = finalValue;
        myCurrentHealthText.text = currentHealth.ToString();
        myCurrentMaxHealthText.text = currentMaxHealth.ToString();
        if (defender)
        {
            defender.healthBarPositionCurrentlyUpdating = false;
            defender.myCurrentHealthTextStatBar.text = currentHealth.ToString();
            defender.myCurrentMaxHealthTextStatBar.text = currentMaxHealth.ToString();
            StartCoroutine(defender.UpdateHealthBarPanelPosition(finalValue));
        }

    }
    #endregion

    // Mouse + Input Related
    #region
    public virtual void OnMouseEnter()
    {
        mouseIsOverCharacter = true;
        SetColor(highlightColour);
        
        if (!inDeathProcess)
        {
            myStatusManager.SetPanelViewState(true);

            if (myActivationWindow != null)
            {
                myActivationWindow.myGlowOutline.SetActive(true);
            }            
        }        
    }
    public void OnMouseExit()
    {
        mouseIsOverCharacter = false;
        SetColor(normalColour);
        if(mouseIsOverStatusIconPanel == false && inDeathProcess == false)
        {
            myStatusManager.SetPanelViewState(false);
        }
        if (!inDeathProcess)
        {
            if(myActivationWindow != null)
            {
                myActivationWindow.myGlowOutline.SetActive(false);
            }
            
        }       

    }

    public void SetColor(Color newColor)
    {
        Debug.Log("Setting Entity Color....");
        if(myEntityRenderer != null)
        {
            myEntityRenderer.Color = new Color(newColor.r, newColor.g, newColor.b, myEntityRenderer.Color.a);
        }
        
    }
    #endregion
}
