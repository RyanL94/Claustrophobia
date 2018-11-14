using UnityEngine;
using System.Collections;

public class BossMeleeHitbox : MonoBehaviour
{
    int lifetime, damage;
    // Use this for initialization
    void Start()
    {
        lifetime = 0;
        //Debug.Log("melee hitbox active");
    }

    // Update is called once per frame
    void Update()
    {
        lifetime++;
        if (lifetime >= 5) Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            //damage player
            //Debug.Log("player damage from boss melee attack !");
            Destroy(gameObject);
        }
    }
}
