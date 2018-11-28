using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public bool deactivated = false;
	public bool delay = false;
	public int damage;

	void Start() {
		if (delay) {
			StartCoroutine(DelayActivation());
		}
	}

	private IEnumerator DelayActivation() {
		deactivated = true;
		yield return new WaitForSeconds(0.75f);
		deactivated = false;
	}
}
