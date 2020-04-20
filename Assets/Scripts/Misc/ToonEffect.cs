using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonEffect : MonoBehaviour
{
    [Header("View Properties")]
    public float myScaleModifier;
    public int mySortingOrder;

    public void InitializeSetup(int sortingOrder, float scaleModifier)
    {
        // Set properties
        myScaleModifier = scaleModifier;
        mySortingOrder = sortingOrder;

        // Applt properties
        SetSortingOrder(gameObject, sortingOrder);
        SetScale(gameObject, scaleModifier);

        // Destroy this after 5 seconds
        Destroy(gameObject, 5);

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
        foreach (ParticleSystem ps in allMyPSystems)
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
}
