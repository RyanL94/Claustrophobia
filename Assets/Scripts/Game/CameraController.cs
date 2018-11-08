using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float speed;
	public Vector3 offset;

	private GameController game;
	private Animator animator;
	private Transform player;

	void Start() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		animator = GetComponent<Animator>();
		player = game.player.transform;
		CenterOnPlayer();
	}
	
	void FixedUpdate() {
		if (player != null) {
			var movement = Time.deltaTime * speed;
			transform.position = new Vector3(
				Mathf.Lerp(transform.position.x, player.position.x, movement),
				transform.position.y,
				Mathf.Lerp(transform.position.z, player.position.z, movement)
			);
			Vector3.Lerp(transform.position, player.position, movement);
			transform.position += offset;
		}
	}

	public void CenterOnPlayer() {
		if (player != null) {
			transform.position = offset + new Vector3(
				player.position.x,
				transform.position.y,
				player.position.z
			);
		}
	}

	public void Shake() {
		animator.CrossFade("Shake", 0.0f);
	}
}