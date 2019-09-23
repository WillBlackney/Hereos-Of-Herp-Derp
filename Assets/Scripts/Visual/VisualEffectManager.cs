using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualEffectManager : MonoBehaviour
{
    public static VisualEffectManager Instance;
    public GameObject DamageEffectPrefab;

    public List<DamageEffect> dfxQueue = new List<DamageEffect>();

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator CreateDamageEffect(Vector3 location, int damageAmount, bool playFXInstantly)
    {
        if (playFXInstantly == true)
        {
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount);
        }

        else if (dfxQueue.Count == 0)
        {
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount);
        }
        else if (dfxQueue.Count == 1)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount);
        }
        else if (dfxQueue.Count == 2)
        {
            yield return new WaitForSeconds(0.4f);
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount);
        }
        else if (dfxQueue.Count == 3)
        {
            yield return new WaitForSeconds(0.6f);
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount);
        }
    }

    public IEnumerator CreateStatusEffect(Vector3 location, string statusEffectName, bool playFXInstantly)
    {
        if (playFXInstantly == true)
        {
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(statusEffectName);
        }

        else if (dfxQueue.Count == 0)
        {
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(statusEffectName);
        }
        else if(dfxQueue.Count == 1)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(statusEffectName);
        }
        else if (dfxQueue.Count == 2)
        {
            yield return new WaitForSeconds(0.4f);
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(statusEffectName);
        }
        else if (dfxQueue.Count == 3)
        {
            yield return new WaitForSeconds(0.6f);
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(statusEffectName);
        }
    }

    public IEnumerator CreateHealingEffect(Vector3 location, int healAmount, bool playFXInstantly)
    {
        if (playFXInstantly == true)
        {
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true);
        }

        else if (dfxQueue.Count == 0)
        {
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true);
        }
        else if (dfxQueue.Count == 1)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true);
        }
        else if (dfxQueue.Count == 2)
        {
            yield return new WaitForSeconds(0.4f);
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true);
        }
        else if (dfxQueue.Count == 3)
        {
            yield return new WaitForSeconds(0.6f);
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true);
        }
    }




}
