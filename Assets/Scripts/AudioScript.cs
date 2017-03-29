using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {
    public AudioSource aSource;
    public AudioClip bodyHit;

    // Use this for initialization
    void Start () {
        // GetComponent<AudioSource>().PlayOneShot(sound);
        // AudioSource audio = GetComponent<AudioSource>();
        // audio.Play();

	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log();
        // what u collided with Debug.Log(collision.collider.gameObject.name);
        // Debug.Log(this.gameObject.tag);

        if (collision.contacts[0].otherCollider.gameObject.CompareTag("Weapon") && collision.collider.gameObject.CompareTag("Player"))
        {
            float dotImpact = Vector2.Dot(collision.contacts[0].normal, collision.relativeVelocity);
            float playerMass = this.GetComponent<Rigidbody2D>().mass;
            Rigidbody2D colliderRB = collision.contacts[0].collider.GetComponent<Rigidbody2D>();
            float colliderMass = 9999;
            if (colliderRB)
            {
                colliderMass = colliderRB.mass;
            }
            // more massive objects you hit hurt you more. More massive players receive slightly less damage, slightly.
            float massRatio = colliderMass / (colliderMass + Mathf.Max(1, playerMass * 0.1f));
            dotImpact *= massRatio;
            if (Mathf.Abs(dotImpact) >= 2)
            {
                //bumpSound.frequency = 2000;
                aSource.pitch = 0.5f + collision.relativeVelocity.magnitude * 0.07f + Random.Range(-0.2f,0.2f);
                float MaxSound = Mathf.Min(1, (Mathf.Abs(dotImpact) - 2) * 0.2f);
                aSource.PlayOneShot(bodyHit, MaxSound);
                //AudioSource.PlayClipAtPoint(bumpSound, collision.contacts[0].point);
                // GetComponent<AudioSource>
            }
        }
    }

}
