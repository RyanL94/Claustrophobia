using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {

    public LayerMask collision;
	public float bullet_speed=6.0f;
    public int ricochey;

    Vector3 Velocity;
   
	void Start()
	{
        Velocity = new Vector3(0, 0, bullet_speed*Time.deltaTime);
       
	}

	void FixedUpdate () {

        Move();
        Reflect();
        	
	}



    void Move (){


        Vector3 pos = transform.position;

        pos += transform.rotation * Velocity;

        transform.position = pos;
    }

    //see vector3.Reflect

    void Reflect(){

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Time.deltaTime * bullet_speed +.2f , collision))//if a collision occurs with the layer "Wall"
        {
           

            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal); //find the reflection direction
            float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;//find the angle of direction through the direction
            transform.eulerAngles = new Vector3(0, rot, 0); //put the angle as tranform's rotation


            ricochey--;

            if (ricochey == 0)
            {

                Destroy(gameObject);
                ricochey = 3;


            }

        }
    
    }

    void OnCollisionEnter(Collision collision)
    {
        if (ricochey < 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //do reflect
            ricochey--;
        }
    }

}
