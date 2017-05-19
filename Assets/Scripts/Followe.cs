using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Followe : MonoBehaviour {

    private Vector3 xvec = new Vector3(2, 2, 0);
    public GameObject main1;
    public GameObject main2;
    private Vector3 wantedPos;
    private Vector3 v1, v2;
    private float d1, d2;
    private Rigidbody2D enemyBody;
    public float turnSpeed;
    private float origTurnSpeed;
    float initialDeltaTime = 0;
    private Vector2 forwardVec = new Vector2(1, 0);
    private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    public float acceleration;
    public float maxSpeed;
    public GameObject[] listOfHittables;
    private Vector3[] listOfHitVectors;
    private int loh_len;
    private float[] listOfHittableDistances;
    public float minDist;

    // Use this for initialization
    void Start()
    {
        v1 = new Vector3(0,0,0);
        v2 = new Vector3(0, 0, 0);
        enemyBody = GetComponent<Rigidbody2D>();
        origTurnSpeed = turnSpeed;
        initialDeltaTime = Time.fixedDeltaTime;
        listOfHitVectors = new Vector3[listOfHittables.Length];
        listOfHittableDistances = new float[listOfHittables.Length];
    }

    // Update is called once per frame
    void Update()
    {
        v1 = this.transform.position - main1.transform.position;
        v2 = this.transform.position - main2.transform.position;
        d1 = v1.magnitude;
        d2 = v2.magnitude;
        if (d1 >= d2)
        {
            wantedPos = main2.transform.position - this.transform.position;
        }
        else
        {
            wantedPos = main1.transform.position - this.transform.position;
        }
        forwardVec.Set(Mathf.Cos(wantedPos.x), Mathf.Sin(wantedPos.y));
        Vector2 forward = acceleration * forwardVec;
        if (enemyBody.velocity.sqrMagnitude > maxSpeed)
        {
            Vector2 baseSpd = forward * maxSpeed / enemyBody.velocity.sqrMagnitude;
            Vector2 additionalSpd = forward - baseSpd;
            forward = baseSpd + additionalSpd * Mathf.Max((maxSpeed + 15 - enemyBody.velocity.sqrMagnitude) / 15, 0);
        }
        enemyBody.AddForce(-1 * enemyBody.velocity);
        enemyBody.AddForce(forward);
        int i = 0;
        foreach(GameObject x in listOfHittables)
        {
            listOfHitVectors[i] = x.transform.position - this.transform.position;
            listOfHittableDistances[i] = listOfHitVectors[i].magnitude;
            i++;
        }
        for(i = 0; i < listOfHittables.Length; i++)
        {
            if(listOfHittableDistances[i] <= minDist)
            {
                forwardVec *= -1;
                MoveRapidlyToPlayer();
            }
        }
        
    }

    void MoveRapidlyToPlayer()
    {
        Vector3 playerPos;
        if (d1 > d2)
            playerPos = main2.transform.position;
        else
            playerPos = main1.transform.position;
        Vector3 wantedDir = this.transform.position - playerPos;
        for(int i = 0; i < 5; i++)
        {
            enemyBody.AddForce(enemyBody.velocity);
            enemyBody.AddForce(wantedDir);
        }
    }
}
