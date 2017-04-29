﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBehavior : MonoBehaviour {
    public float minDmgThreshold = 10; // how much force is required to do any amount of damage 
    public float initBreakThreshold = 40; // initially how much force is required to destroy the armor
    private float currBreakThreshold; // how much force is required to destroy the armor
    public float fragility = 0.5f; // multiplier for how much breakThreshold is lowered whenever force exceeds minDmgThreshold
    // Metal would have a good balance between minDmgThreshold, breakThreshold and fragility
    // Ceramic plates (protective but not good for multiple uses) have medium minDmgThreshold, high breakThreshold,
    // but also high fragility
    FlashDamage FD;
    Color armorColor = new Color(1, 1, 1);
    Vector3 currSize;
    int breakCounter = 99999;
    float tempArmor = 0; // temporary armor that armor gains after being hit, to reduce instagib scenarios
    void Start () {
        currSize = this.transform.localScale;
        currBreakThreshold = initBreakThreshold;
        FD = GetComponent<FlashDamage>();
    }
	
	// Update is called once per frame
	void Update () {
        float nonRedColor = currBreakThreshold / initBreakThreshold;
		if (armorColor.b < nonRedColor)
        {
            float resultColor = Mathf.Min(nonRedColor, armorColor.g + 0.01f);
            armorColor.g = resultColor;
            armorColor.b = resultColor;
        }
	}

    private void FixedUpdate()
    {
        tempArmor = Mathf.Max(0, tempArmor - 0.5f);
        if (breakCounter < 1000)
        {
            breakCounter--;

            if (breakCounter <= 0)
            {
                currSize.x -= 0.02f;
                currSize.y -= 0.02f;
                this.transform.localScale = currSize;
                if (currSize.x < 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        //Debug.Log();
        GameObject impactedPiece = collision.contacts[0].otherCollider.gameObject;
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
        }
        else if (collision.collider.gameObject.CompareTag("SoftWeapon"))
        {
            dotImpact = (dotImpact) * 1.6f + 1;
            colliderMass *= 2;
        }
        else if (collision.collider.gameObject.CompareTag("Player"))
        {
            dotImpact = dotImpact * 1.5f + 1;
        }
        dotImpact *= colliderMass/(1+colliderMass); // more massive objects hurt more
        dotImpact -= tempArmor;
        tempArmor = Mathf.Max(tempArmor, dotImpact + 1);
        float finalImpact = dotImpact - minDmgThreshold;
        Debug.Log(finalImpact);
        if (finalImpact > 0)
        {
            if (dotImpact > currBreakThreshold)
            {
                // armor is broken
                this.transform.parent = null;
                //GetComponent<PolygonCollider2D>().enabled = false;
                breakCounter = 50;
                Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0;
                rb.velocity = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
                rb.AddTorque(Random.Range(-5, 5));
            }
            else
            {
                currBreakThreshold -= fragility * finalImpact;
                if (FD)
                {
                    //Debug.Log(currBreakThreshold +", "+ initBreakThreshold);
                    FD.SetRed(1 - currBreakThreshold / initBreakThreshold);
                    FD.flashRed(1 - currBreakThreshold / initBreakThreshold + 0.5f);
                }
            }
        }
        /*
            float playerMass = this.GetComponent<Rigidbody2D>().mass;
            // more massive objects you hit hurt you more. More massive players receive slightly less damage, slightly.
            float massRatio = colliderMass / (colliderMass + Mathf.Max(1, playerMass * 0.2f));
            dotImpact *= massRatio;
            //Debug.Log("origImpact: "+dotImpact);
            dotImpact -= armorVal;
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
                tempArmor = damageDealt * 2 + 2;
                int bloodEmitted = (int)Mathf.Max(1, damageDealt * 0.35f);
                blood.Emit(bloodEmitted);
                blood.transform.position = collision.contacts[0].point;
                float playerHealthPercentage = (float)playerHealth / (float)initialPlayerHealth;
                if (playerHealthPercentage < 0.65f && playerHealthPercentage > 0.25f)
                {
                    smoke.emissionRate = 4 * (0.65f - ((float)playerHealth / (float)initialPlayerHealth));
                }
                else if (playerHealthPercentage <= 0.25f)
                {
                    smoke.emissionRate = 10 * (0.65f - ((float)playerHealth / (float)initialPlayerHealth));

                }
            }
            */

    }
}
