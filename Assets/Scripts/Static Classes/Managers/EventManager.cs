﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Properties")]
    public bool gameOverEventStarted;
    public TreasureChest activeTreasureChest;
    public WorldEncounter.EncounterType currentEncounterType;

    // Start New Encounter Events
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

        // turn off hexagon highlights
        //WorldMap.Instance.UnHighlightAllHexagons();

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);
        
        StoryEventManager.Instance.DisableEventScreen();    

        // Create a new level
        LevelManager.Instance.CreateLevel();

        // Set up activation window holders
        ActivationManager.Instance.CreateSlotAndWindowHolders();

        // REMOVE AFTER TESTING 
        StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Polished Armour"));

        // Create defender GO's        
        CharacterRoster.Instance.InstantiateDefenders();  
        
        // Instantiate enemies
        EnemySpawner.Instance.SpawnEnemyWave("Basic", enemyWave);      

        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        currentEncounterType = WorldEncounter.EncounterType.BasicEnemy;

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Apply Relevant State Effects
        Action stateApplications = StateManager.Instance.ApplyAllStateEffectsToCharacters();
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
    public IEnumerator StartNewEliteEncounterEventCoroutine(Action action)
    {
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();

        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = false;

        // turn off hexagon highlights
        //WorldMap.Instance.UnHighlightAllHexagons();

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);        

        // Create a new level
        LevelManager.Instance.CreateLevel();

        // Set up activation window holders
        ActivationManager.Instance.CreateSlotAndWindowHolders();

        // Create defender GO's        
        CharacterRoster.Instance.InstantiateDefenders();

        // Instantiate enemies
        EnemySpawner.Instance.SpawnEnemyWave("Elite");        

        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        currentEncounterType = WorldEncounter.EncounterType.EliteEnemy;

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Apply Relevant State Effects
        Action stateApplications = StateManager.Instance.ApplyAllStateEffectsToCharacters();
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
    public IEnumerator StartNewBossEncounterEventCoroutine(Action action, EnemyWaveSO enemyWave = null)
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

        // Set up activation window holders
        ActivationManager.Instance.CreateSlotAndWindowHolders();

        // Create defender GO's        
        CharacterRoster.Instance.InstantiateDefenders();

        // Instantiate enemies
        EnemySpawner.Instance.SpawnEnemyWave("Boss", enemyWave);

        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        currentEncounterType = WorldEncounter.EncounterType.Boss;

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Apply Relevant State Effects
        Action stateApplications = StateManager.Instance.ApplyAllStateEffectsToCharacters();
        yield return new WaitUntil(() => stateApplications.ActionResolved() == true);

        // Check for expired states and remove them
        Action stateExpirations = StateManager.Instance.CheckForStateExpirationsOnCombatStart();
        yield return new WaitUntil(() => stateExpirations.ActionResolved() == true);

        // Start activations / combat start events
        ActivationManager.Instance.OnNewCombatEventStarted();

        // declare this event complete
        action.actionResolved = true;
    }
    public void StartNewCampSiteEncounterEvent()
    {
        StartCoroutine(StartNewCampSiteEncounterEventCoroutine());
    }
    private IEnumerator StartNewCampSiteEncounterEventCoroutine()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        //WorldMap.Instance.canSelectNewEncounter = false;
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

        // REMOVE IN FUTURE, FOR TESTING ONLY
        StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Exhausted"));
        StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Curse Of The Blood God"));
    }
    public Action StartShopEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartShopEncounterEventCoroutine(action));
        return action;
    }
    public IEnumerator StartShopEncounterEventCoroutine(Action action)
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
        ShopScreenManager.Instance.EnableShopScreenView();
        ShopScreenManager.Instance.LoadShopScreenEntities();
        // disable world map/char roster/inventory view to avoid clutter
        UIManager.Instance.DisableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
        
        //CampSiteManager.Instance.ResetEventProperties();
        action.actionResolved = true;
    }
    public Action StartNewTreasureRoomEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewTreasureRoomEncounterEventCoroutine(action));
        return action;
    }
    public IEnumerator StartNewTreasureRoomEncounterEventCoroutine(Action action)
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = true;

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();

        // Create a new level
        LevelManager.Instance.CreateLevel();

        // Set up activation window holders
        ActivationManager.Instance.CreateSlotAndWindowHolders();     
        CharacterRoster.Instance.InstantiateDefenders();

        // disable world map view
        UIManager.Instance.DisableWorldMapView();

        // Instantiate treasure chest and populate it with loot
        CreateNewTreasureChest();

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
    public IEnumerator StartNewStoryEventCoroutine(Action action)
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = false;

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

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
    public IEnumerator StartNewMysteryEncounterEventCoroutine(Action action)
    {
        int randomNumber = Random.Range(1, 101);

        if (randomNumber >= 1 && randomNumber <= 100)
        {
            if(StoryEventManager.Instance.viableStoryEvents.Count > 0)
            {
                StartNewStoryEvent();
            }
            else
            {
                StartNewBasicEncounterEvent();
            }
        }
        else if (randomNumber >= 61 && randomNumber <= 70)
        {
            StartNewTreasureRoomEncounterEvent();
        }
        else if (randomNumber >= 71 && randomNumber <= 90)
        {
            StartShopEncounterEvent();
        }
        else if (randomNumber >= 91 && randomNumber <= 100)
        {
            StartNewBasicEncounterEvent();
        }

        yield return null;
        action.actionResolved = true;

        
    }
    public void StartNewLootRewardEvent(WorldEncounter.EncounterType encounterType)
    {
        Debug.Log("StartNewLootRewardEvent() called...");

        if (encounterType == WorldEncounter.EncounterType.BasicEnemy)
        {
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateItemRewardButton();
            RewardScreen.Instance.PopulateItemScreen();
        }
        else if (encounterType == WorldEncounter.EncounterType.EliteEnemy)
        {
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateItemRewardButton();
            RewardScreen.Instance.PopulateItemScreen();
        }
        else if (encounterType == WorldEncounter.EncounterType.Treasure)
        {
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateStateRewardButton();
            RewardScreen.Instance.PopulateStateRewardScreen();
        }

    }
    #endregion

    // End Encounter Events
    #region
    public void StartNewEndBasicEncounterEvent()
    {
        StartCoroutine(StartNewEndBasicEncounterEventCoroutine());        
    }
    public IEnumerator StartNewEndBasicEncounterEventCoroutine()
    {        
        Debug.Log("StartNewEndBasicEncounterEvent() coroutine started...");
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
        Action lootEvent = StartPreLootScreenVisualEvent(20);
        yield return new WaitUntil(() => lootEvent.ActionResolved() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(20);      
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
    public IEnumerator StartNewEndEliteEncounterEventCoroutine(Action action)
    {
        Debug.Log("StartNewEndEliteEncounterEvent() coroutine started...");
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
        Action lootEvent = StartPreLootScreenVisualEvent(50);
        yield return new WaitUntil(() => lootEvent.ActionResolved() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(50);
//        SpellInfoBox.Instance.HideInfoBox();
        // re enable world map + get next viable enocunter hexagon tiles
        WorldManager.Instance.SetWorldMapReadyState();
        // Start loot creation/display process
        StartNewLootRewardEvent(WorldEncounter.EncounterType.EliteEnemy);        
        action.actionResolved = true;
        
    }
    public void StartNewEndBossEncounterEvent()
    {
        StartCoroutine(StartNewEndBossEncounterEventCoroutine());
    }
    public IEnumerator StartNewEndBossEncounterEventCoroutine()
    {
        Debug.Log("StartNewEndBossEncounterEvent() coroutine started...");
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
        Action lootEvent = StartPreLootScreenVisualEvent(100);
        yield return new WaitUntil(() => lootEvent.ActionResolved() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(100);
        StartNewGameOverVictoryEvent();
        yield return null;
    }
    public void StartNewGameOverDefeatedEvent()
    {
        StartCoroutine(StartNewGameOverDefeatedEventCoroutine());
    }
    public IEnumerator StartNewGameOverDefeatedEventCoroutine()
    {
        Debug.Log("StartNewGameOverDefeatedEventCoroutine() coroutine started...");
        gameOverEventStarted = true;
        // Destroy windows
        //ActivationManager.Instance.ClearAllWindowsFromActivationPanel();

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
    public IEnumerator StartNewGameOverVictoryEventCoroutine()
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
    public void EndNewLootRewardEvent()
    {
        RewardScreen.Instance.ClearRewards();
        UIManager.Instance.DisableRewardScreenView();
        UIManager.Instance.EnableWorldMapView();
        WorldManager.Instance.HighlightNextAvailableEncounters();
    }
    #endregion

    // Misc Events
    #region
    public Action StartPreLootScreenVisualEvent(int xpReward)
    {
        Action action = new Action();
        StartCoroutine(StartPreLootScreenVisualEventCoroutine(xpReward, action));
        return action;
    }
    public IEnumerator StartPreLootScreenVisualEventCoroutine(int xpReward, Action action)
    {
        Debug.Log("StartPreLootScreenVisualEvent() coroutine started...");
        preLootScreenEventFinished = false;

        if (StateManager.Instance.DoesPlayerAlreadyHaveState("Genius"))
        {
            xpReward += (int) (xpReward * 0.5f);
        }

        // disable activation panel view
        ActivationManager.Instance.SetActivationWindowViewState(false);

        // short yield for seconds to smoothen the transistion
        yield return new WaitForSeconds(1f);

        foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            Debug.Log("StartPreLootScreenVisualEvent() creating visual status xp gained effect...");
            // Dead characters get no XP
            if(character.currentHealth > 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "XP + " + xpReward.ToString());
            }            
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            if(character.currentXP + xpReward >= character.currentMaxXP)
            {
                Debug.Log("StartPreLootScreenVisualEvent() creating visual status LEVEL GAINED! effect...");
                VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "LEVEL UP!" + xpReward.ToString());
                yield return new WaitForSeconds(0.5f);

            }            
        }
        yield return new WaitForSeconds(1f);

        action.actionResolved = true;
    }
    public bool preLootScreenEventFinished;
    public bool PreLootScreenEventFinished()
    {
        if(preLootScreenEventFinished == true)
        {
            Debug.Log("PreLootScreenEventFinished() bool returning true...");
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Misc Logic
    #region
    public void ClearPreviousEncounter()
    {
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
        if(activeTreasureChest != null)
        {
            activeTreasureChest.DestroyChest();
        }
    }    
    public void CreateNewTreasureChest()
    {
        // create a treasure chest game object
        GameObject newTreasureChest = Instantiate(PrefabHolder.Instance.TreasureChest);
        newTreasureChest.GetComponent<TreasureChest>().InitializeSetup();
    }
    #endregion






}
