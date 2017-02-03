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
	private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector3 forwardVec = new Vector3(1, 0, 0);
	private float angle = 0.0f;

    void Start () {
		
	}
    void FixedUpdate()
    {
		//this.transform.rotation.eulerAngles
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
            //transform.Rotate(Vector3.left * turnSpeed);
			transform.Rotate(0.0f, 0.0f, -turnSpeed);
			angle = (angle - turnSpeed) % 360.0f;
			forwardVec.Set (Mathf.Cos (angle * DEG_TO_RAD), Mathf.Sin (angle * DEG_TO_RAD), 0);

        }
        else if (Input.GetKey(left))
        {
			transform.Rotate(0.0f, 0.0f, turnSpeed);
			angle = (angle + turnSpeed) % 360.0f;
			forwardVec.Set (Mathf.Cos (angle * DEG_TO_RAD), Mathf.Sin (angle * DEG_TO_RAD), 0);

        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
