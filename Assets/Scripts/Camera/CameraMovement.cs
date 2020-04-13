using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    [Header("Properties + Component References ")]
    public float cameraMoveSpeed = 1f;
    public float cameraZoomSpeed;
    public float smoothSpeed = 0.01f;
    public float currentOrthoSize;
    public Vector3 offset;
    public bool enforceCameraBounds;

    [Header("Camera Component References")]
    public Camera mainCamera;
    //public CinemachineVirtualCamera cinemachineCamera;
    public GameObject CamerasParent;

    [Header("Edge Colliders / View Boundary Objects")]
    public GameObject northCollider;
    public GameObject southCollider;
    public GameObject eastCollider;
    public GameObject westCollider;

    [Header("Current Directional Movement Properties")]
    public bool movingSouth;
    public bool movingNorth;
    public bool movingEast;
    public bool movingWest;

    // Initialization / Update
    #region
    void Start()
    {
        offset = new Vector3(0, 0, -10);
        mainCamera.transform.position = new Vector3(0, 0, -10);
        mainCamera = Camera.main;
        SetPreferedOrthographicSize(5);
    }
    void Update()
    {
        HandleZoomInput();
        MoveTowardsLeftRightUpDown();
    }
    private void LateUpdate()
    {
        LookAtTarget();
    }
    #endregion

    // Camera Movement 
    #region
    public void MoveTowardsLeftRightUpDown()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ClearLookAtTarget();
            if (IsGameObjectVisible(northCollider) == false)
            {
                mainCamera.transform.position += Vector3.up * Time.deltaTime * cameraMoveSpeed;
            }

        }
        if (Input.GetKey(KeyCode.S))
        {
            ClearLookAtTarget();
            if (IsGameObjectVisible(southCollider) == false)
            {
                mainCamera.transform.position += Vector3.down * Time.deltaTime * cameraMoveSpeed;
            }

        }
        if (Input.GetKey(KeyCode.A))
        {
            ClearLookAtTarget();
            if (IsGameObjectVisible(westCollider) == false)
            {
                mainCamera.transform.position += Vector3.left * Time.deltaTime * cameraMoveSpeed;
            }

        }
        if (Input.GetKey(KeyCode.D))
        {
            ClearLookAtTarget();
            if (IsGameObjectVisible(eastCollider) == false)
            {
                mainCamera.transform.position += Vector3.right * Time.deltaTime * cameraMoveSpeed;
            }

        }
    }
    public void MoveTowardsZoomPosition()
    {
        if (mainCamera.orthographicSize != currentOrthoSize)
        {
            Debug.Log("MoveTowardsZoomPosition() detected that camera zoom is not equal to desired zoom size, adjusting...");


            // Zoom in smoothly     
            if (mainCamera.orthographicSize > currentOrthoSize)
            {
                mainCamera.orthographicSize -= 0.05f;
            }
            else if (mainCamera.orthographicSize < currentOrthoSize)
            {
                mainCamera.orthographicSize += 0.05f;
            }

        }
    }
    #endregion

    // Player input
    #region
    public void HandleZoomInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && mainCamera.orthographicSize > 1)
        {
            Debug.Log("HandleZoomInput() detected zoom IN input");
            mainCamera.orthographicSize -= 0.1f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && mainCamera.orthographicSize < 5f)
        {
            Debug.Log("HandleZoomInput() detected zoom OUT input");
            mainCamera.orthographicSize += 0.1f;
        }
    }
    public void SetPreferedOrthographicSize(float newSize)
    {
        Debug.Log("SetPreferedOrthographicSize() called and changed to: " + newSize.ToString());
        float size = newSize;

        if (size < 2)
        {
            size = 2;
        }
        else if (size > 5)
        {
            size = 5;
        }

        mainCamera.orthographicSize = size;
    }
    #endregion

    // Misc
    #region
    public void ClearLookAtTarget()
    {
        CameraManager.Instance.currentLookAtTarget = null;
    }
    public void LookAtTarget()
    {
        if (CameraManager.Instance.currentLookAtTarget != null && !enforceCameraBounds)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, CameraManager.Instance.currentLookAtTarget.transform.position + offset, smoothSpeed);
        }

        else if (CameraManager.Instance.currentLookAtTarget != null && enforceCameraBounds)
        {
            movingSouth = false;
            movingNorth = false;
            movingEast = false;
            movingWest = false;

            Vector3 desiredPosition = CameraManager.Instance.currentLookAtTarget.transform.position + offset;

            // anticipate which directions the camera will attempt to move in
            if (desiredPosition.x > mainCamera.transform.position.x)
            {
                movingEast = true;
            }
            else if (desiredPosition.x < mainCamera.transform.position.x)
            {
                movingWest = true;
            }
            if (desiredPosition.y > mainCamera.transform.position.y)
            {
                movingNorth = true;
            }
            else if (desiredPosition.y < mainCamera.transform.position.y)
            {
                movingSouth = true;
            }

            // Detect and prevent moving camera over the boundary
            if (
                (IsGameObjectVisible(northCollider) && movingNorth) ||
                (IsGameObjectVisible(southCollider) && movingSouth))
            {
                desiredPosition = new Vector3(CameraManager.Instance.currentLookAtTarget.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z) + offset;
            }
            if (
                (IsGameObjectVisible(eastCollider) && movingEast) ||
                (IsGameObjectVisible(westCollider) && movingWest))
            {
                desiredPosition = new Vector3(mainCamera.transform.position.x, CameraManager.Instance.currentLookAtTarget.transform.position.y, mainCamera.transform.position.z) + offset;
            }

            // move cam to newly calculated position, increase lerp speed as camera gets closer to look at target
            if (IsCameraAxisBetweenPositions(mainCamera.transform.position.x, desiredPosition.x + 0.5f, desiredPosition.x - 0.5f) ||
                IsCameraAxisBetweenPositions(mainCamera.transform.position.y, desiredPosition.y + 0.5f, desiredPosition.y - 0.5f))
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed * 3);
            }
            else
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed);
            }

            // Have we reach our target to look at?
            if (mainCamera.transform.position == desiredPosition)
            {
                Debug.Log("Camera has reached LookAtTarget() position");
                ClearLookAtTarget();
            }
        }        

    }


    
    public bool IsGameObjectVisible(GameObject GO)
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(GO.transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            Debug.Log(GO.transform.name.ToString() + " is now visible to the camera");
            return true;            
        }
        else
        {
            return false;
        }
    }

    public bool IsCameraAxisBetweenPositions(float cameraAxis, float positionOne, float positionTwo)
    {
        if (positionOne > positionTwo)
            return cameraAxis >= positionTwo && cameraAxis <= positionOne;
        return cameraAxis >= positionOne && cameraAxis <= positionTwo;
    }

    #endregion

}
