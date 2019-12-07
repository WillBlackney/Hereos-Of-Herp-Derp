using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GainArmorEffect : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI amountText;
    public Animator myAnim;
    public void InitializeSetup(Vector3 location, int blockGained)
    {
        transform.position = new Vector2(location.x, location.y);
        amountText.text = "Block +" + blockGained.ToString();       

    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
