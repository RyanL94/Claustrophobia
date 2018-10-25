using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public float speed;

	private TerrainManager terrain;

	void Start() {
		terrain = GameObject.Find("Terrain").GetComponent<TerrainManager>();
	}

	void Update() {
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.Translate(Vector3.back * speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		}
	}

	void OnCollisionEnter(Collision collider) {
		terrain.Break(LayoutGrid.FromWorldPosition(collider.transform.position));
	}
}
