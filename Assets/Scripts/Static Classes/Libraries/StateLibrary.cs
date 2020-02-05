using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLibrary : Singleton<StateLibrary>
{
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
    public StateDataSO GetRandomStateReward()
    {       
        Debug.Log("StateLibrary.GetRandomState() called...");

        StateDataSO stateReturned = null;
        List<StateDataSO> viableStates = new List<StateDataSO>();

        foreach(StateDataSO data in allStates)
        {
            // TO DO: add code to prevent this from retrieving a state reward that the player already has
            if (!data.affliction && !StateManager.Instance.DoesPlayerAlreadyHaveState(data.stateName))
            {
                viableStates.Add(data);
            }
        }

        stateReturned = viableStates[Random.Range(0, viableStates.Count)];

        Debug.Log("StateLibrary.GetRandomState() returning: " + stateReturned.stateName);
        return stateReturned;

    }
}
