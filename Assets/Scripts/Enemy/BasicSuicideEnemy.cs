using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSuicideEnemy : Enemy
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

    public GameObject explosion;

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
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Player")
        {
            //Debug.Log("boom");
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
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
