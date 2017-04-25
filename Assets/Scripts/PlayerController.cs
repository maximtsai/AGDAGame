using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerController takes player input and calls the proper scripts to activate
/// </summary>
public class PlayerController : MonoBehaviour {
    Rigidbody2D playerBody;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public float turnSpeed = 4;
    private float origTurnSpeed;
    private bool prevFrameTurned = false;
	private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector2 forwardVec = new Vector2(1, 0);
    private bool forwardPressed = false;
    private bool backwardPressed = false;
    private float forwardPressedDuration = 0;
    private GameObject engine;
    private EngineScript engineScript;
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
                engineScript = engine.GetComponent<EngineScript>();
                break;
            }
        }
    }

    void FixedUpdate()
    {
        forwardVec.Set(Mathf.Cos(playerBody.rotation * DEG_TO_RAD), Mathf.Sin(playerBody.rotation * DEG_TO_RAD));
        if (Input.GetKey(up))
        {
            if (!forwardPressed)
            {
                forwardPressed = true;
                engineScript.setForward(true);
            }
        } else
        {
            if (forwardPressed)
            {
                forwardPressed = false;
                engineScript.setForward(false);
            }
        }
        if (Input.GetKey(down))
        {
            if (!backwardPressed)
            {
                backwardPressed = true;
                engineScript.setBackward(true);
            }
        } else
        {
            if (backwardPressed)
            {
                backwardPressed = false;
                engineScript.setBackward(false);
            }
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
                playerBody.AddTorque(-turnSpeed * 4);
            } else if (backwardPressed)
            {
                playerBody.MoveRotation(playerBody.rotation - turnSpeed * 0.75f * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(-turnSpeed * 4);
            }
            else
            {
                playerBody.MoveRotation(playerBody.rotation - turnSpeed * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(-turnSpeed * 4);
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
                playerBody.AddTorque(turnSpeed * 4);

            } else if (backwardPressed)
            {
                playerBody.MoveRotation(playerBody.rotation + turnSpeed * 0.75f * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(turnSpeed * 4);
            }
            else
            {
                Debug.Log(turnSpeed * 4);
                playerBody.MoveRotation(playerBody.rotation + turnSpeed * Time.fixedDeltaTime / initialDeltaTime);
                playerBody.AddTorque(turnSpeed * 4);

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
