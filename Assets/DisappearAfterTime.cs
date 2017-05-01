using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAfterTime : MonoBehaviour {
    public float durationInSeconds = 3;
    float startTime;
    float scaleXChange;
    float scaleYChange;
    Vector3 currScale;
	// Use this for initialization
	void Start () {
        startTime = Time.realtimeSinceStartup;
        scaleXChange = this.transform.localScale.x * 0.05f;
        scaleYChange = this.transform.localScale.y * 0.05f;
    }

    void Update () {
		if (Time.realtimeSinceStartup - startTime >= durationInSeconds)
        {
            currScale = this.transform.localScale;
            currScale.x -= scaleXChange;
            currScale.y -= scaleYChange;
            this.transform.localScale = currScale;
            if (currScale.x <= 0)
            {
                Destroy(this.gameObject);
            }
        }
	}
}
