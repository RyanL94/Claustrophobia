using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGolem : Enemy {
    Animator animator;
    bool idle;
    bool walking;
    bool punching;
    bool dead;
    bool bigRight;
    bool bigLeft;
    bool smashing;
    bool stomping;
    bool turnLeft;
    bool sweeping;
    bool jumping;

    public static bool bossActive;

    Damageable healthGetter;    //todo : death anim
    float bossHealth;

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
    int meleeCooldown;
    [SerializeField]
    int rangedCooldown;
    [SerializeField]
    int specialCooldown;
    [SerializeField]
    float attackRange;
    [SerializeField]
    float chaseDistance;
    [SerializeField]
    GameObject Projectile, meleeHitbox;

    //val stuff
    public Vector3 direction, mazeDirection; //these are only public for testing purposes. Make private on release.

    int meleeTimer, rangedTimer, specialTimer;
    bool doingMelee;

    public float deathHealth;

    static Vector3 destination;
    private float distanceToTarget;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        AnimReset();
        walking = true;
        meleeTimer = 0;
        rangedTimer = 0;
        specialTimer = 0;
        bossActive = false;
        doingMelee = false;

        healthGetter = GetComponent<Damageable>();
    }

    void Update()
    {
        if (bossActive == true && doingMelee == false) Pathfinding();

        bossHealth = healthGetter.health;

        if (bossHealth <= deathHealth)
        {
            bossActive = false;
            AnimReset();
            walking = false;
            dead = true;
            Destroy(gameObject, 5);
        }

        if (meleeTimer < meleeCooldown)
        {
            meleeTimer++;
        }

        if (doingMelee == true && meleeTimer == 130)
        {
            //Debug.Log("attempting to spawn melee hitbox with meleetimer = " + meleeTimer);
            Instantiate(meleeHitbox, transform.position, Quaternion.identity);
        }
        else if (doingMelee == true && meleeTimer == 215)
        {
            doingMelee = false;
        }

        if (rangedTimer < rangedCooldown)
        {
            rangedTimer++;
        }

        if (specialTimer < specialCooldown)
        {
            specialTimer++;
        }

        //Debug.Log("Walking = " + walking);
        //Debug.Log("Punching = " + punching);
        //Debug.Log("Big Right = " + bigRight);

        UpdateAnimator();
        /*if (bossHealth == 0 && dead == false)
        {
            AnimReset();
            dead = true;
        }*/
    }

    void UpdateAnimator() // does this need to be updated every frame or just initialized ?
    {
        animator.SetBool("isIdle", idle);
        animator.SetBool("isWalking", walking);
        animator.SetBool("isPunching", punching);
        animator.SetBool("isDead", dead);
        animator.SetBool("isSweepRight", bigRight);
        animator.SetBool("isSweepLeft", bigLeft);
        animator.SetBool("isSmashing", smashing);
        animator.SetBool("isStomping", stomping);
        animator.SetBool("isTurningLeft", turnLeft);
        animator.SetBool("isSweeping", sweeping);
        animator.SetBool("isJumpingUp", jumping);
    }

    void Pathfinding()
    {
        destination = GameObject.Find("Player").transform.position;
        distanceToTarget = Vector3.Distance(destination, transform.position);
        
        /*RaycastHit hit;
        

        Vector3 forward = transform.position + transform.forward; //what does this do ?

        if (Physics.Raycast(transform.position, (destination - transform.position), out hit, distanceToTarget))
        {
            if (hit.transform.tag != "Player" && hit.transform.tag != "Enemy" && hit.transform.tag != "EnemyProjectile")
            {
                transform.position += (direction.normalized * speed * 0.5f * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                Debug.Log("case3");
            }
            else
            {*/
        if (distanceToTarget < attackRange)
        {
            meleeAttack();
        }
        else if (distanceToTarget < chaseDistance)
        {
            //Debug.Log("walk towards player");
            AnimReset();
            walking = true;
            transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * 0.75f * Time.deltaTime);
        }
        else
        {
            rangedAttack();
        }
        transform.rotation = Quaternion.LookRotation(GameObject.Find("Player").transform.position - transform.position, Vector3.up); //face player
    }
      /*  }
    }*/

    void rangedAttack()
    {
        if (rangedTimer >= rangedCooldown)
        {
            if (punching == false) AnimReset();
            punching = true;
            Instantiate(Projectile, transform.position, Quaternion.identity);
            //Debug.Log("ranged attack");
            rangedTimer = 0;
        }
    }

    void meleeAttack()
    {
        if (meleeTimer >= meleeCooldown)
        {
            //Debug.Log("melee attack");
            if (bigRight == false) AnimReset();
            bigRight = true;
            meleeTimer = 0;
            doingMelee = true;
        }
        
    }

    void AnimReset()
    {
        idle = false;
        //walking = false;
        punching = false;
        dead = false;
        bigRight = false;
        bigLeft = false;
        smashing = false;
        stomping = false;
        turnLeft = false;
        sweeping = false;
        jumping = false;
    }
}
