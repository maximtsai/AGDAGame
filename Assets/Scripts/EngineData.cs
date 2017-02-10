using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineData : MonoBehaviour {
    public string name = "ENGINENAME";
    public float acceleration = 100;
    public float maxSpeed = 200;
    public float turning = 10;
    public int engineType = 0; // 0 is default engine, 1-99999 reserved for any special engine behavior 
    private Vector2 forwardVec = new Vector2(1, 0);
    Rigidbody2D playerBody;
    // Use this for initialization
    void Start () {
        // check if engine is actually attached to the player
		if (this.transform.parent.tag == "Player")
        {
            playerBody = this.transform.parent.GetComponent<Rigidbody2D>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		switch(engineType)
        {
            case 0:
                break;
            case 1:
                break;
        }
	}
    void forward()
    {

    }
    void turn()
    {

    }
}
