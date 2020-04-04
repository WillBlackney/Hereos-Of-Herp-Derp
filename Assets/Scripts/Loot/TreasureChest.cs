using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureChest : MonoBehaviour
{
    [Header("Component References")]
    public SpriteRenderer mySpriteRenderer;

    [Header("Properties")]
    public Color normalColor;
    public Color highLightColor;
    public bool alreadyOpened;

    // Intialization + Setup
    #region
    public void InitializeSetup()
    {
        TreasureRoomManager.Instance.activeTreasureChest = this;
    }
    #endregion

    // Mouse + Click Events
    #region      
    public void OnMouseDown()
    {
        // prevent chest open event triggering twice
        if(alreadyOpened == false)
        {
            alreadyOpened = true;
            Debug.Log("OnMouseDown() detected on treasure chest");
            mySpriteRenderer.color = normalColor;
            EventManager.Instance.StartNewLootRewardEvent(WorldEncounter.EncounterType.Treasure);
        }
        
    }
    public void OnMouseEnter()
    {
        mySpriteRenderer.color = highLightColor;
    }
    public void OnMouseExit()
    {
        mySpriteRenderer.color = normalColor;
    }
    #endregion

    // Logic
    #region
    public void DestroyChest()
    {
        TreasureRoomManager.Instance.activeTreasureChest = null;
        Destroy(gameObject);
        Destroy(this);
    }
    #endregion
}
