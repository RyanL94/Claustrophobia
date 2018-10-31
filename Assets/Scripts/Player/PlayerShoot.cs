using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour {

    public GameObject bulletPrefab;
    public GameObject gunBarel;

    public float fireDelay = 0.25f;
    public int ricochey;
    public float bulletDistance;
    public int ammoClip;
    public int currentAmmo;
    public Text ammoText;

    float cooldownTimer = 0;
    private GameObject lookAtMouseRotation;
    private Animator playerAnimator;

    // Use this for initialization
    void Start () {

        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();
        lookAtMouseRotation = GameObject.Find("GunEnd");

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //fire gun
        if (Input.GetButton("Fire1") && cooldownTimer < Time.time){

            cooldownTimer = fireDelay + Time.time;
            playerAnimator.SetTrigger("FireGun");
            GameObject bulletObject = (GameObject)Instantiate(bulletPrefab, gunBarel.transform.position, lookAtMouseRotation.transform.rotation);
            Physics.IgnoreCollision(bulletObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            bulletObject.GetComponent<MoveForward>().ricochey = ricochey;
            bulletObject.GetComponent<MoveForward>().bulletDistance = bulletDistance;
        }
    }
}
