using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject player1;
    public GameObject player2;
    public float distance = 2f;
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

	// Use this for initialization
	void Start () {
        Debug.Log(Screen.height);
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
        // camera position also influenced by player velocity and player acceleration
        player1Vel = player1.transform.position - player1PrevPos;
        player1Acc = player1Vel - player1PrevVel;
        if (hasPlayerTwo)
        {
            player2Vel = player2.transform.position - player2PrevPos;
            player2Acc = player2Vel - player2PrevVel;
            // wantedPos influenced by p1 and p2 orientation, velocity, and acceleration
            wantedPos = (player1.transform.TransformPoint(distance, 0, height)
                + (player1Vel + player2Vel) * velInfluence * 0.4f
                + (player1Acc + player2Acc) * accInfluence * 0.4f
                + player2.transform.TransformPoint(distance, 0, height)) / 2;
            player2PrevPos = player2.transform.position;
            // adjust camera zoom based on player distance
            playerDist = player1.transform.position - player2.transform.position;
            float goalSize = Mathf.Max(defaultCameraSize, playerDist.magnitude * 0.55f + 2);
            Camera.main.orthographicSize = goalSize;
        } else
        {
            wantedPos = player1.transform.TransformPoint(distance, 0, height) + player1Vel* velInfluence + player1Acc * accInfluence;
            // adjust camera zoom based on player velocity
            Camera.main.orthographicSize += (player1Vel.sqrMagnitude)*3;
            Camera.main.orthographicSize -= (Camera.main.orthographicSize - defaultCameraSize) * 0.1f;
        }
        float p1AccelMag = player1Acc.magnitude;
        // periods of high acceleration do not invoke immediate camera movement.
        this.transform.position = Vector3.Lerp(transform.position, wantedPos, Time.deltaTime * damp / (1 + p1AccelMag * p1AccelMag * 5000));
        player1PrevPos = player1.transform.position;
        player1PrevVel = player1Vel;
        player2PrevVel = player2Vel;
    }
}
