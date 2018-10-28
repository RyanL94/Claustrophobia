using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour {

    public GameObject bulletPrefab;
    public float fireDelay = 0.25f;
    public int ricochey;
    public float bulletDistance;
    public int ammoClip;
    public int currentAmmo;
    public Text ammoText;

    Vector3 bullet_offset;
    float cooldownTimer = 0;
    private GameObject player;
    

	// Use this for initialization
	void Start () {
        bullet_offset = new Vector3(0, 0, 0);
        player = GameObject.Find("Player");
        currentAmmo = ammoClip;
        ammoText.text = currentAmmo.ToString() + "/" + ammoClip.ToString();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        ammoText.text = currentAmmo.ToString() + "/" + ammoClip.ToString();
        //fire gun
        if (Input.GetButton("Fire1") && cooldownTimer < Time.time && currentAmmo > 0){

            currentAmmo--;
            cooldownTimer = fireDelay + Time.time; 
            GameObject bulletObject = (GameObject)Instantiate(bulletPrefab, transform.position + bullet_offset, transform.rotation);
            Physics.IgnoreCollision(bulletObject.GetComponent<Collider>(), player.GetComponent<Collider>());
            bulletObject.GetComponent<MoveForward>().ricochey = ricochey;
            bulletObject.GetComponent<MoveForward>().bulletDistance = bulletDistance;
        }
    }
}
