using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCardInfoTab : MonoBehaviour
{
    [Header("View Component References")]
    public PassiveInfoSheet passiveInfoSheet;
    public RectTransform mainParentTransform;

    public void EnableView()
    {
        mainParentTransform.gameObject.SetActive(true);
        PassiveInfoSheetController.Instance.EnableSheetView(passiveInfoSheet, true, true);
    }
    public void DisableView()
    {
        mainParentTransform.gameObject.SetActive(false);
        PassiveInfoSheetController.Instance.DisableSheetView(passiveInfoSheet);
    }
    

}
