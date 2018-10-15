using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float speed;
	private TerrainManager terrain;

	void Start() {
		terrain = GameObject.Find("Terrain").GetComponent<TerrainManager>();
		var center = terrain.FindCenterPosition();
		transform.Translate(new Vector3(center.x, center.y, 0.0f));
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			terrain.GenerateFloor();
		}
		if (Input.GetKeyDown(KeyCode.X)) {
			terrain.Place(terrain.standardBlock, terrain.FindCenterPosition());
		}
		if (Input.GetKeyDown(KeyCode.Z)) {
			terrain.Break(terrain.FindCenterPosition());
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