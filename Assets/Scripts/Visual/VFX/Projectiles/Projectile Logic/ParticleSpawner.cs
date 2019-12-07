using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public Transform SpawnPos;
    private int WeaponType = 0;
    public Rigidbody rocketPrefab;
    public Rigidbody projectile;
    public float fireDelta = 0.5F;
    private float nextFire = 0.5F;
    private float myTime = 0.0F;
    private Rigidbody newProjectile;
    public Rigidbody SpawnAtmouse;
    public GameObject Homing;

    public Rigidbody[] Weapon1;
    public Rigidbody[] Weapon2;
    public Rigidbody[] Weapon3;
    public GameObject[] Weapon4;
    public int currentWeapon = 0;
    private int nrWeapons;
    
    public float Travelspeed = 300;

    // Use this for initialization
    void Start () {
        WeaponType = 1;
        currentWeapon = 0;
        nrWeapons = Weapon1.Length;
    }
	


	// Update is called once per frame
	void Update ()
    {
        

        //Select Weapon Type
        if (Input.GetKeyDown("2"))
        {
            WeaponType = 1;
            currentWeapon = 0;
        }
        if (Input.GetKeyDown("3"))
        {
            WeaponType = 2;
            currentWeapon = 0;
        }
        if (Input.GetKeyDown("4"))
        {
            WeaponType = 3;
            currentWeapon = 0;
        }
        if (Input.GetKeyDown("5"))
        {
            WeaponType = 4;
            currentWeapon = 0;
        }
        //Speed Change
        if (Input.GetKeyDown("z")) { Travelspeed += 50; }
        if (Input.GetKeyDown("x")) { Travelspeed -= 50; }
        //Change Weapon
        if (Input.GetKeyDown("d")) {
            currentWeapon++;
            SwitchWeapon(currentWeapon);
        }
        if (Input.GetKeyDown("a"))
        {
            currentWeapon--;
            SwitchWeapon(currentWeapon);
        }

        myTime = myTime + Time.deltaTime; //for Hold Click

        //Left click
        if (Input.GetButtonDown("Fire1") && WeaponType == 1)
        {
            Rigidbody rocketInstance;

            rocketInstance = Instantiate(Weapon1[currentWeapon], SpawnPos.position,SpawnPos.rotation)as Rigidbody; //Weapon1[currentWeapon]
            rocketInstance.AddForce(SpawnPos.right * -Travelspeed); //rocketPrefab

        }
        //Hold Click
        if (Input.GetButton("Fire1") && myTime > nextFire && WeaponType == 2)
        {

            nextFire = myTime + fireDelta;
            newProjectile = Instantiate(Weapon2[currentWeapon], transform.position, transform.rotation) as Rigidbody;
            projectile.AddForce(SpawnPos.right * -Travelspeed);
            newProjectile.AddForce(SpawnPos.right * -Travelspeed);

            nextFire = nextFire - myTime;
            myTime = 0.0F;

        }
        //Spawn at mouse click
        if (Input.GetButtonDown("Fire1") && WeaponType == 3)
        {
            Rigidbody spawnInstance;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10.0f; //m away from camera position if your camera z position = -5 then it will be 5
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            spawnInstance = Instantiate(Weapon3[currentWeapon], worldPos, Quaternion.identity) as Rigidbody; 
            spawnInstance.AddForce(transform.up * Travelspeed);
        }
        //Freeze Direction Particles  
        if (Input.GetButtonDown("Fire1") && WeaponType == 4)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10.0f; //m away from camera position if your camera z position = -5 then it will be 5
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Instantiate(Weapon4[currentWeapon], worldPos, Quaternion.identity);
        }
        
    }

    void SwitchWeapon(int index)
    {

        for (int i = 0; i < nrWeapons; i++)
        {
            if (i == index)
            {
                Weapon1[i].gameObject.SetActive(true);;
            }
        }
    }


}
