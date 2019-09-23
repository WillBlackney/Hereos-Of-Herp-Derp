using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageEffect : MonoBehaviour
{
    public TextMeshProUGUI amountText;

    public void InitializeSetup(int damageAmount, bool heal = false)
    {
        VisualEffectManager.Instance.dfxQueue.Add(this);
        if(heal == false)
        {
            amountText.text = "-" + damageAmount.ToString();
            amountText.color = Color.red;
        }

        else if(heal == true)
        {
            amountText.text = "+" + damageAmount.ToString();
            amountText.color = Color.green;
        }
        
    }

    public void InitializeSetup(string statusName)
    {
        VisualEffectManager.Instance.dfxQueue.Add(this);
        amountText.text = statusName;
        amountText.color = Color.white;
    }

    public void DestroyThis()
    {
        VisualEffectManager.Instance.dfxQueue.Remove(this);
        Destroy(gameObject);
    }
}
