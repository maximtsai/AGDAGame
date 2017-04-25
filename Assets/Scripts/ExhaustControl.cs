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
    void Start () {
        exhaustParticleSystem = GetComponent<ParticleSystem>();
        origExhaustSpeed = exhaustParticleSystem.startSpeed;
        origExhaustSize = exhaustParticleSystem.startSize;
    }
	
	// Update is called once per frame
	void Update () {
        if (forwardPressed)
        {
            forwardPressedDuration += Time.deltaTime;
            exhaustParticleSystem.Play();
        }
        if (exhaustParticleSystem.startSize != origExhaustSize && forwardPressedDuration > 0.1f)
        {
            exhaustParticleSystem.startSize = origExhaustSize;
            exhaustParticleSystem.startSpeed = origExhaustSpeed;
        }
    }

    public void setForward(bool isPressed)
    {
        forwardPressed = isPressed;
        if (!isPressed)
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
