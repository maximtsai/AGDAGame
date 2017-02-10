using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject player;
    public float distance = 3f;
    public float damp = 5f;

	// Use this for initialization
	void Start () {
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 wantedPos = player.transform.TransformPoint(distance, distance, -10);
        this.transform.position = Vector3.Lerp(transform.position, wantedPos, Time.deltaTime * damp);
    }
}
