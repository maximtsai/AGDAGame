using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jointFlashScript : MonoBehaviour {
    Joint2D objJoint;
    SpriteRenderer objSR;
    Color tempColor = new Color(1,1,1,1);
	// Use this for initialization
	void Start () {
        objJoint = GetComponent<Joint2D>();
        objSR = GetComponent<SpriteRenderer>();
        tempColor = objSR.color;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (objJoint)
        {
            float reactForce = objJoint.reactionForce.magnitude;
            if (reactForce > 60)
            {
                // a force worth flashing
                tempColor.a = (reactForce - 60) * 0.005f;
                objSR.color = tempColor;
            } else
            {
                tempColor.a = Mathf.Max(0.05f, tempColor.a - 0.1f);
                objSR.color = tempColor;
            }
        } else
        {
            Destroy(this);
        }
    }
}
