using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class button_target : MonoBehaviour {
	public GameObject MY_target;
	GameObject temp_target;
	GameObject[] ALL_target;

	// Use this for initialization
	void Start () {
		this.transform.GetChild (0).GetComponent<Text>().text = MY_target.name;
		//print (this.transform.GetChild (0).GetComponent<Text>().text);
		if (ALL_target == null)
			ALL_target = GameObject.FindGameObjectsWithTag("TAZOFX");
		foreach (GameObject tt in ALL_target)
		{
			tt.SetActive (false);

		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void ShowTarget(){
		ALL_target = GameObject.FindGameObjectsWithTag("TAZOFX");
		foreach (GameObject tt in ALL_target)
		{
			tt.SetActive (false);
		
		}
	
		MY_target.SetActive (true);

		}
}
