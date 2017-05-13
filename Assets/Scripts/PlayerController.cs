using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DarkRift;

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
                DarkRiftAPI.SendMessageToServer(110, 0, 10);
            }
        } else
        {
            if (forwardPressed)
            {
                forwardPressed = false;
                engineScript.setForward(false);
                DarkRiftAPI.SendMessageToServer(110, 0, 11);
            }
        }
        if (Input.GetKey(down))
        {
            if (!backwardPressed)
            {
                backwardPressed = true;
                engineScript.setBackward(true);
                DarkRiftAPI.SendMessageToServer(110, 0, 20);
            }
        } else
        {
            if (backwardPressed)
            {
                backwardPressed = false;
                engineScript.setBackward(false);
                DarkRiftAPI.SendMessageToServer(110, 0, 21);
            }
        }

        if (Input.GetKey(right))
        {
            if (!rightPressed)
            {
                rightPressed = true;
                engineScript.setTurnRight(true);
                DarkRiftAPI.SendMessageToServer(110, 0, 30);
            }
        } else
        {
            if (rightPressed)
            {
                rightPressed = false;
                engineScript.setTurnRight(false);
                DarkRiftAPI.SendMessageToServer(110, 0, 31);
            }
        }

        if (Input.GetKey(left))
        {
            if (!leftPressed)
            {
                leftPressed = true;
                engineScript.setTurnLeft(true);
                DarkRiftAPI.SendMessageToServer(110, 0, 40);
            }
        }
        else if (leftPressed)
        {
            leftPressed = false;
            engineScript.setTurnLeft(false);
            DarkRiftAPI.SendMessageToServer(110, 0, 41);
        }

        if (Input.GetKey(equipWeapButton))
        {
            if (!equipPressed)
            {
                equipPressed = true;
                weaponControlScript.pressEquipKey();
                DarkRiftAPI.SendMessageToServer(110, 0, 50);
            }
        } else if (equipPressed)
        {
            equipPressed = false;
            weaponControlScript.releaseEquipKey();
            DarkRiftAPI.SendMessageToServer(110, 0, 51);
        }

        if (Input.GetKey(activateWeapButton))
        {
            // TODO
            if (!activatePressed)
            {
                activatePressed = true;
                weaponControlScript.pressActivateKey();
                DarkRiftAPI.SendMessageToServer(110, 0, 60);

            }
        }
        else if (activatePressed)
        {
            activatePressed = false;
            weaponControlScript.releaseActivateKey();
            DarkRiftAPI.SendMessageToServer(110, 0, 61);
        }
    }

    public bool isForwardPressed()
    {
        return forwardPressed;
    }
}
