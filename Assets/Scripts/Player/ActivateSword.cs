using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSword : MonoBehaviour {

    public Animator playerAnimator;
    private Collider coll;

    // Use this for initialization
    void Start () {

		coll = gameObject.GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordSwing"))
        {
            coll.enabled = true;
        }
        else
        {
            coll.enabled = false;
        }

    }
}
