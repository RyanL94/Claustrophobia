using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectorCells : MonoBehaviour {

    private float fComparator, startRando; //used to assign a random direction to each cell until they are assigned that of the player
    private Vector3 comparator; //Used to assign the player's path to the cell
    public Vector3 reDirection;
    private bool finalized=false;

    void Start ()
    {
        InvokeRepeating("shuffleCells", 0.0f, 2.0f);
    }

    void shuffleCells ()
    {
        if (finalized == false)
        {
            startRando = Random.Range(0.0f, 1.0f);
            if (startRando > 0.75f) reDirection = new Vector3(1.0f, 0.0f, 0.0f);
            else if (startRando > 0.50f) reDirection = new Vector3(-1.0f, 0.0f, 0.0f);
            else if (startRando > 0.25) reDirection = new Vector3(0.0f, 0.0f, 1.0f);
            else reDirection = new Vector3(0.0f, 0.0f, -1.0f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            comparator = other.transform.position - transform.position;
            if (comparator.x >= 0.75f) reDirection = new Vector3(1.0f, 0.0f, 0.0f);
            else if (comparator.x <= -0.75f) reDirection = new Vector3(-1.0f, 0.0f, 0.0f);
            else if (comparator.z >= 0.75f) reDirection = new Vector3(0.0f, 0.0f, 1.0f);
            else if (comparator.z <= -0.75f) reDirection = new Vector3(0.0f, 0.0f, -1.0f);
            finalized = true;
        }
    }
}
