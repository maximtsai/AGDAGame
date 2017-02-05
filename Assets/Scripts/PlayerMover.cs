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

    void Start () {
        playerBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(up))
        {
            Vector2 forward = acceleration * forwardVec;
            if (playerBody.velocity.sqrMagnitude > maxSpeed)
            {
                forward = forward * (maxSpeed+15 - playerBody.velocity.sqrMagnitude)/ 15;
            }
            Debug.Log(playerBody.velocity.sqrMagnitude);
            playerBody.AddForce(forward);
        }
        else if (Input.GetKey(down))
        {
            Vector2 backward = -1 * acceleration * forwardVec;
            if (playerBody.velocity.sqrMagnitude > maxSpeed)
            {
                backward = backward * (maxSpeed+10 - playerBody.velocity.sqrMagnitude) / 10;
            }
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
