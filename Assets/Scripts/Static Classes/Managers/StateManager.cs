using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;
    private void Awake()
    {       
        Instance = this;        
    }   


    // Properties + Component References
    #region
    [Header("Component References")]
    public GameObject statePanel;
    public GameObject afflicationPanelGridParent;
    public GameObject afflictionPanelCanvasParent;
    public GameObject afflicationPanelVisualParent;

    [Header("Properties")]
    public List<State> activeStates;
    public List<AfflictionOnPanel> afflicationPanelObjects;
    #endregion

    // Misc Logic
    #region
    public void GainState(StateDataSO stateGained)
    {
        Debug.Log("StateManager.GainState() called, gaining state: " + stateGained.stateName);

        // Modify Player Score
        ScoreManager.Instance.statesCollected++;

        // Create State object out of prefab and parent it to the grid view panel
        GameObject newState = Instantiate(PrefabHolder.Instance.statePrefab, statePanel.transform);

        // Get the script component and run the setip
        State stateScript = newState.GetComponent<State>();
        stateScript.InitializeSetup(stateGained);

        // Gain state effects on pick up (where relevant)
        ApplyStateEffectOnPickup(stateGained);

        // Add state to active state lists
        activeStates.Add(stateScript);
    }
    public bool DoesPlayerAlreadyHaveState(string stateName)
    {
        Debug.Log("StateManager.DoesPlayerAlreadyHaveState() called, checking for state: " + stateName);

        bool boolReturned = false;

        foreach(State state in activeStates)
        {
            if(state.myStateData.stateName == stateName)
            {
                Debug.Log("Player already has " + stateName + " as an active state");
                boolReturned = true;
                break;
            }
        }

        return boolReturned;
    }
    public State GetActiveStateByName(string stateName)
    {
        Debug.Log("StateManager.GetActiveStateByName() called, searching for " + stateName);

        State stateReturned = null;

        foreach(State state in activeStates)
        {
            if(state.Name == stateName)
            {
                stateReturned = state;
                break;
            }
        }

        if(stateReturned == null)
        {
            Debug.Log("StateManager.GetActiveStateByName() could not find an active state with the name " + name +
                ", returning null...");
        }

        return stateReturned;

    }
    #endregion

    // Apply States to characters logic
    #region
    public Action ApplyStateEffectOnPickup(StateDataSO data)
    {
        Action action = new Action();
        StartCoroutine(ApplyStateEffectOnPickupCoroutine(data, action));
        return action;
    }
    private IEnumerator ApplyStateEffectOnPickupCoroutine(StateDataSO data, Action action)
    {
        if(data.stateName == "Vengeful")
        {
            foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStrength(2);
            }
        }
        else if (data.stateName == "Educated")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyWisdom(2);
            }
        }
        else if (data.stateName == "Tough")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyDexterity(2);
            }
        }
        else if (data.stateName == "Well Rounded")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStrength(1);
                character.ModifyWisdom(1);
                character.ModifyDexterity(1);
            }
        }
        else if (data.stateName == "Heroism")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStrength(1);
                character.ModifyWisdom(1);
                character.ModifyDexterity(1);
            }
        }
        else if (data.stateName == "Polished Armour")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyUnwavering(1);
            }
        }
        else if (data.stateName == "Pumped Up")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStamina(5);
                character.ModifyMaxEnergy(5);
            }
        }
        else if (data.stateName == "Vampirism")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyLifeSteal(1);
            }
        }

        // Camp site related
        else if (data.stateName == "Resourceful")
        {
            CampSiteManager.Instance.maxActionPoints++;
        }
        else if (data.stateName == "Treasure Hunters")
        {
            CampSiteManager.Instance.EnableDigActionView();
        }
        else if (data.stateName == "Studious")
        {
            CampSiteManager.Instance.EnableReadActionView();
        }


        // Afflictions
        else if (data.stateName == "Curse Of The Blood God")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyFading(2);
            }
        }
        else if (data.stateName == "Exhausted")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStamina(-5);
                character.ModifyMobility(-1);
            }
        }
        else if (data.stateName == "Shame")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStrength(-1);
                character.ModifyWisdom(-1);
                character.ModifyDexterity(-1);
            }
        }

        // boss states
        else if (data.stateName == "Combat Mastery")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStamina(10);
                character.ModifyMaxEnergy(10);
            }
        }
        else if (data.stateName == "Trauma Savant")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStamina(10);
                character.ModifyMaxEnergy(10);
            }
        }
        else if (data.stateName == "Oath Of Honour")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStamina(10);
                character.ModifyMaxEnergy(10);
            }
        }
        else if (data.stateName == "Awesomeness")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStamina(5);
                character.ModifyMaxEnergy(5);
            }
        }
        else if (data.stateName == "Orcish Tendencies")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyStrength(5);
            }
        }
        else if (data.stateName == "Thick Skinned")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyMaxHealth(50);
            }
        }
        else if (data.stateName == "Lottery Winners")
        {
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomEpicItem());
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomEpicItem());
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomEpicItem());
        }


        yield return null;
    }
    public Action ApplyAllStateEffectsToLivingEntities()
    {
        Debug.Log("StateManager.ApplyAllStateEffectsToCharacters() called...");
        Action action = new Action();
        StartCoroutine(ApplyAllStateEffectsToLivingEntitiesCoroutine(action));
        return action;

    }
    private IEnumerator ApplyAllStateEffectsToLivingEntitiesCoroutine(Action action)
    {        
        foreach(State state in activeStates)
        {
            Action stateApplication = ApplyStateEffect(state);
            yield return new WaitUntil(() => stateApplication.ActionResolved());

            // brief pause between each state buff effect
            yield return new WaitForSeconds(1);
        }

        action.actionResolved = true;
    }
    public Action ApplyStateEffect(State stateApplied)
    {
        Action action = new Action();
        StartCoroutine(ApplyStateEffectCoroutine(stateApplied, action));
        return action;

    }
    private IEnumerator ApplyStateEffectCoroutine(State stateApplied, Action action)
    {
        Debug.Log("StateManager.ApplyStateEffectCoroutine() called, applying state: " + stateApplied.Name);
              
        
        if (stateApplied.Name == "Eager")
        {
            Debug.Log("StateManager applying Eager...");

            // Status VFX
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                VisualEffectManager.Instance.CreateStatusEffect(defender.transform.position, "Eager!");
            }
            yield return new WaitForSeconds(.5f);

            // Bonus Mobility
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyTemporaryMobility(1);
            }
            yield return new WaitForSeconds(.5f);

            // Bonus Initiative
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyTemporaryInitiative(1);
            }
            yield return new WaitForSeconds(.5f);
        }        
       
        else if (stateApplied.Name == "King's Decree")
        {
            Debug.Log("StateManager applying King's Decree...");

            // Lose 50% health
            foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
            {
                VisualEffectManager.Instance.CreateStatusEffect(enemy.transform.position, "King's Decree!");
                enemy.ModifyCurrentHealth(-(enemy.currentMaxHealth / 2));
            }
            yield return new WaitForSeconds(.5f);
        }

        else if (stateApplied.Name == "Contract Killers")
        {
            Debug.Log("StateManager applying Contract Killers...");

            // Lose 30% health
            foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
            {
                VisualEffectManager.Instance.CreateStatusEffect(enemy.transform.position, "Contract Killers!");
                enemy.ModifyCurrentHealth(-(int)(enemy.currentMaxHealth * 0.3f));
            }
            yield return new WaitForSeconds(.5f);
        }

        if (stateApplied.expirationCondition == StateDataSO.ExpirationCondition.Timer)
        {
            stateApplied.ModifyCountdown(-1);
        }
        
        action.actionResolved = true;        
    }
    public Action CheckForStateExpirationsOnCombatStart()
    {
        Debug.Log("CheckForStateExpirationsOnCombatStart() called");
        Action action = new Action();
        StartCoroutine(CheckForStateExpirationsOnCombatStartCoroutine(action));
        return action;

    }
    private IEnumerator CheckForStateExpirationsOnCombatStartCoroutine(Action action)
    {
        Debug.Log("CheckForStateExpirationsOnCombatStartCoroutine() called");
        
        // reverse loop, to destroy/GC state without performing invalid operation
        if(activeStates.Count > 0)
        {
            for (int currentIndex = activeStates.Count - 1; currentIndex >= 0; currentIndex--)
            {
                if (activeStates[currentIndex].expirationCondition == StateDataSO.ExpirationCondition.Timer &&
                    activeStates[currentIndex].currentDuration <= 0)
                {
                    activeStates[currentIndex].PlayExpireVfxAndDestroy();
                }
            }
        }

        yield return null;
        action.actionResolved = true;

    }
    #endregion

    // Afflication State Related
    #region
    public bool HasAtleastOneAfflicationState()
    {
        Debug.Log("StateManager.HasAtleastOneAfflicationState() called...");

        bool boolReturned = false;

        foreach(State state in activeStates)
        {
            if (state.affliction)
            {
                boolReturned = true;
                break;
            }
        }

        if(boolReturned == true)
        {
            Debug.Log("Player has at least one afflication state");
        }
        else if (boolReturned == false)
        {
            Debug.Log("Player DOES NOT have an afflication state");
        }

        return boolReturned;
    }
    public void CreateAfflicationOnPanel(State stateReference)
    {
        GameObject aop = Instantiate(PrefabHolder.Instance.afflicationOnPanelPrefab, afflicationPanelGridParent.transform);
        aop.GetComponent<AfflictionOnPanel>().InitializeSetup(stateReference);
    }
    public void PopulateAfflicationsPanel()
    {
        foreach(State state in activeStates)
        {
            if (state.affliction)
            {
                CreateAfflicationOnPanel(state);
            }
        }
    }
    public void ClearAfflicationsPanel()
    {
        foreach(AfflictionOnPanel afflicationPanel in afflicationPanelObjects)
        {
            Destroy(afflicationPanel.gameObject, 0.1f);
        }

        afflicationPanelObjects.Clear();
    }
    public void SetAfflicationPanelViewState(bool onOrOff)
    {
        afflictionPanelCanvasParent.SetActive(onOrOff);
        afflicationPanelVisualParent.SetActive(onOrOff);
    }
    #endregion
}
