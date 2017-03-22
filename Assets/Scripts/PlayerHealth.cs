using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int dmgVelocity;
	public int playerHealth;
    float initialDeltaTime = 0;


	// Use this for initialization
	void Start () {
		Debug.Log ("Initial health: " + playerHealth);
        initialDeltaTime = Time.fixedDeltaTime;

    }
	
	void OnCollisionEnter2D(Collision2D collision) {
		//Debug.Log();
		Debug.Log(this.gameObject.tag);
		Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.relativeVelocity.magnitude >= dmgVelocity)
            {
                Debug.Log("Remaining health: " + playerHealth);
                playerHealth -= (int)Mathf.Abs(Time.fixedDeltaTime / initialDeltaTime*Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity));
                Time.timeScale = 0.03f;
                Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
            }
            if (playerHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }
	}
}