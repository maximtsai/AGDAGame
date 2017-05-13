using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SparkSystem : MonoBehaviour {
    public GameObject spark;
    public bool sparkOnCollide = true; // collisions cause sparks
    GameObject listOfSparks;
    public float sparkMinVel = 10;
    public float sparkIntensity = 1;
    bool sparkHandled = false;
    private void Awake()
    {
        listOfSparks = GameObject.Find("ListOfSparks");
        if (!listOfSparks)
        {
            Destroy(this);
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (sparkOnCollide)
        {
            ContactPoint2D contact = collision.contacts[0];
            // calculate various collision values, higher numbers usually mean stronger collisions
            float impactSpd = collision.relativeVelocity.magnitude;
            float dotImpact = Mathf.Abs(Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity));
            float strengthOfImpact = Mathf.Max(0.8f * impactSpd + 0.15f * dotImpact, dotImpact + 0.2f * impactSpd);
            float sparkExtraSpd = Mathf.Max(0.1f, dotImpact * 0.1f);

            if (strengthOfImpact > sparkMinVel)
            {
                strengthOfImpact *= sparkIntensity;
                Vector3 sparkPos = new Vector3(contact.point.x, contact.point.y);
                createSpark(sparkPos, strengthOfImpact - sparkMinVel);
            }
        }
    }

    public void createSpark(Vector2 sparkPos, float sparkStrength)
    {
        foreach (Transform childSpark in listOfSparks.transform)
        {
            ParticleSystem sparkObj = childSpark.GetComponent<ParticleSystem>();
            // use an existing spark object if it is available
            if (!sparkObj.isPlaying)
            {
                sparkHandled = true;
                sparkObj.transform.position = sparkPos;
                sparkObj.startLifetime = Mathf.Min(1, 0.25f + 0.05f *sparkStrength);
                sparkObj.startSpeed = Mathf.Min(100, 14 + 6 * sparkStrength);
                sparkObj.Emit((int)Mathf.Min(4, (1 + 0.3f * sparkStrength)));

                break;
            }
        }
        if (!sparkHandled)
        {
            // create new spark object
            spark.transform.position = sparkPos;
            Instantiate(spark, listOfSparks.transform, true);
            foreach (Transform childSpark in listOfSparks.transform)
            {
                ParticleSystem sparkObj = childSpark.GetComponent<ParticleSystem>();
                if (!sparkObj.isPlaying)
                {
                    sparkObj.transform.position = sparkPos;
                    sparkObj.startLifetime = Mathf.Min(1, 0.25f + 0.05f * sparkStrength);
                    sparkObj.startSpeed = Mathf.Min(100, 14 + 6 * sparkStrength);
                    sparkObj.Emit((int)Mathf.Min(4, (1 + 0.3f * sparkStrength)));
                    break;
                }
            }
        }
        sparkHandled = false;
    }
}
