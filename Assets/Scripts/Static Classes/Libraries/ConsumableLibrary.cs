using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableLibrary : MonoBehaviour
{
    public List<ConsumableDataSO> allConsumables;
    public static ConsumableLibrary Instance;
    private void Awake()
    {
        Instance = this;
    }
    public ConsumableDataSO GetConsumableDataByName(string name)
    {
        Debug.Log("ConsumableLibrary.GetConsumableDataByName() called, searching for: " + name);

        ConsumableDataSO dataReturned = null;

        foreach(ConsumableDataSO data in allConsumables)
        {
            if(data.consumableName == name)
            {
                dataReturned = data;
                break;
            }
        }

        if(dataReturned == null)
        {
            Debug.Log("ConsumableLibrary.GetConsumableDataByName() could not find a consumable with the name '" +
                name + "', returning null...");
        }

        return dataReturned;
    }
    public ConsumableDataSO GetRandomConsumable()
    {
        Debug.Log("ConsumableLibrary.GetRandomConsumable() called...");

        return allConsumables[Random.Range(0, allConsumables.Count)];
    }
}
