using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    [Header("Component References")]
    public Camera unityCamera;
    public CinemachineVirtualCamera cinemachineCamera;

    [Header("Properties")]
    public GameObject currentLookAtTarget;

    public void SetCameraLookAtTarget(GameObject target)
    {
        currentLookAtTarget = target;
    }
    public void ClearCameraLookAtTarget()
    {
        currentLookAtTarget = null;
    }
}
