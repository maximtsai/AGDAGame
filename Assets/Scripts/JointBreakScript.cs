using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreakScript : MonoBehaviour {
    public float initializedBreakForce = 60;
    public float breakRate = 0.1f;
    SpringJoint2D jointObj;
    bool isActivated = false;
	// Use this for initialization
	void Start () {
        jointObj = GetComponent<SpringJoint2D>();
    }

    // Update is called once per frame
    void Update () {
		if (jointObj && isActivated)
        {
            float reactionForce = jointObj.reactionForce.magnitude;
            if (reactionForce > 85)
            {
                // minimal force required to break;
                jointObj.breakForce -= (reactionForce - 85) * breakRate;
            }
        } else if (!jointObj)
        {
            Rigidbody2D RB = GetComponent<Rigidbody2D>();
            if (RB.velocity.magnitude < 50)
            {
                // break effect
                RB.velocity = RB.velocity * 50 / RB.velocity.magnitude;
            }
            GetComponent<SparkSystem>().createSpark(new Vector2(this.transform.position.x, this.transform.position.y), 15);

            Destroy(this);
        }
	}
    public void enableJointBreak()
    {
        jointObj = GetComponent<SpringJoint2D>();
        jointObj.breakForce = initializedBreakForce;
        isActivated = true;
    }
}
