using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeResume : MonoBehaviour {
    public GameObject pauseText;
    public Camera mainCam;
    Color camBGColor;
    Color camBGOrigColor;
    float initialDeltaTime = 0;
    int slowMoCooldown = 200;
    int slowMoHoldDuration = 0;
    bool isPaused = false;
    bool readyToReset = false;
    public bool useSlowMo = true;
    public string sceneName = "Game";
    float prevTimeScale = 1;
	// Use this for initialization
	void Start () {
        initialDeltaTime = Time.fixedDeltaTime;
        camBGColor = mainCam.backgroundColor;
        camBGOrigColor = mainCam.backgroundColor;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                pauseText.SetActive(true);
            }
            else
            {
                if (readyToReset)
                {
                    // undo reset procedures
                    readyToReset = false;
                    pauseText.GetComponent<Text>().text = "PAUSED\nP TO RESUME\nR TO RESTART";
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
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = initialDeltaTime;
                    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                }
                else
                {
                    pauseText.GetComponent<Text>().text = "CONFIRM RESTART? (R)";
                    readyToReset = true;
                }
            }
        }
    }

	void FixedUpdate () {
        if (isPaused)
        {
            return;
        }
        if (prevTimeScale > Time.timeScale)
        {
            // something recently triggered a time slowdown
            if (slowMoCooldown == 0)
            {
                float timeReduction = (1 - Time.timeScale);
                slowMoHoldDuration = (int)(timeReduction * timeReduction * 60);
                if (slowMoHoldDuration > 40)
                {
                    slowMoCooldown = slowMoHoldDuration * 6; // no long slowmos for awhile
                }
            }
        }
        if (Time.timeScale < 1)
        {
            if (Time.timeScale == 0.0123f)
            {
                // hacky special case used to force slow-mo for critical events such as player core being damaged
                slowMoHoldDuration = 70;
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
            mainCam.backgroundColor = camBGOrigColor * (Mathf.Sqrt(Time.timeScale)*0.4f + 0.6f);
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
