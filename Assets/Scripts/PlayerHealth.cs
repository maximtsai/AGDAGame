using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int dmgVelocity;
	public int playerHealth = 100;
	public int numpcs = 30;
    public float explosionDamage = 20;
	public GameObject[] lop;
	public GameObject piece;

    private int initialPlayerHealth;
    private float tempArmor = 0;
    private float initialDeltaTime = 0;
    private ParticleSystem blood;
    private ParticleSystem smoke;
    
    // Use this for initialization
    void Start () {
        initialPlayerHealth = playerHealth;
        initialDeltaTime = Time.fixedDeltaTime;
        foreach (Transform childObj in this.transform)
        {
            if (childObj.CompareTag("BloodSystem"))
            {
                blood = childObj.GetComponent<ParticleSystem>();
            } else if (childObj.CompareTag("SmokeSystem"))
            {
                smoke = childObj.GetComponent<ParticleSystem>();
            }
        }
    }
    private void FixedUpdate()
    {
        tempArmor = Mathf.Max(0, tempArmor * 0.2f - 1);
        if (playerHealth <= 0)
        {
                gameObject.SetActive(false);
				lop = new GameObject[numpcs];
				for (int i = 0; i < numpcs; i++) {
					Instantiate (piece, this.transform.position, this.transform.rotation);
					lop [i] = piece;
				}
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log();
        // what u collided with Debug.Log(collision.collider.gameObject.name);
        // playear Debug.Log(this.gameObject.tag);
        GameObject impactedPiece = collision.contacts[0].otherCollider.gameObject;
        float armorVal = 0;
        if (impactedPiece.CompareTag("LightArmor"))
        {
            //Debug.Log("impacted light armor");
            armorVal = 8;
        } else if (impactedPiece.CompareTag("HeavyArmor"))
        {
            //Debug.Log("impacted heavy armor");
            armorVal = 20;
        }
        if (!impactedPiece.CompareTag("Weapon") && !impactedPiece.CompareTag("SoftWeapon"))
        {
            float dotImpact = Mathf.Abs(Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity));
            Rigidbody2D colliderRB = collision.contacts[0].collider.GetComponent<Rigidbody2D>();
            float colliderMass = 9999;
            if (colliderRB)
            {
                colliderMass = colliderRB.mass;
            }
            if (collision.collider.gameObject.CompareTag("Weapon"))
            {
                dotImpact = (dotImpact) * 1.7f + 5;
                colliderMass *= 2;
            } else if (collision.collider.gameObject.CompareTag("SoftWeapon")) {
                dotImpact = (dotImpact) * 1.6f + 1;
                colliderMass *= 2;
            } else if (collision.collider.gameObject.CompareTag("Player"))
            {
                dotImpact = dotImpact * 1.5f + 1;
            } else if (playerHealth < 4)
            {
                // low health players less likely to kill self by hitting terrain
                dotImpact = dotImpact - 5;
            }
            if (playerHealth < 5)
            {
                // critical health players just a tad more resilient to keep up tension
                dotImpact = dotImpact - 1;
            }
            float playerMass = this.GetComponent<Rigidbody2D>().mass;
            // more massive objects you hit hurt you more. More massive players receive slightly less damage, slightly.
            float massRatio = colliderMass / (colliderMass + Mathf.Max(1, playerMass * 0.2f));
            dotImpact *= massRatio;
            //Debug.Log("origImpact: "+dotImpact);
            dotImpact -= armorVal;
            Debug.Log("newImpact: " + dotImpact);
            if (dotImpact >= dmgVelocity + tempArmor)
            {
                int damageDealt = (int)Mathf.Max(1, (Time.fixedDeltaTime / initialDeltaTime * (Mathf.Abs(dotImpact) - dmgVelocity - tempArmor)));
                float outputval = dmgVelocity + tempArmor;
                if (playerHealth >= 10 && playerHealth - damageDealt <= 0)
                {
                    // to prevent the impression of instagibbing, a "last lifeline"
                    damageDealt = playerHealth - 1;
                }
                playerHealth -= damageDealt;
                Time.timeScale = Mathf.Max((13 - (float)damageDealt) / 14, 0.03f);
                Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
                tempArmor = damageDealt*2 + 2;
                int bloodEmitted = (int)Mathf.Max(1, damageDealt * 0.35f);
                blood.Emit(bloodEmitted);
                blood.transform.position = collision.contacts[0].point;
                float playerHealthPercentage = (float)playerHealth / (float)initialPlayerHealth;
                if (playerHealthPercentage < 0.65f && playerHealthPercentage > 0.25f)
                {
                    smoke.emissionRate = 4 * (0.65f - ((float)playerHealth / (float)initialPlayerHealth));
                } else if (playerHealthPercentage <= 0.25f)
                {
                    smoke.emissionRate = 10 * (0.65f - ((float)playerHealth / (float)initialPlayerHealth));

                }
            }
            
        }
        
		// what you collided with Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {

        }

        // Deal explosion damage and display burnt effect
        if (collision.gameObject.CompareTag("Explosion"))
        {
            playerHealth -= (int)explosionDamage;
            Debug.Log("Explosion damage taken!!!");
        }
	}
}