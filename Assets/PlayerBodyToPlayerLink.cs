using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyToPlayerLink : MonoBehaviour {
    public GameObject player;
    private Rigidbody2D playerRigid;
    private Rigidbody2D playerBodyRigid;
    private Vector2 forwardVec = new Vector2(100, 0);
    // Use this for initialization
    void Start () {
        playerRigid = player.GetComponent<Rigidbody2D>();
        playerBodyRigid = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        playerBodyRigid.AddForce(playerRigid.velocity - playerBodyRigid.velocity);
        Debug.Log(playerRigid.velocity - playerBodyRigid.velocity);
        playerRigid.position = playerBodyRigid.position;
        playerRigid.velocity = playerBodyRigid.velocity;
        playerRigid.rotation = playerBodyRigid.rotation;
    }
}
