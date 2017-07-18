using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attachedToParentScript : MonoBehaviour {
    Rigidbody2D thisRB;
	// Use this for initialization
	void Start () {
        thisRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        this.transform.localPosition = new Vector3(0, 0, -3);
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        thisRB.velocity = new Vector2(0, 0);

    }
}
