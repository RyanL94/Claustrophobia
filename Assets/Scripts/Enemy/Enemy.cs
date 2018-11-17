using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public static float knockback = 0.15f;

    protected Transform target;
	protected Damageable damageable;

	void Awake() {
		var game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        target = game.player.transform;
		damageable = GetComponent<Damageable>();
		damageable.OnDamage(Knockback);
	}

	private void Knockback() {
		transform.Translate(Vector3.back * knockback);
	}
}
