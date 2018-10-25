using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAndDestroy : MonoBehaviour {
    public float health = 1;
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

                if (deathParticle != null)
                {
                    Instantiate(deathParticle, transform.position, transform.rotation);
                }

            }
        }
    }
}
