﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualEffectManager : MonoBehaviour
{
    [Header("VFX Prefab References")]
    public GameObject DamageEffectPrefab;
    public GameObject StatusEffectPrefab;
    public GameObject ImpactEffectPrefab;
    public GameObject MeleeAttackEffectPrefab;
    public GameObject GainBlockEffectPrefab;
    public GameObject LoseBlockEffectPrefab;
    public GameObject BuffEffectPrefab;
    public GameObject DebuffEffectPrefab;
    public GameObject PoisonAppliedEffectPrefab;
    public GameObject DamagedByPoisonEffect;
    public GameObject TeleportEffectPrefab;
    public GameObject HealEffectPrefab;
    public GameObject AoeMeleeAttackEffectPrefab;

    [Header("Projectile Prefab References")]
    public GameObject ArrowPrefab;
    public GameObject FireBallPrefab;
    public GameObject ShadowBallPrefab;
    public GameObject FrostBoltPrefab;
    public GameObject HolyFirePrefab;

    [Header("TOON Projectile Prefab References")]
    public GameObject toonFireBall;
    public GameObject toonPoisonBall;
    public GameObject toonShadowBall;
    public GameObject toonFrostBall;
    public GameObject toonLightningBall;
    public GameObject toonHolyBall;

    [Header("TOON Nova Prefab References")]
    public GameObject toonFireNova;
    public GameObject toonShadowNova;
    public GameObject toonPoisonNova;
    public GameObject toonLightningNova;
    public GameObject toonFrostNova;
    public GameObject toonHolyNova;
    public GameObject toonBlueNova;

    [Header("TOON Buff Prefab References")]
    public GameObject toonGeneralBuffPrefab;
    public GameObject toonShadowBuffPrefab;
    public GameObject toonHolyBuffPrefab;
    public GameObject toonGainEnergyPrefab;
    public GameObject toonGainCoreStatPrefab;
    public GameObject toonGainCamoflagePrefab;

    [Header("TOON Debuff Prefab References")]
    public GameObject toonGeneralDebuff;
    public GameObject toonApplyStunned;
    public GameObject toonApplyPoisoned;
    public GameObject toonApplyBurning;
    public GameObject toonApplyShocked;
    public GameObject toonApplyChilled;
    public GameObject toonApplyVulnerable;
    public GameObject toonApplyWeakened;

    [Header("TOON Explosion Prefab References")]
    public GameObject smallFrostExplosion;
    public GameObject smallPoisonExplosion;
    public GameObject smallLightningExplosion;

    [Header("TOON Melee Impact Prefab References")]
    public GameObject smallMeleeImpact;
    public GameObject bigMeleeImpact;

    [Header("TOON Misc Prefab References")]
    public GameObject hardLandingEffect;
    public GameObject bloodSplatterEffect;


    [Header("Properties")]
    public List<DamageEffect> vfxQueue = new List<DamageEffect>();
    public int campsiteVfxSortingLayer;
    public float timeBetweenEffectsInSeconds;
    public int queueCount;
    public Color blue;
    public Color red;
    public Color green;
    public Color yellow;

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
    public IEnumerator CreateDamageEffect(Vector3 location, int damageAmount, bool heal = false, bool healthLost = true)
    {
        GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount, heal, healthLost);
        yield return null;       

    }
    public Action CreateStatusEffect(Vector3 location, string statusEffectName, string color = "White")
    {
        Action action = new Action();
        StartCoroutine(CreateStatusEffectCoroutine(location, statusEffectName, action, color));
        return action;
    }
    public IEnumerator CreateStatusEffectCoroutine(Vector3 location, string statusEffectName, Action action, string color = "White")
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
        else if (color == "Yellow")
        {
            //thisColor = green;
            thisColor = Color.yellow;
        }

        queueCount++;
        GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<StatusEffect>().InitializeSetup(statusEffectName, thisColor);

        action.actionResolved = true;
        yield return null;
        
        
    }  
    public IEnumerator CreateGainBlockEffect(Vector3 location, int blockGained)
    {
        GameObject newImpactVFX = Instantiate(GainBlockEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, blockGained);
        yield return null;
    }
    public IEnumerator CreateLoseBlockEffect(Vector3 location, int blockLost)
    {
        GameObject newImpactVFX = Instantiate(LoseBlockEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, blockLost);
        StartCoroutine(CreateDamageEffect(location, blockLost, false, false));
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
        damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true, false);        
        GameObject newHealVFX = Instantiate(HealEffectPrefab, location, Quaternion.identity);
        newHealVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateAoeMeleeAttackEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(AoeMeleeAttackEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }


    #endregion

    // Camp Site VFX
    #region
    public void CreateTriageEffect(Vector3 location, int sortingLayer) 
    {
        StartCoroutine(CreateTriageEffectCoroutine(location, sortingLayer));
    }
    private IEnumerator CreateTriageEffectCoroutine(Vector3 location, int sortingLayer)
    {
        Debug.Log("VisualEffectManager.CreateTriageEffectCoroutine() started...");

        float yOffSet = 0.25f;
        Vector3 screenPos = new Vector3(location.x, location.y + yOffSet, location.z);        

        GameObject newHealVFX = Instantiate(HealEffectPrefab, screenPos, Quaternion.identity);
        SetEffectScale(newHealVFX, 10);;

        newHealVFX.GetComponent<BuffEffect>().InitializeSetup(screenPos, sortingLayer);

        CreateStatusEffectOnCampSiteCharacter(location, "Triage!", sortingLayer);
        yield return null;
    }
    public void CreateTrainEffect(Vector3 location, int sortingLayer)
    {
        StartCoroutine(CreateTrainEffectCoroutine(location, sortingLayer));
    }
    private IEnumerator CreateTrainEffectCoroutine(Vector3 location, int sortingLayer)
    {
        Debug.Log("VisualEffectManager.CreateTrainEffectCoroutine() started...");

        float yOffSet = 0.25f;
        Vector3 screenPos = new Vector3(location.x, location.y + yOffSet, location.z);

        GameObject newHealVFX = Instantiate(BuffEffectPrefab, screenPos, Quaternion.identity);
        SetEffectScale(newHealVFX, 10);

        newHealVFX.GetComponent<BuffEffect>().InitializeSetup(screenPos, sortingLayer);

        CreateStatusEffectOnCampSiteCharacter(location, "Train!", sortingLayer);

        yield return null;
    }
    public void CreateReadEffect(Vector3 location, int sortingLayer)
    {
        StartCoroutine(CreateReadEffectCoroutine(location, sortingLayer));
    }
    private IEnumerator CreateReadEffectCoroutine(Vector3 location, int sortingLayer)
    {
        Debug.Log("VisualEffectManager.CreateReadEffectCoroutine() started...");

        float yOffSet = 0.25f;
        Vector3 screenPos = new Vector3(location.x, location.y + yOffSet, location.z);

        GameObject newHealVFX = Instantiate(BuffEffectPrefab, screenPos, Quaternion.identity);
        SetEffectScale(newHealVFX, 10);

        newHealVFX.GetComponent<BuffEffect>().InitializeSetup(screenPos, sortingLayer);

        CreateStatusEffectOnCampSiteCharacter(location, "Read!", sortingLayer);

        yield return null;
    }
    public void CreateStatusEffectOnCampSiteCharacter(Vector3 location, string statusText, int sortingLayer)
    {
        StartCoroutine(CreateStatusEffectOnCampSiteCharacterCorotuine(location, statusText, sortingLayer));
    }
    private IEnumerator CreateStatusEffectOnCampSiteCharacterCorotuine(Vector3 location, string statusText, int sortingLayer)
    {
        float yOffSet = 0.25f;
        Vector3 screenPos = new Vector3(location.x, location.y + yOffSet, location.z);

        GameObject statusVFX = Instantiate(StatusEffectPrefab, screenPos, Quaternion.identity);
        SetEffectScale(statusVFX, 2);

        statusVFX.GetComponent<StatusEffect>().InitializeSetup(statusText, Color.white, sortingLayer);
        yield return null;
    }
    #endregion

    // Projectiles
    #region
    public Action ShootFireball(Vector3 startPos, Vector3 endPos)
    {
        Action action = new Action();
        StartCoroutine(ShootFireballCoroutine(startPos, endPos, action));
        return action;
    }
    public IEnumerator ShootFireballCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed = 20)
    {        
        GameObject fireBall = Instantiate(FireBallPrefab, startPosition, FireBallPrefab.transform.rotation);
        ExplodeOnHit myExplodeOnHit = fireBall.gameObject.GetComponent<ExplodeOnHit>();

        // make fireball explode instantly if instantiated on the destination 
        if (fireBall.transform.position == endPosition)
        {
            myExplodeOnHit.Explode();
            action.actionResolved = true;
        }

        // else, travel towards destination, explode on arrival
        while (fireBall.transform.position != endPosition)
        {
            fireBall.transform.position = Vector2.MoveTowards(fireBall.transform.position, endPosition, speed * Time.deltaTime);
            if (fireBall.transform.position == endPosition)
            {
                myExplodeOnHit.Explode();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public Action ShootArrow(Vector3 startPos, Vector3 endPos, float speed = 15)
    {
        Debug.Log("VisualEffectManager.ShootArrow() called...");
        Action action = new Action();
        StartCoroutine(ShootArrowCoroutine(startPos, endPos, action, speed));
        return action;
    }
    public IEnumerator ShootArrowCoroutine(Vector3 startPos, Vector3 endPos, Action action, float speed)
    {
        GameObject arrow = Instantiate(ArrowPrefab,startPos,Quaternion.identity);
        Projectile projectileScript = arrow.GetComponent<Projectile>();
        projectileScript.InitializeSetup(startPos, endPos, speed);
        yield return new WaitUntil(() => projectileScript.destinationReached == true);
        action.actionResolved = true;
    }
    public Action ShootShadowBall(Vector3 startPos, Vector3 endPos)
    {
        Action action = new Action();
        StartCoroutine(ShootShadowBallCoroutine(startPos, endPos, action));
        return action;
    }
    public IEnumerator ShootShadowBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed = 4)
    {
        GameObject shadowBall = Instantiate(ShadowBallPrefab, startPosition, ShadowBallPrefab.transform.rotation);
        ExplodeOnHit myExplodeOnHit = shadowBall.gameObject.GetComponent<ExplodeOnHit>();

        while (shadowBall.transform.position != endPosition)
        {
            shadowBall.transform.position = Vector2.MoveTowards(shadowBall.transform.position, endPosition, speed * Time.deltaTime);            
            yield return new WaitForEndOfFrame();
        }

        if (shadowBall.transform.position == endPosition)
        {
            myExplodeOnHit.Explode();
            action.actionResolved = true;
        }
    }
    public Action ShootHolyFire(Vector3 endPos)
    {
        Action action = new Action();
        StartCoroutine(ShootHolyFireCoroutine(endPos, action));
        return action;
    }
    public IEnumerator ShootHolyFireCoroutine(Vector3 endPosition, Action action)
    {
        GameObject holyFire = Instantiate(HolyFirePrefab, endPosition, HolyFirePrefab.transform.rotation);
        Destroy(holyFire, 3);
        action.actionResolved = true;
        yield return null;
        
    }
    public Action ShootFrostBolt(Vector3 startPos, Vector3 endPos)
    {
        Action action = new Action();
        StartCoroutine(ShootFrostBoltCoroutine(startPos, endPos, action));
        return action;
    }
    public IEnumerator ShootFrostBoltCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed = 5)
    {
        GameObject frostBolt = Instantiate(FrostBoltPrefab, startPosition, FrostBoltPrefab.transform.rotation);
        FaceDestination(frostBolt, endPosition);
        ExplodeOnHit myExplodeOnHit = frostBolt.gameObject.GetComponent<ExplodeOnHit>();

        while (frostBolt.transform.position != endPosition)
        {
            frostBolt.transform.position = Vector2.MoveTowards(frostBolt.transform.position, endPosition, speed * Time.deltaTime);
            if (frostBolt.transform.position == endPosition)
            {
                myExplodeOnHit.Explode();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    // Logic
    #region
    public void FaceDestination(GameObject projectile, Vector3 destination)
    {
        Vector2 direction = destination - projectile.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        projectile.transform.rotation = Quaternion.Slerp(projectile.transform.rotation, rotation, 10000f);
    }
    public void SetEffectScale(GameObject scaleParent, int scale)
    {
        Debug.Log("VisualEffectManager.SetEffectScale() called, scaling " +
            scaleParent.name + " to " + scale.ToString());

        RectTransform rectTransform = scaleParent.GetComponent<RectTransform>();
        Transform normalTransform = scaleParent.GetComponent<Transform>();
        Vector3 newScale = new Vector3(scale, scale, scale);

        if (rectTransform)
        {
            rectTransform.localScale = newScale;
        }
        else if (normalTransform)
        {
            normalTransform.localScale = newScale;
        }
    }
    #endregion

    // Toon Vfx Projectiles

    #region
    // Fire Ball
    public Action ShootToonFireball(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.7f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonFireballCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonFireballCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject fireBall = Instantiate(toonFireBall, startPosition, toonFireBall.transform.rotation);
        ToonProjectile tsScript = fireBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (fireBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.actionResolved = true;
        }

        while (fireBall.transform.position != endPosition)
        {
            fireBall.transform.position = Vector2.MoveTowards(fireBall.transform.position, endPosition, speed * Time.deltaTime);
            if (fireBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Poison Ball
    public Action ShootToonPoisonBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonPoisonBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonPoisonBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject poisonBall = Instantiate(toonPoisonBall, startPosition, toonPoisonBall.transform.rotation);
        ToonProjectile tsScript = poisonBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (poisonBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.actionResolved = true;
        }

        while (poisonBall.transform.position != endPosition)
        {
            poisonBall.transform.position = Vector2.MoveTowards(poisonBall.transform.position, endPosition, speed * Time.deltaTime);
            if (poisonBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Shadow Ball
    public Action ShootToonShadowBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonShadowBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonShadowBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject shadowBall = Instantiate(toonShadowBall, startPosition, toonShadowBall.transform.rotation);
        ToonProjectile tsScript = shadowBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (shadowBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.actionResolved = true;
        }

        while (shadowBall.transform.position != endPosition)
        {
            shadowBall.transform.position = Vector2.MoveTowards(shadowBall.transform.position, endPosition, speed * Time.deltaTime);
            if (shadowBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Frost Ball
    public Action ShootToonFrostBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonFrostBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonFrostBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject frostBall = Instantiate(toonFrostBall, startPosition, toonFrostBall.transform.rotation);
        ToonProjectile tsScript = frostBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (frostBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.actionResolved = true;
        }

        while (frostBall.transform.position != endPosition)
        {
            frostBall.transform.position = Vector2.MoveTowards(frostBall.transform.position, endPosition, speed * Time.deltaTime);
            if (frostBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Lightning Ball
    public Action ShootToonLightningBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonLightningBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonLightningBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject lightningBall = Instantiate(toonLightningBall, startPosition, toonLightningBall.transform.rotation);
        ToonProjectile tsScript = lightningBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        // insta explode of created on destination
        if (lightningBall.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.actionResolved = true;
        }

        while (lightningBall.transform.position != endPosition)
        {
            lightningBall.transform.position = Vector2.MoveTowards(lightningBall.transform.position, endPosition, speed * Time.deltaTime);
            if (lightningBall.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // Holy Ball
    public Action ShootToonHolyBall(Vector3 startPos, Vector3 endPos, float speed = 12.5f, int sortingOrderBonus = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonHolyBallCoroutine(startPos, endPos, action, speed, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonHolyBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrderBonus, float scaleModifier)
    {
        bool destinationReached = false;

        GameObject hb = Instantiate(toonHolyBall, startPosition, toonHolyBall.transform.rotation);
        ToonProjectile tsScript = hb.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrderBonus, scaleModifier);

        yield return null;

        // insta explode of created on destination
        if (hb.transform.position == endPosition)
        {
            destinationReached = true;
            tsScript.OnDestinationReached();
            action.actionResolved = true;
        }

        while (hb.transform.position != endPosition)
        {
            hb.transform.position = Vector2.MoveTowards(hb.transform.position, endPosition, speed * Time.deltaTime);
            if (hb.transform.position == endPosition && !destinationReached)
            {
                destinationReached = true;
                tsScript.OnDestinationReached();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    #endregion

    // Toon Auras + Novas
    #region

    // Fire Nova
    public Action CreateFireNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateFireNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateFireNovaCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject fireNova = Instantiate(toonFireNova, location, toonFireNova.transform.rotation);
        ToonEffect teScript = fireNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Poison Nova
    public Action CreatePoisonNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreatePoisonNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreatePoisonNovaCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject poisonNova = Instantiate(toonPoisonNova, location, toonPoisonNova.transform.rotation);
        ToonEffect teScript = poisonNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Shadow Nova
    public Action CreateShadowNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateShadowNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateShadowNovaCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject shadowNova = Instantiate(toonShadowNova, location, toonShadowNova.transform.rotation);
        ToonEffect teScript = shadowNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Frost Nova
    public Action CreateFrostNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateFrostNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateFrostNovaCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject frostNova = Instantiate(toonFrostNova, location, toonFrostNova.transform.rotation);
        ToonEffect teScript = frostNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Lightning Nova
    public Action CreateLightningNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateLightningNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateLightningNovaCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject lightningNova = Instantiate(toonLightningNova, location, toonLightningNova.transform.rotation);
        ToonEffect teScript = lightningNova.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Holy Nova
    public Action CreateHolyNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateHolyNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateHolyNovaCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonHolyNova, location, toonHolyNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Blue Nova
    public Action CreateBlueNova(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateBlueNovaCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateBlueNovaCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonBlueNova, location, toonBlueNova.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }
    #endregion

    // Toon Debuffs
    #region

    // Apply General Debuff
    public Action CreateGeneralDebuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateGeneralDebuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateGeneralDebuffEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonGeneralDebuff, location, toonGeneralDebuff.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Apply Stunned Effect
    public Action CreateStunnedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateStunnedEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateStunnedEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        Vector3 offsetLocation = new Vector3(location.x, location.y + 0.2f, location.z);
        GameObject hn = Instantiate(toonApplyStunned, offsetLocation, toonApplyStunned.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Apply Burning Effect
    public Action CreateApplyBurningEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateApplyBurningEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyBurningEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyBurning, location, toonApplyBurning.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Apply Burning Effect
    public Action CreateApplyPoisonedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateApplyPoisonedEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyPoisonedEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyPoisoned, location, toonApplyPoisoned.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Apply Shocked Effect
    public Action CreateApplyShockedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateApplyShockedEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyShockedEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyShocked, location, toonApplyShocked.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Apply Chilled Effect
    public Action CreateApplyChilledEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateApplyChilledEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyChilledEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyChilled, location, toonApplyChilled.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Apply Weakened Effect
    public Action CreateApplyWeakenedEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateApplyWeakenedEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyWeakenedEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyWeakened, location, toonApplyWeakened.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Apply Vulnerable Effect
    public Action CreateApplyVulnerableEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateApplyVulnerableEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateApplyVulnerableEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonApplyVulnerable, location, toonApplyVulnerable.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }
    #endregion

    // Toon Explosions
    #region

    // Small Frost Explosion
    public Action CreateSmallFrostExplosion(Vector3 location, int sortingOrder = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateSmallFrostExplosionCoroutine(location, action, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator CreateSmallFrostExplosionCoroutine(Vector3 location, Action action, int sortingOrder, float scaleModifier)
    {
        GameObject hn = Instantiate(smallFrostExplosion, location, smallFrostExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrder, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Small Poison Explosion
    public Action CreateSmallPoisonExplosion(Vector3 location, int sortingOrder = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateSmallPoisonExplosionCoroutine(location, action, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator CreateSmallPoisonExplosionCoroutine(Vector3 location, Action action, int sortingOrder, float scaleModifier)
    {
        GameObject hn = Instantiate(smallPoisonExplosion, location, smallPoisonExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrder, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Small Lightning Explosion
    public Action CreateSmallLightningExplosion(Vector3 location, int sortingOrder = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateSmallLightningExplosionCoroutine(location, action, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator CreateSmallLightningExplosionCoroutine(Vector3 location, Action action, int sortingOrder, float scaleModifier)
    {
        GameObject hn = Instantiate(smallLightningExplosion, location, smallLightningExplosion.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrder, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    #endregion

    // Toon Buffs
    #region

    // General Buff
    public Action CreateGeneralBuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateGeneralBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateGeneralBuffEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonGeneralBuffPrefab, location, toonGeneralBuffPrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Shadow Buff
    public Action CreateShadowBuffEffect(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateShadowBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateShadowBuffEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonShadowBuffPrefab, location, toonShadowBuffPrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Holy Buff
    public Action CreateHolyBuffEffect(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateHolyBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateHolyBuffEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonHolyBuffPrefab, location, toonHolyBuffPrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Core Stat Buff
    public Action CreateCoreStatBuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateCoreStatBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateCoreStatBuffEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonGainCoreStatPrefab, location, toonGainCoreStatPrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Gain Energy Buff
    public Action CreateGainEnergyBuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateGainEnergyBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateGainEnergyBuffEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(toonGainEnergyPrefab, location, toonGainEnergyPrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Camoflage Buff
    public Action CreateCamoflageBuffEffect(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateCamoflageBuffEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateCamoflageBuffEffectCoroutine(Vector3 location, Action action, int sortingOrder, float scaleModifier)
    {
        GameObject hn = Instantiate(toonGainCamoflagePrefab, location, toonGainCamoflagePrefab.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrder, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }
    #endregion

    // Melee Impacts
    #region
    // Small Melee Impact
    public Action CreateSmallMeleeImpact(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateSmallMeleeImpactCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateSmallMeleeImpactCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(smallMeleeImpact, location, smallMeleeImpact.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Big Melee Impact
    public Action CreateBigMeleeImpact(Vector3 location, int sortingOrderBonus = 15, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateBigMeleeImpactCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateBigMeleeImpactCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(bigMeleeImpact, location, bigMeleeImpact.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }


    #endregion

    // Toon Misc Effects
    #region

    // Hard Landing
    public Action CreateHardLandingEffect(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateHardLandingEffectCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateHardLandingEffectCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        Vector3 offsetLocation = new Vector3(location.x, location.y - 0.2f, location.z);

        GameObject hn = Instantiate(hardLandingEffect, offsetLocation, hardLandingEffect.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }

    // Blood Splatter
    public Action CreateBloodSplatterEffect(Vector3 location, int sortingOrderBonus = 0, float scaleModifier = 1f)
    {
        Action action = new Action();
        StartCoroutine(CreateBloodSplatterEffecttCoroutine(location, action, sortingOrderBonus, scaleModifier));
        return action;
    }
    private IEnumerator CreateBloodSplatterEffecttCoroutine(Vector3 location, Action action, int sortingOrderBonus, float scaleModifier)
    {
        GameObject hn = Instantiate(bloodSplatterEffect, location, bloodSplatterEffect.transform.rotation);
        ToonEffect teScript = hn.GetComponent<ToonEffect>();
        teScript.InitializeSetup(sortingOrderBonus, scaleModifier);
        action.actionResolved = true;
        yield return null;

    }



    #endregion
}
