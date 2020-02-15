using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : Singleton<StateManager>
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public GameObject statePanel;
    public GameObject afflicationPanelGridParent;
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

        // Create State object out of prefab and parent it to the grid view panel
        GameObject newState = Instantiate(PrefabHolder.Instance.statePrefab, statePanel.transform);

        // Get the script component and run the setip
        State stateScript = newState.GetComponent<State>();
        stateScript.InitializeSetup(stateGained);

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
    #endregion

    // Apply States to characters logic
    #region
    public Action ApplyAllStateEffectsToCharacters()
    {
        Debug.Log("StateManager.ApplyAllStateEffectsToCharacters() called...");
        Action action = new Action();
        StartCoroutine(ApplyAllStateEffectsToCharactersCoroutine(action));
        return action;

    }
    private IEnumerator ApplyAllStateEffectsToCharactersCoroutine(Action action)
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

        if(stateApplied.Name == "Well Fed")
        {
            Debug.Log("StateManager applying Well Fed...");

            // Bonus Strength
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusStrength(2);                
            }
            yield return new WaitForSeconds(.5f);

            // Bonus Dexterity
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusDexterity(2);
            }
            yield return new WaitForSeconds(.5f);

            // Bonus Wisdom
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusWisdom(2);
            }
            yield return new WaitForSeconds(.5f);
        }
        else if (stateApplied.Name == "Well Rested")
        {
            Debug.Log("StateManager applying Well Rested...");

            // Gain +10 Stamina
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusStamina(10);                
            }
            yield return new WaitForSeconds(.5f);

            // Gain +10 Max energy
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.currentMaxEnergy += 10;
                VisualEffectManager.Instance.CreateStatusEffect(defender.transform.position, "Max Energy + 10");
                StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
            }
            yield return new WaitForSeconds(.5f);
        }
        else if (stateApplied.Name == "Heroism")
        {
            Debug.Log("StateManager applying Heroism...");

            // Bonus Strength
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusStrength(1);
            }
            yield return new WaitForSeconds(.5f);

            // Bonus Dexterity
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusDexterity(1);
            }
            yield return new WaitForSeconds(.5f);

            // Bonus Wisdom
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusWisdom(1);
            }
            yield return new WaitForSeconds(.5f);
        }
        else if (stateApplied.Name == "Polished Armour")
        {
            Debug.Log("StateManager applying Polished Armour...");

            // Grant +5 block
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(5, defender));
            }

            yield return new WaitForSeconds(.5f);
        }
        else if (stateApplied.Name == "Vampirism")
        {
            Debug.Log("StateManager applying Vampirism...");

            // Grant Life Steal passive
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyLifeSteal(1);
            }

            yield return new WaitForSeconds(.5f);
        }
        else if (stateApplied.Name == "Pumped Up")
        {
            Debug.Log("StateManager applying Pumped Up...");

            // Gain +10 Stamina
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusStamina(10);
            }
            yield return new WaitForSeconds(.5f);

            // Gain +10 Max energy
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.currentMaxEnergy += 10;
                VisualEffectManager.Instance.CreateStatusEffect(defender.transform.position, "Max Energy + 10");
                StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
            }
            yield return new WaitForSeconds(.5f);
        }
        else if (stateApplied.Name == "Vengeful")
        {
            Debug.Log("StateManager applying Vengeful...");

            // Bonus Strength
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusStrength(4);
            }
            yield return new WaitForSeconds(.5f);

        }
        else if (stateApplied.Name == "Alert")
        {
            Debug.Log("StateManager applying Alert...");

            // Bonus Mobility
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyTemporaryMobility(2);
            }
            yield return new WaitForSeconds(.5f);

            // Bonus Initiative
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyTemporaryInitiative(2);
            }
            yield return new WaitForSeconds(.5f);
        }
        else if (stateApplied.Name == "Curiosity")
        {
            Debug.Log("StateManager applying Curiosity...");

            // Bonus Strength
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusStrength(2);
            }
            yield return new WaitForSeconds(.5f);

            // Bonus Dexterity
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusDexterity(2);
            }
            yield return new WaitForSeconds(.5f);

            // Bonus Wisdom
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusWisdom(2);
            }
            yield return new WaitForSeconds(.5f);
        }
        else if (stateApplied.Name == "Curse Of The Blood God")
        {
            Debug.Log("StateManager applying Curse Of The Blood God...");

            // Apply Fading
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyFading(2);
            }
            yield return new WaitForSeconds(.5f);

        }
        else if (stateApplied.Name == "Exhausted")
        {
            Debug.Log("StateManager applying Exhausted...");

            // Lose 10 Stamina
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusStamina(-10);
            }
            yield return new WaitForSeconds(.5f);

            // Lose 1 Mobility
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusMobility(-1);
            }
            yield return new WaitForSeconds(.5f);
        }
        else if (stateApplied.Name == "Shame")
        {
            Debug.Log("StateManager applying Shame...");

            // Lose Strength
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusStrength(-1);
            }
            yield return new WaitForSeconds(.5f);

            // Lose Dexterity
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusDexterity(-1);
            }
            yield return new WaitForSeconds(.5f);

            // Lose Wisdom
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusWisdom(-1);
            }
            yield return new WaitForSeconds(.5f);

            // Lose Mobility
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.ModifyCurrentMobility(-1);
            }
            yield return new WaitForSeconds(.5f);

            // Lose Initiative
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.myPassiveManager.ModifyBonusInitiative(-1);
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
        foreach (State state in activeStates)
        {
            if(state.expirationCondition == StateDataSO.ExpirationCondition.Timer &&
                state.currentDuration <= 0)
            {
                state.PlayExpireVfxAndDestroy();
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
        afflicationPanelVisualParent.SetActive(onOrOff);
    }
    #endregion
}
