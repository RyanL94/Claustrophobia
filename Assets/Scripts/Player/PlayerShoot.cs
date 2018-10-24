using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public GameObject bulletPrefab;
    public float fireDelay = 0.25f;
    public int ricochey;

    Vector3 bullet_offset;
    float cooldownTimer = 0;
    private GameObject player;
    

	// Use this for initialization
	void Start () {

        bullet_offset = new Vector3(0, 0, 0);
        player = GameObject.Find("Player");

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(Input.GetButton("Fire1") && cooldownTimer < Time.time){

            cooldownTimer = fireDelay + Time.time; 
            GameObject bulletObject = (GameObject)Instantiate(bulletPrefab, transform.position + bullet_offset, transform.rotation);
            Physics.IgnoreCollision(bulletObject.GetComponent<Collider>(), player.GetComponent<Collider>());
            bulletObject.GetComponent<MoveForward>().ricochey = ricochey;
        }
    }



}
