using Spriter2UnityDX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("GUI Component References")]
    public Slider myHealthBar;
    public Canvas myWorldSpaceCanvas;
    public GameObject myWorldSpaceCanvasParent;
    public GameObject myBlockIcon;
    public TextMeshProUGUI myBlockText;
    public TextMeshProUGUI myCurrentHealthText;
    public TextMeshProUGUI myCurrentMaxHealthText;

    [Header("Model Component References")]
    public GameObject myModelParent;
    public UniversalCharacterModel myModel;
    public EntityRenderer myEntityRenderer;
    public Animator myAnimator;

    [Header("Core Component References")]
    public StatusManager myStatusManager;
    public SpellBook mySpellBook;
    public PassiveManager myPassiveManager;
    [HideInInspector] public ActivationWindow myActivationWindow;
    [HideInInspector] public Defender defender;
    [HideInInspector] public Enemy enemy;

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

    [Header("Base Resistance Properties")]
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

    [Header("Current Resistance Properties")]
    public int currentPhysicalResistance;
    public int currentFireResistance;
    public int currentFrostResistance;
    public int currentShadowResistance;
    public int currentPoisonResistance;
    public int currentAirResistance;

    [Header("Weapon Properties")]
    public ItemDataSO myMainHandWeapon;
    public ItemDataSO myOffHandWeapon;

    [Header("Location Properties ")]
    public Tile tile;
    public Point gridPosition;

    [Header("Pathfinding Properties ")]
    public Stack<Node> path;
    public Vector3 destination;

    [Header("Targetting Properties ")]
    public LivingEntity myCurrentTarget;
    public LivingEntity myTaunter;

    [Header("Combat State Properties ")]
    public int moveActionsTakenThisActivation;
    public int meleeAbilityActionsTakenThisActivation;
    public int skillAbilityActionsTakenThisActivation;
    public int rangedAttackAbilityActionsTakenThisActivation;
    public int timesMeleeAttackedThisTurnCycle;
    public int currentInitiativeRoll;

    [Header("Combat Conditional Properties ")]
    public bool myRangedAttackFinished;
    public bool hasActivatedThisTurn;
    public bool inDeathProcess;

    [Header("Mouse Input Properties ")]
    public bool mouseIsOverStatusIconPanel;
    public bool mouseIsOverCharacter;

    [Header("Miscealaneous Properties")]
    public string myName;
    public float movementAnimSpeed;     
    public bool facingRight;

    [Header("Colour Properties ")]
    public Color normalColour;
    public Color highlightColour;
    
   
    #endregion

    // Initialization / Setup
    #region
    public virtual void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {
        Debug.Log("Calling LivingEntity.InitializeSetup...");

        // Get enemy/defender
        defender = GetComponent<Defender>();
        enemy = GetComponent<Enemy>();

        // Set up passive manager
        myPassiveManager.myLivingEntity = this;

        // Set up Spell Book
        mySpellBook.myLivingEntity = this;
        mySpellBook.InitializeSetup();

        // Set up status manager
        myStatusManager.InitializeSetup(this);

        // Connect entity to model
        myModel.myLivingEntity = this;

        // Set idle anim as starting anim.
        PlayIdleAnimation();

        // Auto Get+Set world space canvas event camera to help performance
        AutoSetWorldCanvasEventCamera();

        // Set grid position
        gridPosition = startingGridPosition;

        // Set current tile
        tile = startingTile;

        // Set its tile to 'occupied' state
        LevelManager.Instance.SetTileAsOccupied(startingTile);

        // Place character in the centre point of the tile
        transform.position = startingTile.WorldPosition;

        // Add this to the list of all active enemy and defender characters
        LivingEntityManager.Instance.allLivingEntities.Add(this);
        
        // Face towards the enemy
        if (defender)
        {
            PositionLogic.Instance.SetDirection(this, "Right");
        }

        else if (enemy)
        {
            PositionLogic.Instance.SetDirection(this, "Left");
        }
        
        // Enable status icons view
        myStatusManager.SetPanelViewState(true);

        // Set up all base properties and values (damage, mobility etc)
        SetBaseProperties();

        // Create Activation Window
        ActivationManager.Instance.CreateActivationWindow(this);

        // Update GUI views
        UpdateHealthGUIElements();
        
    }    
    public virtual void SetBaseProperties()
    {
        // Set health
        currentMaxHealth += baseMaxHealth;    
        currentHealth += baseStartingHealth;     
        
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
        currentMaxEnergy = baseMaxEnergy;
        currentMeleeRange = baseMeleeRange;

        // Set up Resistances
        ModifyPhysicalResistance(basePhysicalResistance);
        ModifyPoisonResistance(basePoisonResistance);        
        ModifyFireResistance(baseFireResistance);
        ModifyFrostResistance(baseFrostResistance);
        ModifyShadowResistance(baseShadowResistance);
        ModifyAirResistance(baseAirResistance);

        // Set up misc stats
        ModifyCurrentBlock(baseStartingBlock);
        ModifyCurrentEnergy(baseStartingEnergyBonus);        

        // Set colour        
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
    }
    public virtual void ModifyCurrentWisdom(int wisdomGainedOrLost)
    {
        currentWisdom += wisdomGainedOrLost;        
    }
    public virtual void ModifyCurrentDexterity(int dexterityGainedOrLost)
    {
        currentDexterity += dexterityGainedOrLost;
    }
    public virtual void ModifyCurrentInitiative(int initiativeGainedOrLost)
    {
        currentInitiative += initiativeGainedOrLost;     
    }
    public virtual void ModifyCurrentStamina(int staminaGainedOrLost)
    {
        currentStamina += staminaGainedOrLost;
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
        Debug.Log("LivingEntity.ModifyCurrentBlock() called for " + myName);

        if (!myPassiveManager.terrified && blockGainedOrLost > 0)
        {
            currentBlock += blockGainedOrLost;

            if (blockGainedOrLost > 0)
            {
                StartCoroutine(VisualEffectManager.Instance.CreateGainBlockEffect(transform.position, blockGainedOrLost));
            }
        }

        else if (myPassiveManager.terrified && blockGainedOrLost > 0)
        {
            Debug.Log("Unable to gain block: " + name + " is 'Terrified'");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Terrified!");
        }

        else if(blockGainedOrLost < 0)
        {
            currentBlock += blockGainedOrLost;
            if (currentBlock <= 0)
            {
                currentBlock = 0;
                myBlockIcon.SetActive(false);
            }
        }        

        if(currentBlock > 0)
        {
            myBlockIcon.SetActive(true);
        }        
        else if(currentBlock == 0)
        {
            myBlockIcon.SetActive(false);
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

    // Trigger Animations
    #region
    public void PlayIdleAnimation()
    {
        myAnimator.SetTrigger("Idle");
    }
    public void PlayRangedAttackAnimation()
    {
        myAnimator.SetTrigger("Shoot Bow");
    }
    public void PlaySkillAnimation()
    {
        myAnimator.SetTrigger("Skill One");
    }
    public void TriggerMeleeAttackAnimation()
    {
        myAnimator.SetTrigger("Melee Attack");
    }
    public void PlayMoveAnimation()
    {
        myAnimator.SetTrigger("Move");
    }
    public void PlayHurtAnimation()
    {
        myAnimator.SetTrigger("Hurt");
    }
    public void PlayDeathAnimation()
    {
        myAnimator.SetTrigger("Die");
    }
    #endregion

    // Damage + Death related events and VFX
    #region        
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
    public void AutoSetWorldCanvasEventCamera()
    {
        Debug.Log("LivingEntity.AutoSetWorldCanvasEventCamera() called...");

        if (myWorldSpaceCanvas)
        {
            Debug.Log("LivingEntity.AutoSetWorldCanvasEventCamera() found a canvas component, setting event camera to main unity camera...");
            myWorldSpaceCanvas.worldCamera = CameraManager.Instance.unityCamera.mainCamera;
        }
        else
        {
            Debug.Log("LivingEntity.AutoSetWorldCanvasEventCamera() recieved a null canvas, failed to set event camera...");
        }
    }
    public virtual IEnumerator PlayMeleeAttackAnimation(LivingEntity entityMovedTowards, float speed = 3)
    {
        PositionLogic.Instance.CalculateWhichDirectionToFace(this, entityMovedTowards.tile);

        Vector3 startingPos = transform.position;
        Vector3 targetPos = Vector3.MoveTowards(startingPos, entityMovedTowards.transform.position, 0.5f);

        bool hasCompletedMovement = false;
        bool hasReachedTarget = false;

        TriggerMeleeAttackAnimation();

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
        meleeAbilityActionsTakenThisActivation = 0;
        skillAbilityActionsTakenThisActivation = 0;
        rangedAttackAbilityActionsTakenThisActivation = 0;

        GainEnergyOnActivationStart();
        ReduceCooldownsOnActivationStart();
        ModifyBlockOnActivationStart();

        // check if taunted, and if taunter died 
        if(myPassiveManager.taunted && myTaunter == null)
        {
            myPassiveManager.ModifyTaunted(-myPassiveManager.tauntedStacks, null);
        }

        // Remove time warp
        if (myPassiveManager.timeWarp && hasActivatedThisTurn)
        {
            myPassiveManager.ModifyTimeWarp(-myPassiveManager.timeWarpStacks);
        }

        // Cautious
        if (myPassiveManager.cautious)
        {
            Debug.Log("OnActivationEndCoroutine() checking Cautious...");
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Cautious");
            ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(myPassiveManager.cautiousStacks, this));
            yield return new WaitForSeconds(1f);
        }

        // Growing
        if (myPassiveManager.growing)
        {
            myPassiveManager.ModifyBonusStrength(myPassiveManager.growingStacks);
            yield return new WaitForSeconds(1);
        }

        // Fast Learner
        if (myPassiveManager.fastLearner)
        {
            myPassiveManager.ModifyBonusWisdom(myPassiveManager.fastLearnerStacks);
            yield return new WaitForSeconds(1);
        }
        
        action.actionResolved = true;
    }   
    public Action OnNewTurnCycleStarted()
    {
        Action action = new Action();
        StartCoroutine(OnNewTurnCycleStartedCoroutine(action));
        return action;
    }
    private IEnumerator OnNewTurnCycleStartedCoroutine(Action action)
    {
        Debug.Log("OnNewTurnCycleStartedCoroutine() called for " + myName);

        timesMeleeAttackedThisTurnCycle = 0;
        hasActivatedThisTurn = false;

        // Remove Temporary Parry 
        if (myPassiveManager.temporaryBonusParry)
        {
            Debug.Log("OnNewTurnCycleStartedCoroutine() removing Temporary Bonus Parry...");
            myPassiveManager.ModifyTemporaryParry(-myPassiveManager.temporaryBonusParryStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Bonus Dodge
        if (myPassiveManager.temporaryBonusDodge)
        {
            Debug.Log("OnNewTurnCycleStartedCoroutine() removing Temporary Bonus Dodge...");
            myPassiveManager.ModifyTemporaryDodge(-myPassiveManager.temporaryBonusDodgeStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove Transcendence
        if (myPassiveManager.transcendence)
        {
            Debug.Log("OnNewTurnCycleStartedCoroutine() removing Transcendence...");
            myPassiveManager.ModifyTranscendence(-myPassiveManager.transcendenceStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove Marked
        if (myPassiveManager.marked)
        {
            Debug.Log("OnNewTurnCycleStartedCoroutine() checking Marked...");
            myPassiveManager.ModifyMarked(-myPassiveManager.terrifiedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // gain camo from satyr trickery
        if(TurnChangeNotifier.Instance.currentTurnCount == 1 && myPassiveManager.satyrTrickery)
        {
            VisualEffectManager.Instance.
                CreateStatusEffect(transform.position, "Satyr Trickery!");
            yield return new WaitForSeconds(0.5f);

            myPassiveManager.ModifyCamoflage(1);
            yield return new WaitForSeconds(0.5f);
        }

        // gain max Energy from human ambition
        if (TurnChangeNotifier.Instance.currentTurnCount == 1 && myPassiveManager.humanAmbition)
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Human Ambition");
            VisualEffectManager.Instance.CreateGainEnergyBuffEffect(transform.position);
            ModifyCurrentEnergy(currentMaxEnergy);
            yield return new WaitForSeconds(0.5f);
        }

        action.actionResolved = true;
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
        Debug.Log("LivingEntity.ModifyBlockOnActivationStart() called for " + myName);
        if (myPassiveManager.unwavering)
        {
            Debug.Log(myName + " has 'Unwavering' passive, not removing block");
            return;
        }
        else if(defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Polished Armour"))
        {
            Debug.Log(myName + " has 'Polished Armour' state buff, not removing block");
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
