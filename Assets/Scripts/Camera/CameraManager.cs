using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Component References")]
    public CameraMovement unityCamera;

    [Header("Properties")]
    public GameObject currentLookAtTarget;

    public static CameraManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void SetCameraLookAtTarget(GameObject target)
    {
        currentLookAtTarget = target;
    }
    public void ClearCameraLookAtTarget()
    {
        currentLookAtTarget = null;
    }
    public void ResetCameraOnCombatStart()
    {
        // Zoom out
        unityCamera.SetPreferedOrthographicSize(5); 

        // Focus on world centre tile
        unityCamera.mainCamera.transform.position = new Vector3(LevelManager.Instance.GetWorldCentreTile().WorldPosition.x, LevelManager.Instance.GetWorldCentreTile().WorldPosition.y + 0.5f, -10);
    }
}
