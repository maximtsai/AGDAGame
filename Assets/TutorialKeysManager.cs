using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class used to directly control the disappearing of 
// tutorial related hud elements
public class TutorialKeysManager : MonoBehaviour {
    public GameObject P1UpMove;
    public GameObject P1DownMove;
    public GameObject P1LeftMove;
    public GameObject P1RightMove;
    public GameObject P1Equip;
    public GameObject P1Activate;
    public GameObject P2UpMove;
    public GameObject P2DownMove;
    public GameObject P2LeftMove;
    public GameObject P2RightMove;
    public GameObject P2Equip;
    public GameObject P2Activate;
    // Use this for initialization
    List<GameObject> listOfHighlightedObjects = new List<GameObject>();
    Vector3 tempVec = new Vector3(0,0,0);
    Color tempCol = new Color(1, 1, 1, 0);
    float scaleChange;
    float alphaChange = 0.05f;
    float initialSize = 0;
    float goalAlpha;
    bool continueTutorial = true;
    void Start () {
        scaleChange = P1UpMove.transform.localScale.x * 0.025f;
        initialSize = P1UpMove.transform.localScale.x;
        goalAlpha = P1UpMove.GetComponent<SpriteRenderer>().color.a;

        listOfHighlightedObjects.Add(P1Activate);
        listOfHighlightedObjects.Add(P1Equip);
        listOfHighlightedObjects.Add(P1UpMove);
        listOfHighlightedObjects.Add(P1RightMove);
        listOfHighlightedObjects.Add(P1DownMove);
        listOfHighlightedObjects.Add(P1LeftMove);

        listOfHighlightedObjects.Add(P2Activate);
        listOfHighlightedObjects.Add(P2Equip);
        listOfHighlightedObjects.Add(P2UpMove);
        listOfHighlightedObjects.Add(P2RightMove);
        listOfHighlightedObjects.Add(P2DownMove);
        listOfHighlightedObjects.Add(P2LeftMove);
    }

    // Update is called once per frame
    void Update () {
        if (continueTutorial)
        {
            foreach (GameObject shrinkingObj in listOfHighlightedObjects)
            {
                // shrink object
                tempVec = shrinkingObj.transform.localScale;
                tempVec.x = Mathf.Max(initialSize, tempVec.x - scaleChange);
                tempVec.y = Mathf.Max(initialSize, tempVec.y - scaleChange);
                shrinkingObj.transform.localScale = tempVec;

                // fade out shrinking object
                tempCol = shrinkingObj.GetComponent<SpriteRenderer>().color;
                tempCol.a = Mathf.Max(goalAlpha, tempCol.a - alphaChange);
                shrinkingObj.GetComponent<SpriteRenderer>().color = tempCol;
                // fade out all child texts
                foreach (Transform textTransform in shrinkingObj.transform)
                {
                    tempCol = textTransform.GetComponent<Text>().color;
                    tempCol.a = Mathf.Max(goalAlpha, tempCol.a - alphaChange);
                    textTransform.GetComponent<Text>().color = tempCol;
                }
            }
        }
    }
    public void P1UPMovePressed()
    {
        ButtonPressed(P1UpMove);
    }
    public void P1DownMovePressed()
    {
        ButtonPressed(P1DownMove);
    }
    public void P1LeftMovePressed()
    {
        ButtonPressed(P1LeftMove);
    }
    public void P1RightMovePressed()
    {
        ButtonPressed(P1RightMove);
    }
    public void P1EquipPressed()
    {
        ButtonPressed(P1Equip);
    }
    public void P1ActivatePressed()
    {
        ButtonPressed(P1Activate);
    }


    public void P2UPMovePressed()
    {
        ButtonPressed(P2UpMove);
    }
    public void P2DownMovePressed()
    {
        ButtonPressed(P2DownMove);
    }
    public void P2LeftMovePressed()
    {
        ButtonPressed(P2LeftMove);
    }
    public void P2RightMovePressed()
    {
        ButtonPressed(P2RightMove);
    }
    public void P2EquipPressed()
    {
        ButtonPressed(P2Equip);
    }
    public void P2ActivatePressed()
    {
        ButtonPressed(P2Activate);
    }

    void ButtonPressed(GameObject keyPressed)
    {
        if (continueTutorial)
        {
            goalAlpha -= 0.01f;
            if (goalAlpha <= -0.1f)
            {
                continueTutorial = false;
                return;
            }
            tempCol.a = Mathf.Min(1, goalAlpha + 0.5f);
            keyPressed.GetComponent<SpriteRenderer>().color = tempCol;
            tempVec = keyPressed.transform.localScale;
            tempVec.x *= 1.1f;
            tempVec.y *= 1.1f;
            keyPressed.transform.localScale = tempVec;
            foreach (Transform textTransform in keyPressed.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
        }
    }

};
