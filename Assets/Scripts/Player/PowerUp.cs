﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    public new string name;
    public string description;
    public GameObject effect;
    public AudioClip sound;

    public float health;
    public float maxSpeed;
    public float minSpeed;
    public float dashTime;
    public float swordDelay;
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
            var text = name;
            if (cost != 0) {
                text += string.Format(" ({0}g)", cost);
            }
            game.hud.popUp.Display(text, transform.position);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            game.hud.popUp.Hide();
        }
    }

    void OnTriggerStay(Collider collider) {
        if (collider.gameObject.tag == "Player" && Input.GetButtonDown("Fire1")) {
            if (game.player.money >= cost) {
                Take();
            }
        }
    }

    public void SetCost(int cost) {
        this.cost = cost;
    }

    private void Take() {
        PlayerController player = game.player;
        player.money -= cost;
        // change player input
        // have to set min and max values
        player.maxSpeed += maxSpeed;
        player.minSpeed += minSpeed;
        player.dashTime += dashTime;
        player.swordDelay += swordDelay;
        player.ricochet += ricochet;
        player.precision += precision;
        if (player.precision < 0)
        {
            player.precision = 0;
        }
        player.bulletNumber += bulletNumber;
        if (player.bulletNumber < 1)
        {
            player.bulletNumber = 1;
        }
        player.maxAmmo += ammo;
        player.ammo += ammo;
        player.ammoGainPerHit += ammoGainPerHit;
        player.fireDelay += fireDelay;
        player.bulletDistance += bulletDistance;
        player.gunDamage += gunDamage;
        player.swordDamage += swordDamage;

        // change player health
        player.GetComponent<Damageable>().health = Mathf.Max(player.GetComponent<Damageable>().health + health, 1);
        game.hud.healthBar.maxValue += health;
        game.hud.ammoBar.maxValue += ammo;
        game.hud.ForceUpdate();
        transform.parent = null;
        Destroy(gameObject);
        
        Instantiate(effect, player.transform);
        AudioSource.PlayClipAtPoint(sound, transform.position, 0.25f);
        game.hud.popUp.Hide();
        game.hud.DisplayItem(name, description);
    }
}
