using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisintegrateSlowly : MonoBehaviour {
    int duration = 0;
    float initialXSize;
    float initialYSize;
    Vector3 nextScale;
    // Use this for initialization
    void Start () {
        duration = 200 + (int)Random.Range(0, 7);
        initialXSize = this.transform.localScale.x;
        initialYSize = this.transform.localScale.y;
    }
	
	// Update is called once per frame
	void Update () {
        duration--;
        if (duration < 0)
        {
            nextScale.x = this.transform.localScale.x - initialXSize * 0.05f;
            nextScale.y = this.transform.localScale.y - initialYSize * 0.05f;
            this.transform.localScale = nextScale;
            if (this.transform.localScale.x <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
