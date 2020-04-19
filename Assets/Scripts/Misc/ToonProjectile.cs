using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonProjectile : MonoBehaviour
{
    [Header("View Properties")]   
    public float myScaleModifier;
    public int mySortingOrder;
    public float projectileDestroyDelay;

    [Header("Component References")]
    public Rigidbody myRigidBody;
    public SphereCollider mySphereCollider;
    public List<ParticleSystem> allParticleSystems;

    [Header("Prefab References")]
    public GameObject impactParticle; // Effect spawned when projectile hits a collider
    public GameObject projectileParticle; // Effect attached to the gameobject as child
    public GameObject muzzleParticle; // Effect instantly spawned when gameobject is spawned

    [Header("Misc Properties")]
    public float colliderRadius = 1f;
    [Range(0f, 1f)] // This is an offset that moves the impact effect slightly away from the point of impact to reduce clipping of the impact effect
    public float collideOffset = 0.15f;
    

    // Setup + Initialization
    #region
    
    public void InitializeSetup(int sortingOrder, float scaleModifier)
    {
        // Get core components (rigid body, colliders, etc)
        GetMyComponents();

        // Set parent scale + sorting order
        myScaleModifier = scaleModifier;
        mySortingOrder = sortingOrder;

        // Create missle 
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        SetSortingOrder(projectileParticle, sortingOrder);
        SetScale(projectileParticle, myScaleModifier);

        // Create muzzle
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            SetSortingOrder(muzzleParticle, sortingOrder);
            SetScale(muzzleParticle, myScaleModifier);
            Destroy(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
        }
       
    }
    public void GetMyComponents()
    {
        // get rigid body if we dont have it
        if (myRigidBody == null)
        {
            myRigidBody = GetComponent<Rigidbody>();
        }

        // get rigid body if we dont have it
        if (mySphereCollider == null)
        {
            mySphereCollider = transform.GetComponent<SphereCollider>();
        }
    }   
    public void SetSortingOrder(GameObject parent, int sortingOrder)
    {
        Debug.Log("SetSortingOrder() called on " + parent.name);

        List<ParticleSystem> allMyPSystems = new List<ParticleSystem>();

        // get the ps on the parent
        ParticleSystem parentPS = parent.GetComponent<ParticleSystem>();

        // get the p systems on the children
        ParticleSystem[] pSystems = parent.GetComponentsInChildren<ParticleSystem>();

        // combine into one list
        if (parentPS)
        {
            Debug.Log("SetSortingOrder() found a particle system component on " + parent.name);
            allMyPSystems.Add(parentPS);
        }
        allMyPSystems.AddRange(pSystems);

        // set new sorting order value on each particle system found
        foreach(ParticleSystem ps in allMyPSystems)
        {
            Renderer psr = ps.GetComponent<Renderer>();
            psr.sortingOrder += sortingOrder;
        }
    }
    public void SetScale(GameObject parent, float scalePercentage)
    {
        Debug.Log("SetScale() called for " + parent.name + ", scaling by " + scalePercentage.ToString());

        // get current scale
        Vector3 originalScale = parent.transform.localScale;
        Debug.Log(parent.name + " original scale: " + originalScale.x.ToString() + ", " + originalScale.y.ToString() + ", " + originalScale.z.ToString());

        // calculate new scale
        Vector3 newScale = new Vector3(originalScale.x * scalePercentage, originalScale.y * scalePercentage, originalScale.z * scalePercentage);

        // set new scale
        parent.transform.localScale = newScale;
        Debug.Log(parent.name + " new scale: " + parent.transform.localScale.x.ToString() + ", " + parent.transform.localScale.y.ToString() + ", " + parent.transform.localScale.z.ToString());
    }
    #endregion

    public void OnDestinationReached()
    {
        // Create impact / explosion
        GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.identity) as GameObject;
        SetSortingOrder(impactP, mySortingOrder);
        SetScale(impactP, myScaleModifier);

        // Detach all children + particle systems from the parent
        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>(); 
        for (int i = 1; i < trails.Length; i++) 
        {
            ParticleSystem trail = trails[i];

            if (trail.gameObject.name.Contains("Trail"))
            {
                trail.transform.SetParent(null); 
                Destroy(trail.gameObject, 2f); 
            }
        }

        // Destroy everything
        Destroy(projectileParticle, projectileDestroyDelay); // Destroy projectile
        Destroy(impactP, 3.5f); // Destroy impact / explosion effect
        Destroy(gameObject, 10); // Destroy this script + gameobject
    }
}
