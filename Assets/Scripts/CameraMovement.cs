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

    public Camera mainCamera;
    public CinemachineVirtualCamera cinemachineCamera;
    public GameObject CamerasParent;    

    [Header("Edge Colliders / View Boundary Objects")]
    public GameObject northCollider;
    public GameObject southCollider;
    public GameObject eastCollider;
    public GameObject westCollider;   
    
    void Start()
    {
        offset = new Vector3(0, 0, -10);
        cinemachineCamera.transform.position = new Vector3(0, 0, -10);
        mainCamera = Camera.main;
        SetPreferedOrthographicSize(4);
    }

    public void SetPreferedOrthographicSize(float newSize)
    {
        Debug.Log("SetPreferedOrthographicSize() called and changed to: " + newSize.ToString());
        float size = newSize;

        if (size < 2)
        {
            size = 2;
        }
        else if(size > 4)
        {
            size = 4;
        }

        currentOrthoSize = size;        
    }

    public void ClearLookAtTarget()
    {
        CameraManager.Instance.currentLookAtTarget = null;
        //Debug.Log("ClearLookAtTarget() called...");
    }

    public void LookAtTarget()
    {
        if(CameraManager.Instance.currentLookAtTarget != null)
        {
            Vector3 desiredPosition = CameraManager.Instance.currentLookAtTarget.transform.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(cinemachineCamera.transform.position, desiredPosition, smoothSpeed);
            cinemachineCamera.transform.position = smoothPosition;
            if (cinemachineCamera.transform.position == desiredPosition)
            {
                Debug.Log("Camera has reached LookAtTarget() position");
                ClearLookAtTarget();
            }
        }
    }

    public void HandleZoomInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Debug.Log("HandleZoomInput() detected zoom IN input");
            SetPreferedOrthographicSize(currentOrthoSize - 0.1f);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Debug.Log("HandleZoomInput() detected zoom OUT input");
            SetPreferedOrthographicSize(currentOrthoSize + 0.1f);
        }
    }

    public void MoveTowardsLeftRightUpDown()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ClearLookAtTarget();
            if (IsGameObjectVisible(northCollider) == false)
            {
                cinemachineCamera.transform.position += Vector3.up * Time.deltaTime * cameraMoveSpeed;
            }

        }
        if (Input.GetKey(KeyCode.S))
        {
            ClearLookAtTarget();
            if (IsGameObjectVisible(southCollider) == false)
            {
                cinemachineCamera.transform.position += Vector3.down * Time.deltaTime * cameraMoveSpeed;
            }

        }
        if (Input.GetKey(KeyCode.A))
        {
            ClearLookAtTarget();
            if (IsGameObjectVisible(westCollider) == false)
            {
                cinemachineCamera.transform.position += Vector3.left * Time.deltaTime * cameraMoveSpeed;
            }

        }
        if (Input.GetKey(KeyCode.D))
        {
            ClearLookAtTarget();
            if (IsGameObjectVisible(eastCollider) == false)
            {
                cinemachineCamera.transform.position += Vector3.right * Time.deltaTime * cameraMoveSpeed;
            }

        }
    }

    public void MoveTowardsZoomPosition()
    {
        if(cinemachineCamera.m_Lens.OrthographicSize != currentOrthoSize)
        {
            Debug.Log("MoveTowardsZoomPosition() detected that camera zoom is not equal to desired zoom size, adjusting...");


            // Zoom in smoothly     
            if(cinemachineCamera.m_Lens.OrthographicSize > currentOrthoSize)
            {
                cinemachineCamera.m_Lens.OrthographicSize -= 0.05f;
            }
            else if (cinemachineCamera.m_Lens.OrthographicSize < currentOrthoSize)
            {
                cinemachineCamera.m_Lens.OrthographicSize += 0.05f;
            }
            /*
                if (cinemachineCamera.m_Lens.OrthographicSize < 2 == false)
                {
                    cinemachineCamera.m_Lens.OrthographicSize -= 0.01f;

                    if (cinemachineCamera.m_Lens.OrthographicSize < 2)
                    {
                        cinemachineCamera.m_Lens.OrthographicSize = 2;
                    }
                }
            

            // Zoom out smoothly
            
            
                else if (cinemachineCamera.m_Lens.OrthographicSize > 4 == false)
                {
                    cinemachineCamera.m_Lens.OrthographicSize += 0.01f;

                    if (cinemachineCamera.m_Lens.OrthographicSize > 4)
                    {
                        cinemachineCamera.m_Lens.OrthographicSize = 4;
                    }
                }
                */

        }
    }

    private void Update()
    {
        HandleZoomInput();
    }

    void LateUpdate()
    {
        LookAtTarget();        
        MoveTowardsZoomPosition();
        MoveTowardsLeftRightUpDown();

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
    
}
