using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    protected Transform target;

	void Awake() {
		var game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        target = game.player.transform;
	}
}
