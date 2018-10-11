using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

    public float maxSpeed = 5;
    public float minSpeed = 1;
    public float acceleration = 0.1f;
    public float angleSpeed = 500;

    private float speed;
    private Vector3 movement;
    private Rigidbody playerRigidbody;


	// Use this for initialization
	void Start ()
    {
        speed = maxSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        PlayerInput();

    }

    private void PlayerInput()
    {

        if (Input.GetKey(KeyCode.W))
        {
            MoveTowards(Vector3.forward);

        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveTowards(Vector3.back);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveTowards(Vector3.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveTowards(Vector3.right);
        }

        //if fired
        if (Input.GetMouseButton(0))
        {

            //decelerate
            speed = minSpeed;
        }

    }

    private void MoveTowards(Vector3 direction)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        
        movement = new Vector3(horizontal, 0.0f, vertical);
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), angleSpeed * Time.deltaTime);    
        Accelerate();
    }

    private void Accelerate()
    {
        speed += acceleration;

        if (speed > maxSpeed)
        {
            speed = maxSpeed;

        }
    }

}
