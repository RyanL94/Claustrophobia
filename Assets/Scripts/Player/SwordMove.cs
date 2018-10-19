using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMove : MonoBehaviour {

    public Animator sMove;

    float cooldownTimer = 0;

    float fireDelay = 0.25f;
	
	// Update is called once per frame
	void Update () {

        cooldownTimer -= Time.deltaTime; //How much time it's been since entering the frame,it's negative



        if(Input.GetButtonDown("Fire2") && cooldownTimer <= 0){

            cooldownTimer = fireDelay; 
            sMove.SetTrigger("Swing");


        }

		
	}
}
