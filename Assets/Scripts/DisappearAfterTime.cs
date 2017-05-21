﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAfterTime : MonoBehaviour {
    public float durationInSeconds = 3;
    public bool deactivateInsteadOfDisappear = false;
    public bool disappearFromCollisions = false;
    float startTime;
    float scaleXChange;
    float scaleYChange;
    Vector3 currScale;
	// Use this for initialization
	void Start () {
        startTime = Time.realtimeSinceStartup;
        scaleXChange = this.transform.localScale.x * 0.075f;
        scaleYChange = this.transform.localScale.y * 0.075f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (disappearFromCollisions)
        {
            Destroy(this.gameObject);
        }
    }
    void Update () {
		if (Time.realtimeSinceStartup - startTime >= durationInSeconds)
        {
            currScale = this.transform.localScale;
            currScale.x = Mathf.Max(0, currScale.x - scaleXChange * Time.timeScale);
            currScale.y = Mathf.Max(0, currScale.y - scaleYChange * Time.timeScale);
            this.transform.localScale = currScale;
            if (currScale.x <= 0)
            {
                if (deactivateInsteadOfDisappear)
                {
                    this.gameObject.SetActive(false);
                } else
                {
                    Debug.Log("wat");
                    Destroy(this.gameObject);

                }
            }
        }
	}
    public void resetDisappearTime()
    {
        startTime = Time.realtimeSinceStartup;
    }
}
