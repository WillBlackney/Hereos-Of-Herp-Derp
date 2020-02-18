using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Component References")]
    public Camera unityCamera;
    public CinemachineVirtualCamera cinemachineCamera;

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
}
