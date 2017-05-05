using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is used to keep the core from rotating too much
// to create the illusion that it's magnetic/magical
public class CoreScript : MonoBehaviour {
    float rotateVel;
    float rotateAcc;
    Vector3 currRotation;
    int health = 2;
    bool isInvul = false;
    float recoveryDuration = 14;// seconds
    float invulDuration = 1.5f;
    float initialDeltaTime = 0;
    float timeOfInjury = 0;
    SpriteRenderer SR;
    Color origColor;
    Color currColor;
    bool turningRed = false;
    public GameObject gibPiece;
    void Start () {
        currRotation = new Vector3(0, 0, 0);
        initialDeltaTime = Time.fixedDeltaTime;
        SR = GetComponent<SpriteRenderer>();
        origColor = SR.color;
        currColor = origColor;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInvul && !collision.collider.gameObject.CompareTag("Armor"))
        {
            ContactPoint2D contact = collision.contacts[0];
            float dotImpact = Mathf.Abs(Vector2.Dot(contact.normal, collision.relativeVelocity));
            Rigidbody2D otherColliderRB = collision.rigidbody;
            float sumImpact = dotImpact;
            float colliderMass = 9999;
            if (otherColliderRB)
            {
                colliderMass = otherColliderRB.mass;
                float speedMult = 0.2f;
                float speedImpact = Mathf.Sqrt(Vector2.SqrMagnitude(otherColliderRB.velocity - collision.otherRigidbody.velocity));
                if (otherColliderRB.sharedMaterial)
                {
                    speedMult = otherColliderRB.sharedMaterial.friction * 2;
                }
                sumImpact += speedMult * speedImpact;
            }
            if (sumImpact > 8)
            {
                health--;
                if (health == 1)
                {
                    isInvul = true;
                    timeOfInjury = Time.time;
                    turningRed = true;
                    Time.timeScale = 0.01f;
                    Time.fixedDeltaTime = initialDeltaTime * Time.timeScale;
                }
                else if (health <= 0)
                {
                    // blow up
                    this.transform.parent.gameObject.SetActive(false);
                    for (int i = 0; i < 30; i++)
                    {
                        GameObject pc = Instantiate(gibPiece, this.transform.position, this.transform.rotation);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        /*
        if (Mathf.Abs(this.transform.eulerAngles.z) > 0.01f)
        {
            rotateAcc += 0.004f * getShortestRotation360(currRotation.z, 0);
            rotateVel += rotateAcc;
            currRotation.z = this.transform.eulerAngles.z + rotateVel;
            this.transform.eulerAngles = currRotation;
        }
        rotateVel *= 0.4f;
        rotateAcc *= 0.9f;
        */
    }
    private void LateUpdate()
    {
        if (isInvul && (Time.time - timeOfInjury > invulDuration))
        {
            // no longer invulnerable
            isInvul = false;
        }

        if (health == 1)
        {
            // If damaged, flash red and recover after certain amount of time elapsed.
            if (turningRed)
            {
                // gradually reduce green and blue
                currColor.g = Mathf.Max(0, currColor.g - 0.05f);
                currColor.b = Mathf.Max(0, currColor.b - 0.05f);
                SR.color = currColor;
                if (currColor.g <= 0 && currColor.b <= 0)
                {
                    turningRed = false;
                }
            }
            else
            {
                // gradually add back green and blue
                currColor.g = Mathf.Min(origColor.g, currColor.g + 0.05f);
                currColor.b = Mathf.Min(origColor.b, currColor.b + 0.05f);
                SR.color = currColor;
                if (currColor.g >= origColor.g && currColor.b >= origColor.b)
                {
                    turningRed = true;
                }
            }
            if (Time.time - timeOfInjury > recoveryDuration)
            {
                recoveryDuration += 2;
                currColor = origColor;
                SR.color = currColor;
                health = 2;
            }
        }
    }
    float getShortestRotation360(float startRot, float goalRot)
    {
        float startRotNorm = startRot;
        float goalRotNorm = goalRot;
        while (startRotNorm < 0)
        {
            startRotNorm += 360;
        }

        while (goalRotNorm < 0)
        {
            goalRotNorm += 360;
        }

        float diffVal = goalRotNorm - startRotNorm;
        if (Mathf.Abs(diffVal) > 180)
        {
            if (diffVal < 0)
            {
                diffVal += 360;
            }
            else {
                diffVal -= 360;
            } 
        }

        return diffVal;
    }
    public void setInvul()
    {
        isInvul = true;
    }
}
