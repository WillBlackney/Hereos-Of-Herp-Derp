using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Properties")]
    public TreasureChest activeTreasureChest;
    public WorldEncounter.EncounterType currentEncounterType;
    public bool damageTakenThisEncounter;
    public bool gameOverEventStarted;
    public bool currentCombatEndEventTriggered;
    public bool actTransitionInProcess;
    #endregion

    // Singleton Set up
    #region
    public static EventManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Start New Combat Encounter Events
    #region
    public Action StartNewBasicEncounterEvent(EnemyWaveSO enemyWave = null)
    {
        Action action = new Action();
        StartCoroutine(StartNewBasicEncounterEventCoroutine(action, enemyWave));
        return action;
    }
    private IEnumerator StartNewBasicEncounterEventCoroutine(Action action, EnemyWaveSO enemyWave = null)
    {
        // Destroy the previous level and tiles + reset values/properties, turn off unneeded views
        ClearPreviousEncounter();

        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = false;

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        StoryEventManager.Instance.DisableEventScreen();

        // Create a new level
        LevelManager.Instance.CreateLevel();

        // Reset Camera
        CameraManager.Instance.ResetCameraOnCombatStart();

        // Set up activation window holders
        ActivationManager.Instance.CreateSlotAndWindowHolders();

        // Create defender GO's        
        CharacterRoster.Instance.InstantiateDefenders();

        // Instantiate enemies
        EnemySpawner.Instance.SpawnEnemyWave("Basic", enemyWave);

        // disable unneed UI views
        UIManager.Instance.DisableUnneededCanvasesOnCombatStart();

        // Set encounter type
        currentEncounterType = WorldEncounter.EncounterType.BasicEnemy;

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Apply Relevant State Effects
        Action stateApplications = StateManager.Instance.ApplyAllStateEffectsToLivingEntities();
        yield return new WaitUntil(() => stateApplications.ActionResolved() == true);

        // Check for expired states and remove them
        Action stateExpirations = StateManager.Instance.CheckForStateExpirationsOnCombatStart();
        yield return new WaitUntil(() => stateExpirations.ActionResolved() == true);

        // Start activations / combat start events
        ActivationManager.Instance.OnNewCombatEventStarted();

        // declare this event complete
        action.actionResolved = true;
    }
    public Action StartNewEliteEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewEliteEncounterEventCoroutine(action));
        return action;
    }
    private IEnumerator StartNewEliteEncounterEventCoroutine(Action action)
    {
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();

        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = false;

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // Create a new level
        LevelManager.Instance.CreateLevel();

        // Reset Camera
        CameraManager.Instance.ResetCameraOnCombatStart();

        // Set up activation window holders
        ActivationManager.Instance.CreateSlotAndWindowHolders();

        // Create defender GO's        
        CharacterRoster.Instance.InstantiateDefenders();

        // Instantiate enemies
        currentEncounterType = WorldEncounter.EncounterType.EliteEnemy;
        EnemySpawner.Instance.SpawnEnemyWave("Elite");

        // disable unneed UI views
        UIManager.Instance.DisableUnneededCanvasesOnCombatStart();

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Apply Relevant State Effects
        Action stateApplications = StateManager.Instance.ApplyAllStateEffectsToLivingEntities();
        yield return new WaitUntil(() => stateApplications.ActionResolved() == true);

        // Check for expired states and remove them
        Action stateExpirations = StateManager.Instance.CheckForStateExpirationsOnCombatStart();
        yield return new WaitUntil(() => stateExpirations.ActionResolved() == true);

        // Start activations / combat start events
        ActivationManager.Instance.OnNewCombatEventStarted();

        // declare this event complete
        action.actionResolved = true;
    }
    public Action StartNewBossEncounterEvent(EnemyWaveSO enemyWave = null)
    {
        Action action = new Action();
        StartCoroutine(StartNewBossEncounterEventCoroutine(action, enemyWave));
        return action;
    }
    private IEnumerator StartNewBossEncounterEventCoroutine(Action action, EnemyWaveSO enemyWave = null)
    {
        // Destroy the previous level and tiles + reset values/properties, turn off unneeded views
        ClearPreviousEncounter();

        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = false;

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        StoryEventManager.Instance.DisableEventScreen();

        // Create a new level
        LevelManager.Instance.CreateLevel();

        // Reset Camera
        CameraManager.Instance.ResetCameraOnCombatStart();

        // Set up activation window holders
        ActivationManager.Instance.CreateSlotAndWindowHolders();

        // Create defender GO's        
        CharacterRoster.Instance.InstantiateDefenders();

        // Instantiate enemies
        EnemySpawner.Instance.SpawnEnemyWave("Boss", enemyWave);

        // disable unneed UI views
        UIManager.Instance.DisableUnneededCanvasesOnCombatStart();

        currentEncounterType = WorldEncounter.EncounterType.Boss;

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Apply Relevant State Effects
        Action stateApplications = StateManager.Instance.ApplyAllStateEffectsToLivingEntities();
        yield return new WaitUntil(() => stateApplications.ActionResolved() == true);

        // Check for expired states and remove them
        Action stateExpirations = StateManager.Instance.CheckForStateExpirationsOnCombatStart();
        yield return new WaitUntil(() => stateExpirations.ActionResolved() == true);

        // Start activations / combat start events
        ActivationManager.Instance.OnNewCombatEventStarted();

        // declare this event complete
        action.actionResolved = true;
    }
    #endregion

    // Start Shop + Mystery + Camp Site + Treasure Events
    #region
    public void StartNewCampSiteEncounterEvent()
    {
        StartCoroutine(StartNewCampSiteEncounterEventCoroutine());
    }
    private IEnumerator StartNewCampSiteEncounterEventCoroutine()
    {
        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);
        // turn off hexagon highlights
        WorldManager.Instance.IdleAllEncounters();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        CampSiteManager.Instance.EnableCampSiteView();
        // disable world map/char roster/inventory view to avoid clutter
        UIManager.Instance.DisableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
        CampSiteManager.Instance.ResetEventProperties();
        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Check relaxed state
        if (StateManager.Instance.DoesPlayerAlreadyHaveState("Relaxed"))
        {
            // Reward xp
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyCurrentXP(30);
            }

            // Create status VFX
            foreach(CampSiteCharacter character in CampSiteManager.Instance.allCharacterSlots)
            {
                VisualEffectManager.Instance.CreateStatusEffectOnCampSiteCharacter(character.transform.position, "Relaxed!", VisualEffectManager.Instance.campsiteVfxSortingLayer);
            }
        }

    }
    public Action StartShopEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartShopEncounterEventCoroutine(action));
        return action;
    }
    private IEnumerator StartShopEncounterEventCoroutine(Action action)
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = true;

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // turn off hexagon highlights
        WorldManager.Instance.IdleAllEncounters();

        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();

        // Build shop elements
        ShopScreenManager.Instance.EnableShopScreenView();
        ShopScreenManager.Instance.LoadShopScreenEntities();

        // disable world map/char roster/inventory view to avoid clutter
        UIManager.Instance.DisableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Check Local Heroes State
        if(StateManager.Instance.DoesPlayerAlreadyHaveState("Local Heroes"))
        {
            foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyCurrentHealth(30);
            }
        }

        // Resolve
        action.actionResolved = true;
    }
    public Action StartNewTreasureRoomEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewTreasureRoomEncounterEventCoroutine(action));
        return action;
    }
    private IEnumerator StartNewTreasureRoomEncounterEventCoroutine(Action action)
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = true;

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();

        // Enable view
        TreasureRoomManager.Instance.EnableTreasureRoomView();

        // Instantiate treasure chest 
        TreasureRoomManager.Instance.CreateNewTreasureChest();

        // disable world map view
        UIManager.Instance.DisableWorldMapView();        

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
        action.actionResolved = true;

    }
    public Action StartNewStoryEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewStoryEventCoroutine(action));
        return action;

    }
    private IEnumerator StartNewStoryEventCoroutine(Action action)
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = false;

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // Disable Map + Character Roster
        UIManager.Instance.DisableWorldMapView();
        UIManager.Instance.DisableCharacterRosterView();

        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();

        // Load a random event + set up story event screen
        StoryEventManager.Instance.LoadNewStoryEvent();

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // declare this event complete
        action.actionResolved = true;
    }
    public Action StartNewMysteryEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewMysteryEncounterEventCoroutine(action));
        return action;
    }
    private IEnumerator StartNewMysteryEncounterEventCoroutine(Action action)
    {
        Debug.Log("EventManager.StartNewMysteryEncounterEventCoroutine() called...");

        // Randomly decide encounter type
        int randomNumber = Random.Range(1, 101);
        Debug.Log("EventManager.StartNewMysteryEncounterEventCoroutine() rolled " +
            randomNumber.ToString() + " for mystery event roll");

        // check 'Daring Explorer' state
        if(StateManager.Instance.DoesPlayerAlreadyHaveState("Daring Explorers"))
        {
            PlayerDataManager.Instance.ModifyGold(5);
        }

        // check testing mode
        if (StoryEventManager.Instance.onlyRunTestStoryEvent)
        {
            StartNewStoryEvent();
        }

        // Story Event
        else if (randomNumber >= 1 && randomNumber <= 60)
        {
            if (StoryEventManager.Instance.viableStoryEvents.Count > 0)
            {
                Debug.Log("EventManager.StartNewMysteryEncounterEventCoroutine() rolled a Story event...");
                StartNewStoryEvent();
            }
            else
            {
                Debug.Log("EventManager.StartNewMysteryEncounterEventCoroutine() rolled a story event, " +
                    "but none are valid: Start basic enemy encounter instead");
                StartNewBasicEncounterEvent();
            }
        }

        // Treasure Event
        else if (randomNumber >= 61 && randomNumber <= 70)
        {
            Debug.Log("EventManager.StartNewMysteryEncounterEventCoroutine() rolled a Treasure event...");
            StartNewTreasureRoomEncounterEvent();
        }

        // Shop Event
        else if (randomNumber >= 71 && randomNumber <= 80)
        {
            Debug.Log("EventManager.StartNewMysteryEncounterEventCoroutine() rolled a Shop event...");
            StartShopEncounterEvent();
        }

        // Basic Enemy Event
        else if (randomNumber >= 81 && randomNumber <= 90)
        {
            Debug.Log("EventManager.StartNewMysteryEncounterEventCoroutine() rolled a Basic Enemy event...");
            StartNewBasicEncounterEvent();
        }

        // Elite Enemy Event
        else if (randomNumber >= 91 && randomNumber <= 100)
        {
            Debug.Log("EventManager.StartNewMysteryEncounterEventCoroutine() rolled an Elite Enemy event...");
            StartNewEliteEncounterEvent();
        }

        // Resolve  
        yield return null;
        action.actionResolved = true;


    }
    #endregion  

    // End Encounter Events
    #region
    public void StartNewEndBasicEncounterEvent()
    {
        StartCoroutine(StartNewEndBasicEncounterEventCoroutine());
    }
    private IEnumerator StartNewEndBasicEncounterEventCoroutine()
    {        
        Debug.Log("StartNewEndBasicEncounterEvent() coroutine started...");

        // Modify player score
        ScoreManager.Instance.HandlePostCombatScoring();
        // Destroy windows
        ActivationManager.Instance.ClearAllWindowsFromActivationPanel();
        // Show combat end visual events before loot reward screen appears
        preLootScreenEventFinished = false;
        // Disable end turn button
        UIManager.Instance.DisableEndTurnButtonView();
        // Unselect defender to hide ability bar UI, prevent null behaviors
        DefenderManager.Instance.ClearSelectedDefender();
        // Hide ability info panel
        // Show xp rewards + level ups
        Action lootEvent = StartPreLootScreenVisualEvent(25);
        yield return new WaitUntil(() => lootEvent.ActionResolved() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(25);
        //SpellInfoBox.Instance.HideInfoBox();       
        // re enable world map + get next viable enocunter hexagon tiles
        WorldManager.Instance.SetWorldMapReadyState();
        // Start loot creation/display process
        StartNewLootRewardEvent(WorldEncounter.EncounterType.BasicEnemy);
    }
    public Action StartNewEndEliteEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewEndEliteEncounterEventCoroutine(action));
        return action;
    }
    private IEnumerator StartNewEndEliteEncounterEventCoroutine(Action action)
    {
        Debug.Log("StartNewEndEliteEncounterEvent() coroutine started...");

        // Modify player score
        ScoreManager.Instance.HandlePostCombatScoring();
        // Destroy windows
        ActivationManager.Instance.ClearAllWindowsFromActivationPanel();
        // Show combat end visual events before loot reward screen appears
        preLootScreenEventFinished = false;
        // Disable end turn button
        UIManager.Instance.DisableEndTurnButtonView();
        // Unselect defender to hide ability bar UI, prevent null behaviors
        DefenderManager.Instance.ClearSelectedDefender();
        // Show xp rewards + level ups
        Action lootEvent = StartPreLootScreenVisualEvent(50);
        yield return new WaitUntil(() => lootEvent.ActionResolved() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(50);
        // re enable world map + get next viable enocunter hexagon tiles
        WorldManager.Instance.SetWorldMapReadyState();
        // Start loot creation/display process
        StartNewLootRewardEvent(WorldEncounter.EncounterType.EliteEnemy);
        // Resolve
        action.actionResolved = true;

    }
    public void StartNewEndBossEncounterEvent()
    {
        StartCoroutine(StartNewEndBossEncounterEventCoroutine());
    }
    private IEnumerator StartNewEndBossEncounterEventCoroutine()
    {
        // Modify player score
        ScoreManager.Instance.HandlePostCombatScoring();
        // Destroy windows
        ActivationManager.Instance.ClearAllWindowsFromActivationPanel();
        // Show combat end visual events before loot reward screen appears
        preLootScreenEventFinished = false;
        // Disable end turn button
        UIManager.Instance.DisableEndTurnButtonView();
        // Unselect defender to hide ability bar UI, prevent null behaviors
        DefenderManager.Instance.ClearSelectedDefender();
        // Show xp rewards + level ups
        Action lootEvent = StartPreLootScreenVisualEvent(50);
        yield return new WaitUntil(() => lootEvent.ActionResolved() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(50);
        // Start loot creation/display process
        StartNewLootRewardEvent(WorldEncounter.EncounterType.Boss);
    }
    public void StartNewGameOverDefeatedEvent()
    {
        StartCoroutine(StartNewGameOverDefeatedEventCoroutine());
    }
    private IEnumerator StartNewGameOverDefeatedEventCoroutine()
    {
        Debug.Log("StartNewGameOverDefeatedEventCoroutine() coroutine started...");
        gameOverEventStarted = true;

        // Modify player score
        ScoreManager.Instance.HandlePostCombatScoring();

        // Unselect defender to hide ability bar UI, prevent null behaviors
        DefenderManager.Instance.ClearSelectedDefender();

        // Stop all coroutines
        LivingEntityManager.Instance.StopAllEntityCoroutines();
        CombatLogic.Instance.StopAllCoroutines();
        MovementLogic.Instance.StopAllCoroutines();
        ActivationManager.Instance.StopAllCoroutines();

        // Disable end turn button + UI Changes
        UIManager.Instance.DisableEndTurnButtonView();
        UIManager.Instance.GameOverScreenTitleText.text = "Defeat!";

        // Fade In 'Game Over' screen
        Action fadeAction = UIManager.Instance.FadeInGameOverScreen();
        yield return new WaitUntil(() => fadeAction.ActionResolved() == true);

        Action scoreReveal = ScoreManager.Instance.CalculateFinalScore();
        yield return new WaitUntil(() => scoreReveal.ActionResolved() == true);

        // TO DO: score board visual event and calculations occur as a coroutine here


    }
    public void StartNewGameOverVictoryEvent()
    {
        StartCoroutine(StartNewGameOverVictoryEventCoroutine());
    }
    private IEnumerator StartNewGameOverVictoryEventCoroutine()
    {
        Debug.Log("StartNewGameOverDefeatedEventCoroutine() coroutine started...");
        gameOverEventStarted = true;

        // Stop all coroutines
        LivingEntityManager.Instance.StopAllEntityCoroutines();
        CombatLogic.Instance.StopAllCoroutines();
        MovementLogic.Instance.StopAllCoroutines();
        ActivationManager.Instance.StopAllCoroutines();

        // Disable end turn button + UI changes
        UIManager.Instance.DisableEndTurnButtonView();
        UIManager.Instance.GameOverScreenTitleText.text = "Victory!";

        // Fade In 'Game Over' screen
        Action fadeAction = UIManager.Instance.FadeInGameOverScreen();
        yield return new WaitUntil(() => fadeAction.ActionResolved() == true);

        Action scoreReveal = ScoreManager.Instance.CalculateFinalScore();
        yield return new WaitUntil(() => scoreReveal.ActionResolved() == true);

    }

    #endregion

    // Loot Event Logic
    #region
    public Action StartPreLootScreenVisualEvent(int xpReward)
    {
        Action action = new Action();
        StartCoroutine(StartPreLootScreenVisualEventCoroutine(xpReward, action));
        return action;
    }
    private IEnumerator StartPreLootScreenVisualEventCoroutine(int xpReward, Action action)
    {
        Debug.Log("StartPreLootScreenVisualEvent() coroutine started...");
        preLootScreenEventFinished = false;

        if (StateManager.Instance.DoesPlayerAlreadyHaveState("Fast Learners"))
        {
            xpReward += (int)(xpReward * 0.3f);
        }

        // disable activation panel view
        ActivationManager.Instance.SetActivationWindowViewState(false);

        // short yield for seconds to smoothen the transistion
        yield return new WaitForSeconds(1f);

        foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            Debug.Log("StartPreLootScreenVisualEvent() creating visual status xp gained effect...");
            // Dead characters get no XP
            if (character.currentHealth > 0 && character.myDefenderGO != null)
            {
                VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "XP + " + xpReward.ToString());
            }
        }

        yield return new WaitForSeconds(1f);

        foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            if (character.currentXP + xpReward >= character.currentMaxXP)
            {
                Debug.Log("StartPreLootScreenVisualEvent() creating visual status LEVEL GAINED! effect...");
                VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "LEVEL UP!");

            }
        }
        yield return new WaitForSeconds(1f);

        action.actionResolved = true;
    }
    public bool preLootScreenEventFinished;
    public bool PreLootScreenEventFinished()
    {
        if (preLootScreenEventFinished == true)
        {
            Debug.Log("PreLootScreenEventFinished() bool returning true...");
            return true;
        }
        else
        {
            return false;
        }
    }
    public void EndNewLootRewardEvent()
    {
        Debug.Log("EventManager.EndNewLootRewardEvent() called...");

        // continue normally if not fighting a boss at the end of an act
        if (currentEncounterType != WorldEncounter.EncounterType.Boss)
        {
            RewardScreen.Instance.ClearRewards();
            UIManager.Instance.DisableRewardScreenView();
            UIManager.Instance.EnableWorldMapView();
            WorldManager.Instance.HighlightNextAvailableEncounters();
        }

        // start load new act sequence after looting boss awards
        else if(currentEncounterType == WorldEncounter.EncounterType.Boss)
        {
            Debug.Log("EventManager.EndNewLootRewardEvent() detected a boss loot event ending");

            if (actTransitionInProcess == false)
            {
                if (WorldManager.Instance.currentAct == 1)
                {
                    actTransitionInProcess = true;
                    StartActTwoLoadSequence();
                }
            }           
        }

    }
    public void StartNewLootRewardEvent(WorldEncounter.EncounterType encounterType)
    {
        Debug.Log("EventManager.StartNewLootRewardEvent() called, encounter type = " + encounterType.ToString());

        if (encounterType == WorldEncounter.EncounterType.BasicEnemy)
        {
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateCommonItemRewardButton();
            RewardScreen.Instance.PopulateItemScreen(WorldEncounter.EncounterType.BasicEnemy);

            // 50% chance to get a consumable from basic encounters
            int consumableRoll = Random.Range(1, 101);
            if (consumableRoll > 50)
            {
                RewardScreen.Instance.CreateConsumableRewardButton();
            }
        }

        else if (encounterType == WorldEncounter.EncounterType.EliteEnemy)
        {
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateCommonItemRewardButton();
            RewardScreen.Instance.CreateRareItemRewardButton();
            RewardScreen.Instance.CreateConsumableRewardButton();
            RewardScreen.Instance.CreateStateRewardButton();

            RewardScreen.Instance.PopulateStateRewardScreen(true, true, false);
            RewardScreen.Instance.PopulateItemScreen(WorldEncounter.EncounterType.EliteEnemy);
        }

        else if (encounterType == WorldEncounter.EncounterType.Boss)
        {
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateCommonItemRewardButton();
            RewardScreen.Instance.CreateEpicItemRewardButton();
            RewardScreen.Instance.CreateConsumableRewardButton();
            RewardScreen.Instance.CreateStateRewardButton();

            RewardScreen.Instance.PopulateStateRewardScreen(false, false, true);
            RewardScreen.Instance.PopulateItemScreen(WorldEncounter.EncounterType.Boss);
        }

        else if (encounterType == WorldEncounter.EncounterType.Treasure)
        {
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateStateRewardButton();
            RewardScreen.Instance.PopulateStateRewardScreen(true, true, false);

        }

    }
    #endregion

    // Misc Logic
    #region
    public void ClearPreviousEncounter()
    {
        Debug.Log("EventManager.ClearPreviousEncounter() called...");

        ResetEncounterProperties();
        // Destroy the level data + all tile Go's
        LevelManager.Instance.DestroyCurrentLevel();
        // Destroy defender GO's from previous encounter
        DefenderManager.Instance.DestroyAllDefenders();
        // Destroy all activation windows
        ActivationManager.Instance.ClearAllWindowsFromActivationPanel();
        // Hide rest site view screen if open
        CampSiteManager.Instance.DisableCampSiteView();
        // Hide Shop view screen if open
        ShopScreenManager.Instance.DisableShopScreenView();
        // Destroy active treasure chest if it exists
        StoryEventManager.Instance.ResetStoryEventWindow();
        // Clear treasure room event
        TreasureRoomManager.Instance.DestroyActiveTreasureChest();
        TreasureRoomManager.Instance.DisableTreasureRoomView();
        // Force resolve unresolved actions and flush queue
        ActionManager.Instance.FlushActionQueue();
    }   
    public void ResetEncounterProperties()
    {
        Debug.Log("EventManager.ResetEncounterProperties() called...");

        damageTakenThisEncounter = false;
        currentCombatEndEventTriggered = false;
    }
    #endregion

    // Start + End of Act Transitions and Logic
    #region
    public Action StartActTwoLoadSequence()
    {
        Action action = new Action();
        StartCoroutine(StartActTwoLoadSequenceCoroutine(action));
        return action;
    }
    private IEnumerator StartActTwoLoadSequenceCoroutine(Action action)
    {
        Debug.Log("EventManager.StartActTwoLoadSequenceCoroutine() called...");

        // Destroy the previous level and tiles + reset values/properties, turn off unneeded views
        ClearPreviousEncounter();

        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = false;

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 3, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // Reset reward screen
        RewardScreen.Instance.ClearRewards();
        UIManager.Instance.DisableRewardScreenView();

        // Build new world
        WorldManager.Instance.OnActTwoStarted();

        // Heal all characters to full hp
        foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            character.ModifyCurrentHealth(character.maxHealth);
        }

        // Clear current encounter type
        currentEncounterType = WorldEncounter.EncounterType.NoType;

        // Enable world map view
        UIManager.Instance.EnableWorldMapView();

        // Enable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = true;

        // Refresh act transition bool
        actTransitionInProcess = false;

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 3, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        action.actionResolved = true;      
    }

    #endregion



}
