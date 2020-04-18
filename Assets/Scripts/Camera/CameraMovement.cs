using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public enum MovementStyle { Lerp, SmoothDamp};

    [Header("Movement Properties ")]
    public MovementStyle movementStyle;
    public float cameraMoveSpeed;
    public float cameraZoomSpeed;

    [Header("Positional Properties ")]
    public float currentOrthoSize;
    public Vector3 offset;
    public bool enforceCameraBounds;

    [Header("Component References")]
    public Camera mainCamera;

    [Header("View Boundary Objects")]
    public GameObject northCollider;
    public GameObject southCollider;
    public GameObject eastCollider;
    public GameObject westCollider;

    [Header("Current Directional Movement Properties")]
    public bool movingSouth;
    public bool movingNorth;
    public bool movingEast;
    public bool movingWest;

    [Header("Lerp Properties")]
    public float smoothSpeed;

    [Header("Smooth Damp Properties")]
    public float dampSpeed;
    public Vector3 dampVelocity = Vector3.zero;
    public float maxCloseness;

    // Initialization / Update
    #region
    void Start()
    {
        offset = new Vector3(0, 0, -10);
        mainCamera.transform.position = new Vector3(0, 0, -10);
        SetPreferedOrthographicSize(5);
    }   

    private void FixedUpdate()
    {
        LookAtTarget();
        HandleZoomInput();
        MoveTowardsLeftRightUpDown();
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
            mainCamera.orthographicSize -= 0.1f * cameraZoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && mainCamera.orthographicSize < 5f)
        {
            Debug.Log("HandleZoomInput() detected zoom OUT input");
            mainCamera.orthographicSize += 0.1f * cameraZoomSpeed;
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
            if (!IsCameraAxisBetweenPositions(mainCamera.transform.position.x, CameraManager.Instance.currentLookAtTarget.transform.position.x + maxCloseness, CameraManager.Instance.currentLookAtTarget.transform.position.x - maxCloseness) ||
                !IsCameraAxisBetweenPositions(mainCamera.transform.position.y, CameraManager.Instance.currentLookAtTarget.transform.position.y + maxCloseness, CameraManager.Instance.currentLookAtTarget.transform.position.y - maxCloseness))
            {
                // Smooth Damp
                if (movementStyle == MovementStyle.SmoothDamp)
                {
                    mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, CameraManager.Instance.currentLookAtTarget.transform.position + offset, ref dampVelocity, dampSpeed, 100, Time.deltaTime);
                }

                // Lerp
                if (movementStyle == MovementStyle.Lerp)
                {
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, CameraManager.Instance.currentLookAtTarget.transform.position + offset, smoothSpeed);
                }                    
            }
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

            // Move cam to newly calculated position, if not already very close            
            if (!IsCameraAxisBetweenPositions(mainCamera.transform.position.x, desiredPosition.x + maxCloseness, desiredPosition.x - maxCloseness) ||
                !IsCameraAxisBetweenPositions(mainCamera.transform.position.y, desiredPosition.y + maxCloseness, desiredPosition.y - maxCloseness))
            {
                // Smooth Damp
                if (movementStyle == MovementStyle.SmoothDamp)
                {
                    mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, desiredPosition, ref dampVelocity, dampSpeed, 100, Time.deltaTime);
                }

                // Lerp
                else if (movementStyle == MovementStyle.Lerp)
                {
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed);
                }                   
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
