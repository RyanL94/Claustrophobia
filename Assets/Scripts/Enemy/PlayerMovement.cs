using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    float movementSpeed;

    void Update()
    {
        Move();
    }

    void Move()
    {
        if(transform.position.x >= 4 && transform.position.z < 4)
        {
            transform.Translate(new Vector3(0,0,1) * movementSpeed * Time.deltaTime);
        }
        if (transform.position.z >= 4 && transform.position.x > -4)
        {
            transform.Translate(new Vector3(-1,0,0) * movementSpeed * Time.deltaTime);
        }
        if (transform.position.x <= -4 && transform.position.z > -4)
        {
            transform.Translate(new Vector3(0,0,-1) * movementSpeed * Time.deltaTime);
        }
        if (transform.position.z <= -4 && transform.position.x < 4)
        {
            transform.Translate(new Vector3(1,0,0) * movementSpeed * Time.deltaTime);
        }
    }
}
