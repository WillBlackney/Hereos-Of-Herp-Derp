using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ActivationWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References")]
    public Image myEntityImage;
    public TextMeshProUGUI rollText;
    public Slider myHealthBar;
    public GameObject myGlowOutline;
    public CanvasGroup myCanvasGroup;
    public UniversalCharacterModel myUCM;

    [Header("Properties")]
    public LivingEntity myLivingEntity;
    public bool animateNumberText;
    public bool dontFollowSlot;
    public bool update;
    public void InitializeSetup(LivingEntity entity)
    {
        myLivingEntity = entity;
        myEntityImage.sprite = entity.mySpriteRenderer.sprite;
        entity.myActivationWindow = this;
        myCanvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        Debug.Log("Setting up activation window UC Model for " + entity.myName);

        // Set up model        
        CharacterModelController.BuildModelFromPresetString(myUCM, entity.myName);
        myUCM.SetBaseAnim();
    }

    private void Update()
    {
        int myCurrentActivationOrderIndex = 0;

        if (ActivationManager.Instance.activationOrder.Count > 0 
            && ActivationManager.Instance.updateWindowPositions == true)
        {
            // TO DO: 2 lines below prevent buggy behabiour in unity that prevents the activation window's health bar
            // from rendering correctly. Doing this in update degrades performance. fix in the future

            ActivationManager.Instance.activationPanelParent.GetComponent<Canvas>().overrideSorting = true;
            ActivationManager.Instance.activationPanelParent.GetComponent<Canvas>().sortingOrder = 19;

            for (int i = 0; i < ActivationManager.Instance.activationOrder.Count; i++)
            {
                // Check if GameObject is in the List
                if (ActivationManager.Instance.activationOrder[i] == myLivingEntity)
                {
                    // It is. Return the current index
                    myCurrentActivationOrderIndex = i;
                    break;
                }
            }

            if (ActivationManager.Instance.panelSlots != null &&
                ActivationManager.Instance.panelSlots.Count -1 >= myCurrentActivationOrderIndex &&
                ActivationManager.Instance.panelSlots[myCurrentActivationOrderIndex] != null &&
                transform.position != ActivationManager.Instance.panelSlots[myCurrentActivationOrderIndex].transform.position)
            {
                MoveTowardsSlotPosition(ActivationManager.Instance.panelSlots[myCurrentActivationOrderIndex]);
            }
        }    
        
        
    }
    public Action FadeOutWindow()
    {
        Action action = new Action();
        StartCoroutine(FadeOutWindowCoroutine(action));
        return action;
    }
    public IEnumerator FadeOutWindowCoroutine(Action action)
    {
        while (myCanvasGroup.alpha > 0)
        {
            myCanvasGroup.alpha -= 0.05f;
            if (myCanvasGroup.alpha == 0)
            {
                GameObject slotDestroyed = ActivationManager.Instance.panelSlots[ActivationManager.Instance.panelSlots.Count - 1];
                if (ActivationManager.Instance.activationOrder.Contains(myLivingEntity))
                {
                    ActivationManager.Instance.activationOrder.Remove(myLivingEntity);
                }                
                ActivationManager.Instance.panelSlots.Remove(slotDestroyed);                
                Destroy(slotDestroyed);
               // Destroy(gameObject);
                //action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }

        action.actionResolved = true;
        //Destroy(slotDestroyed);
        Destroy(gameObject);
    }

    public void DestroyWindowOnCombatEnd()
    {
        Destroy(gameObject);
    }
    public void MoveTowardsSlotPosition(GameObject slot)
    {
        //Vector3 targetPos = new Vector3(slot.transform.position.x, slot.transform.position.y - 80, slot.transform.position.z);
        if (!dontFollowSlot)
        {
            transform.position = Vector2.MoveTowards(transform.position, slot.transform.position, 10 * Time.deltaTime);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnPointerClick() called...");
        // Clicking on a character's activation window performs the same logic as clicking on the character itself
        if (myLivingEntity.GetComponent<Defender>())
        {
            myLivingEntity.defender.OnMouseDown();
        }
        else if (myLivingEntity.GetComponent<Enemy>())
        {
            myLivingEntity.enemy.OnMouseDown();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnPointerEnter() called...");
        ActivationManager.Instance.panelIsMousedOver = true;
        myGlowOutline.SetActive(true);
        if (myLivingEntity != null)
        {
            myLivingEntity.SetColor(myLivingEntity.highlightColour);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnMouseEnter called...");
        ActivationManager.Instance.panelIsMousedOver = false;
        myGlowOutline.SetActive(false);
        if (myLivingEntity != null)
        {
            myLivingEntity.SetColor(myLivingEntity.normalColour);
        }
    }    
    public void ButtonTest()
    {
        Debug.Log("BUTTON CLICK DETECTED");
    }
}
