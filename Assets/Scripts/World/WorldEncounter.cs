using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldEncounter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{    
    public enum EncounterType { NoType, BasicEnemy, EliteEnemy, Home, CampSite, Shop, Treasure, Mystery};

    [Header("Properties")]
    public List<WorldEncounter> connectingEncounters;
    public EncounterType myEncounterType;
    public int column;    
    public bool encounterReached;    

    [Header("Component References")]
    public Image myEncounterTypeImage;
    public Image myEncounterTypeShadowImage;
    public Image circleImage;    
    public Animator myAnimator;

    [Header("Encounter Type Images")]
    public Sprite basicEncounterImage;
    public Sprite eliteEncounterImage;    
    public Sprite campSiteEncounterImage;
    public Sprite shopEncounterImage;
    public Sprite treasureEncounterImage;
    public Sprite mysteryEncounterImage;

    [Header("Encounter Type Shadow Images")]
    public Sprite basicEncounterShadowImage;
    public Sprite eliteEncounterShadowImage;
    public Sprite campSiteEncounterShadowImage;
    public Sprite shopEncounterShadowImage;
    public Sprite treasureEncounterShadowImage;
    public Sprite mysteryEncounterShadowImage;

    // Initialization + Setup
    #region
    public void InitializeSetup()
    {
        SetEncounterTypeSprite();        
    }    
    #endregion

    // Visibility + View Logic
    #region
    public void PlayBreatheAnimation()
    {
        myAnimator.SetTrigger("Breathe");
    }
    public void PlayIdleAnimation()
    {
        myAnimator.SetTrigger("Idle");
    }
    public void SetShadowViewState(bool onOrOff)
    {
        myEncounterTypeShadowImage.gameObject.SetActive(onOrOff);
    }
    public void SetCircleBackgroundViewState(bool onOrOff)
    {
        circleImage.gameObject.SetActive(onOrOff);
    }    
    public void SetEncounterTypeSprite()
    {
        if (myEncounterType == EncounterType.BasicEnemy)
        {
            myEncounterTypeImage.sprite = basicEncounterImage;
            myEncounterTypeShadowImage.sprite = basicEncounterShadowImage;
        }

        else if (myEncounterType == EncounterType.CampSite)
        {
            myEncounterTypeImage.sprite = campSiteEncounterImage;
            myEncounterTypeShadowImage.sprite = campSiteEncounterShadowImage;
        }

        else if (myEncounterType == EncounterType.EliteEnemy)
        {
            myEncounterTypeImage.sprite = eliteEncounterImage;
            myEncounterTypeShadowImage.sprite = eliteEncounterShadowImage;
        }

        else if (myEncounterType == EncounterType.Shop)
        {
            myEncounterTypeImage.sprite = shopEncounterImage;
            myEncounterTypeShadowImage.sprite = shopEncounterShadowImage;
        }

        else if (myEncounterType == EncounterType.Treasure)
        {
            myEncounterTypeImage.sprite = treasureEncounterImage;
            myEncounterTypeShadowImage.sprite = treasureEncounterShadowImage;
        }

        else if (myEncounterType == EncounterType.Mystery)
        {
            myEncounterTypeImage.sprite = mysteryEncounterImage;
            myEncounterTypeShadowImage.sprite = mysteryEncounterShadowImage;
        }
    }
    
    #endregion

    // Mouse + Click Events
    #region
    public void OnEncounterButtonClicked()
    {
        List<WorldEncounter> viableEncounters = WorldManager.Instance.GetNextViableEncounters(WorldManager.Instance.playerEncounterPosition);

        // if were in the middle of an unfinished encounter already, or the encounter clicked on is not valid, return
        if(WorldManager.Instance.canSelectNewEncounter == false ||
            viableEncounters.Contains(this) == false)
        {
            Debug.Log("OnEncounterButtonClicked() detected that encounter clicked is not valid on the current path...");
            return;
        }

        else if(myEncounterType == EncounterType.BasicEnemy)
        {
            WorldManager.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new basic enemy encounter event");            
            EventManager.Instance.StartNewBasicEncounterEvent();
        }

        else if (myEncounterType == EncounterType.CampSite)
        {
            WorldManager.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new rest site event");
            EventManager.Instance.StartNewRestSiteEncounterEvent();
        }

        else if (myEncounterType == EncounterType.EliteEnemy)
        {
            WorldManager.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new elite event");
            EventManager.Instance.StartNewEliteEncounterEvent();
        }

        else if (myEncounterType == EncounterType.Shop)
        {
            WorldManager.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new Shop event");
            EventManager.Instance.StartShopEncounterEvent();
        }

        else if (myEncounterType == EncounterType.Treasure)
        {
            WorldManager.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new Treasure Room event");
            EventManager.Instance.StartNewTreasureRoomEncounterEvent();
        }

        else if (myEncounterType == EncounterType.Mystery)
        {
            WorldManager.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new Mystery Room event");
            EventManager.Instance.StartNewMysteryEncounterEvent();
        }

        WorldManager.Instance.UnhighlightAllHexagons();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetShadowViewState(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SetShadowViewState(false);
    }
    #endregion







}
