using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Talent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum TalentPool { Guardian, Duelist, Brawler, Assassination, Pyromania, Cyromancy,
        Ranger, Manipulation, Divinity, Shadowcraft, Corruption, Naturalism};

    [Header("General Component References")]  
    public GameObject myGlowOutline;
    public GameObject blackTintOverlay;
    public GameObject purchasedOverlay;
    public Image talentImage;
    public AbilityInfoSheet abilityInfoSheet;
    public PassiveInfoSheet passiveInfoSheet;

    [Header("Inspector Properties")]
    public string talentName;
    public TalentPool talentPool;
    public int talentTier;
    public bool isAbility;
    public bool isPassive;    
    public int passiveStacks;

    [Header("Properties")]
    public CharacterData myCharacterData;
    public AbilityDataSO myAbilityData;
    public bool purchased;

    [Header("Transform Properties")]
    public RectTransform lerpParent;
    public float originalScale;
    public bool expanding;
    public bool shrinking;

    // Initialization
    #region
    void Start()
    {
        TalentController.Instance.BuildTalentInfoPanelFromData(this);
        originalScale = lerpParent.localScale.x;
    }
    #endregion

    // Mouse + Pointer Events
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Talent.OnPointerClick() triggered...");
        TalentController.Instance.OnTalentButtonClicked(myCharacterData, this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Talent.OnPointerEnter() triggered...");
        StartCoroutine(Expand(1));        
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Talent.OnPointerExit() triggered...");

        // Disable panel info
        if (isAbility)
        {
            AbilityInfoSheetController.Instance.DisableSheetView(abilityInfoSheet);
        }
        else
        {
            PassiveInfoSheetController.Instance.DisableSheetView(passiveInfoSheet);
        }

        // Start shrinking anim
        StartCoroutine(Shrink(1));
    }
    #endregion

    // Visibility + View Logic
    #region
    public IEnumerator Expand(int speed)
    {
        shrinking = false;
        expanding = true;

        float finalScale = originalScale * 1.2f;
        RectTransform transform = lerpParent;

        while (transform.localScale.x < finalScale && expanding == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x + (1 * speed * Time.deltaTime), transform.localScale.y + (1 * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();

            if(transform.localScale.x >= finalScale)
            {
                // Enable info panel views
                if (isAbility)
                {
                    AbilityInfoSheetController.Instance.EnableSheetView(abilityInfoSheet, true, true);
                }
                else
                {
                    PassiveInfoSheetController.Instance.EnableSheetView(passiveInfoSheet, true, true);
                }
            }
        }        
    }
    public IEnumerator Shrink(int speed)
    {
        expanding = false;
        shrinking = true;        

        RectTransform transform = lerpParent;

        while (transform.localScale.x > originalScale && shrinking == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x - (1 * speed * Time.deltaTime), transform.localScale.y - (1 * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
        
    }
    #endregion

}
