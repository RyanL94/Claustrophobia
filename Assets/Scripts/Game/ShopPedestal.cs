using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPedestal : MonoBehaviour {

	private GameController game;

	void Start() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		var powerUp = RandomPicker.Pick(game.powerUps, true);
		var cost = Random.Range(game.itemCost.min, game.itemCost.max + 1);
		var spawnLocation = powerUp.transform.position + transform.position;		
		var instance = Instantiate(powerUp, spawnLocation, powerUp.transform.rotation);
		instance.GetComponent<PowerUp>().SetCost(cost);
		instance.transform.parent = transform;
	}
}