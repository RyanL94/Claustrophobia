using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    public new string name;
    public string description;
    public GameObject effect;

    public float health;
    public float maxSpeed;
    public float minSpeed;
    public float dashTime;
    public float swordDelay; // must have a min limit
    public int ricochet;
    public float precision;
    public int bulletNumber;
    public float ammo;
    public float ammoGainPerHit;
    public float fireDelay;
    public float bulletDistance;
    public int gunDamage;
    public int swordDamage;

    private int cost;
    private GameController game;

    void Start() {
        game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            game.hud.popUp.Display(name, transform.position);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            game.hud.popUp.Hide();
        }
    }

    void OnTriggerStay(Collider collider) {
        if (collider.gameObject.tag == "Player" && Input.GetButtonDown("Fire1")) {
            PlayerController player = game.player;
            if (player.money >= cost) {
                player.money -= cost;
                // change player input
                // have to set min and max values
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

                // change player health
                collider.gameObject.GetComponent<Damageable>().health += health;
                game.hud.healthBar.maxValue += health;
                Destroy(gameObject);
                game.hud.popUp.Hide();

                Instantiate(effect, player.transform);
            }
        }
    }

    public void SetCost(int cost) {
        this.cost = cost;
    }
}
