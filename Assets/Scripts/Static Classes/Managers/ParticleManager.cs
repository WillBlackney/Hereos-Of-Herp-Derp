using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject basicParticlePrefab;
    public GameObject portalParticlePrefab;

    public static ParticleManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void CreateParticleEffect(Vector3 location, GameObject particlePrefab)
    {
        Vector3 idealPos = new Vector3(location.x, location.y - 0.15f, location.z);        
        GameObject newParticle = Instantiate(particlePrefab, idealPos, Quaternion.identity);
        newParticle.transform.Rotate(270, 0, 0, Space.World);
        
    }
}
