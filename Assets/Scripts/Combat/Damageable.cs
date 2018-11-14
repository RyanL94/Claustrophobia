using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {
    public bool invulnerable = false;
    public float health = 1; //Default value. Can be changed per item in the editor.
    public List<string> collisionTags;
    public GameObject onDamageEffect;
    public GameObject onDeathEffect;
    public Vector3 effectOffset;
    public float effectScale;
    public AudioClip onDamageSound;
    public AudioClip onDeathSound;
    public float soundVolume;

    public static float immuneDuration = 0.5f;

    private float immuneUntil;
    private Action onDamageAction;
    private Action onDeathAction;
    private GameController game;

    void Start() {
        game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        immuneUntil = Time.time;
    }

    // if colliding remove health
    void OnCollisionEnter(Collision collider) {        
        CheckCollision(collider.gameObject);
    }

    void OnCollisionStay(Collision collider) {
        CheckCollision(collider.gameObject);
    }

    void OnTriggerEnter(Collider collider) {
        CheckCollision(collider.gameObject);
    }

    public void OnDamage(Action action) {
        onDamageAction = action;
    }

    public void OnDeath(Action action) {
        onDeathAction = action;
    }

    private void CheckCollision(GameObject collider) {
        if (!invulnerable && collisionTags.Contains(collider.tag) && Time.time > immuneUntil) {
            var attack = collider.gameObject.GetComponent<Attack>();
            if (attack.activated) {
                return;
            }
            health -= attack.damage;
            immuneUntil = Time.time + immuneDuration;
            if (onDamageAction != null) {
                onDamageAction();
            }
            PlaySound(onDamageSound);
            DisplayEffect(onDamageEffect);
            if (health <= 0) {
                PlaySound(onDeathSound);
                DisplayEffect(onDeathEffect);
                if (onDeathAction != null) {
                    onDeathAction();
                } else {
                    gameObject.transform.parent = null;
                    Destroy(gameObject);
                }
            }
            if (collider.name == "MeleeAttack" && gameObject.tag == "Enemy") {
                game.player.OnSuccessfulHit();
            }
        }
    }

    private void DisplayEffect(GameObject effect, bool child=false) {
        if (effect != null) {
            var instance = Instantiate(effect, transform.position + effectOffset, effect.transform.rotation);
            instance.transform.localScale *= effectScale;
            if (child) {
                instance.transform.SetParent(transform);
            }
        }
    }

    private void PlaySound(AudioClip clip) {
        if (clip != null) {
            AudioSource.PlayClipAtPoint(clip, transform.position, soundVolume);
        }
    }
}
