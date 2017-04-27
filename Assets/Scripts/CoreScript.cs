using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is used to keep the core from rotating too much
// to create the illusion that it's magnetic/magical
public class CoreScript : MonoBehaviour {
    float rotateVel;
    float rotateAcc;
    Vector3 currRotation;
    // Use this for initialization
    void Start () {
        currRotation = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update () {
        if (Mathf.Abs(this.transform.eulerAngles.z) > 0.01f)
        {
            rotateAcc += 0.004f * getShortestRotation360(currRotation.z, 0);
            rotateVel += rotateAcc;
            currRotation.z = this.transform.eulerAngles.z + rotateVel;
            this.transform.eulerAngles = currRotation;
        }
        rotateVel *= 0.4f;
        rotateAcc *= 0.9f;
        
    }

    float getShortestRotation360(float startRot, float goalRot)
    {
        float startRotNorm = startRot;
        float goalRotNorm = goalRot;
        while (startRotNorm < 0)
        {
            startRotNorm += 360;
        }

        while (goalRotNorm < 0)
        {
            goalRotNorm += 360;
        }

        float diffVal = goalRotNorm - startRotNorm;
        if (Mathf.Abs(diffVal) > 180)
        {
            if (diffVal < 0)
            {
                diffVal += 360;
            }
            else {
                diffVal -= 360;
            } 
        }

        return diffVal;
    }
}
