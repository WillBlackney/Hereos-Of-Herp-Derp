using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverBroadCaster : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string elementName;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Over detected");
        CharacterDataPanelHover.Instance.HandleElementMousedOver(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CharacterDataPanelHover.Instance.DisableView();
    }
}
