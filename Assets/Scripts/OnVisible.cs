using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnVisible : MonoBehaviour
{
    public string colliderName;
    private void OnBecameVisible()
    {
        Debug.Log(colliderName + " just became visible");
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(colliderName + " had a collision with " + other.name);
    }
}
