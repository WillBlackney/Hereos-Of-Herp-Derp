using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHover : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public GameObject myVisualParent;
    #endregion

    // Singleton Pattern
    #region
    public static TileHover Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Update Position + Views
    #region
    public void UpdatePosition(Tile newPos)
    {
        transform.position = newPos.WorldPosition;
    }
    public void SetVisibility(bool onOrOff)
    {
        myVisualParent.SetActive(onOrOff);
    }
    #endregion



}
