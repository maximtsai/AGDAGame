using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustControl : MonoBehaviour {
    bool forwardPressed = false;
    private ParticleSystem exhaustParticleSystem;
    float origExhaustSpeed;
    float origExhaustSize;
    float forwardPressedDuration;
    // Use this for initialization
    void Awake () {
        exhaustParticleSystem = GetComponent<ParticleSystem>();
        origExhaustSpeed = exhaustParticleSystem.startSpeed;
        origExhaustSize = exhaustParticleSystem.startSize;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (forwardPressed)
        {
            forwardPressedDuration += Time.deltaTime;
            exhaustParticleSystem.Play();
        }
        if (exhaustParticleSystem.startSize != origExhaustSize && forwardPressedDuration > 0.1f)
        {
            float randAdd = Random.Range(-0.12f, 0.12f);
            exhaustParticleSystem.startSize = origExhaustSize + randAdd;
            exhaustParticleSystem.startSpeed = origExhaustSpeed + randAdd*6;
        }
    }

    public void setForward(bool goingForward)
    {
        forwardPressed = goingForward;
        if (!goingForward)
        {
            forwardPressedDuration = 0;
            // make a small initial spurt when you press down forward or when you release
            exhaustParticleSystem.startSize = origExhaustSize * 1.4f;
            exhaustParticleSystem.startSpeed = origExhaustSpeed * 1.2f;

        } else
        {
            exhaustParticleSystem.startSize = origExhaustSize * 1.4f;
            exhaustParticleSystem.startSpeed = origExhaustSpeed * 1.2f;
        }

    }
}
