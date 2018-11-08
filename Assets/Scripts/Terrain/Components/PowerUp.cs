using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public bool open = false;

    public string name;
    public int cost;
    public float health;
    public float maxSpeed;
    public float minSpeed;
    public float dashTime;
    public float swordDelay; //must have a min limit
    public int ricochet;
    public float precision;
    public int bulletNumber;
    public float ammo;
    public float ammoGainPerHit;
    public float fireDelay;
    public float bulletDistance;
    public int gunDamage;
    public int swordDamage;

    private GameObject list;
    //ToDo: health and damage

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && open)
        {
            //get player script
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            if (player.money >= cost)
            {
                player.money -= cost;
                //change player input
                //have to set min and max values
                player.maxSpeed += maxSpeed;
                player.minSpeed += minSpeed;
                player.dashTime += dashTime;
                player.swordDelay += swordDelay;
                player.ricochet += ricochet;
                player.precision += precision;
                player.bulletNumber += bulletNumber;
                player.ammo += ammo;
                player.ammoGainPerHit += ammoGainPerHit;
                player.fireDelay += fireDelay;
                player.bulletDistance += bulletDistance;
                player.gunDamage += gunDamage;
                player.swordDamage += swordDamage;

                //change player health
                col.gameObject.GetComponent<Damageable>().health += health;
                Destroy(gameObject);
            }
        }
    }
}
