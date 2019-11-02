using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualEffectManager : MonoBehaviour
{
    public static VisualEffectManager Instance;
    public GameObject DamageEffectPrefab;
    public GameObject StatusEffectPrefab;
    public float timeBetweenEffectsInSeconds;
    public int queueCount;

    public Color blue;
    public Color red;
    public Color green;

    public List<DamageEffect> dfxQueue = new List<DamageEffect>();

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator CreateDamageEffect(Vector3 location, int damageAmount, bool playFXInstantly)
    {
        if (playFXInstantly == true)
        {
            queueCount++;
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount);
        }

        else
        {
            yield return new WaitForSeconds(queueCount * timeBetweenEffectsInSeconds);
            queueCount++;
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount);
        }

    }

    public IEnumerator CreateStatusEffect(Vector3 location, string statusEffectName, bool playFXInstantly, string color = "White")
    {
        Color thisColor = Color.white;
        if(color == "White")
        {
            thisColor = Color.white;
        }
        else if (color == "Blue")
        {
            // to do: find a good colour for buffing
            thisColor = Color.white;
        }
        else if (color == "Red")
        {
            thisColor = red;
        }
        else if (color == "Green")
        {
            thisColor = green;
        }

        if (playFXInstantly == true)
        {
            queueCount++;
            GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(statusEffectName, thisColor);
        }

        else
        {
            yield return new WaitForSeconds(queueCount * timeBetweenEffectsInSeconds);
            queueCount++;
            GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(statusEffectName, thisColor);
        }
        
    }

    public IEnumerator CreateHealingEffect(Vector3 location, int healAmount, bool playFXInstantly)
    {
        if (playFXInstantly == true)
        {
            queueCount++;
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true);
        }
        else
        {
            yield return new WaitForSeconds(queueCount * timeBetweenEffectsInSeconds);
            queueCount++;
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true);
        }

    }




}
