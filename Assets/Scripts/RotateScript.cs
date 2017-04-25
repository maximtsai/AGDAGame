using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour {
    public float rotateSpeed = 1;
    Vector3 rotateVector;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0, 0, rotateSpeed);
	}
}
