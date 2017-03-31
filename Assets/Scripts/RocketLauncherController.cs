using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherController : MonoBehaviour {

	// Minimum time between shots
	public float rechargeTime = 0.5f;
	public GameObject rocket;
	public float initialSpeed = 2f;
	public float acceleration = 1f;

	private const float DEG_TO_RAD = Mathf.PI / 180f;
	private Vector3 position;
	// Rotation in euler angles
	private Vector3 rotation;
	private bool isRecharging;
	private float lastShotTime;
	
	// Use this for initialization
	void Awake () {
		position = GetComponent<Transform>().localPosition;
		rotation = GetComponent<Transform>().eulerAngles;
		isRecharging = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		position = GetComponent<Transform>().localPosition;
		rotation = GetComponent<Transform>().eulerAngles;
		if(isRecharging && Time.time - lastShotTime >= rechargeTime) {
			isRecharging = false;
		}
		if(Input.GetButtonDown("Fire1") && !isRecharging) {
			// When the fire key is pressed, create new rocket
			GameObject newRocket = Instantiate(rocket, position, GetComponent<Transform>().localRotation) as GameObject;
			Vector3 velocity = getDirectionVector() * initialSpeed;
			Vector3 force = getDirectionVector() * acceleration;
			newRocket.GetComponent<Rigidbody2D>().velocity = velocity;
			newRocket.GetComponent<RocketController>().force = force;
			lastShotTime = Time.time;
			isRecharging = true;
		}
	}

	private Vector3 getDirectionVector() {
		float x = Mathf.Cos(rotation.z * DEG_TO_RAD);
		float y = Mathf.Sin(rotation.z * DEG_TO_RAD);
		return new Vector3(x, y, 0f);
	}
}