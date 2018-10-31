using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {
    public float health = 1; //Default value. Can be changed per item in the editor.
    public List<string> collisionTags;
    public GameObject onDamageEffect;
    public GameObject onDeathEffect;
    public Vector3 effectOffset;
    public float effectScale;

    public static float immuneDuration = 0.5f;

    private float immuneUntil;
    private GameController game;
    private PlayerController player;

    void Awake() {
        game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        player = game.player.GetComponent<PlayerController>();
    }
	
    void Start() {
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

    private void CheckCollision(GameObject collider) {
        if (collisionTags.Contains(collider.tag) && Time.time > immuneUntil) {
            var attack = collider.gameObject.GetComponent<Attack>();
            health -= attack.damage;
            immuneUntil = Time.time + immuneDuration;
            DisplayEffect(onDamageEffect);
            if (health <= 0) {
                Destroy(gameObject);
                DisplayEffect(onDeathEffect);
                if (gameObject.tag == "Player") {
                    game.OnPlayerDeath();
                }
            }
            if (collider.name == "MeleeAttack" && gameObject.tag == "Enemy") {
                player.OnSuccessfulHit();
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
}
