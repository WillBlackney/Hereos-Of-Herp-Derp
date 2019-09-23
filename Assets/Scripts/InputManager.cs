using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UnselecteDefender();
        }
    }

    public void UnselecteDefender()
    {
        if(DefenderManager.Instance.selectedDefender != null)
        {
            DefenderManager.Instance.ClearSelectedDefender();
        }
    }
}
