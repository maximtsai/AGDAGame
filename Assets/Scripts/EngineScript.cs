using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineScript : MonoBehaviour
{
    // external references
    public Rigidbody2D playerBody; // the engine will be moving this object
    public ExhaustControl exhaustController; // should reference the particle system simulating engine exhaust

    // tuning variables
    public float acceleration = 100;
    public float maxSpeed = 15;
    public float baseTurnPower = 200; // how much turning power at theoretically infinite weight
    public float bonusTurnPower = 20; // how much additional turning power at 0 weight
    public float initialTurnMult = 0.4f; // 0-1 starting turning capability as percent of bonusTurnPower
    public float turnAcc = 0.5f; // 0-1, how fast max turning capability reached
    public int engineType = 0; // 0 is default engine, 1-99999 reserved for any special engine behavior 
    float warmupTurnMult = 0.1f; // 0 - 1, simulates acceleration on turning
    const float ForwardmovementTurnMult = 0.7f;
    const float BackwardmovementTurnMult = 0.85f;

    // player state variables
    bool goingForward;
    bool goingBackwards;
    bool turningLeft;
    bool turningRight;
    int prevTurnDirection; // 0 for no turn, 1 for turning left, -1 for turning right

    // helpful constants
    private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector2 forwardVec = new Vector2(0, 1);
    
    void Start()
    {
        maxSpeed = maxSpeed * maxSpeed; // optimization thing, removes need to squareroot player speed.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forwardVec.Set(-Mathf.Sin(playerBody.rotation * DEG_TO_RAD), Mathf.Cos(playerBody.rotation * DEG_TO_RAD));

        switch (engineType)
        {
            case 0:
                float movementTurnMult = 1; // if moving forward/backwards, turn rate will decrease
                if (goingForward)
                {
                    movementTurnMult = ForwardmovementTurnMult;
                    Vector2 forward = acceleration * forwardVec;
                    if (playerBody.velocity.sqrMagnitude > maxSpeed)
                    {
                        // soft cap for max speed
                        Vector2 baseSpd = forward * maxSpeed / playerBody.velocity.sqrMagnitude;
                        Vector2 additionalSpd = forward - baseSpd;
                        forward = baseSpd + additionalSpd * Mathf.Max((maxSpeed + 15 - playerBody.velocity.sqrMagnitude) / 15, 0);
                    }
                    // -3 increases control of player
                    playerBody.AddForce(forward - 4 * playerBody.velocity);
                }
                if (goingBackwards)
                {
                    movementTurnMult = BackwardmovementTurnMult;
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
                    playerBody.AddForce(backward * 0.25f);
                }

                if (turningLeft)
                {
                    if (prevTurnDirection == 1)
                    {
                        // In previous frame you were already turning right, so turn right faster
                        warmupTurnMult += (1 - warmupTurnMult) * turnAcc;
                    }
                    else
                    {
                        // player was previously turning in another direction, so engine needs to "warm up" again
                        warmupTurnMult = initialTurnMult;
                    }
                    //playerBody.MoveRotation(playerBody.rotation + turnSpeed * Time.fixedDeltaTime / initialDeltaTime);
                    playerBody.AddTorque(movementTurnMult* bonusTurnPower * warmupTurnMult);
                    playerBody.angularVelocity += movementTurnMult * baseTurnPower * warmupTurnMult;

                    if (!turningRight)
                    {
                        prevTurnDirection = 1;
                    }
                }
                if (turningRight)
                {
                    if (prevTurnDirection == -1)
                    {
                        // In previous frame you were already turning right, so turn right faster
                        warmupTurnMult += (1 - warmupTurnMult) * turnAcc;
                    } else
                    {
                        // player was previously turning in another direction, so engine needs to "warm up" again
                        warmupTurnMult = initialTurnMult;
                    }
                    //playerBody.MoveRotation(playerBody.rotation - turnSpeed * Time.fixedDeltaTime / initialDeltaTime);
                    playerBody.AddTorque(-movementTurnMult* bonusTurnPower * warmupTurnMult);
                    playerBody.angularVelocity -= movementTurnMult* baseTurnPower * warmupTurnMult;

                    if (!turningLeft)
                    {
                        prevTurnDirection = -1;
                    }
                }
                if (!turningLeft && !turningRight)
                {
                    prevTurnDirection = 0;
                }

                break;
            case 1:
                break;
        }
        // slow down player speed by a constant amount for tighter feeling
        float speed = playerBody.velocity.magnitude;
        if (speed > 0.00001f)
        {
            playerBody.AddForce(playerBody.velocity / (-0.1f * speed));
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
