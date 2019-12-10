using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Properties")]
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
    public IEnumerator StartNewBasicEncounterEventCoroutine(Action action, EnemyWaveSO enemyWave = null)
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
    public void StartNewRestSiteEncounterEvent()
    {
        StartCoroutine(StartNewRestSiteEncounterEventCoroutine());
    }
    public IEnumerator StartNewRestSiteEncounterEventCoroutine()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        //WorldMap.Instance.canSelectNewEncounter = false;
        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);
        // turn off hexagon highlights
        WorldManager.Instance.UnhighlightAllHexagons();
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
        WorldManager.Instance.UnhighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        ShopScreenManager.Instance.EnableShopScreenView();
        ShopScreenManager.Instance.LoadShopScreenEntities();
        // disable world map/char roster/inventory view to avoid clutter
        UIManager.Instance.DisableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
        //CampSiteManager.Instance.ResetEventProperties();
        action.actionResolved = true;
    }
    public void StartNewTreasureRoomEncounterEvent()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = true;
        // turn off hexagon highlights
        //WorldMap.Instance.UnHighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        // Create a new level
        LevelManager.Instance.CreateLevel();
        //EnemySpawner.Instance.PopulateEnemySpawnLocations();        
        CharacterRoster.Instance.InstantiateDefenders();        
        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        // Instantiate treasure chest and populate it with loot
        CreateNewTreasureChest();
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

        // turn off hexagon highlights
        //WorldMap.Instance.UnHighlightAllHexagons();

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();

        // Load a random event + set up story event screen
        StoryEventManager.Instance.LoadNewStoryEvent();

        /* LOGIC FOR GENREATING A NEW STORY EVENT GOES HERE
         * 
         * 
         */

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
    public void StartNewLootRewardEvent(bool basicEnemy = true)
    {
        Debug.Log("StartNewLootRewardEvent() called...");

        if (basicEnemy == true)
        {
            //BlackScreenManager.Instance.FadeOut(5, .5f,true);
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateItemRewardButton();
            RewardScreen.Instance.PopulateItemScreen();
        }
        else if (basicEnemy == false)
        {
            //BlackScreenManager.Instance.FadeOut(5, .5f);
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateItemRewardButton();
            RewardScreen.Instance.CreateArtifactRewardButton();
            RewardScreen.Instance.PopulateItemScreen();
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
        ActivationManager.Instance.ClearAllWindowsFromActivationPanel();
        Debug.Log("StartNewEndBasicEncounterEvent() coroutine started...");
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
        StartNewLootRewardEvent();
        yield return null;
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
        StartNewLootRewardEvent(false);        
        action.actionResolved = true;
        
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
        // THIS COROUTINE DOES NOT ACTUALLY MODIFY XP/LEVEL!!!! only displays visual info

        // disable activation panel view
        ActivationManager.Instance.SetActivationWindowViewState(false);

        // short yield for seconds to smoothen the transistion
        yield return new WaitForSeconds(1f);

        foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            Debug.Log("StartPreLootScreenVisualEvent() creating visual status xp gained effect...");
            // Dead characters get no XP
            if(character.CurrentHealth > 0)
            {
                StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "XP + " + xpReward.ToString(), true));
            }            
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            if(character.currentXP + xpReward >= character.currentMaxXP)
            {
                Debug.Log("StartPreLootScreenVisualEvent() creating visual status LEVEL GAINED! effect...");
                StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "LEVEL UP!" + xpReward.ToString(), true));
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
        RewardScreen.Instance.CreateArtifactRewardButton();
    }
    #endregion






}
