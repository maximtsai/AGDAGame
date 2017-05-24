﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeResume : MonoBehaviour {
    public GameObject pauseText;
    float initialDeltaTime = 0;
    float slowMoCharge = 70;
    bool slowMoReady = true;
    bool isPaused = false;
    bool readyToReset = false;
    public bool useSlowMo = true;
    public string sceneName = "Game";
	// Use this for initialization
	void Start () {
        initialDeltaTime = Time.fixedDeltaTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.P))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                pauseText.SetActive(true);
            } else
            {
                if (readyToReset)
                {
                    // undo reset procedures
                    readyToReset = false;
                    pauseText.GetComponent<Text>().text = "PAUSE";
                }
                Time.timeScale = 0.9f;
                pauseText.SetActive(false);
            }
        }
        if (isPaused)
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                // Pressing R triggers reset
                if (readyToReset)
                {
                    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                } else
                {
                    pauseText.GetComponent<Text>().text = "PRESS R AGAIN TO RESET";
                    readyToReset = true;
                }
            }
            return;
        }
        if (Time.timeScale < 1)
        {
            if (Time.timeScale == 0.0314f)
            {
                // hacky special case used to force slow-mo for critical events such as player core being damaged
                slowMoReady = true;
            }
            if (!useSlowMo)
            {
                Time.timeScale += (1 - Time.timeScale) * 0.5f;
                Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
            }
            else
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
                }
                else
                {
                    // quickly get back to normal time cuz no more slowmo charge
                    slowMoReady = false;
                    Time.timeScale = Mathf.Min(1, Time.timeScale + 0.1f);
                    Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
                }
            }
        }
        // regenerate slowmo
        if (Time.timeScale > 0.99f)
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
