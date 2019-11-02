using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHover : Singleton<TileHover>
{
    [Header("Component References")]
    public GameObject visualParent;    

    [Header("Component References")]
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
        visualParent.SetActive(onOrOff);
        isActive = onOrOff;
    }
    
}
