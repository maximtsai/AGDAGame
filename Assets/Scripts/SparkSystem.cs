using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SparkSystem : MonoBehaviour {
    public GameObject spark;
    public GameObject listOfSparks;
    public float sparkVel = 11;
    bool sparkHandled = false;

    void Start()
    {

    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        float impactSpd = collision.relativeVelocity.magnitude;
        if (impactSpd > sparkVel)
        {
            Vector3 sparkPos = new Vector3(contact.point.x, contact.point.y);
            foreach (Transform childSpark in listOfSparks.transform)
            {
                ParticleSystem sparkObj = childSpark.GetComponent<ParticleSystem>();
                if (!sparkObj.isPlaying)
                {
                    sparkHandled = true;
                    sparkObj.transform.position = sparkPos;
                    sparkObj.startLifetime = Mathf.Min(1, 0.15f+0.06f*(impactSpd - sparkVel));
                    sparkObj.startSpeed = Mathf.Min(100, 5+9*(impactSpd - sparkVel));
                    sparkObj.Emit((int)(1+0.3f*(impactSpd - sparkVel)));
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
                        sparkObj.startLifetime = Mathf.Min(1, 0.1f + 0.05f * (impactSpd - sparkVel));
                        sparkObj.startSpeed = Mathf.Min(100, 5 + 7 * (impactSpd - sparkVel));
                        sparkObj.Emit((int)(1 + 0.3f * (impactSpd - sparkVel)));
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
