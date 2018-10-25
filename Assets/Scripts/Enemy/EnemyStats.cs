using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    [SerializeField]
    float EnemyHealth;
    [SerializeField]
    float EnemyDamage;
    [SerializeField]
    float MovementSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TakeDamage (float damage)
    {
        EnemyHealth -= damage;
    }
}
