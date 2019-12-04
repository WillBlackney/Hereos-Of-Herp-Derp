using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ActivationWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public Image myEntityImage;
    public TextMeshProUGUI rollText;
    public Slider myHealthBar;
    public GameObject myGlowOutline;

    [Header("Properties")]
    public LivingEntity myLivingEntity;
    public bool animateNumberText;
    public void InitializeSetup(LivingEntity entity)
    {
        myLivingEntity = entity;
        myEntityImage.sprite = entity.mySpriteRenderer.sprite;
        entity.myActivationWindow = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myGlowOutline.SetActive(true);
        if(myLivingEntity != null)
        {
            myLivingEntity.SetColor(myLivingEntity.highlightColour);
        }        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myGlowOutline.SetActive(false);
        if(myLivingEntity != null)
        {
            myLivingEntity.SetColor(myLivingEntity.normalColour);
        }        
    }
}
