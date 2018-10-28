using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

    public float maxSpeed = 5;
    public float minSpeed = 1;
    public float acceleration = 0.1f;
    public float angleSpeed = 500;
    public float dashSpeed = 10;
    public float dashTime = 0.25f;
    public GameObject sword;
    public float swordDelay = 1;

    private float swordDelayTime;
    private float startDashTime;
    private Rigidbody rBody;
    private float speed;
    private Vector3 movement;
    private Rigidbody playerRigidbody;
    private bool dash = false;
    private Vector3 direction;
    private PlayerShoot playerShoot;

    // Use this for initialization
    void Start ()
    {
        speed = maxSpeed;
        rBody = GetComponent<Rigidbody>();
        playerShoot = GetComponentInChildren <PlayerShoot>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        PlayerInput();

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
        if (Input.GetButton("Fire1"))
        {

            Decelarate();
        }

        //swing sword reduces speed
        if (Input.GetButton("Fire2") && swordDelayTime < Time.time)
        {

            swordDelayTime = Time.time + swordDelay;
            GameObject swordObject = (GameObject) Instantiate(sword, transform.position, transform.rotation);
            Physics.IgnoreCollision(swordObject.GetComponent<Collider>(), GetComponent<Collider>());

            reloadAmmo();

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

    public void reloadAmmo()
    {
        playerShoot.currentAmmo += 5;

        if (playerShoot.currentAmmo > 20)
        {
            playerShoot.currentAmmo = 20;
        }
    }


}
