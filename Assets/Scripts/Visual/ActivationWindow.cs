using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivationWindow : MonoBehaviour
{
    [Header("Component References")]
    public Image myEntityImage;
    public TextMeshProUGUI rollText;
    public Slider myHealthBar;

    [Header("Properties")]
    public LivingEntity myLivingEntity;
    public bool animateNumberText;
    public void InitializeSetup(LivingEntity entity)
    {
        myLivingEntity = entity;
        myEntityImage.sprite = entity.mySpriteRenderer.sprite;
        entity.myActivationWindow = this;
    }
}
