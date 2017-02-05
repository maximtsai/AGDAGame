using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {
    Rigidbody2D playerBody;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public float acceleration;
    public float maxSpeed;
    public float turnSpeed;
    // Use this for initialization
	private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector2 forwardVec = new Vector3(1, 0);
    private bool forwardPressed = false;
    void Start () {
        playerBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(up))
        {
            forwardPressed = true;
            Vector2 forward = acceleration * forwardVec;
            if (playerBody.velocity.sqrMagnitude > maxSpeed)
            {
                // soft cap for max speed
                forward = forward * (maxSpeed+15 - playerBody.velocity.sqrMagnitude)/ 15;
            }
            playerBody.AddForce(forward);
        }
        else
        {
            forwardPressed = false;
        }
        /*
        else if (Input.GetKey(down))
        {
            forwardPressed = true;
            Vector2 backward = -1 * acceleration * forwardVec;
            if (playerBody.velocity.sqrMagnitude > maxSpeed)
            {
                // soft cap for max speed
                backward = backward * (maxSpeed+10 - playerBody.velocity.sqrMagnitude) / 10;
            }
            playerBody.AddForce(backward);
        }
        */


        if (Input.GetKey(right))
        {
            if (forwardPressed)
            {
                // turn slower while moving
                playerBody.AddTorque(-turnSpeed * 0.5f, ForceMode2D.Force);
            } else
            {
                playerBody.AddTorque(-turnSpeed, ForceMode2D.Force);
            }
            //playerBody.Rotate(0.0f, 0.0f, -turnSpeed);
            forwardVec.Set(Mathf.Cos (playerBody.rotation * DEG_TO_RAD), Mathf.Sin (playerBody.rotation * DEG_TO_RAD));
        }
        else if (Input.GetKey(left))
        {
            if (forwardPressed)
            {
                // turn slower while moving
                playerBody.AddTorque(turnSpeed * 0.5f, ForceMode2D.Force);
            }
            else
            {
                playerBody.AddTorque(turnSpeed, ForceMode2D.Force);
            }
			forwardVec.Set(Mathf.Cos (playerBody.rotation * DEG_TO_RAD), Mathf.Sin (playerBody.rotation * DEG_TO_RAD));
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
