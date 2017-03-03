using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDuelist : MonoBehaviour {
    public GameObject player;
    public float acceleration = 10;
    public float maxSpeed = 50;
    public float turnSpeed = 0.4f;
    private Rigidbody2D playerBody;
    private Rigidbody2D selfBody;
    private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector2 forwardVec;
    // private bool goingForward = false;
    private float angleToPlayer = 0;
    private float angleChange = 0;
    private string combatState = "Alert"; // Alert, Charging, turning, evading
    // Use this for initialization
    void Start () {
        selfBody = GetComponent<Rigidbody2D>();
        playerBody = player.GetComponent<Rigidbody2D>();
        forwardVec = new Vector2(Mathf.Cos(selfBody.rotation * DEG_TO_RAD), Mathf.Sin(selfBody.rotation * DEG_TO_RAD));
        angleToPlayer = Mathf.Atan2((playerBody.position.y - selfBody.position.y), (playerBody.position.x - selfBody.position.x));
    }
	
	// Update is called once per frame
	void Update () {
        bool turningRight = goalAtRight(selfBody.position, playerBody.position, selfBody.rotation);
        forwardVec = new Vector2(Mathf.Cos(selfBody.rotation * DEG_TO_RAD), Mathf.Sin(selfBody.rotation * DEG_TO_RAD));
        Vector2 forward = acceleration * forwardVec;

        if (selfBody.velocity.sqrMagnitude > maxSpeed)
        {
            // soft cap for max speed
            forward = forward * Mathf.Max((maxSpeed + 15 - selfBody.velocity.sqrMagnitude) / 15, 0);
        }

        switch(combatState)
        {
            case "ALERT":
                break;
            case "CHARGING":
                float dist = Vector2.Distance(selfBody.position, playerBody.position);
                if (turningRight)
                {
                    angleChange = Mathf.Max(Mathf.Min(angleChange - turnSpeed*0.5f, 2f), -2);
                }
                else
                {
                    angleChange = Mathf.Max(Mathf.Min(angleChange + turnSpeed * 0.5f, 2f), -2);
                }
                selfBody.AddTorque(angleChange);
                selfBody.AddForce(-selfBody.velocity);
                selfBody.AddForce(forward);
                break;
            case "TURNING":
                if (turningRight)
                {
                    // rotated right of player, so want to try and rotate right
                    angleChange = Mathf.Max(Mathf.Min(angleChange - turnSpeed, 2f), -2);
                }
                else
                {
                    angleChange = Mathf.Max(Mathf.Min(angleChange + turnSpeed, 2f), -2);
                }
                selfBody.AddTorque(angleChange);
                break;
        }
    }

    bool goalAtRight(Vector2 srcPos, Vector2 goalPos, float rot) {
        angleToPlayer = Mathf.Atan2((goalPos.y - srcPos.y), (goalPos.x - srcPos.x)) / DEG_TO_RAD;
        float rotation = rot;
        // normalize rotation for easier angle comparison calculations
        if (rotation < -180)
        {
            rotation = rotation + 360;
        }
        else if (rotation > 180)
        {
            rotation = rotation - 360;
        }
        bool normalLeft = angleToPlayer > rotation && angleToPlayer < rotation + 180;
        bool selfBodyNegAnglePosLeft = angleToPlayer < rotation + 360 && angleToPlayer > rotation + 180;
        bool angleNegSelfBodyPosLeft = angleToPlayer < rotation - 180 && angleToPlayer > rotation - 360;
        if (normalLeft || selfBodyNegAnglePosLeft || angleNegSelfBodyPosLeft)
        {
            // rotated left of player, so want to try and rotate right
            return false;
        }
        else
        {
            return true;
        }
    }
}
