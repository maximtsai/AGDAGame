using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBehavior : MonoBehaviour {
    public GameObject dmgSpark;
    GameObject listOfDmgSparks;

    public float minDmgThreshold = 10; // how much force is required to do any amount of damage 
    public float initBreakThreshold = 40; // initially how much force is required to destroy the armor
    private float currBreakThreshold; // how much force is required to destroy the armor
    public float brittleness = 0.5f; // multiplier for how much breakThreshold is lowered whenever force exceeds minDmgThreshold
    public bool createSparks = true;
    public bool flashRed = true;
    // Metal would have a good balance between minDmgThreshold, breakThreshold and brittleness
    // Ceramic plates (protective but not good for multiple uses) have medium minDmgThreshold, high breakThreshold,
    // but also high brittlenss
    FlashDamage FD;
    Vector3 currSize;
    int breakCounter = 99999;
    float initialDeltaTime = 0;
    float tempArmor = 0; // temporary armor that armor gains after being hit, to reduce instagib scenarios
    void Awake () {
        initialDeltaTime = Time.fixedDeltaTime;
        currBreakThreshold = initBreakThreshold;
        if (flashRed)
        {
            FD = GetComponent<FlashDamage>();
        }
        listOfDmgSparks = GameObject.Find("ListOfDmgSparks");
        if (!dmgSpark)
        {
            dmgSpark = listOfDmgSparks.transform.GetChild(0).gameObject;
        }

    }

    private void FixedUpdate()
    {
        if (tempArmor > 0)
        {
            tempArmor = Mathf.Max(0, tempArmor - 0.25f * Time.timeScale);
        }
        else if (tempArmor < 0)
        {
            tempArmor = Mathf.Min(0, tempArmor + 0.25f * Time.timeScale);
        }
        if (breakCounter < 1000)
        {
            breakCounter--;

            if (breakCounter <= 0)
            {
                currSize = this.transform.localScale;
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
        ContactPoint2D contact = collision.contacts[0];
        GameObject impactedPiece = contact.otherCollider.gameObject;
        float dotImpact = Mathf.Abs(Vector2.Dot(contact.normal, collision.relativeVelocity));
        Rigidbody2D otherColliderRB = collision.rigidbody;
        float sumImpact = dotImpact;
        float colliderMass = 9999;
        if (otherColliderRB)
        {
            colliderMass = otherColliderRB.mass;
            float speedMult = 0.2f;
            float speedImpact = Mathf.Sqrt(Vector2.SqrMagnitude(otherColliderRB.velocity - collision.otherRigidbody.velocity));
            if (otherColliderRB.gameObject.CompareTag("Player")) {
                // attacker was a player
                PlayerController PControl = otherColliderRB.gameObject.GetComponent<PlayerController>();
                if (PControl.isForwardPressed())
                {
                    // Add a bit of extra damage if player is accelerating into the attack
                    // Done so that pushing up against an opponent with a weapon will do a bit more damage.
                    float facingDirX = -Mathf.Sin(otherColliderRB.rotation * Mathf.Deg2Rad);
                    float facingDirY = Mathf.Cos(otherColliderRB.rotation * Mathf.Deg2Rad);
                    Vector2 otherPlayerDirectionVector = new Vector2(facingDirX, facingDirY);

                    sumImpact += 5*Vector2.Dot(contact.normal, otherPlayerDirectionVector);
                }
            }

            if (otherColliderRB.sharedMaterial)
            {
                speedMult = otherColliderRB.sharedMaterial.friction;
            }
            sumImpact += speedMult * speedImpact;
        }
        if (collision.collider.gameObject.CompareTag("Weapon"))
        {
            sumImpact = sumImpact * 2f + 6;
        }
        else if (collision.collider.gameObject.CompareTag("Player"))
        {
            sumImpact = sumImpact * 1.2f + 1;
        }
        sumImpact *= colliderMass/(1+colliderMass); // more massive objects hurt more
        float finalImpact = sumImpact - minDmgThreshold - tempArmor;
        if (finalImpact > 0)
        {
            tempArmor = Mathf.Max(tempArmor, finalImpact*1.5f + 1);
            int armorRemainDuration = 100;

            if (dotImpact > currBreakThreshold)
            {
                // armor is broken
                breakArmor(armorRemainDuration);
                if (flashRed)
                {
                    FD.flashRed(0.9f);
                    Time.timeScale = Mathf.Max(0, Mathf.Min(Time.timeScale, 1-0.05f*finalImpact));
                    Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
                }
                if (createSparks && breakCounter > armorRemainDuration - 3)
                {
                    createDmgSpark(new Vector3(contact.point.x, contact.point.y, 0), finalImpact * 1.5f + 1);
                }
            }
            else
            {
                currBreakThreshold -= brittleness * finalImpact;
                if (createSparks && breakCounter > armorRemainDuration - 3)
                {
                    createDmgSpark(new Vector3(contact.point.x, contact.point.y, 0), finalImpact);
                }
                if (FD && flashRed)
                {
                    //Debug.Log(currBreakThreshold +", "+ initBreakThreshold);
                    FD.SetRed(1 - currBreakThreshold / initBreakThreshold);
                    FD.flashRed(1 - currBreakThreshold / initBreakThreshold + 0.5f);
                    Time.timeScale = Mathf.Max(0, Mathf.Min(Time.timeScale, 1 - 0.02f * finalImpact));
                    Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
                }
            }
        } else
        {
            // temporarily make armor a bit more vulnerable after it's deflected an attack
            tempArmor -= sumImpact * 0.2f;
        }
    }

    void createDmgSpark(Vector3 sparkPos, float impactMag)
    {
        bool sparkHandled = false;

        foreach (Transform childSpark in listOfDmgSparks.transform)
        {
            ParticleSystem sparkObj = childSpark.GetComponent<ParticleSystem>();
            if (!sparkObj.isPlaying)
            {
                sparkHandled = true;
                sparkObj.transform.position = sparkPos;
                sparkObj.startLifetime = Mathf.Min(1, 1.2f + 0.04f * (impactMag));
                sparkObj.startSpeed = Mathf.Min(100, 25 + 5 * (impactMag));
                sparkObj.Emit((int)(0.75f + 0.15f * impactMag + Random.Range(0, 0.5f)));
                break;
            }
        }
        if (!sparkHandled)
        {
            // create new spark object
            dmgSpark.transform.position = sparkPos;
            Instantiate(dmgSpark, listOfDmgSparks.transform, true);
            foreach (Transform childSpark in listOfDmgSparks.transform)
            {
                ParticleSystem sparkObj = childSpark.GetComponent<ParticleSystem>();
                if (!sparkObj.isPlaying)
                {
                    sparkObj.transform.position = sparkPos;
                    sparkObj.startLifetime = Mathf.Min(1, 1 + 0.04f * (impactMag));
                    sparkObj.startSpeed = Mathf.Min(100, 15 + 6 * (impactMag)) + Random.Range(0, 15);
                    sparkObj.Emit((int)(0.7f + 0.15f * impactMag + Random.Range(0, 0.3f)));
                    break;
                }
            }
        }
    }

    public void breakArmor(int durationUntilDisappear)
    {
        this.transform.parent = null;
        breakCounter = durationUntilDisappear;
        if (!gameObject.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            if (rb)
            {
                rb.gravityScale = 0;
                rb.drag = 1.5f;
                rb.angularDrag = 1;
                rb.mass = 0.01f;
                rb.velocity = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
                rb.AddTorque(Random.Range(-5, 5));
            }
        }
        foreach(Transform child in this.transform)
        {
            BreakableBehavior BB = child.gameObject.GetComponent<BreakableBehavior>();
            if (BB)
            {
                BB.breakArmor(durationUntilDisappear + 15);
            }
        }
    }
}
