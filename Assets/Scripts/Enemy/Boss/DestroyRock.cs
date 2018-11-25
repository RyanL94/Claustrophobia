using UnityEngine;
using UnityEditor;

public class DestroyRock : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -1.0f) Destroy(gameObject);
    }
}