using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectorCells : MonoBehaviour {

    private float fComparator, startRando; //used to assign a random direction to each cell until they are assigned that of the player
    private Vector3 comparator; //Used to assign the player's path to the cell
    public Vector3 direction;
    void Start ()
    {
        startRando = Random.Range(0.0f, 1.0f);
        if (startRando > 0.75f) direction = new Vector3(1.0f, 0.0f, 0.0f);
        else if (startRando > 0.50f) direction = new Vector3(-1.0f, 0.0f, 0.0f);
        else if (startRando > 0.25) direction = new Vector3(0.0f, 0.0f, 1.0f);
        else direction = new Vector3(0.0f, 0.0f, -1.0f);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            comparator = other.transform.position - transform.position;
            if (comparator.x >= 0.75f) direction = new Vector3(1.0f, 0.0f, 0.0f);
            else if (comparator.x <= -0.75f) direction = new Vector3(-1.0f, 0.0f, 0.0f);
            else if (comparator.z >= 0.75f) direction = new Vector3(0.0f, 0.0f, 1.0f);
            else if (comparator.z <= -0.75f) direction = new Vector3(0.0f, 0.0f, -1.0f);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "MeleeEnemy" || other.tag == "RangedEnemy")
        {
            Debug.Log("enemy is in and distance is " + (other.transform.position - transform.position).magnitude);
            if ((other.transform.position - transform.position).magnitude <= 0.8)
            {
                Debug.Log("changing mazedirection!");
                EnemyMovement enemyToRedirect = other.GetComponent<EnemyMovement>();
                enemyToRedirect.mazeDirection = direction;
            }
        }

    }
}
