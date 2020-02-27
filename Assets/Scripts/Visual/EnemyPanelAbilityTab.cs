using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EnemyPanelAbilityTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image abilityImage;
    public GameObject abilityInfoPanelParent;

    public TextMeshProUGUI abilityNameText;
    public TextMeshProUGUI energyCostText;
    public TextMeshProUGUI cdText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI descriptionText;

    [Header("Type Button References")]
    public GameObject meleeAttackIcon;
    public GameObject rangedAttackIcon;
    public GameObject skillIcon;
    public GameObject powerIcon;

    public void OnPointerEnter(PointerEventData eventData)
    {
        abilityInfoPanelParent.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        abilityInfoPanelParent.SetActive(false);
    }
}
