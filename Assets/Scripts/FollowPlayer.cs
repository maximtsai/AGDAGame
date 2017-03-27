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
    Vector3 camAccel;
    Vector3 camVel;
    float randCamVelX = 0;
    float randCamVelY = 0;
    float randCamOffsetX = 0;
    float randCamOffsetY = 0;
    float cameraMovReduction = 0;
    PlayerMover player1Script;
	// Use this for initialization
	void Start () {
        player1Script = player1.GetComponent<PlayerMover>();
        if (player2 && player2.active)
        {
            hasPlayerTwo = true;
            player2PrevPos = player2.transform.position;
        }
        camAccel = new Vector3(0, 0, 0);
        camVel = new Vector3(0, 0, 0);
        wantedPos = new Vector3(0, 0, 0);
        player1PrevPos = player1.transform.position;
        // player1PrevRot = player1.transform.eulerAngles.z;
        this.transform.position = new Vector3(player1.transform.position.x, player1.transform.position.y, -10);
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        if (!player1Script.isForwardPressed())
        {
            cameraMovReduction = 0.4f;
        }
        // add a bit of random sway to the camera to keep things from staying too still
        randCamVelX += (Random.value - 0.5f) * (player1Vel.magnitude + 0.01f) * 0.08f;
        randCamVelY += (Random.value - 0.5f) * (player1Vel.magnitude + 0.01f) * 0.05f;
        randCamOffsetX += randCamVelX;// (Random.value - 0.5f) * player1Vel.magnitude;
        randCamOffsetY += randCamVelY;// (Random.value - 0.5f) * player1Vel.magnitude;
        randCamVelX *= 0.982f;
        randCamVelY *= 0.978f;
        randCamOffsetX *= 0.96f;
        randCamOffsetY *= 0.95f;
        // camera wanted position influenced by player velocity and player acceleration
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
            playerDist.x /= Camera.main.aspect;
            float goalSize = Mathf.Max(defaultCameraSize - 1 + Time.timeScale, (playerDist.magnitude) * 0.57f + 1 + Time.timeScale);
            Camera.main.orthographicSize = goalSize;
        } else
        {
            // additional amount to go forward, increased by velocity and decreased by turning or sudden stops
            movementBasedDistance += player1Vel.magnitude*1.8f - Mathf.Sqrt(Mathf.Max(0,player1Acc.magnitude - 0.01f)*2.5f);
            wantedPos = player1.transform.TransformPoint(distance + movementBasedDistance + randCamOffsetX, randCamOffsetY, height);
            // adjust camera zoom based on player velocity
            Camera.main.orthographicSize += player1Vel.magnitude * 0.25f;
            Camera.main.orthographicSize -= (Camera.main.orthographicSize - defaultCameraSize) * 0.024f;
        }
        // determine camera movement via acceleration+velocity
        float p1AccelMag = player1Acc.magnitude;
        camAccel = (wantedPos - this.transform.position - camVel * 15);
        if (camAccel.sqrMagnitude > 1)
        {
            camAccel = camAccel.normalized;
        }
        camAccel *= 0.1f;
        camVel += camAccel;
        this.transform.position += camVel;
        camVel *= 0.95f;
        movementBasedDistance *= 0.945f - cameraMovReduction;
        player1PrevPos = player1.transform.position;
        player1PrevVel = player1Vel;
        player2PrevVel = player2Vel;
        cameraMovReduction = 0;
    }
}
