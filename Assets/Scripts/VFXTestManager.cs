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

    [Header("FX Sortin Properties")]
    public string currentEffect;
    public int currentEffectIndex;
    public List<string> effectNames;

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

        // Debuffs
        effectNames.Add("Apply Stunned");
        effectNames.Add("Apply Poisoned");
        effectNames.Add("Apply Burning");
        effectNames.Add("Apply Shocked");
        effectNames.Add("Apply Chilled");
        effectNames.Add("Apply Weakened");
        effectNames.Add("Apply Vulnerable");
        effectNames.Add("General Debuff");

    }

    public void PlayCurrentEffect()
    {
        // Projectiles
        if(currentEffect ==  "Fire Ball")
        {
            VisualEffectManager.Instance.ShootToonFireball(caster.transform.position, target.transform.position, projectileSpeed, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Poison Ball")
        {
            VisualEffectManager.Instance.ShootToonPoisonBall(caster.transform.position, target.transform.position, projectileSpeed, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Shadow Ball")
        {
            VisualEffectManager.Instance.ShootToonShadowBall(caster.transform.position, target.transform.position, projectileSpeed, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Frost Ball")
        {
            VisualEffectManager.Instance.ShootToonFrostBall(caster.transform.position, target.transform.position, projectileSpeed, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Lightning Ball")
        {
            VisualEffectManager.Instance.ShootToonLightningBall(caster.transform.position, target.transform.position, projectileSpeed, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Holy Ball")
        {
            VisualEffectManager.Instance.ShootToonHolyBall(caster.transform.position, target.transform.position, projectileSpeed, projectileSortingOrder, projectileScale);
        }

        // Novas
        else if (currentEffect == "Fire Nova")
        {
            VisualEffectManager.Instance.CreateFireNova(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Poison Nova")
        {
            VisualEffectManager.Instance.CreatePoisonNova(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Shadow Nova")
        {
            VisualEffectManager.Instance.CreateShadowNova(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Frost Nova")
        {
            VisualEffectManager.Instance.CreateFrostNova(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Lightning Nova")
        {
            VisualEffectManager.Instance.CreateLightningNova(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Holy Nova")
        {
            VisualEffectManager.Instance.CreateHolyNova(caster.transform.position, projectileSortingOrder, projectileScale);
        }

        // Debuffs
        else if (currentEffect == "General Debuff")
        {
            //VisualEffectManager.Instance.CreateStunnedEffect(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Apply Stunned")
        {
            VisualEffectManager.Instance.CreateStunnedEffect(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Apply Poisoned")
        {
            VisualEffectManager.Instance.CreateApplyPoisonedEffect(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Apply Burning")
        {
            VisualEffectManager.Instance.CreateApplyBurningEffect(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Apply Shocked")
        {
            //VisualEffectManager.Instance.CreateStunnedEffect(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Apply Chilled")
        {
            //VisualEffectManager.Instance.CreateStunnedEffect(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Apply Weakened")
        {
            //VisualEffectManager.Instance.CreateStunnedEffect(caster.transform.position, projectileSortingOrder, projectileScale);
        }
        else if (currentEffect == "Apply Vulnerable")
        {
            //VisualEffectManager.Instance.CreateStunnedEffect(caster.transform.position, projectileSortingOrder, projectileScale);
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
