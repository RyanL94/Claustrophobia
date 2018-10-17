using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float rotationalDamp;
    [SerializeField]
    float rayCastOffset;
    [SerializeField]
    float rayDistance;
	
	void Update ()
    {
        if (target != null)
        {
            Turn();
            Move();
            Pathfinding();
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player1")
        {
            Destroy(col.gameObject);
        }
    }

    void Turn()
    {
        Vector3 pos = (target.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
        Debug.Log(pos);
    }

    void Move()
    {
        movementSpeed += 0.001f;
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
        Debug.Log(movementSpeed);
    }

    void Pathfinding()
    {
        RaycastHit hit;
        Vector3 raycastOffset = Vector3.zero;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 forward = transform.position + transform.forward * rayCastOffset;
        Vector3 backward = transform.position - transform.forward * rayCastOffset;

        Debug.DrawRay(left, -transform.right * rayDistance, Color.green);
        Debug.DrawRay(right, transform.right * rayDistance, Color.green);
        Debug.DrawRay(forward, transform.forward * rayDistance, Color.green);
        Debug.DrawRay(backward, -transform.forward * rayDistance, Color.green);

        if(Physics.Raycast(left, -transform.right, out hit, rayDistance))
        {
            
        }
        else if(Physics.Raycast(right, transform.right, out hit, rayDistance))
        {
            
        }

        if (Physics.Raycast(forward, transform.forward, out hit, rayDistance))
        {
            
        }
        else if (Physics.Raycast(backward, -transform.forward, out hit, rayDistance))
        {
            
        }

        if(raycastOffset != Vector3.zero)
        {
            
        }
        else
        {

        }
    }
}
