using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour {
    public GameObject player1;
    public GameObject player2;
    public GameObject menuStart;
    public GameObject menuSound;
    public GameObject menuSoundNon;
    public GameObject menuMusic;
    public GameObject menuMusicNon;
    public GameObject menuCredits;
    public GameObject menuHelp;
    public GameObject menu2P;

    public GameObject cameraObj;
    CameraFollowPlayer cameraScript;

    public GameObject breakChainBot;
    public GameObject breakChainLeft;
    public GameObject breakChainRight;
    PlayerController P1Control;
    // List<GameObject> expandedObjects;
    float startSize;
    float maxSize;
    float shrinkRate = 0.015f;
    int prevSelectedSpot = 0;
    bool prevIsForwardPressed = false;
    bool justPressedForward = false;
    Vector3 tempVec;
    GameObject tempMenuObj;
    Color tempColor;
    Text tempText;
    GameObject menuOptions;
    List<GameObject> listOfShrinking = new List<GameObject>(); // non prim types are pointers, null
    bool disappearing = false;
    bool hasSound = true;
    bool hasMusic = true;
    bool lookingAtCredits = false;

    Sprite menuSoundSprite;
    Sprite menuSoundNonSprite;
    Sprite menuMusicSprite;
    Sprite menuMusicNonSprite;
    // Use this for initialization
    void Start () {
        startSize = menuStart.transform.localScale.x;
        maxSize = startSize + 0.2f;
        tempVec = new Vector3(0, 0, 0);
        tempColor = new Color(1, 1, 1, 1);
        listOfShrinking.Add(menuStart);
        listOfShrinking.Add(menuSound);
        listOfShrinking.Add(menuMusic);
        listOfShrinking.Add(menuCredits);
        listOfShrinking.Add(menuHelp);
        listOfShrinking.Add(menu2P);
        menuSoundSprite = menuSound.GetComponent<SpriteRenderer>().sprite;
        menuSoundNonSprite = menuSoundNon.GetComponent<SpriteRenderer>().sprite;
        menuMusicSprite = menuMusic.GetComponent<SpriteRenderer>().sprite;
        menuMusicNonSprite = menuMusicNon.GetComponent<SpriteRenderer>().sprite;

        cameraScript = cameraObj.GetComponent<CameraFollowPlayer>();

        P1Control = player1.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (disappearing)
        {
            // this happens when you press start button
            tempVec.x = this.transform.localScale.x + 0.008f;
            tempVec.y = this.transform.localScale.y + 0.008f;
            this.transform.localScale = tempVec;
            foreach (GameObject fadingObj in listOfShrinking)
            {
                tempColor = fadingObj.GetComponent<SpriteRenderer>().color;
                tempColor.a = tempColor.a * 0.9f - 0.01f;
                SpriteRenderer SR = fadingObj.GetComponent<SpriteRenderer>();
                SR.color = tempColor;
                if (tempColor.a < 0.01f)
                {
                    // allow chains to be broken and remove menu
                    breakChainBot.GetComponent<JointBreakScript>().enableJointBreak();
                    breakChainLeft.GetComponent<JointBreakScript>().enableJointBreak();
                    breakChainRight.GetComponent<JointBreakScript>().enableJointBreak();

                    cameraScript.defaultCameraSize = 7f;

                    Destroy(this.gameObject);
                }
            }

            return;
        } else if (lookingAtCredits)
        {
            cameraScript.forceChangeCameraPos(0, -15, 6);
        }

        if (player1.transform.eulerAngles.z < 0)
        {
            player1.transform.eulerAngles.Set(0, 0, player1.transform.eulerAngles.z + 360);
        }
        // selectedSpot is int used to determine which button the player is facing
        int selectedSpot = (int)(((player1.transform.eulerAngles.z + 30) % 360) / 60); // +30 deg since 0 deg is center in start button 
        if (!prevIsForwardPressed && P1Control.isForwardPressed())
        {
            justPressedForward = true;
        } else
        {
            justPressedForward = false;
        }

        if (!P1Control.isForwardPressed())
        {
            // forward button no longer being pressed
            prevIsForwardPressed = false;
        }

        switch (selectedSpot) {
            case 0:
                tempMenuObj = menuStart;
                if (justPressedForward)
                {

                    disappearing = true;
                }
                break;
            case 1:
                tempMenuObj = menuSound;
                if (justPressedForward)
                {
                    // flip between sound and hasSound states
                    hasSound = !hasSound;
                    if (hasSound)
                    {
                        menuSound.GetComponent<SpriteRenderer>().sprite = menuSoundSprite;
                    }
                    else
                    {
                        menuSound.GetComponent<SpriteRenderer>().sprite = menuSoundNonSprite;
                    }

                }
                break;
            case 2:
                tempMenuObj = menuMusic;
                if (justPressedForward)
                {
                    // flip between sound and hasSound states
                    hasMusic = !hasMusic;
                    if (hasMusic)
                    {
                        menuMusic.GetComponent<SpriteRenderer>().sprite = menuMusicSprite;
                    }
                    else
                    {
                        menuMusic.GetComponent<SpriteRenderer>().sprite = menuMusicNonSprite;
                    }
                }
                break;
            case 3:
                tempMenuObj = menuCredits;
                if (justPressedForward)
                {
                    lookingAtCredits = !lookingAtCredits;
                    if (!lookingAtCredits)
                    {
                        cameraScript.defaultCameraSize = 3.6f;
                    }
                }
                break;
            case 4:
                tempMenuObj = menuHelp;
                if (justPressedForward)
                {

                }
                break;
            case 5:
                tempMenuObj = menu2P;
                if (justPressedForward)
                {

                }
                break;
            default:
                break;
        };

        if (tempMenuObj != menuCredits)
        {
            // disable looking at credits if you look away
            lookingAtCredits = false;
            cameraScript.defaultCameraSize = 3.6f;
        }

        if (justPressedForward)
        {
            // pressed buttons are emphasized
            tempColor.r = 0.4f;
            tempColor.g = 0.65f;
            tempColor.b = 0.9f;
            tempColor.a = 1f;
            tempMenuObj.GetComponent<SpriteRenderer>().color = tempColor;
            tempMenuObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = tempColor;

            // make slight pop in size upon selection
            tempVec.x = maxSize+0.05f;
            tempVec.y = tempVec.x;
            tempMenuObj.transform.localScale = tempVec;
            prevIsForwardPressed = true;
        }

        foreach (GameObject shrinkingObj in listOfShrinking)
        {
            SpriteRenderer SRTemp = shrinkingObj.GetComponent<SpriteRenderer>();
            if (shrinkingObj.transform.localScale.x > startSize)
            {
                // shrink each menu object
                tempVec.x = Mathf.Max(startSize, shrinkingObj.transform.localScale.x - shrinkRate);
                tempVec.y = tempVec.x;
                shrinkingObj.transform.localScale = tempVec;
                // de-color each menu object
                tempText = shrinkingObj.transform.GetChild(0).GetChild(0).GetComponent<Text>();
                tempColor = SRTemp.color;
                tempColor.a = 0.5f + (shrinkingObj.transform.localScale.x - startSize)*3;
                SRTemp.color = tempColor;
                tempText.color = tempColor;
            }
            if (SRTemp.color.b < 1)
            {
                tempColor = SRTemp.color;
                tempColor.b += 0.0001f + tempColor.b * 0.004f;
                tempColor.g = 1 - (1 - tempColor.b) * 3.5f;
                tempColor.r = 1 - (1 - tempColor.b) * 6;
                SRTemp.color = tempColor;
                tempMenuObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = tempColor;
            }
        }
        // increase size and brightness of menu option you are facing towards
        tempVec.x = Mathf.Min(maxSize+500, tempMenuObj.transform.localScale.x * 0.8f + maxSize * 0.2f);
        tempVec.y = tempVec.x;
        tempMenuObj.transform.localScale = tempVec;

        tempText = tempMenuObj.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        tempColor = tempText.color;
        tempColor.a = 1f;
        tempText.color = tempColor;
        tempMenuObj.GetComponent<SpriteRenderer>().color = tempColor;

        /*
		if (menuStart.transform.localScale.x > startSize)
        {
            tempVec.x = Mathf.Max(startSize, menuStart.transform.localScale.x - shrinkRate);
            tempVec.y = tempVec.x;
            menuStart.transform.localScale = tempVec;
        }
        if (menuSound.transform.localScale.x > startSize)
        {
            tempVec.x = Mathf.Max(startSize, menuSound.transform.localScale.x - shrinkRate);
            tempVec.y = tempVec.x;
            menuSound.transform.localScale = tempVec;
        }
        if (menuMusic.transform.localScale.x > startSize)
        {
            tempVec.x = Mathf.Max(startSize, menuMusic.transform.localScale.x - shrinkRate);
            tempVec.y = tempVec.x;
            menuMusic.transform.localScale = tempVec;
        }
        if (menuHelp.transform.localScale.x > startSize)
        {
            tempVec.x = Mathf.Max(startSize, menuHelp.transform.localScale.x - shrinkRate);
            tempVec.y = tempVec.x;
            menuHelp.transform.localScale = tempVec;
        }
        if (menuCredits.transform.localScale.x > startSize)
        {
            tempVec.x = Mathf.Max(startSize, menuCredits.transform.localScale.x - shrinkRate);
            tempVec.y = tempVec.x;
            menuCredits.transform.localScale = tempVec;
        }
        if (menu2P.transform.localScale.x > startSize)
        {
            tempVec.x = Mathf.Max(startSize, menu2P.transform.localScale.x - shrinkRate);
            tempVec.y = tempVec.x;
            menu2P.transform.localScale = tempVec;
        }
        */
    }

    void ExpandObj(GameObject obj)
    {
        tempVec.x = Mathf.Min(startSize + 0.15f, obj.transform.localScale.x + shrinkRate * 3);
        tempVec.y = tempVec.x;
        obj.transform.localScale = tempVec;
    }
}
