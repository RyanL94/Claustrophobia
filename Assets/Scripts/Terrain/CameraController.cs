using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float speed;

	private Transform player;

	void Start() {
		player = GameObject.FindWithTag("Player").transform;
		CenterOnPlayer();
	}
	
	void FixedUpdate() {
		var movement = Time.deltaTime * speed;
		transform.position = new Vector3(
			Mathf.Lerp(transform.position.x, player.position.x, movement),
			transform.position.y,
			Mathf.Lerp(transform.position.z, player.position.z, movement)
		);
		Vector3.Lerp(transform.position, player.position, movement);
	}

	public void CenterOnPlayer() {
		transform.position = new Vector3(
			player.position.x,
			transform.position.y,
			player.position.z
		);
	}
}