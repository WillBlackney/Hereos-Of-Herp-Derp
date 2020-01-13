using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public string GetDamageTypeFromWeapon(ItemDataSO weapon)
    {
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

        return damageTypeStringReturned;
    }

    public void AssignWeaponToCharacter(LivingEntity entity, ItemDataSO weapon)
    {
        entity.myMainHandWeapon = weapon;
    }
    public void AssignShieldToCharacter(LivingEntity entity, ItemDataSO shield)
    {
        entity.myOffHandWeapon = shield;
    }
}
