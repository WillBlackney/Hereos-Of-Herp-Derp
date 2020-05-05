using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldEncounter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Properties + Component References
    #region
    public enum EncounterType { NoType, BasicEnemy, EliteEnemy, Home, CampSite, Shop, Treasure, Mystery, Boss};

    [Header("Properties")]
    public List<WorldEncounter> connectingEncounters;
    public EncounterType myEncounterType;
    public int column;
    public int position;
    public bool encounterReached;    

    [Header("Component References")]
    public Image myEncounterTypeImage;
    public Image myEncounterTypeShadowImage;
    public Image glowOutline;
    public GameObject redXOverlay;
    public GameObject visualParent;
    public RectTransform visualParentTransform;
    public Animator myAnimator;
    #endregion

    // Initialization + Setup
    #region
    public void InitializeSetup()
    {
        SetEncounterType();
        SetEncounterTypeSprite();        
    }    
    #endregion

    // Visibility + View Logic
    #region
    public void PlayBreatheAnimation()
    {
        visualParentTransform.localScale = new Vector3(1, 1);
        myAnimator.SetTrigger("Idle");
    }
    public void PlayIdleAnimation()
    {
        myAnimator.SetTrigger("New Trigger");
    }
    public void SetShadowViewState(bool onOrOff)
    {
        myEncounterTypeShadowImage.gameObject.SetActive(onOrOff);
    }
    public void SetGlowOutlineViewState(bool onOrOff)
    {
        glowOutline.gameObject.SetActive(onOrOff);
    }
    public void SetRedXOverlayViewState(bool onOrOff)
    {
        redXOverlay.SetActive(onOrOff);
    }    
    public void SetEncounterTypeSprite()
    {
        if (myEncounterType == EncounterType.BasicEnemy)
        {
            myEncounterTypeImage.sprite = WorldManager.Instance.basicEncounterImage;
            myEncounterTypeShadowImage.sprite = WorldManager.Instance.basicEncounterImage;
        }

        else if (myEncounterType == EncounterType.CampSite)
        {
            myEncounterTypeImage.sprite = WorldManager.Instance.campSiteEncounterImage;
            myEncounterTypeShadowImage.sprite = WorldManager.Instance.campSiteEncounterImage;
        }

        else if (myEncounterType == EncounterType.EliteEnemy)
        {
            myEncounterTypeImage.sprite = WorldManager.Instance.eliteEncounterImage;
            myEncounterTypeShadowImage.sprite = WorldManager.Instance.eliteEncounterImage;
        }

        else if (myEncounterType == EncounterType.Shop)
        {
            myEncounterTypeImage.sprite = WorldManager.Instance.shopEncounterImage;
            myEncounterTypeShadowImage.sprite = WorldManager.Instance.shopEncounterImage;
        }

        else if (myEncounterType == EncounterType.Treasure)
        {
            myEncounterTypeImage.sprite = WorldManager.Instance.treasureEncounterImage;
            myEncounterTypeShadowImage.sprite = WorldManager.Instance.treasureEncounterImage;
        }

        else if (myEncounterType == EncounterType.Mystery)
        {
            myEncounterTypeImage.sprite = WorldManager.Instance.mysteryEncounterImage;
            myEncounterTypeShadowImage.sprite = WorldManager.Instance.mysteryEncounterImage;
        }
        else if (myEncounterType == EncounterType.Boss)
        {
            myEncounterTypeImage.sprite = WorldManager.Instance.bossEncounterImage;
            myEncounterTypeShadowImage.sprite = WorldManager.Instance.bossEncounterImage;
        }
    }
    public void SetEncounterType()
    {
        if (WorldManager.Instance.onlySpawnBasics)
        {
            myEncounterType = EncounterType.BasicEnemy;
        }
        else if (WorldManager.Instance.onlySpawnElites)
        {
            myEncounterType = EncounterType.EliteEnemy;
        }
        else if (WorldManager.Instance.onlySpawnMysterys)
        {
            myEncounterType = EncounterType.Mystery;
        }
        else if (WorldManager.Instance.onlySpawnCampSites)
        {
            myEncounterType = EncounterType.CampSite;
        }
        else if (WorldManager.Instance.onlySpawnMysterys)
        {
            myEncounterType = EncounterType.Mystery;
        }
        else if (WorldManager.Instance.onlySpawnShops)
        {
            myEncounterType = EncounterType.Shop;
        }
        else if (WorldManager.Instance.onlySpawnTreasures)
        {
            myEncounterType = EncounterType.Treasure;
        }
    }
    
    #endregion

    // Mouse + Click Events
    #region
    public void OnEncounterButtonClicked()
    {
        Debug.Log("WorldEncounter.OnEncounterButtonClicked() called...");

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
            EventManager.Instance.StartNewCampSiteEncounterEvent();
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

        else if (myEncounterType == EncounterType.Boss)
        {
            WorldManager.Instance.SetPlayerPosition(this);
            Debug.Log("Starting new Mystery Room event");
            EventManager.Instance.StartNewBossEncounterEvent();
        }

        WorldManager.Instance.IdleAllEncounters();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //SetShadowViewState(true);
        SetGlowOutlineViewState(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //SetShadowViewState(false);
        SetGlowOutlineViewState(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnEncounterButtonClicked();
    }
    #endregion







}
