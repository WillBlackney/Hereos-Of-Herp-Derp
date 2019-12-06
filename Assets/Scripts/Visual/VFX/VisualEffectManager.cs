using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualEffectManager : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject DamageEffectPrefab;
    public GameObject StatusEffectPrefab;
    public GameObject ImpactEffectPrefab;
    public GameObject MeleeAttackEffectPrefab;
    public GameObject GainBlockEffectPrefab;
    public GameObject BuffEffectPrefab;
    public GameObject DebuffEffectPrefab;
    public GameObject PoisonAppliedEffectPrefab;
    public GameObject DamagedByPoisonEffect;
    public GameObject TeleportEffectPrefab;
    public GameObject HealEffectPrefab;

    [Header("Properties")]
    public List<DamageEffect> vfxQueue = new List<DamageEffect>();
    public float timeBetweenEffectsInSeconds;
    public int queueCount;
    public Color blue;
    public Color red;
    public Color green;    

    // Initialization + Singleton Pattern
    #region
    public static VisualEffectManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Create VFX
    #region
    public IEnumerator CreateDamageEffect(Vector3 location, int damageAmount, bool playFXInstantly = true)
    {
        GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount);
        yield return null;       

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
            //thisColor = red;
            thisColor = Color.white;
        }
        else if (color == "Green")
        {
            //thisColor = green;
            thisColor = Color.white;
        }
        queueCount++;
        GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<StatusEffect>().InitializeSetup(statusEffectName, thisColor);

        yield return null;
        /*
        if (playFXInstantly == true)
        {
            queueCount++;
            GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<StatusEffect>().InitializeSetup(statusEffectName, thisColor);
        }

        else
        {
            yield return new WaitForSeconds(queueCount * timeBetweenEffectsInSeconds);
            queueCount++;
            GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<StatusEffect>().InitializeSetup(statusEffectName, thisColor);
        }
        */
        
    }
  
    public IEnumerator CreateImpactEffect(Vector3 location)
    {
        GameObject damageEffect = Instantiate(ImpactEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<ImpactEffect>().InitializeSetup(location);

        yield return null;
    }
    public IEnumerator CreateMeleeAttackEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(MeleeAttackEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<MeleeAttackEffect>().InitializeSetup();
        yield return null;
    }
    public IEnumerator CreateGainBlockEffect(Vector3 location, int blockGained)
    {
        GameObject newImpactVFX = Instantiate(GainBlockEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, blockGained);
        yield return null;
    }
    public IEnumerator CreateBuffEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(BuffEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateDebuffEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(DebuffEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreatePoisonAppliedEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(PoisonAppliedEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateDamagedByPoisonEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(DamagedByPoisonEffect, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, 0);
        yield return null;
    }
    public IEnumerator CreateTeleportEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(TeleportEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateHealEffect(Vector3 location, int healAmount)
    {
        GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true);        
        GameObject newImpactVFX = Instantiate(HealEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    

    #endregion




}
