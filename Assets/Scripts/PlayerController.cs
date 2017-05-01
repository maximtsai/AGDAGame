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
    public KeyCode equipWeapButton = KeyCode.F;
    public KeyCode activateWeapButton = KeyCode.G;
    Text indicatorTextDisplay;

    //public float turnSpeed = 4;
    //private float origTurnSpeed;
    Vector2 forwardVec = new Vector2(1, 0);
    bool forwardPressed = false;
    bool backwardPressed = false;
    bool leftPressed = false;
    bool rightPressed = false;
    bool equipPressed = false;
    bool activatePressed = false;
    GameObject engine;
    EngineScript engineScript;
    WeaponControl weaponControlScript;
    float initialDeltaTime = 0;
    void Start() {
        //origTurnSpeed = turnSpeed;
        initialDeltaTime = Time.fixedDeltaTime;
        playerBody = GetComponent<Rigidbody2D>();
        weaponControlScript = GetComponent<WeaponControl>();
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
        // set weapon indicator text
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("WeaponPickupIndicator"))
            {
                Transform indicatorCanvasTrans = child.gameObject.transform.FindChild("Canvas");
                indicatorTextDisplay = indicatorCanvasTrans.FindChild("Text").GetComponent<Text>();
                indicatorTextDisplay.text = equipWeapButton.ToString();
                break;
            }
        }
    }

    void FixedUpdate()
    {
        forwardVec.Set(Mathf.Cos(playerBody.rotation * Mathf.Deg2Rad), Mathf.Sin(playerBody.rotation * Mathf.Deg2Rad));
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
        else if (leftPressed)
        {
                leftPressed = false;
                engineScript.setTurnLeft(false);
        }

        if (Input.GetKey(equipWeapButton))
        {
            if (!equipPressed)
            {
                equipPressed = true;
                weaponControlScript.pressEquipKey();
            }
        } else if (equipPressed)
        {
            equipPressed = false;
            weaponControlScript.releaseEquipKey();
        }

        if (Input.GetKey(activateWeapButton))
        {
            // TODO
            if (!activatePressed)
            {
                activatePressed = true;
                weaponControlScript.pressActivateKey();

            }
        }
        else if (activatePressed)
        {
            activatePressed = false;
            weaponControlScript.releaseActivateKey();
        }
    }

    public bool isForwardPressed()
    {
        return forwardPressed;
    }
}
