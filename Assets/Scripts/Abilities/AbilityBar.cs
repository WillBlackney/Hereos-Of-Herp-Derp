    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBar : MonoBehaviour
{
    [Header("Component References")]
    public GameObject abilityButtonFitterParent;

    [Header("Properties")]
    public Defender myDefender;   
    public void PlaceButtonOnNextAvailableSlot(string abilityName)
    {
        // Create new ability button game object
        GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, myDefender.myAbilityBar.abilityButtonFitterParent.transform);
        Ability newAbility = newAbilityGO.GetComponent<Ability>();

        // Set up ability properties
        newAbility.myLivingEntity = myDefender;
        newAbility.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));
        myDefender.mySpellBook.myActiveAbilities.Add(newAbility);
        myDefender.mySpellBook.SetAbilityIndexPosition(newAbility);

        // 10th ability is triggered bt num key 0
        string abBarPos = myDefender.mySpellBook.myActiveAbilities.Count.ToString();
        if (abBarPos == "10")
        {
            abBarPos = "9";
        }

        // Set number on bar text
        newAbility.abilityNumberText.text = abBarPos;
    }

   
}
