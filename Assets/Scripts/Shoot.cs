using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject projectile;
    public float speed = 20;
    public GameObject main1;
    public GameObject main2;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Rigidbody2D instantiatedProjectile = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;
        Vector3 playerPos;
        //if (d1 > d2)
           // playerPos = main2.transform.position;
        //else
          //  playerPos = main1.transform.position;
        //instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(playerPos.transform.position.x-this.transform.position.x, playerPos.transform.position.y - this.transform.position.y, 0) * speed);
    }
}