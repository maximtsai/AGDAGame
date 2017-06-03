using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindRocketAI : MonoBehaviour {
    public float fuelDuration = 3;
    public ExhaustControl exhaustController; // should reference the particle system simulating engine exhaust
    public float acceleration = 30;
    public float maxSpeed = 20;
    public float presetSway = 0; // positive to sway left, negative to sway right
    public float randomSwayMagnitude = 0.5f;
    float randomSway = 0; // random swaying of rocket
    Rigidbody2D rocketRB;
    float startTime;
    bool stillHasFuel = true;
    private Vector2 forwardVec = new Vector2(0, 1);
    // Use this for initialization
    void Start () {
        startTime = Time.time;
        rocketRB = GetComponent<Rigidbody2D>();
        exhaustController.setForward(true);

    }

    // Update is called once per frame
    void FixedUpdate () {
        if (stillHasFuel)
        {
            randomSway = randomSway * 0.95f + Random.Range(-1.0f, 1.0f) * randomSwayMagnitude * Time.timeScale;
            rocketRB.rotation += randomSway + presetSway;
            forwardVec.Set(-Mathf.Sin(rocketRB.rotation * Mathf.Deg2Rad), Mathf.Cos(rocketRB.rotation * Mathf.Deg2Rad));
            if (startTime + fuelDuration < Time.time)
            {
                // no longer going forward
                exhaustController.setForward(false);
                stillHasFuel = false;
            } else
            {
                Vector2 forward = acceleration * forwardVec;
                if (rocketRB.velocity.sqrMagnitude > maxSpeed)
                {
                    // soft cap for max speed
                    Vector2 baseSpd = forward * maxSpeed / rocketRB.velocity.sqrMagnitude;
                    Vector2 additionalSpd = forward - baseSpd;
                    forward = baseSpd + additionalSpd * Mathf.Max((maxSpeed + 10 - rocketRB.velocity.sqrMagnitude) / 10, 0);
                }
                rocketRB.AddForce(forward);
            }
        }
	}
}
