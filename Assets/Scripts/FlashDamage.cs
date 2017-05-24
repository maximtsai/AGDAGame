using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDamage : MonoBehaviour {
    SpriteRenderer SR;
    // Use this for initialization
    float origRedStrength;
    float origBlueStrength;
    float origGreenStrength;
    float goalRedStrength;
    float goalBlueStrength;
    float goalGreenStrength;
    float redChangeFactor;
    float greenChangeFactor;
    float blueChangeFactor;
    Color currColor;
    Color origColor;
    public float colorChangeSpeed = 0.015f;
    void Start () {
        SR = GetComponent<SpriteRenderer>();
        currColor = SR.color;
        origColor = SR.color;
        origRedStrength = SR.color.r;
        origGreenStrength = SR.color.g;
        origBlueStrength = SR.color.b;
        goalRedStrength = origRedStrength;
        goalGreenStrength = origGreenStrength;
        goalBlueStrength = origBlueStrength;
        redChangeFactor = (1 - goalRedStrength) * 0.5f;
        greenChangeFactor = goalGreenStrength * 0.5f;
        blueChangeFactor = goalBlueStrength * 0.5f;
    }

    // Update is called once per frame
    void Update () {
        if (SR.color.r < goalRedStrength)
        {
            currColor.r = Mathf.Min(goalRedStrength, currColor.r + colorChangeSpeed);
            SR.color = currColor;
        } else if (SR.color.r > goalRedStrength)
        {
            currColor.r = Mathf.Min(goalRedStrength, currColor.r - colorChangeSpeed);
            SR.color = currColor;
        }

        if (SR.color.g < goalGreenStrength)
        {
            currColor.g = Mathf.Min(goalGreenStrength, currColor.g + colorChangeSpeed);
            SR.color = currColor;
        }
        else if (SR.color.g > goalGreenStrength)
        {
            currColor.g = Mathf.Min(goalGreenStrength, currColor.g - colorChangeSpeed);
            SR.color = currColor;
        }

        if (SR.color.b < goalBlueStrength)
        {
            currColor.b = Mathf.Min(goalBlueStrength, currColor.b + colorChangeSpeed);
            SR.color = currColor;
        }
        else if (SR.color.b > goalBlueStrength)
        {
            currColor.b = Mathf.Min(goalBlueStrength, currColor.b - colorChangeSpeed);
            SR.color = currColor;
        }

    }
    public void SetRed(float redColor)
    {
        goalRedStrength = Mathf.Min(1, origRedStrength + redColor * redChangeFactor);
        goalGreenStrength = Mathf.Max(0, origGreenStrength - redColor * greenChangeFactor);
        goalBlueStrength = Mathf.Max(0, origBlueStrength - redColor * blueChangeFactor);
    }
    public void flashRed(float flashIntensity)
    {
        currColor.r = Mathf.Min(1.2f, goalRedStrength + flashIntensity);
        currColor.g = Mathf.Max(-0.2f, goalGreenStrength - flashIntensity);
        currColor.b = Mathf.Max(-0.2f, goalBlueStrength - flashIntensity);
        SR.color = currColor;
    }
    public void SetGrey()
    {
        goalRedStrength = 0.25f;
        goalGreenStrength = 0.25f;
        goalBlueStrength = 0.25f;
    }
}
