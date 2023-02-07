using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleControls : MonoBehaviour
{
    private float paddleSpeed = 1.0f;
    private Vector3 up;
    private Vector3 down;
    private GameObject _p1;
    private GameObject _p2;
    private Rigidbody rb1;
    private Rigidbody rb2;

    // Start is called before the first frame update
    void Start()
    {
        _p1 = GameObject.Find("Paddle1");
        _p2 = GameObject.Find("Paddle2");
        up = new Vector3(0.0f, 0.0f, paddleSpeed);
        down = new Vector3(0.0f, 0.0f, -paddleSpeed);
        rb1 = _p1.GetComponent<Rigidbody>();
        rb2 = _p2.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W)) 
        {
            rb1.AddForce(up, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb1.AddForce(down, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb2.AddForce(up, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb2.AddForce(down, ForceMode.VelocityChange);
        }
    }
}
