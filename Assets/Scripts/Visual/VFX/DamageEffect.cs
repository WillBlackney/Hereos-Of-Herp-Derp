using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageEffect : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI amountText;
    public Animator myAnim;

    // Initialization + Setup
    #region
    public void InitializeSetup(int damageAmount, bool heal = false)
    {
        transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);

        VisualEffectManager.Instance.vfxQueue.Add(this);
        if(heal == false)
        {
            amountText.text = "-" + damageAmount.ToString();
            //amountText.color = Color.red;
        }

        else if(heal == true)
        {
            amountText.text = "+" + damageAmount.ToString();
            amountText.color = Color.green;
        }

        ChooseRandomDirection();
        
    }
    public void InitializeSetup(string statusName, Color textColor)
    {        
        VisualEffectManager.Instance.vfxQueue.Add(this);       
        amountText.text = statusName;
        amountText.color = textColor;
    }
    #endregion

    // Logic
    #region
    public void DestroyThis()
    {
        VisualEffectManager.Instance.queueCount--;
        VisualEffectManager.Instance.vfxQueue.Remove(this);
        
        Destroy(gameObject);
    }

    public void ChooseRandomDirection()
    {
        int randomNumber = Random.Range(0, 100);
        if(randomNumber < 50)
        {
            myAnim.SetTrigger("Right");
        }
        else
        {
            myAnim.SetTrigger("Left");
        }
    }
    #endregion
}
