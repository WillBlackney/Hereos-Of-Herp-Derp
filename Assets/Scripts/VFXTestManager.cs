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

    public void PlayCurrentEffect()
    {
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
