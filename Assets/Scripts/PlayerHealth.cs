using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int dmgVelocity;
	public int playerHealth;
	public int minimumDmg;



	// Use this for initialization
	void Start () {
		Debug.Log ("Initial health: " + playerHealth);
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		
		if(collision.relativeVelocity.magnitude >= dmgVelocity) {
			playerHealth -= (int)(collision.relativeVelocity.magnitude - dmgVelocity + 1) * minimumDmg;
			Debug.Log ("Remaining health: " + playerHealth);
		}
		if(playerHealth <= 0) {
			gameObject.SetActive (false);
		}
	}

}
