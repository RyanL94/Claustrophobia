using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : MonoBehaviour
{

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

    //val stuff
    public Vector3 direction, mazeDirection; //these are only public for testing purposes. Make private on release.

    int cooldownTimer, attentionSpan;

    public int aggroRange;

    static Vector3 destination;
    private float distanceToTarget;

    //together stuff
    [SerializeField]
    private bool inRoom = false;

    void Start()
    {
        attentionSpan = 180;
        cooldownTimer = 0;
    }

    void Update()
    {
        if (target != null)
        {
            Pathfinding();
          //  Move();
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
        Vector3 pos = GameObject.Find("Player").transform.position;
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
        /*if (inRoom == true)
        {
            /* float leftRay = 0.99f * rayDistance;
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

            distanceToTarget = (GameObject.Find("Player").transform.position - transform.position).magnitude;
            if (distanceToTarget < 5.0f)// && Physics.Linecast(transform.position, destination, out hit))
            {
                transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * Time.deltaTime);
                Debug.Log("approach player");
                Attack();
                transform.rotation = Quaternion.LookRotation(GameObject.Find("Player").transform.position - transform.position, Vector3.up);
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
            }

            distanceToTarget = (GameObject.Find("Player").transform.position - transform.position).magnitude;
            if (distanceToTarget < 5.0f)// && Physics.Linecast(transform.position, destination, out hit))
            {
                transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * Time.deltaTime);
                Debug.Log("approach player");
                Attack();
                transform.rotation = Quaternion.LookRotation(GameObject.Find("Player").transform.position - transform.position, Vector3.up);
            }
            else
            {
                transform.position += (direction.normalized * speed * 0.5f * Time.deltaTime);
                Debug.Log("maze movement");
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            }

        }*/

        RaycastHit hit;
        destination = GameObject.Find("Player").transform.position;
        distanceToTarget = Vector3.Distance(destination, transform.position);

        Vector3 forward = transform.position + transform.forward; //what does this do ?

        if (distanceToTarget < aggroRange)
        {
            if (Physics.Raycast(transform.position, (destination - transform.position), out hit, distanceToTarget))
            {
                if (hit.transform.tag != "Player" && hit.transform.tag != "Enemy" && hit.transform.tag != "EnemyProjectile" && attentionSpan == 0)
                {
                    transform.position += (direction.normalized * speed * 0.5f * Time.deltaTime);
                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                    //  Debug.Log("case3");
                }
                else
                {
                    if (attentionSpan == 0) attentionSpan = 180;
                    if (hit.transform.tag != "Player" && hit.transform.tag != "Enemy" && hit.transform.tag != "EnemyProjectile") attentionSpan--;
                    transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * 0.75f * Time.deltaTime);
                   // Debug.Log("approach player");
                    transform.rotation = Quaternion.LookRotation(GameObject.Find("Player").transform.position - transform.position, Vector3.up);
                }
            }
        }
        else
        {
            transform.position += (direction.normalized * speed * 0.5f * Time.deltaTime);
           // Debug.Log("maze movement");
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        }
    }

    void Attack()
    {
        if (cooldownTimer == attackCooldown)
        {
            //special attack effects go here, if we wanna add stuff other than charging
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Ground(Clone)")
        {
            //Debug.Log("enemy is in and distance is " + (other.transform.position - transform.position).magnitude);
            if ((other.transform.position - transform.position).magnitude <= 0.85)
            {
                //Debug.Log("changing mazedirection!");
                RedirectorCells cellRedirector = other.GetComponent<RedirectorCells>();
                direction = cellRedirector.reDirection;
            }
        }
    }
}
