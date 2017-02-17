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
            penetratedObjectCounter = 80 + ((int)collision.relativeVelocity.magnitude*4);
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
                this.transform.SetParent(listOfWeapons.transform);
                Rigidbody2D childRB = this.gameObject.AddComponent<Rigidbody2D>();
                childRB.gravityScale = 0;
                childRB.drag = 0.75f;
                childRB.angularDrag = 1;
                penetratedObject = false;
                penetratedObjectCounter = 0;
            }
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
