using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayerScript : MonoBehaviour {

    private Transform player;
    private Vector3 offset;
    // Use this for initialization

    void Awake () {
        player = GameObject.FindWithTag("Player").transform;
        offset = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void FixedUpdate () {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
	}
}
