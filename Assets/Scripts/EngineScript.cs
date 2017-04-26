using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineScript : MonoBehaviour
{
    // public string name = "ENGINENAME";
    public float acceleration = 100;
    public float maxSpeed = 14;
    public float turnPower = 30;
    public int engineType = 0; // 0 is default engine, 1-99999 reserved for any special engine behavior 
    // private Vector2 forwardVec = new Vector2(1, 0);
    Rigidbody2D playerBody;
    bool goingForward;
    bool goingBackwards;
    bool turningLeft;
    bool turningRight;
    private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector2 forwardVec = new Vector2(0, 1);
    float initialDeltaTime = 0;

    const float ForwardTurnMultiplier = 0.7f;
    const float BackwardTurnMultiplier = 0.85f;
    ExhaustControl exhaustController;
    // Use this for initialization
    void Start()
    {
        initialDeltaTime = Time.fixedDeltaTime;
        maxSpeed = maxSpeed * maxSpeed; // optimization thing, removes need to squareroot player speed.
        // check if engine is actually attached to the player
        if (this.transform.parent.tag == "Player")
        {
            playerBody = this.transform.parent.GetComponent<Rigidbody2D>();
        }
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.tag == "ParticleSystem")
            {
                exhaustController = child.gameObject.GetComponent<ExhaustControl>();
                break;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forwardVec.Set(-Mathf.Sin(playerBody.rotation * DEG_TO_RAD), Mathf.Cos(playerBody.rotation * DEG_TO_RAD));
        switch (engineType)
        {
            case 0:
                float turnMultiplier = 1; // if moving forward/backwards, turn rate will decrease
                if (goingForward)
                {
                    turnMultiplier = ForwardTurnMultiplier;
                    Vector2 forward = acceleration * forwardVec;
                    if (playerBody.velocity.sqrMagnitude > maxSpeed)
                    {
                        // soft cap for max speed
                        Vector2 baseSpd = forward * maxSpeed / playerBody.velocity.sqrMagnitude;
                        Vector2 additionalSpd = forward - baseSpd;
                        forward = baseSpd + additionalSpd * Mathf.Max((maxSpeed + 15 - playerBody.velocity.sqrMagnitude) / 15, 0);
                    }
                    // -3 increases control of player
                    playerBody.AddForce(forward - 3 * playerBody.velocity);
                }
                if (goingBackwards)
                {
                    turnMultiplier = BackwardTurnMultiplier;
                    Vector2 backward = -acceleration * forwardVec;
                    if (playerBody.velocity.sqrMagnitude > maxSpeed)
                    {
                        // soft cap for max speed
                        Vector2 baseSpd = backward * maxSpeed / playerBody.velocity.sqrMagnitude;
                        Vector2 additionalSpd = (backward - baseSpd);
                        backward = baseSpd + additionalSpd * Mathf.Max((maxSpeed + 5 - playerBody.velocity.sqrMagnitude) / 5, 0);
                    }
                    // increases control of player
                    playerBody.AddForce(-playerBody.velocity);
                    playerBody.AddForce(backward * 0.15f);
                }

                if (turningLeft)
                {
                    //playerBody.MoveRotation(playerBody.rotation + turnSpeed * Time.fixedDeltaTime / initialDeltaTime);
                    playerBody.AddTorque(turnMultiplier*turnPower);
                    playerBody.angularVelocity += turnMultiplier*250f;
                }
                if (turningRight)
                {
                    //playerBody.MoveRotation(playerBody.rotation - turnSpeed * Time.fixedDeltaTime / initialDeltaTime);
                    playerBody.AddTorque(-turnMultiplier*turnPower);
                    playerBody.angularVelocity -= turnMultiplier*250f;
                }
                break;
            case 1:
                break;
        }
        // slow down player speed by a constant amount for tighter feeling
        float speed = playerBody.velocity.magnitude;
        if (speed > 0.00001f)
        {
            playerBody.AddForce(playerBody.velocity / (-0.25f * speed));
        }
    }
    public void setForward(bool goForward)
    {
        goingForward = goForward;
        //goingBackwards = false;
        // make exhaust react properly
        exhaustController.setForward(goForward);
    }
    public void setBackward(bool goBackward)
    {
        goingBackwards = goBackward;
    }
    public void setTurnLeft(bool isTurningLeft)
    {
        turningLeft = isTurningLeft;
    }
    public void setTurnRight(bool isTurningRight)
    {
        turningRight = isTurningRight;
    }
}
