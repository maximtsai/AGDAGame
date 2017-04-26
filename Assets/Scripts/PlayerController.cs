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
    //public float turnSpeed = 4;
    //private float origTurnSpeed;
    private bool prevFrameTurned = false;
	private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector2 forwardVec = new Vector2(1, 0);
    private bool forwardPressed = false;
    private bool backwardPressed = false;
    private bool leftPressed = false;
    private bool rightPressed = false;
    private float forwardPressedDuration = 0;
    private GameObject engine;
    private EngineScript engineScript;
    float initialDeltaTime = 0;
    void Start() {
        //origTurnSpeed = turnSpeed;
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
            if (!rightPressed)
            {
                rightPressed = true;
                engineScript.setTurnRight(true);
            }
        } else
        {
            if (rightPressed)
            {
                rightPressed = false;
                engineScript.setTurnRight(false);
            }
        }

        if (Input.GetKey(left))
        {
            if (!leftPressed)
            {
                leftPressed = true;
                engineScript.setTurnLeft(true);
            }
        }
        else
        {
            if (leftPressed)
            {
                leftPressed = false;
                engineScript.setTurnLeft(false);
            }
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
