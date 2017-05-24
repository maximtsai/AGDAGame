using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeResume : MonoBehaviour {
    public GameObject pauseText;
    float initialDeltaTime = 0;
    int slowMoCooldown = 250;
    int slowMoHoldDuration = 0;
    bool isPaused = false;
    bool readyToReset = false;
    public bool useSlowMo = true;
    public string sceneName = "Game";
    float prevTimeScale = 1;
	// Use this for initialization
	void Start () {
        initialDeltaTime = Time.fixedDeltaTime;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
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
        if (prevTimeScale > Time.timeScale)
        {
            // something recently triggered a time slowdown
            if (slowMoCooldown == 0)
            {
                float timeReduction = (1 - Time.timeScale);
                slowMoHoldDuration = (int)(timeReduction * timeReduction * 50);
                if (slowMoHoldDuration < 30)
                {
                    // slowmo not enough to be useful
                    slowMoHoldDuration = 0;
                } else
                {
                    slowMoCooldown = slowMoHoldDuration * 7; // no long slowmos for awhile
                }
            }
        }
        if (Time.timeScale < 1)
        {
            if (Time.timeScale == 0.0123f)
            {
                // hacky special case used to force slow-mo for critical events such as player core being damaged
                slowMoHoldDuration = 55;
                slowMoCooldown = 0;
            }
            if (Time.timeScale < 0.025f)
            {
                Time.timeScale = 0.025f;
            }
            if (!useSlowMo)
            {
                Time.timeScale += Mathf.Min(1, Time.timeScale + 0.2f);
            }
            else
            {
                if (slowMoHoldDuration > 0)
                {
                    slowMoHoldDuration--;
                } else
                {
                    Time.timeScale = Mathf.Min(1, Time.timeScale + 0.1f);
                }
            }
            Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
        }
        // regenerate slowmo
        if (Time.timeScale > 0.99f)
        {
            if (slowMoCooldown > 0)
            {
                slowMoCooldown--;
            }
        }
        //Time.fixedDeltaTime = Time.timeScale;
        prevTimeScale = Time.timeScale;
    }
}
