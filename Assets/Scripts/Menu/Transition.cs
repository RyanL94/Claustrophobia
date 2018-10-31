using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour {

	public float duration;

	private Animator animator;
	public Image image;
	
	void Awake() {
		animator = GetComponent<Animator>();
		image = GetComponent<Image>();
	}

	public void FadeIn() {
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Opaque")) {
			image.enabled = true;
			animator.CrossFadeInFixedTime("Opaque", duration);
		}
	}

	public void FadeOut() {
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Transparent")) {
			StartCoroutine(FadeOutHelper());
		}
	}

	private IEnumerator FadeOutHelper() {
		animator.CrossFadeInFixedTime("Transparent", duration);
		yield return new WaitForSeconds(duration);
		image.enabled = false;
	}
}
