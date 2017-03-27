using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int dmgVelocity;
	public int playerHealth;
    float initialDeltaTime = 0;
    ParticleSystem blood;

    // Use this for initialization
    void Start () {
        initialDeltaTime = Time.fixedDeltaTime;
        foreach (Transform childObj in this.transform)
        {
            if (childObj.CompareTag("BloodSystem"))
            {
                blood = childObj.GetComponent<ParticleSystem>();
                break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log();
        // what u collided with Debug.Log(collision.collider.gameObject.name);
        // playear Debug.Log(this.gameObject.tag);
        if (!collision.contacts[0].otherCollider.gameObject.CompareTag("Weapon"))
        {
            float dotImpact = Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity);
            if (collision.collider.gameObject.CompareTag("Weapon"))
            {
                dotImpact = dotImpact * 2;
                Debug.Log("new impact: " + dotImpact);
            }
            float playerMass = this.GetComponent<Rigidbody2D>().mass;
            Rigidbody2D colliderRB = collision.contacts[0].collider.GetComponent<Rigidbody2D>();
            float colliderMass = 9999;
            if (colliderRB)
            {
                colliderMass = colliderRB.mass;
            }
            // more massive objects you hit hurt you more. More massive players receive slightly less damage, slightly.
            float massRatio = colliderMass / (colliderMass + Mathf.Max(1, playerMass * 0.1f));
            dotImpact *= massRatio;
            if (Mathf.Abs(dotImpact) >= dmgVelocity)
            {

                int damageDealt = (int)Mathf.Max(1, (Time.fixedDeltaTime / initialDeltaTime * (Mathf.Abs(dotImpact) - dmgVelocity)));
                Debug.Log("damageDealt: "+ damageDealt);
                playerHealth -= damageDealt;
                Debug.Log("playerHealth: " + playerHealth);
                Time.timeScale = Mathf.Max((12 - (float)damageDealt) / 15, 0.03f);
                Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;

                int bloodEmitted = (int)Mathf.Max(1, damageDealt * 0.25f);
                blood.Emit(bloodEmitted);
                blood.transform.position = collision.contacts[0].point;
            }
            if (playerHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        
		// what you collided with Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {

        }
	}
}