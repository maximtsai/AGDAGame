using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareOnCollision : MonoBehaviour {

    public GameObject flares;
    public GameObject player;
    public GameObject player1;

    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.contacts);
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.Log(this.gameObject.name);
            Instantiate(flares, this.gameObject.transform, true);
            Vector3 flarePos = new Vector3(contact.point.x, contact.point.y);
            flares.transform.position = flarePos;
        }
        //if (collision.relativeVelocity.magnitude > 2)
    }
}
