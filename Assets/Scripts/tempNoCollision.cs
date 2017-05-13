using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Temporarily prevents collision of two rigidbodies.
 * Usually used to prevent bullets from colliding with their parent guns as they leave the barrel.
 */
public class tempNoCollision : MonoBehaviour {
    bool isIgnoringCollider = false;
    public float durationToIgnore = 0.1f;
    float timeOfIgnoring;
    Collider2D ownCollider;
    List<Collider2D> listOfIgnoredColliders = new List<Collider2D>();
    void Start () {

    }

    // Update is called once per frame
    void Update () {
		if (isIgnoringCollider && Time.time > timeOfIgnoring + durationToIgnore)
        {
            foreach (Collider2D ignoredCollider in listOfIgnoredColliders)
            {
                Physics2D.IgnoreCollision(ignoredCollider, ownCollider, false);
            }
            Destroy(this); // no longer need this script anymore
        }
    }

    public void SetNoCollision(Collider2D colliderToIgnore)
    {
        listOfIgnoredColliders.Add(colliderToIgnore);
        ownCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(colliderToIgnore, ownCollider, true);
        isIgnoringCollider = true;
        timeOfIgnoring = Time.time;
    }
}
