using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayerScript : MonoBehaviour {

    public Transform player;
    private Vector3 offset;
    // Use this for initialization

    void Awake () {
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
