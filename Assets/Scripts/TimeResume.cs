using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeResume : MonoBehaviour {
    float initialDeltaTime = 0;
    float slowMoCharge = 70;
    bool slowMoReady = true;
	// Use this for initialization
	void Start () {
        initialDeltaTime = Time.fixedDeltaTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale < 1)
        {
            if (slowMoCharge > 0 && slowMoReady)
            {
                // default slow down time
                slowMoCharge -= 1;
                if (Time.timeScale < 0.05)
                {
                    Time.timeScale = Mathf.Min(1, Time.timeScale + 0.001f);
                }
                else
                {
                    Time.timeScale = Mathf.Min(1, Time.timeScale + 0.025f);
                }
                Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
            } else
            {
                // quickly get back to normal time cuz no more slowmo charge
                slowMoReady = false;
                Time.timeScale = Mathf.Min(1, Time.timeScale + 0.1f);
                Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
            }
        } else
        {
            if (slowMoCharge < 70)
            {
                slowMoCharge += 0.25f;
                slowMoReady = false;
            } else
            {
                slowMoReady = true;
            }
        }
        //Time.fixedDeltaTime = Time.timeScale;
    }
}
