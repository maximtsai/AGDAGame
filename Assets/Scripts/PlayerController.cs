﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerController takes player input and calls the proper scripts to activate
/// </summary>
public class PlayerController : MonoBehaviour {
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode equipWeapButton = KeyCode.F;
    public KeyCode activateWeapButton = KeyCode.G;
    public string horizontalAxis = "emptyAxis";
    public Rigidbody2D playerBody;

    public EngineScript engineScript;
    public WeaponControl weaponControlScript;
    public Text indicatorTextDisplay;

    bool forwardPressed = false;
    bool backwardPressed = false;
    bool leftPressed = false;
    bool rightPressed = false;
    bool equipPressed = false;
    bool activatePressed = false;

    private void Start()
    {
        indicatorTextDisplay.text = equipWeapButton.ToString();
    }

    void FixedUpdate()
    {
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

        if (Input.GetKey(right) || (Input.GetAxisRaw(horizontalAxis) > 0))
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

        if (Input.GetKey(left) || (Input.GetAxisRaw(horizontalAxis) < 0))
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
