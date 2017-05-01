using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBehavior : MonoBehaviour {
    public GameObject dmgSpark;
    GameObject listOfDmgSparks;

    public float minDmgThreshold = 10; // how much force is required to do any amount of damage 
    public float initBreakThreshold = 40; // initially how much force is required to destroy the armor
    private float currBreakThreshold; // how much force is required to destroy the armor
    public float fragility = 0.5f; // multiplier for how much breakThreshold is lowered whenever force exceeds minDmgThreshold
    // Metal would have a good balance between minDmgThreshold, breakThreshold and fragility
    // Ceramic plates (protective but not good for multiple uses) have medium minDmgThreshold, high breakThreshold,
    // but also high fragility
    FlashDamage FD;
    Vector3 currSize;
    int breakCounter = 99999;
    float tempArmor = 0; // temporary armor that armor gains after being hit, to reduce instagib scenarios
    void Start () {
        currBreakThreshold = initBreakThreshold;
        FD = GetComponent<FlashDamage>();
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
            tempArmor = Mathf.Max(0, tempArmor - 0.5f);
        }
        else if (tempArmor < 0)
        {
            tempArmor = Mathf.Min(0, tempArmor + 0.5f);
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
        GameObject impactedPiece = collision.contacts[0].otherCollider.gameObject;
        float dotImpact = Mathf.Abs(Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity));
        Rigidbody2D otherColliderRB = collision.rigidbody;
        float sumImpact = dotImpact;
        
        float colliderMass = 9999;
        if (otherColliderRB)
        {
            colliderMass = otherColliderRB.mass;
        }
        if (collision.collider.gameObject.CompareTag("Weapon"))
        {
            sumImpact = sumImpact * 1.7f + 5;
        }
        else if (collision.collider.gameObject.CompareTag("Player"))
        {
            sumImpact = sumImpact * 1.2f + 1;
        }
        sumImpact *= colliderMass/(1+colliderMass); // more massive objects hurt more
        float finalImpact = sumImpact - minDmgThreshold - tempArmor;
        if (finalImpact > 0)
        {
            tempArmor = Mathf.Max(tempArmor, finalImpact + 1);

            if (dotImpact > currBreakThreshold)
            {
                // armor is broken
                breakArmor(100);
                FD.flashRed(0.9f);
                createDmgSpark(new Vector3(contact.point.x, contact.point.y, 0), finalImpact*1.5f);
            }
            else
            {
                currBreakThreshold -= fragility * finalImpact;
                createDmgSpark(new Vector3(contact.point.x, contact.point.y, 0), finalImpact);
                if (FD)
                {
                    //Debug.Log(currBreakThreshold +", "+ initBreakThreshold);
                    FD.SetRed(1 - currBreakThreshold / initBreakThreshold);
                    FD.flashRed(1 - currBreakThreshold / initBreakThreshold + 0.5f);
                }
            }
        } else
        {
            // temporarily make armor a bit more vulnerable after it's deflected an attack
            tempArmor -= sumImpact * 0.25f;
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
                sparkObj.startLifetime = Mathf.Min(1, 1 + 0.04f * (impactMag));
                sparkObj.startSpeed = Mathf.Min(100, 15 + 6 * (impactMag));
                sparkObj.Emit((int)(0.7f + 0.15f * impactMag + Random.Range(0, 0.3f)));
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
