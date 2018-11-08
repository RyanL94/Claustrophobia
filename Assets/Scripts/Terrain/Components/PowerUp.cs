using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public string name;
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

    private GameObject list;
    //ToDo: health and damage

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //get player script
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            //change player input
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


            Destroy(gameObject);

        }
    }
}
