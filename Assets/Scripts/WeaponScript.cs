﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {
    // Use this for initialization
    Rigidbody2D rbComponent;
    GameObject listOfWeapons;
    public float weaponOffset = 1;
    void Start () {
        listOfWeapons = GameObject.Find("ListOfWeapons");
        rbComponent = this.GetComponent<Rigidbody2D>();
    }

    virtual public void activateWeapon(Rigidbody2D playerRigidBody)
    {

    }
    virtual public void deactivateWeapon()
    {

    }
}
