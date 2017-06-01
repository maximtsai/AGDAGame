using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonWeapon : WeaponScript
{
    public GameObject chargeSparkEmitter;
    ParticleSystem chargeSparkParticleSystem;
    public GameObject ammo;
    public int reloadDuration = 100;
    float currReload = 0;
    public int clipSize = 5;
    int remainingAmmo;
    public int fireDelay = 30;
    float currFireDelay = 0;
    Vector3 ammoPos = new Vector3(0, 0, 0);
    bool isActivated = false;
    public float fireVel = 20;
    public float warmupDuration = 25;// ticks before weapon fires
    float warmupCounter = 0;
    bool firing = false;
    bool sparkHandled = false; // used to handle chargeSparks
    Vector2 aimDir;
    Collider2D weaponCollider;
    // Use this for initialization
    Rigidbody2D currentRB;
    float chargeSparkTimeAccum = 0;
    void Start () {
        remainingAmmo = clipSize;
        weaponCollider = GetComponent<Collider2D>();
        chargeSparkParticleSystem = chargeSparkEmitter.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (firing)
        {
            warmupCounter += Time.timeScale;
            if (warmupCounter <= warmupDuration - 11) {
                // play charging up animation
                if (Time.timeScale > 0.95f)
                {
                    chargeSparkParticleSystem.Emit(2);
                } else if (Time.timeScale > 0.4f)
                {
                    chargeSparkParticleSystem.Emit(1);
                } else if (chargeSparkTimeAccum > 1)
                {
                    chargeSparkTimeAccum = 0;
                    chargeSparkParticleSystem.Emit(1);
                } else
                {
                    chargeSparkTimeAccum += Time.timeScale;
                }
            } else if (warmupCounter >= warmupDuration)
            {
                // fire a bullet and adjust ammo and reload times as necessary
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
            currReload = Mathf.Max(0, currReload - Time.timeScale);
            currFireDelay = Mathf.Max(0, currFireDelay - Time.timeScale); ;
            if (isActivated && currFireDelay <= 0 && currReload <= 0)
            {
                firing = true;
                if (engScript)
                {
                    engScript.setWeaponTurnMult(this.turnMultiplier * 0.2f);
                    engScript.setWeaponMoveMult(this.moveMultiplier * 0.2f);
                }
            }
        }
	}

    public override void activateWeapon(Rigidbody2D playerRigidBody)
    {
        // What happens when the fire button is pressed
        currentRB = playerRigidBody;
        isActivated = true;
    }
    public override void deactivateWeapon()
    {
        // what happens when the fire button is lifted
        isActivated = false;
    }
    public override void equipWeaponExtra(Rigidbody2D playerRigidBody)
    {
        currentRB = playerRigidBody;
    }
    public override void unequipWeaponExtra()
    {
        deactivateWeapon();
        currentRB = GetComponent<Rigidbody2D>();
        engScript = null;
    }
    void fireWeapon()
    {
        if (engScript)
        {
            engScript.setWeaponTurnMult(this.turnMultiplier);
            engScript.setWeaponMoveMult(this.moveMultiplier);
        }
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
        if (currentRB)
        {
            newAmmo.GetComponent<Rigidbody2D>().velocity = currentRB.velocity + aimDir * fireVel;
            currentRB.AddForce(aimDir * -fireVel, ForceMode2D.Impulse);//.velocity = currentRB.velocity + (new Vector2(facingDirX, facingDirY) * -fireVel * 0.1f);
        }
    }

}
