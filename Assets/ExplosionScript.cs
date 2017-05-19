using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {
    public float detonationImpactVelocity = 3;
    public float detonationDelay = 3;
    public GameObject explosionPrefab;
    public GameObject sparks;
    float startTime;
    // Use this for initialization
    void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint2D contact = collision.contacts[0];
            // compare relative velocity to collision normal - so we don't explode from a fast but gentle glancing collision

            float velocityAlongCollisionNormal = Mathf.Abs(Vector2.Dot(contact.normal, collision.relativeVelocity));

            if (velocityAlongCollisionNormal > detonationImpactVelocity)
            {
            }
        }
        if (Time.time > startTime + detonationDelay)
        {

        }
    }
    private void Explode()
    {
        Instantiate(explosionPrefab, this.transform.position, new Quaternion(0,0,0,0));
    }
}
