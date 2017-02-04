using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {
    Rigidbody2D playerBody;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public float speed;
    public float turnSpeed;
    // Use this for initialization
	private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector2 forwardVec = new Vector3(1, 0);

    void Start () {
        playerBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
		//this.transform.rotation.eulerAngles
        if (Input.GetKey(up))
        {
            Vector2 forward = speed * forwardVec;
            playerBody.AddForce(forward);
        }
        else if (Input.GetKey(down))
        {
            Vector2 backward = -1 * speed * forwardVec;
            playerBody.AddForce(backward);
        }

        if (Input.GetKey(right))
        {
            playerBody.AddTorque(-turnSpeed, ForceMode2D.Force);
            //playerBody.Rotate(0.0f, 0.0f, -turnSpeed);
			forwardVec.Set(Mathf.Cos (playerBody.rotation * DEG_TO_RAD), Mathf.Sin (playerBody.rotation * DEG_TO_RAD));
        }
        else if (Input.GetKey(left))
        {
            playerBody.AddTorque(turnSpeed, ForceMode2D.Force);// (0.0f, 0.0f, turnSpeed);
			forwardVec.Set(Mathf.Cos (playerBody.rotation * DEG_TO_RAD), Mathf.Sin (playerBody.rotation * DEG_TO_RAD));
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
