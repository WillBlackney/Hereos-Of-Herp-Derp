using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImpactEffect : MonoBehaviour
{
    [Header("Component References")]    
    public Animator myAnim;

    public void InitializeSetup(Vector3 location)
    {

    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
