using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionHover : MonoBehaviour
{
    [Header("Component References")]
    public GameObject visualParent;
    public TextMeshProUGUI instructionText;

    [Header("Properties")]
    public bool followMouse;


    // Singleton set up
    #region
    public static InstructionHover Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    void Update()
    {
        if (followMouse)
        {
            FollowMouse();
        }       
    }

    private void FollowMouse()
    {
        visualParent.transform.position = Input.mousePosition;
    }
    public void SetText(string newText)
    {
        instructionText.text = newText;
    }

    public void EnableInstructionHover(string message)
    {
        StartCoroutine(EnableInstructionHoverCoroutine(message));
    }
    private IEnumerator EnableInstructionHoverCoroutine(string message)
    {
        EnableView();
        SetText(message);
        SetFollowMouseState(true);
        yield return null;
    }
    public void DisableInstructionHover()
    {
        StartCoroutine(DisableInstructionHoverCoroutine());
    }
    private IEnumerator DisableInstructionHoverCoroutine()
    {
        DisableView();
        SetFollowMouseState(false);
        yield return null;
    }
   

    public void EnableView()
    {
        visualParent.SetActive(true);
    }
    public void DisableView()
    {
        visualParent.SetActive(false);
    }
    public void SetFollowMouseState(bool onOrOff)
    {
        followMouse = onOrOff;
    }

}
