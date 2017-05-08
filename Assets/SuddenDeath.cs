using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuddenDeath : MonoBehaviour {
    public GameObject spikeWall;
    public GameObject normalWall;
    public GameObject listOfDisappearingObjects;
    Vector3 tempVec3;
    bool sDeath = false;

    int counter = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyUp(KeyCode.Y))
        {
            sDeath = true;
        }

        if (sDeath)
        {
            counter++;
            if (counter < 500)
            {
                if (counter < 15)
                {
                    spikeWall.transform.localScale = new Vector3(spikeWall.transform.localScale.x - 0.01f, spikeWall.transform.localScale.y - 0.01f, spikeWall.transform.localScale.z);
                }
                else
                {
                    normalWall.transform.localScale = new Vector3(normalWall.transform.localScale.x - 0.001f, normalWall.transform.localScale.y - 0.001f, normalWall.transform.localScale.z);
                }
            } else if (counter < 300)
            {
                normalWall.transform.localScale = new Vector3(normalWall.transform.localScale.x - 0.0005f, normalWall.transform.localScale.y - 0.0005f, normalWall.transform.localScale.z);
            }
            foreach (Transform childTrans in listOfDisappearingObjects.transform)
            {
                tempVec3 = childTrans.localScale;
                tempVec3.x = Mathf.Max(0, tempVec3.x * 0.98f - 0.001f);
                tempVec3.y = Mathf.Max(0, tempVec3.y * 0.98f - 0.001f);
                childTrans.localScale = tempVec3;// Mathf.Max(0, )
            }
        }
    }
}
