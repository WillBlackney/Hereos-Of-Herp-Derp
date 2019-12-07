using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingScript : MonoBehaviour {

    float Countdown = 2.0f;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {

        Countdown -= Time.deltaTime;

        if (Countdown <= 0.0f)
        {
            GameObject Target = GameObject.FindWithTag("Enemy");
            transform.LookAt(Target.transform);
            transform.Rotate(0, 0, 0);
        }
	}
}
