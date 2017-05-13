using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {
    // Use this for initialization
    Rigidbody2D rbComponent;
    GameObject listOfWeapons;
    internal EngineScript engScript; // Internal means object can be used by subclasses, not just read
    public float weaponOffset = 1;
    public float moveMultiplier = 1;
    public float turnMultiplier = 1;
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
    public void setEngineScript(EngineScript ES)
    {
        engScript = ES;
    }
}
