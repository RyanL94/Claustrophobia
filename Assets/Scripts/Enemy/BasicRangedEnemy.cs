using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedEnemy : MonoBehaviour {

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
    [SerializeField]
    GameObject Projectile;

    //val stuff
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
        //movementSpeed += 0.001f;
        transform.position += transform.forward * speed * Time.deltaTime;
        //Debug.Log(movementSpeed);
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
            //RaycastHit hit;

            /*
			//destination = toChase.transform.position;
            distanceToTarget = (destination - transform.position).magnitude;
            if (distanceToTarget < 0.15) Debug.Log("case 1"); //attack
            else if (distanceToTarget < 0.4f && Physics.Linecast(transform.position, destination, out hit))
            {
                if (hit.transform.name == "Target")
                {
                    transform.position = Vector3.MoveTowards(transform.position, destination, speed*Time.deltaTime);
                    //Debug.Log(toChase.transform.position);
                    Debug.Log("ww" + destination);
                }
            }
            else
            {
                transform.Translate(direction * speed * Time.deltaTime);
                Debug.Log("case 3");
            }*/

            direction = GameObject.Find("Player").transform.position - transform.position;
            distanceToTarget = (direction).magnitude;
            if (distanceToTarget < 8.0f)// && Physics.Linecast(transform.position, destination, out hit))
            {
                //if (hit.transform.name == "Player")
                //{
               if (distanceToTarget < 4.0f)
               {
                     transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, -speed * 0.5f * Time.deltaTime);
                     //Debug.Log("kite the player");
               }
               else if (distanceToTarget >= 6.0f)
               {
                     transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * 0.5f * Time.deltaTime);
                     //Debug.Log("approach player");
               }
               Attack();
                // }
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
            Instantiate(Projectile, transform.position, Quaternion.identity);
            //Debug.Log("ranged attack");
            cooldownTimer = 0;
        }

    }
}
