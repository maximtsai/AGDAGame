using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheWeapon : WeaponScript
{
    Vector3 localWeaponRotation = new Vector3(0, 0, 0);
    Vector3 localWeaponPos = new Vector3(0, 0, 0);
    public float swingBackTime = 1;
    float rotationAngle = 0;
    float startChargeTime = 0;
    bool charging = false;
    bool fullyCharged = false;
    bool swinging = false;
    bool almostDoneSwinging = false;
    HingeJoint2D hingeJointCompo;
    // Use this for initialization
    void Start () {
        rotationAngle = this.transform.localEulerAngles.z;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (charging)
        {
            if (!fullyCharged)
            {
                float timeSpentCharging = Time.fixedTime - startChargeTime;
                // 
                float chargeAmt = (1 - (swingBackTime / (swingBackTime + timeSpentCharging)));

                rotationAngle = (155-155*(chargeAmt)) - 155;
                if (rotationAngle <= -115)
                {
                    // at almost drawback, snap to full drawback
                    rotationAngle = -120;
                    fullyCharged = true;
                }
                // calculate position and rotation of weapon to swing back to
                localWeaponRotation.z = rotationAngle;
                float wieldPosMultiplier = weaponOffset * this.transform.localScale.y + wielderRB.gameObject.transform.localScale.y * 0.5f;
                localWeaponPos.x = -Mathf.Sin(localWeaponRotation.z * Mathf.Deg2Rad) * wieldPosMultiplier;
                localWeaponPos.y = Mathf.Cos(localWeaponRotation.z * Mathf.Deg2Rad) * wieldPosMultiplier;

                this.transform.localEulerAngles = localWeaponRotation;
                this.transform.localPosition = localWeaponPos;
                // weapon swings back slower as you charge it up more
            }
        } else if (swinging)
        {
            Debug.Log("torqu");
            rbComponent.AddRelativeForce(new Vector2(-10, 0), ForceMode2D.Impulse);
            //rbComponent.AddTorque(1000);
            //rbComponent.angularVelocity += 1000;
            // calculate how close you are to reaching end rotation.
            float normalizedWielderRot = ((wielderRB.rotation % 360) + 360) % 360;
            float normalizedSelfRot = ((rbComponent.rotation % 360) + 360) % 360;
            float rotationDiff = normalizedWielderRot - normalizedSelfRot;
            if (rotationDiff <= 0 && rotationDiff > -180 || rotationDiff > 180 && rotationDiff <= 360)
            {
                Debug.Log(rotationDiff);
                // end
                swinging = false;
                almostDoneSwinging = true;
                Destroy(hingeJointCompo);
                Destroy(rbComponent);

                
                this.transform.SetParent(wielderRB.gameObject.transform);
                localWeaponRotation = this.transform.localEulerAngles;
                localWeaponRotation.z = 0;
                this.transform.localEulerAngles = localWeaponRotation;
                /*
                float finalOffset = weaponOffset * this.transform.localScale.y + wielderRB.gameObject.transform.localScale.y * 0.5f;

                localWeaponPos = new Vector3(0, finalOffset, 0);
                this.transform.localEulerAngles = localWeaponRotation;
                this.transform.localPosition = localWeaponPos;
                */
            }
        } else if (almostDoneSwinging)
        {
            //this.transform.localEulerAngles = localWeaponRotation;
            //this.transform.localPosition = localWeaponPos;

        }
        if (isActivated && !swinging)
        {
            charging = true;
        }
    }
    public override void activateWeapon(Rigidbody2D playerRigidBody)
    {
        // What happens when the fire button is pressed
        isActivated = true;
        startChargeTime = Time.fixedTime;
    }
    public override void deactivateWeapon()
    {
        // what happens when the fire button is lifted
        isActivated = false;
        // making a slight assumption that there is always some bit of chargeup if the
        // deactivateWeapon button is triggered
        charging = false;
        swinging = true;
        fullyCharged = false;
        // transform weapon into its own entity so that it can be swung realistically
        this.transform.SetParent(null);
        rbComponent = this.gameObject.AddComponent<Rigidbody2D>();
        rbComponent.gravityScale = 0;
        rbComponent.angularDrag = 1;
        rbComponent.drag = 0.5f;

        hingeJointCompo = this.gameObject.AddComponent<HingeJoint2D>();
        float finalOffset = weaponOffset * this.transform.localScale.y + wielderRB.gameObject.transform.localScale.y * 0.5f;
        hingeJointCompo.anchor = new Vector2(0, -finalOffset);
        hingeJointCompo.connectedBody = wielderRB;
    }
    public override void equipWeaponExtra(Rigidbody2D playerRigidBody)
    {
        wielderRB = playerRigidBody;
        //localWeaponRotation.z = 0;
        //localWeaponPos.x = 0;
        //localWeaponPos.y = 0;
        rotationAngle = 0;
        //this.transform.localPosition;
    }
    public override void unequipWeaponExtra()
    {
        charging = false;
        swinging = false;
    }
}
