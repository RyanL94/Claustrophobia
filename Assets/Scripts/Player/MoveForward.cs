using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {

	public float bullet_speed=6.0f;
    public int ricochey;
    public GameObject bulletExplosion;
    public GameObject bulletRicochet;
    public float bulletDistance;

    private Vector3 oldVelocity;
    private Rigidbody rb;
    private float lifeTime;

    public static float incSpee;


    void Start()
	{
        rb = GetComponent<Rigidbody>();
        MoveFoward();
        lifeTime = Time.time + bulletDistance;
        ricochey = PlayerController.collision;
        incSpee = 50.0f;
    }

	void FixedUpdate () {
        //if life time
        if (Time.time > lifeTime && bulletDistance != 0)
        {
            Explode();
        }
        oldVelocity = rb.velocity;


    }
    //make bullet move towards direction it faces
    void MoveFoward()
    {
        incSpee = PlayerController.incSpeed;
        rb.velocity =  transform.forward* bullet_speed * Time.deltaTime* incSpee;

    }
    //on collision reflect or destroys
    void OnCollisionEnter(Collision collision)
    {
        if (ricochey < 1)
        {
            Explode();
        }
        else
        {
            //instantiate ricochet sprite
            if (bulletRicochet != null)
            {
                Instantiate(bulletRicochet, transform.position, Quaternion.identity);
            }
            //do reflect
            ContactPoint contact = collision.contacts[0];
            Vector3 reflectedVelocity = Vector3.Reflect(oldVelocity, contact.normal);
            rb.velocity = reflectedVelocity;
            //rotate object
            Quaternion rotation = Quaternion.FromToRotation(oldVelocity, reflectedVelocity);
            transform.rotation = rotation * transform.rotation;

            ricochey--;
        }
    }
    //destroy bullet
    void Explode()
    {
        if (bulletExplosion != null)
        {
            Instantiate(bulletExplosion, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
