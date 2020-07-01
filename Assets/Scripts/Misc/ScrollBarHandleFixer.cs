using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarHandleFixer : MonoBehaviour
{
    public Scrollbar myScrollbar;

    private void Awake()
    {
        Debug.Log("ScrollBarHandleFixer.Awake() setting handle size to 0");
        myScrollbar.size = 0f;
        myScrollbar.value = 1f;
    }
    private void OnEnable()
    {
        Debug.Log("ScrollBarHandleFixer.OnEnable() setting handle size to 0");
        myScrollbar.size = 0f;
        myScrollbar.value = 1f;
    }

    
    private void LateUpdate()
    {
        myScrollbar.size = 0f;
    }
    
}
