using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour {

	public float rocketDuration = 5f;
	public GameObject explosion;
	[HideInInspector]
	public Vector3 force;

	private Vector3 position;
	private Rigidbody2D rigidbody;

	// Initialize the private fields
	void Awake()
	{
		position = transform.position;
		rigidbody = GetComponent<Rigidbody2D>();
		transform.Rotate(new Vector3(0f, 0f, -90f));
		//Debug.Log("Initial Velocity = " + rigidbody.velocity);
	}

	// Set the timer for the explosion
	void Start () {
		Invoke("Explode", rocketDuration);
	}

	// Update the position and add the acceleration
	void FixedUpdate()
	{
		position = transform.position;
		rigidbody.AddForce(force);	
	}

	// Explode as soon as missile hits some collider box
	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log("I bumped into something!");
		Explode();
	}

	// Destroy object after limit time or after collision
	void Explode() {
		CancelInvoke("Explode");
		Instantiate(explosion, position, Quaternion.identity);
		Destroy(gameObject);
	}
}
