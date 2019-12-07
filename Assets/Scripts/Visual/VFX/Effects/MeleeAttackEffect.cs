using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeAttackEffect : MonoBehaviour
{
    public Image myImage;
    public List<Sprite> randomImpactSprites;

    public void InitializeSetup()
    {
        myImage.sprite = randomImpactSprites[Random.Range(0, randomImpactSprites.Count)];
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
