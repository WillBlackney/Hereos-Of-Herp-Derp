using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactVFXManager : Singleton<ImpactVFXManager>
{
    [Header("Component + Prefab References")]
    public GameObject impactPrefabOne;
    public List<Sprite> randomImpactSprites;
   
}
