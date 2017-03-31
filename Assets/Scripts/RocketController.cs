using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour {

	public float rocketDuration = 5f;
	public GameObject explosion;

	private Vector3 position;

	// Initialize the private fields
	void Awake()
	{
		position = transform.position;
	}

	// Set the timer for the explosion
	void Start () {
		Invoke("Explode", rocketDuration);
	}

	// Explode as soon as missile hits some collider box
	void OnCollisionEnter(Collision collision) {
		Explode();
	}

	// Destroy object after limit time or after collision
	void Explode() {
		CancelInvoke("Explode");
		Instantiate(explosion, position, Quaternion.identity);
		Destroy(gameObject);
	}
}
