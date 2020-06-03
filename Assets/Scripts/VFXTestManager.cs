using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VFXTestManager : MonoBehaviour
{
    // Properties + Component Refs
    #region
    [Header("Component References")]
    public TextMeshProUGUI currentFxText;

    [Header("Character References")]
    public LivingEntity caster;
    public LivingEntity target;

    [Header("Projectile Settings")]
    public float projectileSpeed;
    public float projectileScale;
    public int projectileSortingOrder;

    [Header("VFX Sorting Properties")]
    public string currentEffect;
    public int currentEffectIndex;
    public List<string> effectNames;

    [Header("Reward Overlay Properties")]
    public ItemDataSO testItemData;

    #endregion

    // Singleton Pattern
    #region
    public static VFXTestManager Instance;

    private void Awake()
    {
        Instance = this;
        currentEffectIndex = 0;
        PopulateEffectNamesList();
        SetCurrentEffect("Fire Ball");
    }
    #endregion

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayCurrentEffect();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetPreviousEffect();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GetNextEffect();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetNextEffect();
        }
    }
    public void PopulateEffectNamesList()
    {
        // Projectiles
        effectNames.Add("Fire Ball");
        effectNames.Add("Poison Ball");
        effectNames.Add("Shadow Ball");
        effectNames.Add("Frost Ball");
        effectNames.Add("Lightning Ball");
        effectNames.Add("Holy Ball");

        // Novas
        effectNames.Add("Fire Nova");
        effectNames.Add("Poison Nova");
        effectNames.Add("Shadow Nova");
        effectNames.Add("Frost Nova");
        effectNames.Add("Lightning Nova");
        effectNames.Add("Holy Nova");
        effectNames.Add("Blue Nova");

        // Debuffs
        effectNames.Add("Apply Stunned");
        effectNames.Add("Apply Poisoned");
        effectNames.Add("Apply Burning");
        effectNames.Add("Apply Shocked");
        effectNames.Add("Apply Chilled");
        effectNames.Add("Apply Weakened");
        effectNames.Add("Apply Vulnerable");
        effectNames.Add("General Debuff");

        // Small Explosions
        effectNames.Add("Small Frost Explosion");
        effectNames.Add("Small Poison Explosion");
        effectNames.Add("Small Lightning Explosion");

        // Buff
        effectNames.Add("General Buff");
        effectNames.Add("Shadow Buff");
        effectNames.Add("Holy Buff");
        effectNames.Add("Core Stat Buff");
        effectNames.Add("Gain Energy Buff");
        effectNames.Add("Gain Camoflage Buff");

        // Melee
        effectNames.Add("Small Melee Impact");
        effectNames.Add("Big Melee Impact");

        // Misc
        effectNames.Add("Blood Splatter");
        effectNames.Add("Hard Landing");

    }

    public void PlayCurrentEffect()
    {
        // Projectiles
        if(currentEffect ==  "Fire Ball")
        {
            VisualEffectManager.Instance.ShootToonFireball(caster.transform.position, target.transform.position );
        }
        else if (currentEffect == "Poison Ball")
        {
            VisualEffectManager.Instance.ShootToonPoisonBall(caster.transform.position, target.transform.position);
        }
        else if (currentEffect == "Shadow Ball")
        {
            VisualEffectManager.Instance.ShootToonShadowBall(caster.transform.position, target.transform.position );
        }
        else if (currentEffect == "Frost Ball")
        {
            VisualEffectManager.Instance.ShootToonFrostBall(caster.transform.position, target.transform.position);
        }
        else if (currentEffect == "Lightning Ball")
        {
            VisualEffectManager.Instance.ShootToonLightningBall(caster.transform.position, target.transform.position);
        }
        else if (currentEffect == "Holy Ball")
        {
            VisualEffectManager.Instance.ShootToonHolyBall(caster.transform.position, target.transform.position);
        }

        // Novas
        else if (currentEffect == "Fire Nova")
        {
            VisualEffectManager.Instance.CreateFireNova(caster.transform.position );
        }
        else if (currentEffect == "Poison Nova")
        {
            VisualEffectManager.Instance.CreatePoisonNova(caster.transform.position);
        }
        else if (currentEffect == "Shadow Nova")
        {
            VisualEffectManager.Instance.CreateShadowNova(caster.transform.position);
        }
        else if (currentEffect == "Frost Nova")
        {
            VisualEffectManager.Instance.CreateFrostNova(caster.transform.position);
        }
        else if (currentEffect == "Lightning Nova")
        {
            VisualEffectManager.Instance.CreateLightningNova(caster.transform.position);
        }
        else if (currentEffect == "Holy Nova")
        {
            VisualEffectManager.Instance.CreateHolyNova(caster.transform.position);
        }
        else if (currentEffect == "Blue Nova")
        {
            VisualEffectManager.Instance.CreateBlueNova(caster.transform.position);
        }

        // Debuffs
        else if (currentEffect == "General Debuff")
        {
            VisualEffectManager.Instance.CreateGeneralDebuffEffect(caster.transform.position);
        }
        else if (currentEffect == "Apply Stunned")
        {
            VisualEffectManager.Instance.CreateStunnedEffect(caster.transform.position);
        }
        else if (currentEffect == "Apply Poisoned")
        {
            VisualEffectManager.Instance.CreateApplyPoisonedEffect(caster.transform.position);
        }
        else if (currentEffect == "Apply Burning")
        {
            VisualEffectManager.Instance.CreateApplyBurningEffect(caster.transform.position);
        }
        else if (currentEffect == "Apply Shocked")
        {
            VisualEffectManager.Instance.CreateApplyShockedEffect(caster.transform.position);
        }
        else if (currentEffect == "Apply Chilled")
        {
            VisualEffectManager.Instance.CreateApplyChilledEffect(caster.transform.position);
        }
        else if (currentEffect == "Apply Weakened")
        {
            //VisualEffectManager.Instance.CreateApplyWeakenedEffect(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Apply Vulnerable")
        {
           // VisualEffectManager.Instance.CreateApplyVulnerableEffect(caster.transform.position, projectileSortingOrder, projectileScale);
        }

        // Small Explosions
        else if (currentEffect == "Small Frost Explosion")
        {
            VisualEffectManager.Instance.CreateSmallFrostExplosion(caster.transform.position);
        }
        else if (currentEffect == "Small Poison Explosion")
        {
            VisualEffectManager.Instance.CreateSmallPoisonExplosion(caster.transform.position);
        }
        else if (currentEffect == "Small Lightning Explosion")
        {
            VisualEffectManager.Instance.CreateSmallLightningExplosion(caster.transform.position);
        }

        // Buffs
        else if (currentEffect == "General Buff")
        {
            VisualEffectManager.Instance.CreateGeneralBuffEffect(caster.transform.position);
        }
        else if (currentEffect == "Shadow Buff")
        {
            VisualEffectManager.Instance.CreateShadowBuffEffect(caster.transform.position);
        }
        else if (currentEffect == "Holy Buff")
        {
            VisualEffectManager.Instance.CreateHolyBuffEffect(caster.transform.position);
        }
        else if (currentEffect == "Core Stat Buff")
        {
            VisualEffectManager.Instance.CreateCoreStatBuffEffect(caster.transform.position);
        }
        else if (currentEffect == "Gain Energy Buff")
        {
            VisualEffectManager.Instance.CreateGainEnergyBuffEffect(caster.transform.position);
        }
        else if (currentEffect == "Gain Camoflage Buff")
        {
            VisualEffectManager.Instance.CreateCamoflageBuffEffect(caster.transform.position);
        }

        // Melee
        else if (currentEffect == "Small Melee Impact")
        {
            VisualEffectManager.Instance.CreateSmallMeleeImpact(caster.transform.position);
        }
        else if (currentEffect == "Big Melee Impact")
        {
            VisualEffectManager.Instance.CreateBigMeleeImpact(caster.transform.position);
        }

        // Misc
        else if (currentEffect == "Hard Landing")
        {
            VisualEffectManager.Instance.CreateHardLandingEffect(caster.transform.position);
        }
        else if (currentEffect == "Blood Splatter")
        {
            VisualEffectManager.Instance.CreateBloodSplatterEffect(caster.transform.position);
        }
    }

    public void GetPreviousEffect()
    {
        if(currentEffectIndex == 0)
        {
            SetCurrentEffect(effectNames[effectNames.Count -1]);
            currentEffectIndex = effectNames.Count - 1;
        }

        else
        {
            SetCurrentEffect(effectNames[currentEffectIndex - 1]);
            currentEffectIndex--;
        }
    }
    public void GetNextEffect()
    {
        if (currentEffectIndex == effectNames.Count - 1)
        {
            SetCurrentEffect(effectNames[0]);
            currentEffectIndex = 0;
        }

        else
        {
            SetCurrentEffect(effectNames[currentEffectIndex + 1]);
            currentEffectIndex++;
        }
    }

    public void SetCurrentEffect(string effectName)
    {
        currentEffect = effectName;
        currentFxText.text = effectName;
    }

}
