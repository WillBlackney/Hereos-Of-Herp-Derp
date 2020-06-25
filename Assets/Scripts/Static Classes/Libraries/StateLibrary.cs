using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLibrary : MonoBehaviour
{
    // Singleton Pattern
    #region
    public static StateLibrary Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Properties + Component References
    #region
    [Header("Properties")]
    public List<StateDataSO> allStates;
    #endregion

    // Get States + Data Logic
    #region
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
    public StateDataSO GetRandomCommonState(bool onlyGetViable = true)
    {
        Debug.Log("StateLibrary.GetRandomCommonState() called...");

        StateDataSO stateReturned = null;
        List<StateDataSO> commonStates = new List<StateDataSO>();

        foreach (StateDataSO state in allStates)
        {
            if (state.rarity == StateDataSO.Rarity.Common)
            {
                if (onlyGetViable)
                {
                    if (!StateManager.Instance.DoesPlayerAlreadyHaveState(state.stateName))
                    {
                        commonStates.Add(state);
                    }
                }
                else
                {
                    commonStates.Add(state);
                }
            }
        }

        stateReturned = commonStates[Random.Range(0, commonStates.Count)];
        Debug.Log("StateLibrary.GetRandomCommonState() returning " + stateReturned.stateName);
        return stateReturned;
    }
    public StateDataSO GetRandomRareState(bool onlyGetViable = true)
    {
        Debug.Log("StateLibrary.GetRandomRareState() called...");

        StateDataSO stateReturned = null;
        List<StateDataSO> rareStates = new List<StateDataSO>();

        foreach (StateDataSO state in allStates)
        {
            if (state.rarity == StateDataSO.Rarity.Rare)
            {
                if (onlyGetViable)
                {
                    if (!StateManager.Instance.DoesPlayerAlreadyHaveState(state.stateName))
                    {
                        rareStates.Add(state);
                    }
                }
                else
                {
                    rareStates.Add(state);
                }
            }
        }

        stateReturned = rareStates[Random.Range(0, rareStates.Count)];
        Debug.Log("StateLibrary.GetRandomRareState() returning " + stateReturned.stateName);
        return stateReturned;
    }
    public StateDataSO GetRandomBossState(bool onlyGetViable = true)
    {
        Debug.Log("StateLibrary.GetRandomBossState() called...");

        StateDataSO stateReturned = null;
        List<StateDataSO> bossStates = new List<StateDataSO>();

        foreach (StateDataSO state in allStates)
        {
            if (state.rarity == StateDataSO.Rarity.Boss)
            {
                if (onlyGetViable)
                {
                    if (!StateManager.Instance.DoesPlayerAlreadyHaveState(state.stateName))
                    {
                        bossStates.Add(state);
                    }
                }
                else
                {
                    bossStates.Add(state);
                }
            }
        }

        stateReturned = bossStates[Random.Range(0, bossStates.Count)];
        Debug.Log("StateLibrary.GetRandomBossState() returning " + stateReturned.stateName);
        return stateReturned;
    }
    public List<StateDataSO> GetAllCommonStates(bool onlyGetViable = true)
    {
        List<StateDataSO> dataReturned = new List<StateDataSO>();

        foreach(StateDataSO state in allStates)
        {
            if(state.rarity == StateDataSO.Rarity.Common)
            {
                if (onlyGetViable)
                {
                    if (!StateManager.Instance.DoesPlayerAlreadyHaveState(state.stateName))
                    {
                        dataReturned.Add(state);
                    }
                }
                else
                {
                    dataReturned.Add(state);
                }
                
            }
        }

        return dataReturned;
    }
    public List<StateDataSO> GetAllRareStates(bool onlyGetViable = true)
    {
        List<StateDataSO> dataReturned = new List<StateDataSO>();

        foreach (StateDataSO state in allStates)
        {
            if (state.rarity == StateDataSO.Rarity.Rare)
            {
                if (onlyGetViable)
                {
                    if (!StateManager.Instance.DoesPlayerAlreadyHaveState(state.stateName))
                    {
                        dataReturned.Add(state);
                    }
                }
                else
                {
                    dataReturned.Add(state);
                }
            }
        }

        return dataReturned;
    }
    public List<StateDataSO> GetAllBossStates(bool onlyGetViable = true)
    {
        List<StateDataSO> dataReturned = new List<StateDataSO>();

        foreach (StateDataSO state in allStates)
        {
            if (state.rarity == StateDataSO.Rarity.Boss)
            {
                if (onlyGetViable)
                {
                    if (!StateManager.Instance.DoesPlayerAlreadyHaveState(state.stateName))
                    {
                        dataReturned.Add(state);
                    }
                }

                else
                {
                    dataReturned.Add(state);
                }
            }
        }

        return dataReturned;
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
    #endregion
}
