using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spriter2UnityDX;

public class EnemyInfoPanel : MonoBehaviour
{
    [Header("Properties")]
    public Enemy myEnemy;
    
    [Header("Component References")]
    public GameObject visualParent;
    public CanvasGroup myCG;
    public GameObject spellToolTipParent;  
    public TextMeshProUGUI characterNameText;
    public UniversalCharacterModel myModel;
    public EntityRenderer myEntityRenderer;
    public Canvas myParentRenderCanvas;
    public GameObject characterShadow;
    public GameObject centrePos;
    public GameObject northPos;

    [Header("Core Stat Text References")]
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI wisdomText;
    public TextMeshProUGUI dexterityText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI mobilityText;
    public TextMeshProUGUI initiativeText;   

    [Header("Secondary Stat Text References")]
    public TextMeshProUGUI criticalText;
    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI parryText;
    public TextMeshProUGUI auraSizeText;
    public TextMeshProUGUI maxEnergyText;
    public TextMeshProUGUI meleeRangeText;

    [Header("Resistance Text References")]
    public TextMeshProUGUI physicalResistanceText;
    public TextMeshProUGUI fireResistanceText;
    public TextMeshProUGUI frostResistanceText;
    public TextMeshProUGUI poisonResistanceText;
    public TextMeshProUGUI airResistanceText;
    public TextMeshProUGUI shadowResistanceText;

    // Initialization + Setup
    #region
    public void InitializeSetup(Enemy enemy)
    {
        // Establish enemy connection
        myEnemy = enemy;

        // Fix camera/canvas issue weirdness
        myParentRenderCanvas = GetComponentInParent<Canvas>();
        myParentRenderCanvas.worldCamera = Camera.main;

        // Build character model
        CharacterModelController.BuildModelFromPresetString(myModel, myEnemy.myName);

        // Set up text files
        characterNameText.text = enemy.myName;

        // Core stats
        strengthText.text = enemy.currentStrength.ToString();
        dexterityText.text = enemy.currentDexterity.ToString();
        wisdomText.text = enemy.currentWisdom.ToString();
        staminaText.text = enemy.currentStamina.ToString();
        mobilityText.text = enemy.currentMobility.ToString();
        initiativeText.text = enemy.currentInitiative.ToString();

        // Secondary stats
        criticalText.text = enemy.currentCriticalChance.ToString();
        dodgeText.text = enemy.currentDodgeChance.ToString();
        parryText.text = enemy.currentParryChance.ToString();
        auraSizeText.text = enemy.currentAuraSize.ToString();
        dodgeText.text = enemy.currentDodgeChance.ToString();
        maxEnergyText.text = enemy.currentMaxEnergy.ToString();
        meleeRangeText.text = enemy.currentMeleeRange.ToString();

        // Resistances
        physicalResistanceText.text = enemy.currentPhysicalResistance.ToString();
        fireResistanceText.text = enemy.currentFireResistance.ToString();
        frostResistanceText.text = enemy.currentFrostResistance.ToString();
        poisonResistanceText.text = enemy.currentPoisonResistance.ToString();
        airResistanceText.text = enemy.currentAirResistance.ToString();
        shadowResistanceText.text = enemy.currentShadowResistance.ToString();

    }
    public void AddAbilityToolTipToView(Ability ability)
    {
        GameObject newToolTip = Instantiate(PrefabHolder.Instance.enemyPanelAbilityTab, spellToolTipParent.transform);
        EnemyPanelAbilityTab tooltipData = newToolTip.GetComponent<EnemyPanelAbilityTab>();

        tooltipData.abilityImage.sprite = ability.myAbilityData.sprite;
        tooltipData.abilityNameText.text = ability.myAbilityData.abilityName;
        tooltipData.energyCostText.text = ability.myAbilityData.energyCost.ToString();
        tooltipData.cdText.text = ability.myAbilityData.baseCooldownTime.ToString();
        tooltipData.rangeText.text = ability.myAbilityData.range.ToString();
        tooltipData.descriptionText.text = ability.myAbilityData.description;

    }
    #endregion

    // Visibility + View Logic
    #region
    public void EnablePanelView()
    {
        StartCoroutine(FadeIn());
        StartCoroutine(MoveToCentrePosition());
    }
    public void DisablePanelView()
    {
        StartCoroutine(FadeOut());
    }   
    public IEnumerator FadeIn()
    {
        visualParent.SetActive(true);
        myCG.alpha = 0;
        characterShadow.SetActive(true);
        myEntityRenderer.Color = new Color(myEntityRenderer.Color.r, myEntityRenderer.Color.g, myEntityRenderer.Color.b, 0);

        while (myCG.alpha < 1)
        {
            myCG.alpha += 0.1f;
            myEntityRenderer.Color = new Color(myEntityRenderer.Color.r, myEntityRenderer.Color.g, myEntityRenderer.Color.b, myEntityRenderer.Color.a + 25);
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator FadeOut()
    {
        characterShadow.SetActive(false);
        visualParent.SetActive(true);
        myCG.alpha = 1;        
        myEntityRenderer.Color = new Color(myEntityRenderer.Color.r, myEntityRenderer.Color.g, myEntityRenderer.Color.b, 1);

        while (myCG.alpha > 0)
        {
            myCG.alpha -= 0.1f;
            myEntityRenderer.Color = new Color(myEntityRenderer.Color.r, myEntityRenderer.Color.g, myEntityRenderer.Color.b, myEntityRenderer.Color.a - 25);
            yield return new WaitForEndOfFrame();
        }

        visualParent.SetActive(false);

    }
    public IEnumerator MoveToCentrePosition()
    {
        Debug.Log("EnemyInfoPanel.MoveToCentrePosition() coroutine started...");

        visualParent.transform.position = new Vector2(northPos.transform.position.x, northPos.transform.position.y);        

        while(visualParent.transform.position != centrePos.transform.position)
        {
            Vector3 endPos = new Vector2(centrePos.transform.position.x, centrePos.transform.position.y);
            visualParent.transform.position = Vector2.MoveTowards(visualParent.transform.position, endPos, 30 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator MoveToNorthPosition()
    {
        Debug.Log("EnemyInfoPanel.MoveToNorthPosition() coroutine started...");

        visualParent.transform.position = new Vector2(centrePos.transform.position.x, centrePos.transform.position.y);        

        while (visualParent.transform.position != northPos.transform.position)
        {
            Vector3 endPos = new Vector2(northPos.transform.position.x, northPos.transform.position.y);
            visualParent.transform.position = Vector2.MoveTowards(visualParent.transform.position, endPos, 30 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        
    }
    #endregion
}
