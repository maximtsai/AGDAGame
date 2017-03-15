using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int dmgVelocity;
	public int playerHealth;



	// Use this for initialization
	void Start () {
		Debug.Log ("Initial health: " + playerHealth);
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		//Debug.Log();
		Debug.Log(this.gameObject.tag);
		Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.relativeVelocity.magnitude >= dmgVelocity)
            {
				playerHealth -= (int)Mathf.Abs(Vector2.Dot (collision.contacts [0].normal, collision.relativeVelocity));
                Debug.Log("Remaining health: " + playerHealth);
            }
            if (playerHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }
	}

}
