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
    // GameObject[] listOfDisappearingThings;
    List<GameObject> listOfDisappearingThings = new List<GameObject>();
    Vector3 tempVec = new Vector3(0,0,0);
    Color tempCol = new Color(1, 1, 1, 0);
    float scaleChange;
    float alphaChange = 0.05f;
    float initialSize = 0;
    void Start () {
        scaleChange = P1UpMove.transform.localScale.x * 0.05f;
        initialSize = P1UpMove.transform.localScale.x + 0.0001f;
    }

    // Update is called once per frame
    void Update () {
        foreach (GameObject disappearingObj in listOfDisappearingThings)
        {
            // expand disappearing object
            tempVec = disappearingObj.transform.localScale;
            tempVec.x += scaleChange;
            tempVec.y += scaleChange;
            disappearingObj.transform.localScale = tempVec;

            // fade out disappearing object
            tempCol = disappearingObj.GetComponent<SpriteRenderer>().color;
            tempCol.a = Mathf.Max(0, tempCol.a - alphaChange);
            disappearingObj.GetComponent<SpriteRenderer>().color = tempCol;

            // fade out all child texts
            foreach(Transform textTransform in disappearingObj.transform)
            {
                tempCol = textTransform.GetComponent<Text>().color;
                tempCol.a = Mathf.Max(0, tempCol.a - alphaChange);
                textTransform.GetComponent<Text>().color = tempCol;
            }
        }
    }
    public void P1UPMovePressed()
    {
        if (P1UpMove.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P1UpMove.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P1UpMove.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P1UpMove);
        }
    }
    public void P1DownMovePressed()
    {
        if (P1DownMove.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P1DownMove.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P1DownMove.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P1DownMove);
        }
    }
    public void P1LeftMovePressed()
    {
        if (P1LeftMove.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P1LeftMove.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P1LeftMove.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P1LeftMove);
        }
    }
    public void P1RightMovePressed()
    {
        if (P1RightMove.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P1RightMove.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P1RightMove.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P1RightMove);
        }
    }
    public void P1EquipPressed()
    {
        if (P1Equip.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P1Equip.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P1Equip.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P1Equip);
        }
    }
    public void P1ActivatePressed()
    {
        if (P1Activate.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P1Activate.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P1Activate.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P1Activate);
        }
    }


    public void P2UPMovePressed()
    {
        if (P2UpMove.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P2UpMove.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P2UpMove.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P2UpMove);
        }
    }
    public void P2DownMovePressed()
    {
        if (P2DownMove.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P2DownMove.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P2DownMove.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P2DownMove);
        }
    }
    public void P2LeftMovePressed()
    {
        if (P2LeftMove.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P2LeftMove.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P2LeftMove.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P2LeftMove);
        }
    }
    public void P2RightMovePressed()
    {
        if (P2RightMove.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P2RightMove.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P2RightMove.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P2RightMove);
        }
    }
    public void P2EquipPressed()
    {
        if (P2Equip.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P2Equip.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P2Equip.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P2Equip);
        }
    }
    public void P2ActivatePressed()
    {
        if (P2Activate.transform.localScale.x <= initialSize)
        {
            tempCol.a = 1;
            P2Activate.GetComponent<SpriteRenderer>().color = tempCol;
            foreach (Transform textTransform in P2Activate.transform)
            {
                textTransform.GetComponent<Text>().color = tempCol;
            }
            listOfDisappearingThings.Add(P2Activate);
        }
    }

};
