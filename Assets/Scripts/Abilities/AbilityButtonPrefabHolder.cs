using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonPrefabHolder : MonoBehaviour
{   
    //public GameObject AbilityButtonPrefab;    

    public static AbilityButtonPrefabHolder Instance;

    private void Awake()
    {
        Instance = this;
    }
}
