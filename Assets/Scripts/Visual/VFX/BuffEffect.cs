using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffEffect : MonoBehaviour
{
    [Header("Component References")]    
    public Animator myAnim;
    public void InitializeSetup(Vector3 location)
    {
        transform.position = new Vector2(location.x, location.y);       

    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
