using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallExplodeOnHit : MonoBehaviour
{

    public GameObject explosionWall;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnParticleCollision(GameObject other)
    {
        Instantiate(explosionWall, transform.position, transform.rotation);
    }
}
