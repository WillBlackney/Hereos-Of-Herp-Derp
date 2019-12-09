using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverMe : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Attatch this script to a game object that needs mouse over/exit/click events to disable/enable UI components

    public List<GameObject> myElements; 
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach(GameObject element in myElements)
        {
            element.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject element in myElements)
        {
            element.SetActive(false);
        }
    }
}
