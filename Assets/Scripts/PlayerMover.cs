using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {
    Rigidbody2D playerBody;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public float acceleration;
    public float maxSpeed;
    public float turnSpeed;
    private float origTurnSpeed;
    private bool prevFrameTurned = false;
	private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector2 forwardVec = new Vector2(1, 0);
    private bool forwardPressed = false;
    private bool backwardPressed = false;
    private float forwardPressedDuration = 0;
    private GameObject engine;
    private EngineData engineScript;
    private ParticleSystem exhaust;
    float initialDeltaTime = 0;
    void Start() {
        origTurnSpeed = turnSpeed;
        initialDeltaTime = Time.fixedDeltaTime;
        playerBody = GetComponent<Rigidbody2D>();
        // set the engine
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.tag == "Engine")
            {
                engine = child.gameObject;
                engineScript = engine.GetComponent<EngineData>();
                foreach (Transform engineChild in engine.transform)
                {
                    if (engineChild.gameObject.CompareTag("ParticleSystem") || engineChild.gameObject.name == "Particle System")
                    {
                        exhaust = engineChild.gameObject.GetComponent<ParticleSystem>(); ;
                        break;
                    }
                }
                break;
            }
        }
    }

    void FixedUpdate()
    {
        forwardVec.Set(Mathf.Cos(playerBody.rotation * DEG_TO_RAD), Mathf.Sin(playerBody.rotation * DEG_TO_RAD));
        if (Input.GetKey(up))
        {
            forwardPressed = true;
            backwardPressed = false;
            Vector2 forward = acceleration * forwardVec;
            if (playerBody.velocity.sqrMagnitude > maxSpeed)
            {
                // soft cap for max speed
                Vector2 baseSpd = forward * maxSpeed / playerBody.velocity.sqrMagnitude;
                Vector2 additionalSpd = forward - baseSpd;
                forward = baseSpd + additionalSpd * Mathf.Max((maxSpeed+15 - playerBody.velocity.sqrMagnitude)/ 15, 0);
            }
            // increases control of player
            playerBody.AddForce(-3*playerBody.velocity);
            playerBody.AddForce(forward);
        } else if (Input.GetKey(down))
        {
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
            playerBody.AddForce(backward* 0.15f);
            backwardPressed = true;
            forwardPressed = false;
        }
        else
        {
            backwardPressed = false;
            forwardPressed = false;
        }
        // determining exhaust animations

        if (forwardPressed)
        {
            // make a small initial spurt when you press down forward
            forwardPressedDuration += Time.deltaTime;
            exhaust.Play();
            if (forwardPressedDuration < 0.1f)
            {
                exhaust.startSize = 2.5f;
                exhaust.startSpeed = 8f;
            }
            else if (exhaust.startSize != 1.5f)
            {
                exhaust.startSize = 1.5f;
                exhaust.startSpeed = 4f;
            }
            // animator.SetBool("jet", true);
        } else if (forwardPressedDuration > 0)
        {
            forwardPressedDuration = 0;
            exhaust.startSize = 2;
            exhaust.startSpeed = 6f;
        }


        if (Input.GetKey(right))
        {
            if (!prevFrameTurned)
            {
                prevFrameTurned = true;
                turnSpeed = origTurnSpeed * 0.25f;
            }
            else
            {
                turnSpeed += (origTurnSpeed - turnSpeed)*0.25f;
            }
            if (forwardPressed)
            {
                // turn slower while moving
                playerBody.MoveRotation(playerBody.rotation - turnSpeed * 0.5f * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(-turnSpeed * 2f);
            } else if (backwardPressed)
            {
                playerBody.MoveRotation(playerBody.rotation - turnSpeed * 0.75f * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(-turnSpeed * 3f);
            }
            else
            {
                playerBody.MoveRotation(playerBody.rotation - turnSpeed * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(-turnSpeed * 4f);
            }
            //playerBody.Rotate(0.0f, 0.0f, -turnSpeed);
        }
        else if (Input.GetKey(left))
        {
            if (!prevFrameTurned)
            {
                prevFrameTurned = true;
                turnSpeed = origTurnSpeed * 0.25f;
            } else
            {
                turnSpeed += (origTurnSpeed - turnSpeed) * 0.25f;
            }
            if (forwardPressed)
            {
                // turn slower while moving
                playerBody.MoveRotation(playerBody.rotation + turnSpeed * 0.5f * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(turnSpeed * 2f);

            } else if (backwardPressed)
            {
                playerBody.MoveRotation(playerBody.rotation + turnSpeed * 0.75f * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(turnSpeed * 3f);
            }
            else
            {
                //playerBody.MoveRotation(playerBody.rotation + turnSpeed * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.MoveRotation(playerBody.rotation + turnSpeed * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(turnSpeed * 4f);

            }
        } else
        {
            prevFrameTurned = false;
        }
        // implementation for tighter feeling
        float speed = playerBody.velocity.magnitude;
        if (speed > 0.00001f)
        {
            playerBody.AddForce(playerBody.velocity / (-0.14f*speed));
        }
    }
    // Update is called once per frame
    void Update () {
		
	}

    public bool isForwardPressed()
    {
        return forwardPressed;
    }
}
