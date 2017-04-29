using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDamage : MonoBehaviour {
    SpriteRenderer SR;
    // Use this for initialization
    float goalNonRedStrength = 1;
    Color currColor;
	void Start () {
        SR = GetComponent<SpriteRenderer>();
        currColor = new Color(1, 1, 1);
    }
	
	// Update is called once per frame
	void Update () {
		if (SR.color.b < goalNonRedStrength)
        {
            currColor.g = Mathf.Min(goalNonRedStrength, currColor.g + 0.02f);
            currColor.b = currColor.g;
            SR.color = currColor;
            //Debug.Log(goalNonRedStrength);
        }
        else if (SR.color.b > goalNonRedStrength)
        {
            currColor.g = Mathf.Max(goalNonRedStrength, currColor.g - 0.02f);
            currColor.b = currColor.g;
            SR.color = currColor;
        }
    }
    public void SetRed(float redColor)
    {
        // Debug.Log(1 - redColor);
        goalNonRedStrength = Mathf.Max(0, 1-redColor);
    }
    public void flashRed(float flashIntensity)
    {
        currColor.g = Mathf.Max(0, 1 - flashIntensity);
        currColor.b = currColor.g;
        SR.color = currColor;
    }
}
