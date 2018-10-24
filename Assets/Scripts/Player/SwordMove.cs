using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMove : MonoBehaviour {

    public float swingDelay = 0.25f;
    public float speed = 5;
    public int lifeSpand = 25;
    private GameObject player;
	
    void Start()
    {
        player = GameObject.Find("Player");
    }

	// Update is called once per frame
	void FixedUpdate () {

        if (player != null && lifeSpand > 0)
        {
            lifeSpand--;
            transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.Self);
            transform.position = player.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
