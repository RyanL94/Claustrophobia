using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {

    Vector3 direction;
    public float speed;
    float modifierX, modifierZ;

	void Start () 
    {
		modifierX = Random.Range(-5.0f, 5.0f);
        modifierZ = Random.Range(-5.0f, 5.0f);
        //this will raise errors if we name the player object anything else than Player
        direction = GameObject.Find("Player").transform.position - transform.position;
        direction = new Vector3(direction.x+modifierX, 0.0f, direction.z + modifierZ);
	}
	
	void Update () 
    {
        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * Time.deltaTime);
        Debug.Log("test");
	}
}
