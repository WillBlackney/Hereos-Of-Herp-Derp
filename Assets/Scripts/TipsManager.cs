using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TipsManager : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI tipText;
    public CanvasGroup tipTextCG;

    [Header("Properties")]
    public bool displayTips;
    public List<string> allTips;
    public bool fadingOut;
    public bool fadingIn;

    // Singleton Pattern Setup
    #region
    public static TipsManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            PopulateTipsList();
        }
        
    }
    #endregion

    // Logic
    public void DisplayTips()
    {
        StartCoroutine(DisplayTipsCoroutine());
    }
    private IEnumerator DisplayTipsCoroutine()
    {
        tipTextCG.alpha = 0;
        displayTips = true;

        // get a random tip
        // set text to show tip string

        // fade in text
        while (displayTips)
        {
            // get a random tip
            string nextTip = GetRandomTip();

            // set text to show tip string
            tipText.text = nextTip;

            // fade in text
            Action fadeIn = FadeInTipsText();
            yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

            // yield so player actually has enough time to read the tip
            yield return new WaitForSeconds(5);

            // fade out text
            Action fadeOut = FadeOutTipsText();
            yield return new WaitUntil(() => fadeOut.ActionResolved() == true);
        }
    }

    public Action FadeOutTipsText()
    {
        Action action = new Action();
        StartCoroutine(FadeOutTipsTextCoroutine(action));
        return action;
    }
    private IEnumerator FadeOutTipsTextCoroutine(Action action)
    {
        fadingIn = false;
        fadingOut = true;
        while(tipTextCG.alpha > 0 && fadingOut)
        {
            tipTextCG.alpha -= 1 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        fadingOut = false;
        action.actionResolved = true;
    }
    public Action FadeInTipsText()
    {
        Action action = new Action();
        StartCoroutine(FadeInTipsTextCoroutine(action));
        return action;
    }
    private IEnumerator FadeInTipsTextCoroutine(Action action)
    {
        fadingOut = false;
        fadingIn = true;

        while (tipTextCG.alpha < 1 && fadingIn)
        {
            tipTextCG.alpha += 1 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        fadingIn = false;
        action.actionResolved = true;
    }

    public void PopulateTipsList()
    {
        allTips.Add("Know your enemy! Right click on enemies to view more information about them.");

        allTips.Add("Melee attacks that deal magic damage will benefit from both Strength AND Wisdom!");

        allTips.Add("Negative states (Afflictions) can be removed at Camp Sites and Villages.");

        allTips.Add("Characters that end their activation standing on a grass tile gain Camoflage.");

        allTips.Add("Use the terrain to your advantage! Tree's block line of sight, water prevents movement, and grass provides Camoflage.");

        allTips.Add("You gotta risk it to get the biscuit! If your in a good spot, try and take on an Elite enemy.");

        allTips.Add("Avoiding combat events is a great way to make sure your characters never collect items or level up.");

        allTips.Add("Defeating an elite is guaranteed to reward a State, a rare item, and a consumable.");

        allTips.Add("Fighting an elite you are not prepared for is a great way to end your journey prematurely.");

        allTips.Add("Gold can be spent in villages to aquire new items, weapons and consumables for your characters.");

        allTips.Add("When characters level up, they gain 1 'Talent Point' and 1 'Attribute point', allowing them to learn a new ability, and unlock new talent trees.");

        allTips.Add("You can view information about your characters by clicking the 'Character Roster' button in the top right corner!");

        allTips.Add("Remember to check an enemy's resistances before commiting to an attack!");

        allTips.Add("Today a Berserker, tomorrow a Battle Mage! A character's starting preset doesn't have to determine their playstyle for the whole game.");

        allTips.Add("A character's weapons grant additional abilities. One melee weapon enables 'Strike', dual wielding enables 'Twin Strike', bows enable 'Shoot', and shields enable 'Defend'.");


    }
    public string GetRandomTip()
    {
        return allTips[Random.Range(0, allTips.Count)];
    }

}
