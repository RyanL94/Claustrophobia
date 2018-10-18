using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect : MonoBehaviour {

    public Vector3 redirector;
	void Start ()
    {
        if (transform.name == "WestRedirectors") redirector = new Vector3(1.0f, 0.0f, 0.0f);
        else if (transform.name == "SouthRedirectors") redirector = new Vector3(0.0f, 0.0f, 1.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Baddie")
        {
            EnemyBehavior toChange = other.GetComponent<EnemyBehavior>();
            toChange.direction = redirector;
        }
        
    }
}
