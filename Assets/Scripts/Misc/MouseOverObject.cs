using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public LivingEntity myLivingEntity;
    public bool mousedOver;

    void Awake()
    {
        myLivingEntity = GetComponentInParent<LivingEntity>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("MouseOfObject detected...");
        mousedOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mousedOver = false;
    }

    public void OnMouseEnter()
    {
        Debug.Log("MouseOfObject detected...");
        mousedOver = true;
    }

    public void OnMouseExit()
    {
        mousedOver = false;
    }
}
