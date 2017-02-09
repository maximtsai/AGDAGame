using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour {
    private GameObject playerObject;
    private float distToPlayer;
    // Use this for initialization
    void Start () {
        playerObject = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        // Debug.Log(Vector2.Distance(playerObject.transform.position, this.transform.position ));
	}
}
