using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableTopPanelSlot : MonoBehaviour
{
    public GameObject slotImageParent;
    public bool occupied;

    public void HideSlotView()
    {
        slotImageParent.SetActive(false);
    }
    public void ShowSlotView()
    {
        slotImageParent.SetActive(true);
    }
}
