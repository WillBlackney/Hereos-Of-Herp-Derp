using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHover : Singleton<TileHover>
{
    [Header("Component References")]
    public GameObject myVisualParent;    

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
        isActive = onOrOff;
    }
    
}
