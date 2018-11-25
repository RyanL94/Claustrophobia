using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedEnemy : Enemy
{

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

    int cooldownTimer, attentionSpan;


    static Vector3 destination;
    private float distanceToTarget;

    void Start()
    {
        cooldownTimer = 0;
        attentionSpan = 180;
    }

    void Update()
    {
        if (target != null)
        {
            Pathfinding();
        }

        if (cooldownTimer < attackCooldown)
        {
            cooldownTimer++;
        }

    }

    void Pathfinding()
    {
        RaycastHit hit;
        destination = GameObject.Find("Player").transform.position;
        distanceToTarget = Vector3.Distance(destination, transform.position);

        Vector3 forward = transform.position + transform.forward; //what does this do ?

        if (distanceToTarget < aggroRange)
        {
            if (Physics.Raycast(transform.position, (destination-transform.position), out hit, distanceToTarget))
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
                    if (distanceToTarget < kiteDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, -speed * 0.75f * Time.deltaTime);
                      //  Debug.Log("kite the player");
                    }
                    else if (distanceToTarget >= approachDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * 0.75f * Time.deltaTime);
                       // Debug.Log("approach player");
                    }
                  //  else Debug.Log("Maintain position");
                    Attack();
                    transform.rotation = Quaternion.LookRotation(GameObject.Find("Player").transform.position - transform.position, Vector3.up);
                }
            }

        }
        else
        {
            transform.position += (direction.normalized * speed * 0.5f * Time.deltaTime);
          //  Debug.Log("maze movement");
            if (direction != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }

        // prevent the kiting movement from moving the enemy upwards
        transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
    }

    void Attack()
    {
        if (cooldownTimer == attackCooldown)
        {
            Instantiate(Projectile, transform.position, Quaternion.identity);
          //  Debug.Log("ranged attack");
            cooldownTimer = 0;
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Ground(Clone)")
        {
            //Debug.Log("enemy is in and distance is " + (other.transform.position - transform.position).magnitude);
            if ((other.transform.position - transform.position).magnitude <= 0.8f)
            {
                //Debug.Log("changing mazedirection!");
                RedirectorCells cellRedirector = other.GetComponent<RedirectorCells>();
                direction = cellRedirector.reDirection;
            }
        }
    }
}
