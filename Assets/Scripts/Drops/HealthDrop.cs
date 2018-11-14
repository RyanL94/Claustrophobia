using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : Drop {

	public int health;

	protected override void Take() {
		base.Take();
		var currentHealth = game.player.GetComponent<Damageable>().health;
		var healedHealth = Mathf.Min(currentHealth + health, game.hud.healthBar.maxValue);
		game.player.GetComponent<Damageable>().health = healedHealth;
	}
}