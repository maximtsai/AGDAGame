using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject player1;
    public GameObject player2;
    public float distance = 2f;
    float movementBasedDistance = 0;
    public float damp = 5f;
    public float height = -10;
    public float velInfluence = 20;
    public float accInfluence = 50;
    bool hasPlayerTwo = false;
    float defaultCameraSize = 8;
    Vector3 player1PrevPos;
    Vector3 player2PrevPos;
    Vector3 player1Vel, player2Vel;
    Vector3 player1PrevVel, player2PrevVel;
    Vector3 player1Acc, player2Acc;
    Vector3 playerDist;
    Vector3 wantedPos;
    float randCamVelX = 0;
    float randCamVelY = 0;
    float randCamOffsetX = 0;
    float randCamOffsetY = 0;
	// Use this for initialization
	void Start () {
        if (player2)
        {
            hasPlayerTwo = true;
            player2PrevPos = player2.transform.position;
        }
        wantedPos = new Vector3(0, 0, 0);
        player1PrevPos = player1.transform.position;
        this.transform.position = new Vector3(player1.transform.position.x, player1.transform.position.y, -10);
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        randCamVelX += (Random.value - 0.5f) * player1Vel.magnitude * 0.08f;
        randCamVelY += (Random.value - 0.5f) * player1Vel.magnitude * 0.05f;
        randCamOffsetX += randCamVelX;// (Random.value - 0.5f) * player1Vel.magnitude;
        randCamOffsetY += randCamVelY;// (Random.value - 0.5f) * player1Vel.magnitude;
        randCamVelX *= 0.98f;
        randCamVelY *= 0.975f;
        randCamOffsetX *= 0.96f;
        randCamOffsetY *= 0.95f;
        // camera position also influenced by player velocity and player acceleration
        player1Vel = player1.transform.position - player1PrevPos;
        player1Acc = player1Vel - player1PrevVel;
        if (hasPlayerTwo)
        {
            player2Vel = player2.transform.position - player2PrevPos;
            player2Acc = player2Vel - player2PrevVel;

            // wantedPos influenced by p1 and p2 orientation, velocity, and acceleration
            wantedPos = (player1.transform.TransformPoint(distance, 0, height)
                + (player1Vel + player2Vel) * velInfluence
                + (player1Acc + player2Acc) * accInfluence
                + player2.transform.TransformPoint(distance, 0, height)) / 2;
            player2PrevPos = player2.transform.position;
            // adjust camera zoom based on player distance
            playerDist = player1.transform.position - player2.transform.position;
            float goalSize = Mathf.Max(defaultCameraSize, playerDist.magnitude * 0.55f + 2);
            Camera.main.orthographicSize = goalSize;
        } else
        {
            movementBasedDistance += player1Vel.magnitude*2 - Mathf.Sqrt(Mathf.Max(0,player1Acc.magnitude - 0.01f))*2;
            wantedPos = player1.transform.TransformPoint(distance + movementBasedDistance + randCamOffsetX, randCamOffsetY, height);
            // adjust camera zoom based on player velocity
            Camera.main.orthographicSize += (player1Vel.sqrMagnitude);
            Camera.main.orthographicSize -= (Camera.main.orthographicSize - defaultCameraSize) * 0.03f;
        }
        float p1AccelMag = player1Acc.magnitude;
        // periods of high acceleration do not invoke immediate camera movement.
        this.transform.position = Vector3.Lerp(transform.position, wantedPos, Time.deltaTime * damp / (1 + p1AccelMag * p1AccelMag * 2000));
        player1PrevPos = player1.transform.position;
        player1PrevVel = player1Vel;
        player2PrevVel = player2Vel;
        movementBasedDistance *= 0.9f;
    }
}
