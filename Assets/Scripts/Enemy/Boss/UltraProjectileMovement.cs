using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraProjectileMovement : MonoBehaviour {

    Vector3 direction;
    public float speed;
    float modifierX, modifierZ;

	void Start () 
    {
        //the following will raise errors if we name the player object anything else than Player
        direction = GameObject.Find("Player").transform.position-transform.position;
        direction = direction.normalized;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (other.name == "Player")
        {
            Destroy(gameObject);
            //damage player
        } 
    }
	
	void Update () 
    {
        transform.position += direction * speed*Time.deltaTime;
        Destroy(gameObject, 5); //failsafe if projectiles not destroyed by walls
	}
}
