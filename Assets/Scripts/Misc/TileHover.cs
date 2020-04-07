using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHover : MonoBehaviour
{
    [Header("Component References")]
    public GameObject myVisualParent;
    public Animator myAnimator;

    [Header("Properties")]
    public bool isActive;
    void Update()
    {
        FollowMouse();
    }
    private void FollowMouse()
    {
        if (LevelManager.Instance.mousedOverTile != null)
        {
            transform.position = LevelManager.Instance.mousedOverTile.WorldPosition;
        }
    }
    public void SetVisibility(bool onOrOff)
    {
        myVisualParent.SetActive(onOrOff);
        myAnimator.SetTrigger("New Trigger");
        isActive = onOrOff;
    }

    public static TileHover Instance;
    private void Awake()
    {
        Instance = this;
    }

}
