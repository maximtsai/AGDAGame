using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDamage : MonoBehaviour {
    SpriteRenderer SR;
    // Use this for initialization
    float goalRedStrength;
    float goalBlueStrength;
    float goalGreenStrength;
    float redChangeFactor;
    float greenChangeFactor;
    float blueChangeFactor;
    Color currColor;
    Color origColor;
    void Start () {
        SR = GetComponent<SpriteRenderer>();
        currColor = SR.color;
        origColor = SR.color;
        goalRedStrength = SR.color.r;
        goalGreenStrength = SR.color.g;
        goalBlueStrength = SR.color.b;
        redChangeFactor = (1 - goalRedStrength) * 0.33f;
        greenChangeFactor = goalGreenStrength * 0.33f;
        blueChangeFactor = goalBlueStrength * 0.33f;
    }

    // Update is called once per frame
    void Update () {
        if (SR.color.r < goalRedStrength)
        {
            currColor.r = Mathf.Min(goalRedStrength, currColor.r + 0.02f);
            SR.color = currColor;
        } else if (SR.color.r > goalRedStrength)
        {
            currColor.r = Mathf.Min(goalRedStrength, currColor.r - 0.02f);
            SR.color = currColor;
        }

        if (SR.color.g < goalGreenStrength)
        {
            currColor.g = Mathf.Min(goalGreenStrength, currColor.g + 0.02f);
            SR.color = currColor;
        }
        else if (SR.color.g > goalGreenStrength)
        {
            currColor.g = Mathf.Min(goalGreenStrength, currColor.g - 0.02f);
            SR.color = currColor;
        }

        if (SR.color.b < goalBlueStrength)
        {
            currColor.b = Mathf.Min(goalBlueStrength, currColor.b + 0.02f);
            SR.color = currColor;
        }
        else if (SR.color.b > goalBlueStrength)
        {
            currColor.b = Mathf.Min(goalBlueStrength, currColor.b - 0.02f);
            SR.color = currColor;
        }

    }
    public void SetRed(float redColor)
    {
        // Debug.Log(1 - redColor);
        goalRedStrength = Mathf.Min(1, goalRedStrength + redColor * redChangeFactor);
        goalGreenStrength = Mathf.Max(0, goalGreenStrength - redColor * greenChangeFactor);
        goalBlueStrength = Mathf.Max(0, goalBlueStrength - redColor * blueChangeFactor);
    }
    public void flashRed(float flashIntensity)
    {
        currColor.r = Mathf.Min(1, goalRedStrength + flashIntensity);
        currColor.g = Mathf.Max(0, goalGreenStrength - flashIntensity);
        currColor.b = Mathf.Max(0, goalBlueStrength - flashIntensity);
        SR.color = currColor;
    }
}
