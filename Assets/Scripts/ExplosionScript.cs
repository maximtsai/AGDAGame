using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

	public float explosionDuration;

	// Use this for initialization
	void Awake () {
		Invoke("EndExplosion", explosionDuration);
	}

	private void EndExplosion() {
		Destroy(gameObject);
	}
}
