using System.Collections;
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
    public IEnumerator ShootFireballCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed = 7)
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
        // FOR TESTING, REMOVE LATER!
        speed = VFXTestManager.Instance.projectileSpeed;

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

    public Action ShootToonFireball(Vector3 startPos, Vector3 endPos, float speed, int sortingOrder = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonFireballCoroutine(startPos, endPos, action, speed, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonFireballCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrder, float scaleModifier)
    {
        // override speed for testing
        bool destinationReached = false;

        GameObject fireBall = Instantiate(toonFireBall, startPosition, toonFireBall.transform.rotation);
        ToonProjectile tsScript = fireBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrder, scaleModifier);

        yield return null;

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
    public Action ShootToonPoisonBall(Vector3 startPos, Vector3 endPos, float speed, int sortingOrder = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonPoisonBallCoroutine(startPos, endPos, action, speed, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonPoisonBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrder, float scaleModifier)
    {
        // override speed for testing
        bool destinationReached = false;

        GameObject fireBall = Instantiate(toonPoisonBall, startPosition, toonPoisonBall.transform.rotation);
        ToonProjectile tsScript = fireBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrder, scaleModifier);

        yield return null;

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
    public Action ShootToonShadowBall(Vector3 startPos, Vector3 endPos, float speed, int sortingOrder = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonShadowBallCoroutine(startPos, endPos, action, speed, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonShadowBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrder, float scaleModifier)
    {
        // override speed for testing
        bool destinationReached = false;

        GameObject shadowBall = Instantiate(toonShadowBall, startPosition, toonShadowBall.transform.rotation);
        ToonProjectile tsScript = shadowBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrder, scaleModifier);

        yield return null;

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
    public Action ShootToonFrostBall(Vector3 startPos, Vector3 endPos, float speed, int sortingOrder = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonFrostBallCoroutine(startPos, endPos, action, speed, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonFrostBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrder, float scaleModifier)
    {
        // override speed for testing
        bool destinationReached = false;

        GameObject frostBall = Instantiate(toonFrostBall, startPosition, toonFrostBall.transform.rotation);
        ToonProjectile tsScript = frostBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrder, scaleModifier);

        yield return null;

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
    public Action ShootToonLightningBall(Vector3 startPos, Vector3 endPos, float speed, int sortingOrder = 15, float scaleModifier = 0.5f)
    {
        Action action = new Action();
        StartCoroutine(ShootToonLightningBallCoroutine(startPos, endPos, action, speed, sortingOrder, scaleModifier));
        return action;
    }
    private IEnumerator ShootToonLightningBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed, int sortingOrder, float scaleModifier)
    {
        // override speed for testing
        bool destinationReached = false;

        GameObject frostBall = Instantiate(toonLightningBall, startPosition, toonLightningBall.transform.rotation);
        ToonProjectile tsScript = frostBall.GetComponent<ToonProjectile>();
        tsScript.InitializeSetup(sortingOrder, scaleModifier);

        yield return null;

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
}
