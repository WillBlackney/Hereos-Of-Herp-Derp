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


    [Header("Properties")]
    public LivingEntity myLivingEntity;
    public bool animateNumberText;
    public void InitializeSetup(LivingEntity entity)
    {
        myLivingEntity = entity;
        myEntityImage.sprite = entity.mySpriteRenderer.sprite;
        entity.myActivationWindow = this;
        myCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        
        int myCurrentActivationOrderIndex = 0;

        for (int i = 0; i < ActivationManager.Instance.activationOrder.Count; i++)
        {
            //Check if GameObject is in the List
            if (ActivationManager.Instance.activationOrder[i] == myLivingEntity)
            {
                //It is. Return the current index
                myCurrentActivationOrderIndex = i;
                break;
            }
        }

        if (ActivationManager.Instance.panelSlots[myCurrentActivationOrderIndex] != null &&
            transform.position != ActivationManager.Instance.panelSlots[myCurrentActivationOrderIndex].transform.position)
        {
            MoveTowardsSlotPosition(ActivationManager.Instance.panelSlots[myCurrentActivationOrderIndex]);
        }
        
    }

    public Action DestroyWindow()
    {
        Action action = new Action();
        StartCoroutine(DestroyWindowCoroutine(action));
        return action;
    }
    public IEnumerator DestroyWindowCoroutine(Action action)
    {
        while (myCanvasGroup.alpha > 0)
        {
            myCanvasGroup.alpha -= 0.05f;
            if (myCanvasGroup.alpha == 0)
            {
                GameObject slotDestroyed = ActivationManager.Instance.panelSlots[ActivationManager.Instance.panelSlots.Count - 1];
                ActivationManager.Instance.activationOrder.Remove(myLivingEntity);
                ActivationManager.Instance.panelSlots.Remove(slotDestroyed);                
                Destroy(slotDestroyed);
                Destroy(gameObject);
            }
            yield return new WaitForEndOfFrame();
        }        
        action.actionResolved = true;

    }
    public void MoveTowardsSlotPosition(GameObject slot)
    {
        transform.position = Vector2.MoveTowards(transform.position, slot.transform.position, 400 * Time.deltaTime);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ActivationWindow.OnPointerClick() running....");
        // Clicking on a character's activation window performs the same logic as clicking on the character itself
        if(myLivingEntity.GetComponent<Defender>())
        {
            myLivingEntity.defender.OnMouseDown();
        }
        else if(myLivingEntity.GetComponent<Enemy>())
        {
            myLivingEntity.enemy.OnMouseDown();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ActivationManager.Instance.panelIsMousedOver = true;
        myGlowOutline.SetActive(true);
        if(myLivingEntity != null)
        {
            myLivingEntity.SetColor(myLivingEntity.highlightColour);
        }        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ActivationManager.Instance.panelIsMousedOver = false;
        myGlowOutline.SetActive(false);
        if(myLivingEntity != null)
        {
            myLivingEntity.SetColor(myLivingEntity.normalColour);
        }        
    }
}
