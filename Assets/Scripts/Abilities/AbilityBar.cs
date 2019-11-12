    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBar : MonoBehaviour
{
    [Header("Properties")]
    public Defender myDefender;   

    [Header("Ability Parent Objects")]
    public GameObject AbilityOneParent;
    public GameObject AbilityTwoParent;
    public GameObject AbilityThreeParent;
    public GameObject AbilityFourParent;
    public GameObject AbilityFiveParent;
    public GameObject AbilitySixParent;
    public GameObject AbilitySevenParent;
    public GameObject AbilityEightParent;
    public GameObject AbilityNineParent;
    public GameObject AbilityTenParent;
 
    public void PlaceButtonOnNextAvailableSlot(string abilityName)
    {
        if (myDefender.mySpellBook.AbilityOne == null)
        {
            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilityOneParent.transform);
            myDefender.mySpellBook.AbilityOne = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilityOne.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilityOne.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));
            newAbilityGO.GetComponent<Ability>().abilityNumberText.text = "1";
            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilityOne);

        }

        else if (myDefender.mySpellBook.AbilityTwo == null)
        {

            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilityTwoParent.transform);
            myDefender.mySpellBook.AbilityTwo = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilityTwo.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilityTwo.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));
            newAbilityGO.GetComponent<Ability>().abilityNumberText.text = "2";
            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilityTwo);

        }

        else if (myDefender.mySpellBook.AbilityThree == null)
        {

            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilityThreeParent.transform);
            myDefender.mySpellBook.AbilityThree = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilityThree.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilityThree.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));
            newAbilityGO.GetComponent<Ability>().abilityNumberText.text = "3";
            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilityThree);

        }


        else if (myDefender.mySpellBook.AbilityFour == null)
        {
            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilityFourParent.transform);
            myDefender.mySpellBook.AbilityFour = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilityFour.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilityFour.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));
            newAbilityGO.GetComponent<Ability>().abilityNumberText.text = "4";
            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilityFour);

        }

        else if (myDefender.mySpellBook.AbilityFive == null)
        {
            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilityFiveParent.transform);
            myDefender.mySpellBook.AbilityFive = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilityFive.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilityFive.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));
            newAbilityGO.GetComponent<Ability>().abilityNumberText.text = "5";
            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilityFive);

        }

        else if (myDefender.mySpellBook.AbilitySix == null)
        {
            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilitySixParent.transform);
            myDefender.mySpellBook.AbilitySix = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilitySix.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilitySix.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));
            newAbilityGO.GetComponent<Ability>().abilityNumberText.text = "6";
            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilitySix);
        }

        else if (myDefender.mySpellBook.AbilitySeven == null)
        {
            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilitySevenParent.transform);
            myDefender.mySpellBook.AbilitySeven = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilitySeven.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilitySeven.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));

            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilitySeven);
        }

        else if (myDefender.mySpellBook.AbilityEight == null)
        {
            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilityEightParent.transform);
            myDefender.mySpellBook.AbilityEight = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilityEight.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilityEight.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));

            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilityEight);
        }

        else if (myDefender.mySpellBook.AbilityNine == null)
        {
            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilityNineParent.transform);
            myDefender.mySpellBook.AbilityNine = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilityNine.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilityNine.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));

            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilityNine);
        }

        else if (myDefender.mySpellBook.AbilityTen == null)
        {
            GameObject newAbilityGO = Instantiate(PrefabHolder.Instance.AbilityButtonPrefab, AbilityTenParent.transform);
            myDefender.mySpellBook.AbilityTen = newAbilityGO.GetComponent<Ability>();
            myDefender.mySpellBook.AbilityTen.myLivingEntity = myDefender;
            myDefender.mySpellBook.AbilityTen.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));

            myDefender.mySpellBook.myActiveAbilities.Add(myDefender.mySpellBook.AbilityTen);
        }

    }

   
}
