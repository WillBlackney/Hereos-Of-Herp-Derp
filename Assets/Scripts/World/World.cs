using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public List<WorldEncounter> allEncounters;

    public void InitializeSetup()
    {
        foreach(WorldEncounter encounter in allEncounters)
        {
            encounter.InitializeSetup();
        }
    }
}
