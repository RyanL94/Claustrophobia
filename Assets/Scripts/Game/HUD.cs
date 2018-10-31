using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Slider healthBar;
	public Slider ammoBar;
	public float updateSpeed;

	private GameController game;
	private GameObject player;
	private Damageable playerHealth;
	private PlayerController playerController;

	void Awake() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		player = game.player;
		playerHealth = player.GetComponent<Damageable>();
		playerController = player.GetComponent<PlayerController>();
		healthBar.maxValue = playerHealth.health;
		healthBar.value = playerHealth.health;
		ammoBar.maxValue = playerController.ammo;
		ammoBar.value = playerController.ammo;
	}
	
	void Update() {
		var progression = Time.deltaTime * updateSpeed;
		if (playerHealth.health != healthBar.value) {
			healthBar.value = Mathf.Lerp(healthBar.value, playerHealth.health, progression);
		}
		if (playerController.ammo != ammoBar.value) {
			ammoBar.value = Mathf.Lerp(ammoBar.value, playerController.ammo, progression);
		}
	}
}
