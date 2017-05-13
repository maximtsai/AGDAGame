using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperWeapon : WeaponScript
{
    public GameObject ammo;
    public float reloadDuration = 100;
    float currReload = 0;
    public int clipSize = 5;
    int remainingAmmo;
    public float fireDelay = 30;
    float currFireDelay = 0;
    Vector3 ammoPos = new Vector3(0, 0, 0);
    bool isActivated = false;
    public float fireVel = 20;
    int warmupDuration = 10;// ticks before weapon fires
    int warmupCounter = 0;
    bool firing = false;
    Vector2 aimDir;
    Collider2D weaponCollider;
    // Use this for initialization
    Rigidbody2D playerRB;
    void Start()
    {
        remainingAmmo = clipSize;
        weaponCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            currReload-= Time.timeScale;
            currFireDelay-= Time.timeScale;
            if (isActivated && currFireDelay <= 0 && currReload <= 0)
            {
                fireWeapon();
                currFireDelay = fireDelay;
                remainingAmmo--;
                if (remainingAmmo <= 0)
                {
                    remainingAmmo = clipSize;
                    currReload = reloadDuration;
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
        aimDir = new Vector2(facingDirX + Random.Range(-0.01f, 0.01f), facingDirY + Random.Range(-0.01f, 0.01f));
        float angle = Mathf.Atan2(facingDirY, facingDirX);
        Debug.Log(angle * Mathf.Rad2Deg);
        ammoPos.x = facingDirX*-0.2f;
        ammoPos.y = facingDirY*-0.2f;
        ammoPos = ammoPos + this.transform.position;
        ammoPos.z = 0;
        ammo.transform.position = ammoPos;
        GameObject newAmmo = Instantiate(ammo);
        newAmmo.transform.eulerAngles = new Vector3(0,0,angle*Mathf.Rad2Deg+90);
        tempNoCollision ammoNoClipScript = newAmmo.GetComponent<tempNoCollision>();
        if (ammoNoClipScript)
        {
            if (weaponCollider)
            {
                ammoNoClipScript.SetNoCollision(weaponCollider);
            }
        }
        newAmmo.GetComponent<Rigidbody2D>().velocity = playerRB.velocity + aimDir * fireVel;
        playerRB.AddForce(aimDir * -fireVel*0.01f, ForceMode2D.Impulse);//.velocity = playerRB.velocity + (new Vector2(facingDirX, facingDirY) * -fireVel * 0.1f);
    }
}
