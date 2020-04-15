using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLibrary : MonoBehaviour
{
    public static StateLibrary Instance;
    private void Awake()
    {
        Instance = this;
    }
    public List<StateDataSO> allStates;
    public StateDataSO GetStateByName(string name)
    {
        Debug.Log("StateLibrary.GetStateByName() called, searching for " + name + "...");

        StateDataSO stateReturned = null;
        foreach(StateDataSO state in allStates)
        {
            if(state.stateName == name)
            {
                stateReturned = state;
                break;
            }
        }

        if (stateReturned == null)
        {
            Debug.Log("StateLibrary.GetStateByName() could not find a state with the name '" + name + "', return null...");
        }

        return stateReturned;
    }
    public StateDataSO GetRandomCommonState()
    {
        Debug.Log("StateLibrary.GetRandomCommonState() called...");

        StateDataSO stateReturned = null;
        List<StateDataSO> commonStates = new List<StateDataSO>();

        foreach (StateDataSO state in allStates)
        {
            if (state.rarity == StateDataSO.Rarity.Common)
            {
                commonStates.Add(state);
            }
        }

        stateReturned = commonStates[Random.Range(0, commonStates.Count)];
        Debug.Log("StateLibrary.GetRandomCommonState() returning " + stateReturned.stateName);
        return stateReturned;
    }
    public StateDataSO GetRandomRareState()
    {
        Debug.Log("StateLibrary.GetRandomRareState() called...");

        StateDataSO stateReturned = null;
        List<StateDataSO> rareStates = new List<StateDataSO>();

        foreach (StateDataSO state in allStates)
        {
            if (state.rarity == StateDataSO.Rarity.Rare)
            {
                rareStates.Add(state);
            }
        }

        stateReturned = rareStates[Random.Range(0, rareStates.Count)];
        Debug.Log("StateLibrary.GetRandomRareState() returning " + stateReturned.stateName);
        return stateReturned;
    }
    public StateDataSO GetRandomBossState()
    {
        Debug.Log("StateLibrary.GetRandomBossState() called...");

        StateDataSO stateReturned = null;
        List<StateDataSO> bossStates = new List<StateDataSO>();

        foreach (StateDataSO state in allStates)
        {
            if (state.rarity == StateDataSO.Rarity.Boss)
            {
                bossStates.Add(state);
            }
        }

        stateReturned = bossStates[Random.Range(0, bossStates.Count)];
        Debug.Log("StateLibrary.GetRandomBossState() returning " + stateReturned.stateName);
        return stateReturned;
    }
    public StateDataSO GetRandomStateReward()
    {       
        Debug.Log("StateLibrary.GetRandomState() called...");

        StateDataSO stateReturned = null;
        List<StateDataSO> viableStates = new List<StateDataSO>();

        foreach(StateDataSO data in allStates)
        {
            if (!data.affliction && 
                !StateManager.Instance.DoesPlayerAlreadyHaveState(data.stateName) &&
                !data.eventReward)
            {
                viableStates.Add(data);
            }
        }

        stateReturned = viableStates[Random.Range(0, viableStates.Count)];

        Debug.Log("StateLibrary.GetRandomState() returning: " + stateReturned.stateName);
        return stateReturned;

    }
    public StateDataSO GetRandomAffliction()
    {
        Debug.Log("StateLibrary.GetRandomAfflication() called...");

        StateDataSO stateReturned = null;
        List<StateDataSO> viableStates = new List<StateDataSO>();

        foreach (StateDataSO data in allStates)
        {
            if (data.affliction)
            {
                viableStates.Add(data);
            }
        }

        stateReturned = viableStates[Random.Range(0, viableStates.Count)];

        Debug.Log("StateLibrary.GetRandomState() returning: " + stateReturned.stateName);
        return stateReturned;

    }
}
