using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreakScript : MonoBehaviour {
    public float initializedBreakForce = 60;
    public float breakRate = 0.1f;
    float tempArmor = 0;
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
            float minBreakForce = 85 + tempArmor;
            tempArmor = Mathf.Max(0, tempArmor - 1f);
            if (reactionForce > minBreakForce)
            {
                // minimal force required to break;
                tempArmor = Mathf.Max(tempArmor, (reactionForce - minBreakForce) * breakRate * 15);
                jointObj.breakForce -= (reactionForce - minBreakForce) * breakRate;
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
            this.transform.parent.gameObject.AddComponent<DisappearAfterTime>();
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
