using UnityEngine;
using System.Collections;

public class LookAtMouseScript : MonoBehaviour
{
    private Vector3 mousePositon;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateToMouse();

    }

    private void RotateToMouse()
    {

        mousePositon = Input.mousePosition;
        mousePositon.z = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
        mousePositon = Camera.main.ScreenToWorldPoint(mousePositon);
        transform.LookAt(mousePositon);
    }

}