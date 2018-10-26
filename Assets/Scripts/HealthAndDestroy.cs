using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAndDestroy : MonoBehaviour {
    public float dropRate, health = 1; //Default value. Can be changed per item in the editor.
    public GameObject dropped; //instantiate droppables in the editor using this. Eg : walls can drop enemies, enemies can drop gold ...
    public string collisionTag;
    public GameObject deathParticle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}
    // if colliding remove health
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == collisionTag)
        {
            health--;
            if (health == 0)
            {
                Destroy(gameObject);

                if (Random.Range(0.0f, 100.0f) < dropRate)
                {
                    Instantiate(dropped, transform.position, Quaternion.identity);
                }

                if (deathParticle != null)
                {
                    
                    Instantiate(deathParticle, transform.position, transform.rotation);
                }

            }
        }
    }
}
