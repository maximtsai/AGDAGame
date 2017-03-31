using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherController : MonoBehaviour {

	// Minimum time between shots
	public float rechargeTime;
	public float initialSpeed;
	public float acceleration;
	public GameObject rocket;

	private Vector3 position;
	private Quaternion rotation;
	private bool isRecharging;
	private float lastShotTime;
	
	// Use this for initialization
	void Awake () {
		position = GetComponent<Transform>().localPosition;
		rotation = GetComponent<Transform>().localRotation;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(isRecharging && Time.time - lastShotTime >= rechargeTime) {
			isRecharging = false;
		}
		if(Input.GetButtonDown("Fire1") && !isRecharging) {
			// When the fire key is pressed, create new rocket
			Rigidbody2D newRocket = Instantiate(rocket, position, rotation).GetComponent<Rigidbody2D>();
			newRocket.velocity = transform.forward * initialSpeed;
			float forceMagnitude = acceleration * newRocket.mass;
			newRocket.AddForce(transform.forward * forceMagnitude, ForceMode2D.Force);
			lastShotTime = Time.time;
			isRecharging = true;
		}
	}
}