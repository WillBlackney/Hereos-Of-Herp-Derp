using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControl : MonoBehaviour {
    Vector3 targetPos;
    Vector3 lastPosition;
    Vector3 startPosition;
    bool done = false;
    float distanceTravelled = 0;


    // Use this for initialization
    void Start () {
        done = false;
        lastPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        //Aim player at mouse
        //which direction is up
        Vector3 upAxis = new Vector3(0, 0, -1);
        Vector3 mouseScreenPosition = Input.mousePosition;
        //set mouses z to your targets
        mouseScreenPosition.z = transform.position.z;
        Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        transform.LookAt(mouseWorldSpace, upAxis);
        //zero out all rotations except the axis I want
        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);

        targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = transform.position.z;
        //targetPos.x = Mathf.Clamp(targetPos.x, 0, 20);
        //targetPos.y = Mathf.Clamp(targetPos.y, 0, 0);
        float move = 2*Time.deltaTime;
        //distanceTravelled += Vector3.Distance(targetPos, transform.position);
        //lastPosition = transform.position;
        //print("last"+lastPosition);
        //print("Travel" + distanceTravelled);
        //print("start" + startPosition);
        print(done);

        if (Input.GetButton("Fire1") && done==false)
        {
            keepStartposition();
            distanceTravelled += Vector3.Distance(targetPos, transform.position);
            lastPosition = transform.position;

            if (distanceTravelled < 35 && done==true)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, move);
            }

        }
        if (Input.GetButtonUp("Fire1"))
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, move);
        }

        //transform.position = Vector3.Lerp(transform.position, -targetPos, move);


    }

    void keepStartposition()
    {
        startPosition = transform.position;
        done = true;

    }
}
