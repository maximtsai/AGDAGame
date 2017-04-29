using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SparkSystem : MonoBehaviour {
    public GameObject spark;
    GameObject listOfSparks;
    public float sparkVel = 11;
    bool sparkHandled = false;

    void Start()
    {
        listOfSparks = GameObject.Find("ListOfSparks");
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        float impactSpd = collision.relativeVelocity.magnitude;
        float dotImpact = Mathf.Abs(Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity));
        float impactMag = Mathf.Max(0.8f * impactSpd + 0.15f * dotImpact, dotImpact + 0.2f * impactSpd);
        float sparkExtraSpd = Mathf.Max(0.1f, dotImpact * 0.1f);

        if (impactMag > sparkVel)
        {
            Vector3 sparkPos = new Vector3(contact.point.x, contact.point.y);
            foreach (Transform childSpark in listOfSparks.transform)
            {
                ParticleSystem sparkObj = childSpark.GetComponent<ParticleSystem>();
                if (!sparkObj.isPlaying)
                {
                    sparkHandled = true;
                    sparkObj.transform.position = sparkPos;
                    sparkObj.startLifetime = Mathf.Min(1, 0.15f+0.06f*(impactMag - sparkVel));
                    sparkObj.startSpeed = Mathf.Min(100, 5+7*(impactMag - sparkVel) + sparkExtraSpd);
                    sparkObj.Emit((int)(1+0.3f*(impactMag - sparkVel)));
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
                        sparkObj.startLifetime = Mathf.Min(1, 0.1f + 0.05f * (impactMag - sparkVel));
                        sparkObj.startSpeed = Mathf.Min(100, 5 + 7 * (impactMag - sparkVel));
                        sparkObj.Emit((int)(1 + 0.3f * (impactMag - sparkVel)));
                        break;
                    }
                }
            }
            sparkHandled = false;
        }
            //spark.transform.position = flarePos;
            /*
            if(Mathf.Abs(Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity)) <= sparkVel)
            {
                Debug.Log("Low Speed Collision");
                spark = new GameObject();
                Instantiate(spark, this.transform, true);
            }
            */
        //foreach (ContactPoint2D contact in collision.contacts)
        //{
        //}
    }
}
