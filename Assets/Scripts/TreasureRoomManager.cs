using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoomManager : MonoBehaviour
{
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject canvasParent;
    public Transform chestParent;


    [Header("Properties")]
    public List<TreasureRoomCharacter> allCharacterSlots;
    public TreasureChest activeTreasureChest;

    // Singleton Setup
    #region
    public static TreasureRoomManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public void CreateNewTreasureChest()
    {
        // create a treasure chest game object
        GameObject newTreasureChest = Instantiate(PrefabHolder.Instance.TreasureChest, chestParent);
        newTreasureChest.GetComponent<TreasureChest>().InitializeSetup();
    }
    public void DestroyActiveTreasureChest()
    {
        if(activeTreasureChest != null)
        {
            activeTreasureChest.DestroyChest();
        }
    }

    public void SetUpTreasureRoomCharacter(TreasureRoomCharacter tChar, CharacterData cData)
    {
        tChar.InitializeSetup(cData);
    }

    public void EnableTreasureRoomView()
    {
        //canvasParent.SetActive(true);
        visualParent.SetActive(true);
        IdleAllCharacterAnims();
    }
    public void DisableTreasureRoomView()
    {
       // canvasParent.SetActive(false);
        visualParent.SetActive(false);
    }
    public void IdleAllCharacterAnims()
    {
        foreach(TreasureRoomCharacter character in allCharacterSlots)
        {
            character.myModel.SetIdleAnim();
        }
    }
}
