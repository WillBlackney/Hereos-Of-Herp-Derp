using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibrary : MonoBehaviour
{
    public static ItemLibrary Instance;

    public List<ItemDataSO> allItems;   

    private void Awake()
    {
        Instance = this;        
    }
       

    // Item getters and search functions
    public ItemDataSO GetRandomItem()
    {
        return allItems[Random.Range(0, allItems.Count)];
    }

    public ItemDataSO GetRandomCommonItem()
    {
        List<ItemDataSO> allCommonItems = new List<ItemDataSO>();
        int randomIndex;

        foreach(ItemDataSO item in allItems)
        {
            if(item.itemRarity == ItemDataSO.ItemRarity.Common)
            {
                allCommonItems.Add(item);
            }
        }

        randomIndex = Random.Range(0, allCommonItems.Count);
        return allCommonItems[randomIndex];
    }

    public ItemDataSO GetRandomRareItem()
    {
        List<ItemDataSO> allRareItems = new List<ItemDataSO>();
        int randomIndex;

        foreach (ItemDataSO item in allItems)
        {
            if (item.itemRarity == ItemDataSO.ItemRarity.Rare)
            {
                allRareItems.Add(item);
            }
        }

        randomIndex = Random.Range(0, allRareItems.Count);
        return allRareItems[randomIndex];
    }
    public ItemDataSO GetRandomEpicItem()
    {
        List<ItemDataSO> allEpicItems = new List<ItemDataSO>();
        int randomIndex;

        foreach (ItemDataSO item in allItems)
        {
            if (item.itemRarity == ItemDataSO.ItemRarity.Epic)
            {
                allEpicItems.Add(item);
            }
        }

        randomIndex = Random.Range(0, allEpicItems.Count);
        return allEpicItems[randomIndex];
    }

    public ItemDataSO GetRandomLootRewardItem()
    {
        ItemDataSO itemReturned = null;
        int randomNumber = Random.Range(0, 100);

        if(randomNumber >= 0 && randomNumber <= 69)
        {
            itemReturned = GetRandomCommonItem();
        }
        else if (randomNumber >= 70 && randomNumber <= 94)
        {
            itemReturned = GetRandomRareItem();
        }
        else if (randomNumber >= 95 && randomNumber <= 99)
        {
            itemReturned = GetRandomEpicItem();
        }

        return itemReturned;

    }
    public ItemDataSO GetItemByName(string itemName)
    {
        ItemDataSO itemReturned = null;

        foreach(ItemDataSO itemData in allItems)
        {
            if(itemData.itemName == itemName)
            {
                itemReturned = itemData;
            }
        }

        if(itemReturned == null)
        {
            Debug.Log("ItemLibrary.GetItem() search error: could not find a matching item in library");
        }

        return itemReturned;
    }

    public void AssignItem(CharacterData character, string itemName)
    {
        if(itemName == "Rusty Sword")
        {
            AssignRustySword(character);
        }

        else if (itemName == "Elven Slippers")
        {
            AssignElvenSlippers(character);
        }

        else if(itemName == "Crystal Fragment")
        {
            AssignCrystalFragment(character);
        }

        else if(itemName == "Orcish War Paint")
        {
            AssignOrcishWarPaint(character);
        }
        else if(itemName == "Steroids")
        {
            AssignSteroids(character);
        }
        else if (itemName == "Dwarven Rune")
        {
            AssignDwarvenRune(character);
        }
        else if (itemName == "Simple Shield")
        {
            AssignSimpleShield(character);
        }
        else if (itemName == "Acid Vial")
        {
            AssignAcidVial(character);
        }
        else if (itemName == "Organic Carrot")
        {
            AssignOrganicCarrot(character);
        }
        else if (itemName == "Copper Ring")
        {
            AssignCopperRing(character);
        }
        else if (itemName == "Copper Plate")
        {
            AssignCopperPlate(character);
        }
        else if (itemName == "Elven Cloak")
        {
            AssignElvenCloak(character);
        }
        else if (itemName == "Ghillie Suit")
        {
            AssignGhillieSuit(character);
        }
        else if (itemName == "Spiked Carapace")
        {
            AssignSpikedCarapace(character);
        }
        else if (itemName == "Cocaine")
        {
            AssignCocaine(character);
        }
        else if (itemName == "Ninja Boots")
        {
            AssignNinjaBoots(character);
        }
        else if (itemName == "Reaper Scythe")
        {
            AssignReaperScythe(character);
        }
        else if (itemName == "Emerald Amulet")
        {
            AssignEmeraldAmulet(character);
        }
        else if (itemName == "Troll Blood Vial")
        {
            AssignTrollBloodVial(character);
        }
        else if (itemName == "Champion Belt")
        {
            AssignChampionBelt(character);
        }
        else if (itemName == "Mithril Vest")
        {
            AssignMithrilVest(character);
        }
        else if (itemName == "Vaccine")
        {
            AssignVaccine(character);
        }
        else if (itemName == "Visor Of Rage")
        {
            AssignVisorOfRage(character);
        }
    }

   
    // Item giving logic
    public void AssignRustySword(CharacterData character)
    {
        character.ModifyStrength(1);
    }    

    public void AssignElvenSlippers(CharacterData character)
    {
        character.ModifyMobility(1);
    }   

    public void AssignCrystalFragment(CharacterData character)
    {
        character.ModifyEnergy(1);
    }    

    public void AssignOrcishWarPaint(CharacterData character)
    {
        character.enrageStacks += 1;
    }    

    public void AssignSteroids(CharacterData character)
    {
        character.growingStacks += 1;
    }    

    public void AssignDwarvenRune(CharacterData character)
    {
        character.barrierStacks += 1;
    }

    public void AssignSimpleShield(CharacterData character)
    {
        character.cautiousStacks += 2;
    }

    public void AssignAcidVial(CharacterData character)
    {
        character.poisonousStacks += 1;
    }

    public void AssignOrganicCarrot(CharacterData character)
    {
        character.ModifyMaxHealth(10);
    }

    public void AssignCopperRing(CharacterData character)
    {
        character.ModifyMaxAP(2);
    }
    public void AssignCopperPlate(CharacterData character)
    {
        character.ModifyStartingBlock(5);
    }

    public void AssignElvenCloak(CharacterData character)
    {
        character.stealth = true;
    }

    public void AssignGhillieSuit(CharacterData character)
    {
        character.camoflage = true;
    }

    public void AssignSpikedCarapace(CharacterData character)
    {
        character.ModifyThorns(1);
    }

    public void AssignCocaine(CharacterData character)
    {
        character.ModifyStartingAPBonus(2);
    }

    public void AssignNinjaBoots(CharacterData character)
    {
        character.ModifyMobility(2);
    }

    public void AssignReaperScythe(CharacterData character)
    {
        character.ModifyMeleeRange(1);
    }

    public void AssignEmeraldAmulet(CharacterData character)
    {
        character.ModifyAdaptive(2);
    }

    public void AssignTrollBloodVial(CharacterData character)
    {
        character.ModifyStrength(3);
    }

    public void AssignChampionBelt(CharacterData character)
    {
        character.ModifyDexterity(1);
    }

    public void AssignMithrilVest(CharacterData character)
    {
        character.ModifyDexterity(3);
    }

    public void AssignVaccine(CharacterData character)
    {
        character.poisonImmunity = true;
    }

    public void AssignVisorOfRage(CharacterData character)
    {
        character.hatefulPresenceStacks += 1;
    }


}
