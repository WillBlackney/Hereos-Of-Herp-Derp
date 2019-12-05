using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    private void Update()
    {
        RayCast();
    }

    public void RayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        int i = 0;
        while (i < hits.Length)
        {
            RaycastHit hit = hits[i];
            Debug.Log("Raycaster hit " + hit.collider.gameObject.name);
            i++;
        }
    }
}
