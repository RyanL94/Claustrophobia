using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileMovement : MonoBehaviour {

    Vector3 direction;
    public float speed;
    float modifierX, modifierZ;

	void Start () 
    {
        //Modifier variables make enemies inaccurate and allows them to sometimes hit moving players (borrowed from Metroid Prime)
		modifierX = Random.Range(-0.75f, 0.75f);
        modifierZ = Random.Range(-0.75f, 0.75f);
        //the following will raise errors if we name the player object anything else than Player
        direction = GameObject.Find("Player").transform.position-transform.position;
        direction.x += modifierX;
        direction.z += modifierZ;
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
