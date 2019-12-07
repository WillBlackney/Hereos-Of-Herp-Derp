using UnityEngine;
using System.Collections;

public class tazo_rotate : MonoBehaviour {

		public float rotate_x = 0f;
		public float rotate_y = 0f;
		public float rotate_z = 0f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
				transform.Rotate(rotate_x*Time.deltaTime,rotate_y*Time.deltaTime,rotate_z*Time.deltaTime);
	}
}
