using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thruster : MonoBehaviour {

	private Rigidbody2D rb;
	public int thrusterx = 10;
	public GameObject x;

	// Use this for initialization
	void Start () {
		rb = x.GetComponent<Rigidbody2D> ();
		Vector2 speed = new Vector2 (2, 2);
		rb.AddForce (speed * thrusterx);
	}
	
	// Update is called once per frame
	void Update () {
		//rb.velocity--;
	}
}
