using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImpactVFX : MonoBehaviour
{
    public Image myImageComponent;

    public void SetSprite(Sprite newSprite)
    {
        myImageComponent.sprite = newSprite;
    }
}
