using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {
    public Rigidbody playerBody;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public float speed;
    public float turnSpeed;
    // Use this for initialization
    private Vector3 forwardVec = new Vector3(1, 0, 0);
    void Start () {
		
	}
    void FixedUpdate()
    {
        if (Input.GetKey(up))
        {
            Vector3 forward = speed * forwardVec;
            playerBody.AddForce(forward);
        }
        else if (Input.GetKey(down))
        {
            Vector3 backward = -1 * speed * forwardVec;
            playerBody.AddForce(backward);
        }

        if (Input.GetKey(right))
        {
            transform.Rotate(Vector3.left * turnSpeed);
        }
        else if (Input.GetKey(left))
        {
            transform.Rotate(Vector3.right * turnSpeed);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
