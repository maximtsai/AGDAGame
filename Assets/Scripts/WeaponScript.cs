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
    Vector3 weaponPos;
    void Start () {
        listOfWeapons = GameObject.Find("ListOfWeapons");
        rbComponent = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.z > 0)
        {
            // weapon's Z position is used as a slightly hacky way to indicate whether
            // the item can be picked up or not. If weapon's z position is > 0, this indicates
            // it was recently thrown (as a result of WeaponControl.cs) and thus cannot be picked up
            weaponPos = transform.position;
            weaponPos.z = Mathf.Max(0, weaponPos.z-Time.timeScale*0.75f);
            transform.position = weaponPos;
        }
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
    virtual public void equipWeaponExtra(Rigidbody2D playerRigidBody)
    {

    }
    virtual public void unequipWeaponExtra()
    {
    }
}
