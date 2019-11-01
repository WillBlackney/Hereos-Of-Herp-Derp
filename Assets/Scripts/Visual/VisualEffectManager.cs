using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualEffectManager : MonoBehaviour
{
    public static VisualEffectManager Instance;
    public GameObject DamageEffectPrefab;
    public float timeBetweenEffectsInSeconds;
    public int queueCount;

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

    public IEnumerator CreateStatusEffect(Vector3 location, string statusEffectName, bool playFXInstantly)
    {
        if (playFXInstantly == true)
        {
            queueCount++;
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(statusEffectName);
        }

        else
        {
            yield return new WaitForSeconds(queueCount * timeBetweenEffectsInSeconds);
            queueCount++;
            GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<DamageEffect>().InitializeSetup(statusEffectName);
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
