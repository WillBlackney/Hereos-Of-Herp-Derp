using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Animator myAnim;
    public Vector3 destination;
    public bool readyToMove;
    public bool destinationReached;
    public float travelSpeed;
    public GameObject imageParent;

    private void Update()
    {
        if (readyToMove)
        {
            MoveTowardsTarget();
        }
    }
    public void InitializeSetup(Vector3 startPos, Vector3 endPos, float speed)
    {
        transform.position = startPos;
        destination = endPos;
        travelSpeed = speed;
        FaceDestination();
        readyToMove = true;
    }
    public void MoveTowardsTarget()
    {
        if(transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, travelSpeed * Time.deltaTime);
            if(transform.position == destination)
            {
                destinationReached = true;
                DestroySelf();
            }
        }
    }
    public void FaceDestination()
    {
        Vector2 direction = destination - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10000f);
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    
}
