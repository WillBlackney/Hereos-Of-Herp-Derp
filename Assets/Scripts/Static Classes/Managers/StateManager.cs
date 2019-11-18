using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : Singleton<StateManager>
{
    public List<State> activeStates;
    public GameObject statePanel;

    public void GainState(StateDataSO stateGained)
    {
        Debug.Log("StateManager.GainState() called, gaining state: " + stateGained.Name);
        // Create State object out of prefab and parent it to the grid view panel
        GameObject newState = Instantiate(PrefabHolder.Instance.statePrefab, statePanel.transform);

        // Get the script component and run the setip
        State stateScript = newState.GetComponent<State>();
        stateScript.InitializeSetup(stateGained);

        // Add state to active state lists
        activeStates.Add(stateScript);
    }

    // Apply States to characters logic
    #region
    public Action ApplyAllStateEffectsToCharacters()
    {
        Debug.Log("StateManager.ApplyAllStateEffectsToCharacters() called...");
        Action action = new Action();
        StartCoroutine(ApplyAllStateEffectsToCharactersCoroutine(action));
        return action;

    }
    public IEnumerator ApplyAllStateEffectsToCharactersCoroutine(Action action)
    {        
        foreach(State state in activeStates)
        {
            Action stateApplication = ApplyStateEffect(state);
            yield return new WaitUntil(() => stateApplication.ActionResolved());
        }

        action.actionResolved = true;
    }
    public Action ApplyStateEffect(State stateApplied)
    {
        Action action = new Action();
        StartCoroutine(ApplyStateEffectCoroutine(stateApplied, action));
        return action;

    }
    public IEnumerator ApplyStateEffectCoroutine(State stateApplied, Action action)
    {
        if(stateApplied.Name == "Well Fed")
        {
            foreach(Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.ModifyCurrentStrength(1);
                Debug.Log("StateManager applying Well Fed...");
            }
            yield return new WaitForSeconds(1);
        }

        else if (stateApplied.Name == "Well Rested")
        {
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.ModifyCurrentEnergy(1);
                Debug.Log("StateManager applying Well Rested...");
            }
            yield return new WaitForSeconds(1);
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
    public IEnumerator CheckForStateExpirationsOnCombatStartCoroutine(Action action)
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
}
