using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedEnemy : MonoBehaviour
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
    [SerializeField]
    float aggroRange;
    [SerializeField]
    float kiteDistance;
    [SerializeField]
    float approachDistance;
    [SerializeField]
    GameObject Projectile;

    //val stuff
    public Vector3 direction, mazeDirection; //these are only public for testing purposes. Make private on release.

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
            //Move();
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

    void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        //Debug.Log(speed);
    }

    void Pathfinding()
    {
        RaycastHit hit;
        destination = GameObject.Find("Player").transform.position;
        distanceToTarget = Vector3.Distance(destination, transform.position);

        Vector3 forward = transform.position + transform.forward;
        Debug.DrawRay(forward, (destination - transform.position) * distanceToTarget, Color.green);
        Debug.Log(distanceToTarget);

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

        if (inRoom == true && distanceToTarget < aggroRange)
        {
            if (Physics.Raycast(forward, transform.forward, out hit, distanceToTarget))
            {
                if (hit.transform.tag != "Player")
                {

                }
                else
                {
                    if (distanceToTarget < kiteDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, -speed * 0.75f * Time.deltaTime);
                        Debug.Log("kite the player");
                    }
                    else if (distanceToTarget >= approachDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * 0.75f * Time.deltaTime);
                        Debug.Log("approach player");
                    }
                    Attack();
                    transform.rotation = Quaternion.LookRotation(GameObject.Find("Player").transform.position - transform.position, Vector3.up);
                }
            }

        }
        else
        {
            transform.position += (direction.normalized * speed * 0.5f * Time.deltaTime);
            Debug.Log("maze movement");
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        }
    }

    void Attack()
    {
        if (cooldownTimer == attackCooldown)
        {
            Instantiate(Projectile, transform.position, Quaternion.identity);
            Debug.Log("ranged attack");
            cooldownTimer = 0;
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Ground(Clone)")
        {
            //Debug.Log("enemy is in and distance is " + (other.transform.position - transform.position).magnitude);
            if ((other.transform.position - transform.position).magnitude <= 0.85)
            {
                Debug.Log("changing mazedirection!");
                RedirectorCells cellRedirector = other.GetComponent<RedirectorCells>();
                direction = cellRedirector.reDirection;
            }
        }
    }
}
