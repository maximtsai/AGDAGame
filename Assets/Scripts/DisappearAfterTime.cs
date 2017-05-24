using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAfterTime : MonoBehaviour {
    public float durationInSeconds = 3;
    public bool deactivateInsteadOfDisappear = false;
    public bool disappearFromCollisions = false;
    public bool shrinkAway = true;
    public bool hackSizeChangeVar = false;
    float startTime;
    float scaleXChange;
    float scaleYChange;
    Vector3 currScale;
    Vector3 origScale;
	// Use this for initialization
	void Start () {
        startTime = Time.realtimeSinceStartup;
        scaleXChange = this.transform.localScale.x * 0.075f;
        scaleYChange = this.transform.localScale.y * 0.075f;
        if (hackSizeChangeVar)
        {
            this.transform.localScale = new Vector3(3, 3, 1);

        }
        origScale = this.transform.localScale;
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
            if (shrinkAway)
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
                    }
                    else
                    {
                        Destroy(this.gameObject);

                    }
                }
            }
            else
            {
                currScale = this.transform.localScale;
                currScale.x = Mathf.Max(0, currScale.x - scaleXChange * 0.1f * Time.timeScale);
                currScale.y = Mathf.Max(0, currScale.y - scaleYChange * 0.1f * Time.timeScale);
                this.transform.localScale = currScale;
                if (currScale.x <= origScale.x * 0.95f)
                {
                    if (deactivateInsteadOfDisappear)
                    {
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        Destroy(this.gameObject);

                    }
                }
            }
        }
	}
    public void resetDisappearTime()
    {
        startTime = Time.realtimeSinceStartup;
    }
}
