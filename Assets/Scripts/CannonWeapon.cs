﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonWeapon : WeaponScript
{
    public GameObject ammo;
    public int reloadDuration = 100;
    int currReload = 0;
    public int clipSize = 5;
    int remainingAmmo;
    public int fireDelay = 30;
    int currFireDelay = 0;
    Vector3 ammoPos = new Vector3(0, 0, 0);
    bool isActivated = false;
    public float fireVel = 20;
    int warmupDuration = 4;// ticks before weapon fires
    int warmupCounter = 0;
    bool firing = false;
    Vector2 aimDir;
    Collider2D weaponCollider;
    // Use this for initialization
    Rigidbody2D playerRB;
    void Start () {
        remainingAmmo = clipSize;
        weaponCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (firing)
        {
            warmupCounter++;
            if (warmupCounter == warmupDuration)
            {
                fireWeapon();
                currFireDelay = fireDelay;
                remainingAmmo--;
                if (remainingAmmo <= 0)
                {
                    remainingAmmo = clipSize;
                    currReload = reloadDuration;
                }
                warmupCounter = 0;
                firing = false;
            }            
        } else
        {
            currReload--;
            currFireDelay--;
            if (isActivated && currFireDelay <= 0 && currReload <= 0)
            {
                firing = true;
            }
        }
	}

    public override void activateWeapon(Rigidbody2D playerRigidBody)
    {
        // What happens when the fire button is held  down
        playerRB = playerRigidBody;
        isActivated = true;
    }
    public override void deactivateWeapon()
    {
        // what happens when the fire button is lifted
        isActivated = false;
    }
    void fireWeapon()
    {
        float facingDirX = -Mathf.Sin(this.transform.eulerAngles.z * Mathf.Deg2Rad);
        float facingDirY = Mathf.Cos(this.transform.eulerAngles.z * Mathf.Deg2Rad);
        aimDir = new Vector2(facingDirX, facingDirY);
        ammoPos.x = -facingDirX*0.25f;
        ammoPos.y = -facingDirY*0.25f;
        ammoPos = ammoPos + this.transform.position;
        ammoPos.z = 0;
        ammo.transform.position = ammoPos;
        GameObject newAmmo = Instantiate(ammo);
        tempNoCollision ammoNoClipScript = newAmmo.GetComponent<tempNoCollision>();
        if (ammoNoClipScript)
        {
            ammoNoClipScript.SetNoCollision(weaponCollider);
        }
        newAmmo.GetComponent<Rigidbody2D>().velocity = playerRB.velocity + aimDir * fireVel;
        playerRB.AddForce(aimDir * -fireVel, ForceMode2D.Impulse);//.velocity = playerRB.velocity + (new Vector2(facingDirX, facingDirY) * -fireVel * 0.1f);
    }
}
