using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {

	public GameObject panel;
	public Text label;
	public Vector3 offset;

	void Start() {
		panel.SetActive(false);
	}

	// Display an in-game pop-up containing the text at the given position.
	public void Display(string text, Vector3 position) {
		transform.position = position + offset;
		panel.SetActive(true);
		label.text = text;
	}

	// Hide the pop-up text.
	public void Hide() {
		panel.SetActive(false);
	}
}