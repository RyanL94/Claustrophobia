﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 5;
    public float minSpeed = 1;
    public float acceleration = 0.1f;
    public float angleSpeed = 500;
    public float dashSpeed = 10;
    public float dashTime = 0.25f;
    public float swordDelay = 1;
    public float swingDuration;
    public float swingDelay;

    public GameObject meleeAttack;
    public GameObject bulletPrefab;
    public GameObject gunBarel;
    
    public float ammo;
    public float ammoGainPerHit;
    public float fireDelay = 0.25f;
    public int ricochey;
    public float bulletDistance;

    public Animator playerAnimator;

    float cooldownTimer = 0;
    private GameObject lookAtMouseRotation;

    private float maxAmmo;
    private float swordDelayTime;
    private float startDashTime;
    private Rigidbody rBody;
    private float speed;
    private Vector3 movement;
    private Rigidbody playerRigidbody;
    private bool dash = false;
    private Vector3 direction;
    private GameObject faceTowards;

    // Use this for initialization
    void Start ()
    {
        speed = maxSpeed;
        rBody = GetComponent<Rigidbody>();
        faceTowards = GameObject.Find("CartPassenger");
        lookAtMouseRotation = GameObject.Find("GunEnd");
        maxAmmo = ammo;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        PlayerInput();

    }

    public void OnSuccessfulHit()
    {
        ammo = Mathf.Min(ammo += ammoGainPerHit, maxAmmo);
    }

    private void PlayerInput()
    {
        //make player dash at direction facing
        Dash();

        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector3.forward;
            MoveTowards();
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = Vector3.back;
            MoveTowards();
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector3.left;
            MoveTowards();
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.right;
            MoveTowards();
        }

        //fired gun reduces speed
        if (Input.GetButton("Fire2") && cooldownTimer < Time.time && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordSwing"))
        {
            if (ammo >= 1) {
                playerAnimator.Play("FireGun");
                Fire();
                --ammo;
            }
            Decelarate();
        }

        //swing sword reduces speed
        if (Input.GetButton("Fire1") && swordDelayTime < Time.time)
        {
            StartCoroutine(Swing());
            Decelarate();
        }

        //make player do a dash
        if (Input.GetKeyDown(KeyCode.Space) && !dash)
        {
            dash = true;
            startDashTime = Time.time + dashTime;
            Decelarate();
        }
    }

    //move and face towards direction
    private void MoveTowards()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        movement = new Vector3(horizontal, 0.0f, vertical);
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
        Accelerate();
    }
    //make player dash at direction
    private void Dash()
    {
        if (dash && Time.time < startDashTime)
        {
            transform.Translate(direction * dashSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            dash = false;
        } 
    }

    private void Fire()
    {
        cooldownTimer = fireDelay + Time.time;
        GameObject bulletObject = (GameObject)Instantiate(bulletPrefab, gunBarel.transform.position, lookAtMouseRotation.transform.rotation);
        Physics.IgnoreCollision(bulletObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        bulletObject.GetComponent<MoveForward>().ricochey = ricochey;
        bulletObject.GetComponent<MoveForward>().bulletDistance = bulletDistance;

    }

    // gradualy increse movement speed to max
    private void Accelerate()
    {
        speed += acceleration;

        if (speed > maxSpeed)
        {
            speed = maxSpeed;

        }
    }
    // decrease speed to min
    public void Decelarate(){

        speed = minSpeed;

    }

    // swing the melee weapon
    private IEnumerator Swing() {
        var swordCollider = meleeAttack.GetComponent<Collider>();
        swordDelayTime = Time.time + swordDelay;
        playerAnimator.Play("SwordSwing");
        yield return new WaitForSeconds(swingDelay);
        swordCollider.enabled = true;
        yield return new WaitForSeconds(swingDuration);
        swordCollider.enabled = false;        
    }
}