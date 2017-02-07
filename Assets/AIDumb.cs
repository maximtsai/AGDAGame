using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDumb : MonoBehaviour {
    public GameObject player;
    private Rigidbody2D playerBody;
    private Rigidbody2D selfBody;
    private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private float acceleration = 10;
    private float maxSpeed = 50;
    private Vector2 forwardVec;
    private bool goingForward = false;
    private float angleToPlayer = 0;
    private float angleChange = 0;
    bool turningRight = false;
    // Use this for initialization
    void Start () {
        selfBody = GetComponent<Rigidbody2D>();
        playerBody = player.GetComponent<Rigidbody2D>();
        forwardVec = new Vector2(Mathf.Cos(selfBody.rotation * DEG_TO_RAD), Mathf.Sin(selfBody.rotation * DEG_TO_RAD));
        angleToPlayer = Mathf.Atan2((playerBody.position.y - selfBody.position.y), (playerBody.position.x - selfBody.position.x));
    }
	
	// Update is called once per frame
	void Update () {
        angleToPlayer = Mathf.Atan2((playerBody.position.y - selfBody.position.y), (playerBody.position.x - selfBody.position.x))/ DEG_TO_RAD;
        forwardVec = new Vector2(Mathf.Cos(selfBody.rotation * DEG_TO_RAD), Mathf.Sin(selfBody.rotation * DEG_TO_RAD));
        Vector2 forward = acceleration * forwardVec;

        // normalize rotation for easier angle comparison calculations
        if (selfBody.rotation < -180)
        {
            selfBody.rotation = selfBody.rotation + 360;
        } else if (selfBody.rotation > 180)
        {
            selfBody.rotation = selfBody.rotation - 360;
        }
        bool normalLeft = angleToPlayer > selfBody.rotation && angleToPlayer < selfBody.rotation + 180;
        bool selfBodyNegAnglePosLeft = angleToPlayer < selfBody.rotation + 360 && angleToPlayer > selfBody.rotation + 180;
        bool angleNegSelfBodyPosLeft = angleToPlayer < selfBody.rotation - 180 && angleToPlayer > selfBody.rotation - 360;
        if (normalLeft || selfBodyNegAnglePosLeft || angleNegSelfBodyPosLeft)
        {
            // rotated left of player, so want to try and rotate right
            turningRight = false;
            angleChange = Mathf.Max(Mathf.Min(angleChange + 0.5f, 2f), -2);
        } else
        {
            turningRight = true;
            angleChange = Mathf.Max(Mathf.Min(angleChange - 0.5f, 2f), -2);
        }

        if (selfBody.velocity.sqrMagnitude > maxSpeed)
        {
            // soft cap for max speed
            forward = forward * Mathf.Max((maxSpeed + 15 - selfBody.velocity.sqrMagnitude) / 15, 0);
        }
        selfBody.AddTorque(angleChange);
        selfBody.AddForce(-selfBody.velocity);
        selfBody.AddForce(forward);
    }
}
