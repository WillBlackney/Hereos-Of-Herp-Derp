using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInfoPrefabHolder : MonoBehaviour
{
    //public GameObject spellInfoPrefab;

    public static SpellInfoPrefabHolder Instance;

    private void Awake()
    {
        Instance = this;
    }
}
