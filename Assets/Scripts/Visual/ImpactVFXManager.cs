using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactVFXManager : Singleton<ImpactVFXManager>
{
    [Header("Component + Prefab References")]
    public GameObject impactPrefabOne;
    public List<Sprite> randomImpactSprites;
    public void CreateImpactVFX(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(impactPrefabOne, location, Quaternion.identity);
        newImpactVFX.GetComponent<ImpactVFX>().SetSprite(GetRandomImpactSprite());
        Destroy(newImpactVFX,1f);

    }
    public Sprite GetRandomImpactSprite()
    {
        return randomImpactSprites[Random.Range(0, randomImpactSprites.Count)];
    }
}
