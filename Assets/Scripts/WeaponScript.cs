using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {
    // Use this for initialization
    FixedJoint2D fjComponent;
    Rigidbody2D rbComponent;
    public float weaponOffset = 1;
    void Start () {
        fjComponent = this.GetComponent<FixedJoint2D>();
        rbComponent = this.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PickUpWeapon(GameObject wielder, Rigidbody2D bodyToConnectTo)
    {
        float xOffset = Mathf.Abs(fjComponent.anchor.y) * Mathf.Sin(wielder.transform.eulerAngles.x * Mathf.Deg2Rad);
        float yOffset = Mathf.Abs(fjComponent.anchor.y) * Mathf.Cos(wielder.transform.eulerAngles.y * Mathf.Deg2Rad);
        Vector3 wielderLookDirection = new Vector3(-xOffset, -yOffset, 0);
        this.transform.position = wielder.transform.position + wielderLookDirection;
        this.transform.eulerAngles =new Vector3(0, 0, bodyToConnectTo.rotation);
        fjComponent.enabled = true;
        fjComponent.connectedBody = bodyToConnectTo;
    }
    public void DropWeapon()
    {
        Vector2 currVel = this.GetComponent<Rigidbody2D>().velocity;
        currVel = currVel * 2;
        rbComponent.velocity = currVel;
        fjComponent.enabled = false;
        fjComponent.connectedBody = null;

        /*
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("Weapon") || child.CompareTag("SoftWeapon"))
            {
                // throw weapon away
                child.SetParent(listOfWeapons.transform);
                Rigidbody2D childRB = child.gameObject.AddComponent<Rigidbody2D>();
                childRB.gravityScale = 0;
                childRB.drag = 0.75f;
                childRB.angularDrag = 1;
                Vector2 throwVelocity = playerRB.GetPointVelocity(child.position) * 2f;
                // also need to take into account player rotation velocity
                float velX = 0;
                float velY = 0;
                if (rotationDiff > 0)
                {
                    // counterclockwise fling
                    velX = Mathf.Cos((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 0.75f;
                    velY = Mathf.Sin((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 0.75f;
                }
                else
                {
                    // clockwise fling
                    velX = Mathf.Cos((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 0.75f;
                    velY = Mathf.Sin((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 0.75f;
                }
                throwVelocity.x += velX;
                throwVelocity.y += velY;

                child.gameObject.GetComponent<Rigidbody2D>().velocity = throwVelocity;
                child.gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationDiff * 4);
                break;
            }
        }
        */
    }
}
