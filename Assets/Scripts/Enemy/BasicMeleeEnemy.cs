using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : MonoBehaviour {

    [SerializeField]
    Transform target;
    [SerializeField]
    float rotationalDamp;
    [SerializeField]
    float rayCastOffset;
    [SerializeField]
    float rayDistance;
    [SerializeField]
    float rotate;
    [SerializeField]
    float speed;
    [SerializeField]
    int attackCooldown;

    public Vector3 direction, mazeDirection;
    int cooldownTimer;

    static Vector3 destination;
    private float distanceToTarget;

    //together stuff
    [SerializeField]
    private bool inRoom = false;

    void Start()
    {
        cooldownTimer = 0;
    }

    void Update()
    {
        if (target != null)
        {
            Pathfinding();
            Move();
        }

        if (cooldownTimer < attackCooldown)
        {
            cooldownTimer++;
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
    }

    void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        //Debug.Log(speed);
    }

    void Pathfinding()
    {
        if (inRoom == true)
        {
            float leftRay = 0.99f * rayDistance;
            RaycastHit hit;
            Vector3 raycastOffset = Vector3.zero;

            Vector3 left = transform.position - transform.right * rayCastOffset;
            Vector3 right = transform.position + transform.right * rayCastOffset;
            Vector3 forward = transform.position + transform.forward * rayCastOffset;
            Vector3 backward = transform.position - transform.forward * rayCastOffset;

            Debug.DrawRay(left, transform.forward * rayDistance, Color.green);
            Debug.DrawRay(right, transform.forward * rayDistance, Color.green);
            Debug.DrawRay(forward, transform.forward * rayDistance, Color.green);
            Debug.DrawRay(backward, -transform.forward * rayDistance, Color.green);

            if (Physics.Raycast(left, transform.forward, out hit, leftRay))
            {
                raycastOffset += Vector3.up;
            }
            else if (Physics.Raycast(right, transform.forward, out hit, rayDistance))
            {
                raycastOffset -= Vector3.up;
            }

            if (Physics.Raycast(forward, transform.forward, out hit, rayDistance))
            {

            }
            else if (Physics.Raycast(backward, -transform.forward, out hit, rayDistance))
            {

            }

            if (raycastOffset != Vector3.zero)
            {
                transform.Rotate(raycastOffset * rotate * Time.deltaTime);
            }
            else
            {
                Turn();
            }
        }
        else
        {
            direction = GameObject.Find("Player").transform.position - transform.position;
            distanceToTarget = (direction).magnitude;
            if (distanceToTarget < 1.0f)
            {
                Attack();
                //Debug.Log("melee range");
            }
            else if (distanceToTarget < 8.0f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                //Debug.Log("close in for melee attack");
            }
            else
            {
                transform.position += (mazeDirection * speed * 0.3f * Time.deltaTime);
                //Debug.Log("maze movement");
            }
        }
    }

    void Attack()
    {
        if (cooldownTimer == attackCooldown)
        {
            //melee attack
            // Debug.Log("melee attack");
            cooldownTimer = 0;
        }

    }
}
