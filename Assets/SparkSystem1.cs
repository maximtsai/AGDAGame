using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SparkSystem1 : MonoBehaviour
{
    public GameObject flares;
    public GameObject listOfFlares;
    public float sparkVel = 11;
    public GameObject flareX;


    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.contacts);
        foreach (ContactPoint2D contact in collision.contacts)
        {
            flares = flareX;
            Debug.Log("sparking");
            Debug.Log(collision.relativeVelocity.magnitude);
            //Instantiate(flares, listOfFlares.transform, true);
            Vector3 flarePos = new Vector3(contact.point.x, contact.point.y);
            flares.transform.position = flarePos;

            if (collision.relativeVelocity.magnitude <= sparkVel)
            {
                Debug.Log("Low Speed Collision");
                flares = new GameObject();
            }
            Instantiate(flares, listOfFlares.transform, true);
        }
        //if (collision.relativeVelocity.magnitude > 2)
    }
}
