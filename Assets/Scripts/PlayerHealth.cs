using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int dmgVelocity;
	public int playerHealth;
    private float tempArmor = 0;
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
    private void FixedUpdate()
    {
        tempArmor = Mathf.Max(0, tempArmor * 0.5f - 1);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log();
        // what u collided with Debug.Log(collision.collider.gameObject.name);
        // playear Debug.Log(this.gameObject.tag);
        if (!collision.contacts[0].otherCollider.gameObject.CompareTag("Weapon"))
        {
            float dotImpact = Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity);
            Rigidbody2D colliderRB = collision.contacts[0].collider.GetComponent<Rigidbody2D>();
            float colliderMass = 9999;
            if (colliderRB)
            {
                colliderMass = colliderRB.mass;
            }
            if (collision.collider.gameObject.CompareTag("Weapon"))
            {
                dotImpact = dotImpact * 2 + 3;
                colliderMass *= 2;
            } else if (collision.collider.gameObject.CompareTag("Player"))
            {
                dotImpact = dotImpact * 1.7f + 1;
            }
            float playerMass = this.GetComponent<Rigidbody2D>().mass;
            // more massive objects you hit hurt you more. More massive players receive slightly less damage, slightly.
            float massRatio = colliderMass / (colliderMass + Mathf.Max(1, playerMass * 0.2f));
            dotImpact *= massRatio;
            if (Mathf.Abs(dotImpact) >= dmgVelocity + tempArmor)
            {
                int damageDealt = (int)Mathf.Max(1, (Time.fixedDeltaTime / initialDeltaTime * (Mathf.Abs(dotImpact) - dmgVelocity - tempArmor)));
                float outputval = dmgVelocity + tempArmor;
                Debug.Log("damageDealt: " + damageDealt + " tempArmor: " + tempArmor + " dotimpact:" + Mathf.Abs(dotImpact));

                playerHealth -= damageDealt;
                Time.timeScale = Mathf.Max((10 - (float)damageDealt) / 11, 0.03f);
                Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
                tempArmor = damageDealt + 10;
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