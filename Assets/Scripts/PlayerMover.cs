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
	private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    private Vector2 forwardVec = new Vector2(1, 0);
    private bool forwardPressed = false;
    private float forwardPressedDuration = 0;
    private GameObject engine;
    private EngineData engineScript;
    private ParticleSystem exhaust;

    void Start() {
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
        }
        else
        {
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
            } else if (exhaust.startSize != 1.5f)
            {
                exhaust.startSize = 1.5f;
            }
            // animator.SetBool("jet", true);
        } else if (forwardPressedDuration > 0)
        {
            forwardPressedDuration = 0;
            exhaust.startSize = 2;
        }


        if (Input.GetKey(right))
        {
            if (forwardPressed)
            {
                // turn slower while moving
                playerBody.MoveRotation(playerBody.rotation - turnSpeed * 0.5f);
            } else
            {
                playerBody.MoveRotation(playerBody.rotation - turnSpeed);
            }
            //playerBody.Rotate(0.0f, 0.0f, -turnSpeed);
        }
        else if (Input.GetKey(left))
        {
            if (forwardPressed)
            {
                // turn slower while moving
                playerBody.MoveRotation(playerBody.rotation + turnSpeed * 0.5f);
            }
            else
            {
                playerBody.MoveRotation(playerBody.rotation + turnSpeed);
            }
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
