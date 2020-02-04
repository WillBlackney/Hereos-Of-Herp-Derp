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
}
