using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCardRewardOverlay : MonoBehaviour
{
    [Header("Component References")]
    public StateCard stateCard;
    public RectTransform stateCardScaleParent;
    public RectTransform masterLocationParent;
    public GameObject sparkleParticleEffectParent;
    public GameObject fireDestroyParticleEffectParent;
    public CanvasGroup canvasGroup;
}
