using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public GameObject bulletPrefab;

    Vector3 bullet_offset;

    float cooldownTimer = 0;

    float fireDelay = 0.25f;

	// Use this for initialization
	void Start () {

        bullet_offset = new Vector3(0, 0, 0);

	}
	
	// Update is called once per frame
	void Update () {

        cooldownTimer -= Time.deltaTime; //How much time it's been since entering the frame,it's negative


        if(Input.GetButton("Fire1") && cooldownTimer <= 0){

            cooldownTimer = fireDelay; 
            Instantiate(bulletPrefab, transform.position + bullet_offset, transform.rotation);

        }
		
	}


    void Kill(){



        Destroy(gameObject, 1.5f);
    }
}
