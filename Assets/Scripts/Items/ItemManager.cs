using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public string GetDamageTypeFromWeapon(ItemDataSO weapon)
    {
        Debug.Log("ItemManager.GetDamageTypeFromWeapon() called...");
        string damageTypeStringReturned = "None";

        if(weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Physical)
        {
            damageTypeStringReturned = "Physical";
        }
        else if (weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Fire)
        {
            damageTypeStringReturned = "Fire";
        }
        else if (weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Shadow)
        {
            damageTypeStringReturned = "Shadow";
        }
        else if (weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Poison)
        {
            damageTypeStringReturned = "Poison";
        }
        else if (weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Frost)
        {
            damageTypeStringReturned = "Frost";
        }

        Debug.Log("ItemManager.GetDamageTypeFromWeapon() detected that " + weapon.Name + " has a damage type of " + damageTypeStringReturned);
        return damageTypeStringReturned;
    }

    public void AssignWeaponToCharacter(LivingEntity entity, ItemDataSO weapon)
    {
        Debug.Log("ItemManager.AssignWeaponToCharacter() called, assigning " + weapon.Name + " to " + entity.name);
        entity.myMainHandWeapon = weapon;
    }
    public void AssignShieldToCharacter(LivingEntity entity, ItemDataSO shield)
    {
        Debug.Log("ItemManager.AssignShieldToCharacter() called, assigning " + shield.Name + " to " + entity.name);
        entity.myOffHandWeapon = shield;
    }
}
