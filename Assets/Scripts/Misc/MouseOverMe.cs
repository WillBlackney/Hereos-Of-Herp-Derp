using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverMe : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Attatch this script to a game object that needs mouse over/exit/click events to disable/enable UI components

    public List<GameObject> myElements;

    // Pointer Listeners
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("MouseOverMe.OnPointerEnter() called...");
        ActivateElements();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("MouseOverMe.OnPointerExit() called...");
        DeactivateElements();
    }
    #endregion

    // Collider Listeners
    #region
    public void OnMouseEnter()
    {
        Debug.Log("MouseOverMe.OnMouseEnter() called...");
        ActivateElements();
    }
    public void OnMouseExit()
    {
        Debug.Log("MouseOverMe.OnMouseExit() called...");
        DeactivateElements();
    }
    #endregion

    // View Logic
    #region
    public void ActivateElements()
    {
        foreach (GameObject element in myElements)
        {
            element.SetActive(true);
        }
    }
    public void DeactivateElements()
    {
        foreach (GameObject element in myElements)
        {
            element.SetActive(false);
        }
    }
    #endregion
}
