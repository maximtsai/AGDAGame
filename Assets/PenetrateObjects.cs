using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateObjects : MonoBehaviour {
    bool penetratedObject = false;
    public GameObject listOfWeapons;
    int penetratedObjectCounter = 0;
    Rigidbody2D thisRB;
    float currRotation;
    float prevPrevRotation;
    float prevRotation;
    Vector3 currPosition;
    Vector3 prevPosition;
    // Use this for initialization
    void Start () {
        currRotation = this.transform.eulerAngles.z;
        prevRotation = currRotation;
        currPosition = this.transform.position;
        prevPosition = currPosition;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.gameObject.CompareTag("Weapon") && collision.collider.CompareTag("Weapon"))
        {
            // weapons don't get stuck in other weapons
            return;
        }
        thisRB = this.GetComponent<Rigidbody2D>();
        if (thisRB && this.transform.parent.tag != "Player" && collision.relativeVelocity.magnitude > 7)
        {
            // make object stick inside thing it's penetrated
            penetratedObject = true;
            this.transform.SetParent(collision.gameObject.transform);
            Vector3 vel = currPosition - prevPosition;
            Vector3 newPos = new Vector3(this.transform.position.x + vel.x, this.transform.position.y + vel.y, this.transform.position.z);
            this.transform.position = newPos;
            thisRB.rotation = 0;
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, prevRotation);
            Destroy(thisRB);
            // this.transform.parent = collision.gameObject.transform;
            penetratedObjectCounter = 50 + ((int)collision.relativeVelocity.magnitude*4);
        }
    }
    void FixedUpdate()
    {
        //prevPrevRotation = prevRotation;
        prevPosition = currPosition;
        prevRotation = currRotation;
        currPosition = this.transform.position;
        currRotation = this.transform.eulerAngles.z;
        if (penetratedObject)
        {
            penetratedObjectCounter--;
            if (penetratedObjectCounter <= 0)
            {
                // fall off
                this.transform.SetParent(listOfWeapons.transform);
                // Add rigidbody component
                Rigidbody2D childRB = this.gameObject.GetComponent<Rigidbody2D>();
                if (!childRB)
                {
                    childRB = this.gameObject.AddComponent<Rigidbody2D>();
                }
                if (childRB)
                {
                    childRB.gravityScale = 0;
                    childRB.drag = 0.75f;
                    childRB.angularDrag = 1;
                    penetratedObject = false;
                    penetratedObjectCounter = 0;
                }
            }
        }
    }
}
