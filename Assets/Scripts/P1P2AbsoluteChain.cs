using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This Script is used to put a limit on how far p1 and p2 can be apart. Without this script p1 and p2
 * can go infinitely far away which will lead to overly zoomed out cameras.
 **/
public class P1P2AbsoluteChain : MonoBehaviour {
    public GameObject player1;
    public GameObject player2;
    public float maxDist = 20;
    public float softDist = 15;
    public GameObject Chain1;
    public GameObject Chain2;
    public GameObject Chain3;
    public GameObject Chain4;

    SpriteRenderer Chain1SR;
    SpriteRenderer Chain2SR;
    SpriteRenderer Chain3SR;
    SpriteRenderer Chain4SR;
    Color tempColor;
    Vector3 tempVec;

    float potentialSoftDist;
    Rigidbody2D p1RigidBody;
    Rigidbody2D p2RigidBody;
    Vector2 forceAdded;
    // Use this for initialization
    void Start() {
        potentialSoftDist = 0.7f * softDist;
        p1RigidBody = player1.GetComponent<Rigidbody2D>();
        p2RigidBody = player2.GetComponent<Rigidbody2D>();
        forceAdded = new Vector2(0, 0);
        Chain1SR = Chain1.GetComponent<SpriteRenderer>();
        Chain2SR = Chain2.GetComponent<SpriteRenderer>();
        Chain3SR = Chain3.GetComponent<SpriteRenderer>();
        Chain4SR = Chain4.GetComponent<SpriteRenderer>();
        tempColor = new Color(1, 1, 1, 0);
        tempVec = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (player2.activeSelf)
        {
            float xDiff = player1.transform.position.x - player2.transform.position.x;
            float yDiff = player1.transform.position.y - player2.transform.position.y;

            if (Mathf.Abs(yDiff) > potentialSoftDist || Mathf.Abs(xDiff) > potentialSoftDist)
            {
                // actually check if softdist is violated
                float sqrtDist = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff);
                if (sqrtDist > softDist)
                {
                    // if two players are too far apart, apply force bringing them closer together
                    float multRatio = (sqrtDist / softDist - 1) * 20;
                    forceAdded.x = xDiff * multRatio;
                    forceAdded.y = yDiff * multRatio;

                    p1RigidBody.AddForce(-forceAdded);
                    p2RigidBody.AddForce(forceAdded);

                    // make a chain in between the characters light up to indicate pull
                    tempColor.a = multRatio * 0.1f - 0.05f;
                    Chain1SR.color = tempColor;
                    Chain2SR.color = tempColor;
                    Chain3SR.color = tempColor;
                    Chain4SR.color = tempColor;

                    // make sure chains are in right position and orientation
                    Chain1.transform.position = 0.2f * player1.transform.position + 0.8f * player2.transform.position;
                    Chain2.transform.position = 0.4f * player1.transform.position + 0.6f * player2.transform.position;
                    Chain3.transform.position = 0.6f * player1.transform.position + 0.4f * player2.transform.position;
                    Chain4.transform.position = 0.8f * player1.transform.position + 0.2f * player2.transform.position;

                    tempVec.z = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg + 90;
                    Chain1.transform.eulerAngles = tempVec;
                    Chain2.transform.eulerAngles = tempVec;
                    Chain3.transform.eulerAngles = tempVec;
                    Chain4.transform.eulerAngles = tempVec;
                }
                else if (Chain1SR.color.a > 0)
                {
                    tempColor.a = 0;
                    Chain1SR.color = tempColor;
                    Chain2SR.color = tempColor;
                    Chain3SR.color = tempColor;
                    Chain4SR.color = tempColor;
                }
            }
        }
    }
}
