using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float speed;
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			GameObject.Find("Terrain").GetComponent<Terrain>().GenerateFloor();
		}

		var direction = new Vector3(0.0f, 0.0f, 0.0f);
		if (Input.GetKey(KeyCode.KeypadPlus)) {
			direction.z += 1.0f;
		}
		if (Input.GetKey(KeyCode.KeypadMinus)) {
			direction.z -= 1.0f;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			direction.y += 1.0f;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			direction.y -= 1.0f;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			direction.x += 1.0f;
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			direction.x -= 1.0f;
		}
		var movement = direction * speed * Time.deltaTime;
		transform.Translate(movement);
	}
}