using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SparkSystem : MonoBehaviour {
    public GameObject flares;

    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Instantiate(flares, contact.otherCollider.transform, true);
        }
        //if (collision.relativeVelocity.magnitude > 2)
        

    }
}
