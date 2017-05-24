using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherWeapon : WeaponScript
{
    public GameObject chargeSparkEmitter;
    ParticleSystem chargeSparkParticleSystem;

    public GameObject ammo;
    public int reloadDuration = 100;
    float currReload = 0;
    int clipSize = 16;
    int remainingAmmo = 0;
    public float fireDelay = 10;
    float currFireDelay = 0;
    int rocketsStored = 0;
    public GameObject leftLauncher;
    public GameObject rightLauncher;
    public GameObject farLeftLauncher;
    public GameObject farRightLauncher;
    public GameObject leftLauncherTip;
    public GameObject rightLauncherTip;
    public GameObject farLeftLauncherTip;
    public GameObject farRightLauncherTip;
    Vector3 leftLauncherOrigPos;
    Vector3 rightLauncherOrigPos;
    Vector3 farLeftLauncherOrigPos;
    Vector3 farRightLauncherOrigPos;
    Vector3 leftLauncherTipOrigPos;
    Vector3 rightLauncherTipOrigPos;
    Vector3 farLeftLauncherTipOrigPos;
    Vector3 farRightLauncherTipOrigPos;
    bool firstLocked = false;
    bool secondLocked = false;
    Vector3 ammoPos = new Vector3(0, 0, 0);

    Vector3 tempVector = new Vector3(0, 0, 0);
    bool isActivated = false;
    // public float fireVel = 20;
    public float warmUpDuration = 50;// ticks before weapon fires
    float warmupCounter = 0;
    bool warmingUp = false; // the initial setup period before firing
    bool firing = false; // actually shooting stuff
    bool coolingDown = false;
    float finishedFiringCounter = 50;
    float finishedFiringFullCounter = 50;
    int currentCannonFiring = 1; // the cannon that is firing the next ammo
    Vector2 aimDir;
    Collider2D weaponColliderLeft;
    Collider2D weaponColliderRight;
    Collider2D weaponColliderFarLeft;
    Collider2D weaponColliderFarRight;
    // Use this for initialization
    Rigidbody2D playerRB;

    void Start()
    {
        leftLauncherOrigPos = leftLauncher.transform.localPosition;
        rightLauncherOrigPos = rightLauncher.transform.localPosition;
        farLeftLauncherOrigPos = farLeftLauncher.transform.localPosition;
        farRightLauncherOrigPos = farRightLauncher.transform.localPosition;
        leftLauncherTipOrigPos = leftLauncherTip.transform.localPosition;
        rightLauncherTipOrigPos = rightLauncherTip.transform.localPosition;
        farLeftLauncherTipOrigPos = farLeftLauncherTip.transform.localPosition;
        farRightLauncherTipOrigPos = farRightLauncherTip.transform.localPosition;
        chargeSparkParticleSystem = chargeSparkEmitter.GetComponent<ParticleSystem>();
        weaponColliderLeft = leftLauncher.GetComponent<Collider2D>();
        weaponColliderRight = rightLauncher.GetComponent<Collider2D>();
        weaponColliderFarLeft = farLeftLauncher.GetComponent<Collider2D>();
        weaponColliderFarRight = farRightLauncher.GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (warmingUp)
        {
            warmupCounter += Time.timeScale;
            // Rocktlauncher has somewhat complex "expanding" sequence
            float expandDuration = 0.75f;
            if (warmupCounter <= warmUpDuration / 3f)
            {
                float firstStageProgress = warmupCounter / (warmUpDuration / 3f);
                if (firstStageProgress < expandDuration)
                {
                    // shift launchers to edges
                    float MoveAmt = firstStageProgress * 0.3f + firstStageProgress * firstStageProgress * 0.5f;

                    tempVector = leftLauncher.transform.localPosition;
                    tempVector.x = leftLauncherOrigPos.x - MoveAmt;
                    leftLauncher.transform.localPosition = tempVector;

                    tempVector = rightLauncher.transform.localPosition;
                    tempVector.x = rightLauncherOrigPos.x + MoveAmt;
                    rightLauncher.transform.localPosition = tempVector;
                }
                else
                {
                    // launchers are intentionally slightly overextended, bring them back
                    float shrinkAmt = (firstStageProgress - expandDuration);
                    float MoveAmt = expandDuration * 0.3f + expandDuration * expandDuration * 0.5f - shrinkAmt;

                    tempVector = leftLauncher.transform.localPosition;
                    tempVector.x = leftLauncherOrigPos.x - MoveAmt;
                    leftLauncher.transform.localPosition = tempVector;

                    tempVector = rightLauncher.transform.localPosition;
                    tempVector.x = rightLauncherOrigPos.x + MoveAmt;
                    rightLauncher.transform.localPosition = tempVector;
                }

            }
            else if (warmupCounter <= warmUpDuration * (2f / 3f))
            {
                // second part of expanding
                if (!firstLocked)
                {
                    // make sure first launcher is in place
                    float shrinkAmt = 1f - expandDuration;
                    float MoveAmt = expandDuration * 0.3f + expandDuration * expandDuration * 0.5f - shrinkAmt;
                    tempVector = leftLauncher.transform.localPosition;
                    tempVector.x = leftLauncherOrigPos.x - MoveAmt;
                    leftLauncher.transform.localPosition = tempVector;
                    tempVector = rightLauncher.transform.localPosition;
                    tempVector.x = rightLauncherOrigPos.x + MoveAmt;
                    rightLauncher.transform.localPosition = tempVector;
                    firstLocked = true;
                }

                float secondStageProgress = (warmupCounter - (warmUpDuration / 3f)) / (warmUpDuration / 3f);
                if (secondStageProgress < expandDuration)
                {
                    // shift launchers to edges
                    float MoveAmt = secondStageProgress * 0.3f + secondStageProgress * secondStageProgress * 0.5f;

                    tempVector = farLeftLauncher.transform.localPosition;
                    tempVector.x = farLeftLauncherOrigPos.x + MoveAmt;
                    farLeftLauncher.transform.localPosition = tempVector;

                    tempVector = farRightLauncher.transform.localPosition;
                    tempVector.x = farRightLauncherOrigPos.x + MoveAmt;
                    farRightLauncher.transform.localPosition = tempVector;
                }
                else
                {
                    // launchers are intentionally slightly overextended, bring them back
                    float shrinkAmt = (secondStageProgress - expandDuration);
                    float MoveAmt = expandDuration * 0.2f + expandDuration * expandDuration * 0.5f - shrinkAmt;

                    tempVector = farLeftLauncher.transform.localPosition;
                    tempVector.x = farLeftLauncherOrigPos.x + MoveAmt;
                    farLeftLauncher.transform.localPosition = tempVector;

                    tempVector = farRightLauncher.transform.localPosition;
                    tempVector.x = farRightLauncherOrigPos.x + MoveAmt;
                    farRightLauncher.transform.localPosition = tempVector;
                }
            }
            else if (warmupCounter <= warmUpDuration)
            {
                if (warmupCounter < warmUpDuration - 9)
                {
                    // brief chargeSpark animation
                    chargeSparkParticleSystem.Emit((int)(6 * Time.timeScale));
                }
            }
            else if (warmupCounter >= warmUpDuration)
            {
                // fire a bullet and adjust ammo and reload times as necessary
                fireWeapon();
                warmupCounter = 0;
                warmingUp = false;
            }
        }
        else if (firing)
        {
            // ROCKETSROCKETSROCKETSROCKETS pewpew
            currFireDelay -= Time.timeScale;
            if (currFireDelay <= 0 && clipSize >= 1)
            {
                // reduce clip for every ammo we fire
                clipSize--;
                if (clipSize == 0)
                {
                    firing = false; // no more ammo after this last batch
                    coolingDown = true;
                }
                currFireDelay = fireDelay;
                float facingDirX;
                float facingDirY;
                float xOffset;
                float yOffset;
                GameObject newAmmo = Instantiate(ammo);
                tempNoCollision ammoNoClipScript = newAmmo.GetComponent<tempNoCollision>();
                newAmmo.GetComponent<Rigidbody2D>().velocity = playerRB.velocity + aimDir * 5; // initial rocket velocity
                playerRB.AddForce(aimDir * -0.1f, ForceMode2D.Impulse); // recoil
                playerRB.MovePosition(playerRB.position + aimDir * -0.1f); // recoil

                switch (currentCannonFiring)
                {
                    case 1:
                        // withdraw left rocket tip
                        tempVector = leftLauncherTipOrigPos;
                        tempVector.y = -0.3f;
                        leftLauncherTip.transform.localPosition = tempVector;
                        // aim rocket in same direction that player is facing
                        facingDirX = -Mathf.Sin(this.transform.eulerAngles.z * Mathf.Deg2Rad);
                        facingDirY = Mathf.Cos(this.transform.eulerAngles.z * Mathf.Deg2Rad);
                        aimDir = new Vector2(facingDirX, facingDirY);
                        // set rocket a bit to the left since this is the left cannon firing
                        xOffset = -1f * facingDirY;
                        yOffset = 1f * facingDirX;
                        ammoPos.x = facingDirX * 0.95f + xOffset;
                        ammoPos.y = facingDirY * 0.95f + yOffset;
                        ammoPos = ammoPos + this.transform.position;
                        ammoPos.z = 0;
                        newAmmo.transform.position = ammoPos;
                        newAmmo.transform.eulerAngles = new Vector3(0, 0, this.transform.eulerAngles.z);
                        if (ammoNoClipScript)
                        {
                            // prevent rocket from colliding with launcher
                            // not foolproof, which is fine since exploding in face
                            // is good if player was obviously careless
                            ammoNoClipScript.SetNoCollision(weaponColliderLeft);
                        }
                        currentCannonFiring = 2;
                        playerRB.AddTorque(400); // recoil
                        break;
                    case 2:
                        // withdraw left rocket tip
                        tempVector = rightLauncherTipOrigPos;
                        tempVector.y = -0.3f;
                        rightLauncherTip.transform.localPosition = tempVector;

                        facingDirX = -Mathf.Sin(this.transform.eulerAngles.z * Mathf.Deg2Rad);
                        facingDirY = Mathf.Cos(this.transform.eulerAngles.z * Mathf.Deg2Rad);
                        aimDir = new Vector2(facingDirX, facingDirY);
                        xOffset = 1f * facingDirY;
                        yOffset = -1f * facingDirX;
                        ammoPos.x = facingDirX * 0.95f + xOffset;
                        ammoPos.y = facingDirY * 0.95f + yOffset;
                        ammoPos = ammoPos + this.transform.position;
                        ammoPos.z = 0;
                        newAmmo.transform.position = ammoPos;
                        newAmmo.transform.eulerAngles = new Vector3(0, 0, this.transform.eulerAngles.z);
                        if (ammoNoClipScript)
                        {
                            ammoNoClipScript.SetNoCollision(weaponColliderRight);
                        }
                        currentCannonFiring = 3;
                        playerRB.AddTorque(-400); // recoil
                        break;
                    case 3:
                        tempVector = farLeftLauncherTipOrigPos;
                        tempVector.y = -0.3f;
                        farLeftLauncherTip.transform.localPosition = tempVector;

                        facingDirX = -Mathf.Sin(this.transform.eulerAngles.z * Mathf.Deg2Rad);
                        facingDirY = Mathf.Cos(this.transform.eulerAngles.z * Mathf.Deg2Rad);
                        aimDir = new Vector2(facingDirX, facingDirY);
                        xOffset = -1.8f * facingDirY;
                        yOffset = 1.8f * facingDirX;
                        ammoPos.x = facingDirX * 0.95f + xOffset;
                        ammoPos.y = facingDirY * 0.95f + yOffset;
                        ammoPos = ammoPos + this.transform.position;
                        ammoPos.z = 0;
                        newAmmo.transform.position = ammoPos;
                        newAmmo.transform.eulerAngles = new Vector3(0, 0, this.transform.eulerAngles.z);
                        if (ammoNoClipScript)
                        {
                            ammoNoClipScript.SetNoCollision(weaponColliderLeft);
                        }
                        currentCannonFiring = 4;
                        playerRB.AddTorque(600); // recoil
                        break;
                    case 4:
                        tempVector = farRightLauncherTipOrigPos;
                        tempVector.y = -0.3f;
                        farRightLauncherTip.transform.localPosition = tempVector;

                        facingDirX = -Mathf.Sin(this.transform.eulerAngles.z * Mathf.Deg2Rad);
                        facingDirY = Mathf.Cos(this.transform.eulerAngles.z * Mathf.Deg2Rad);
                        aimDir = new Vector2(facingDirX, facingDirY);
                        xOffset = 1.8f * facingDirY;
                        yOffset = -1.8f * facingDirX;
                        ammoPos.x = facingDirX * 0.95f + xOffset;
                        ammoPos.y = facingDirY * 0.95f + yOffset;
                        ammoPos = ammoPos + this.transform.position;
                        ammoPos.z = 0;
                        newAmmo.transform.position = ammoPos;
                        newAmmo.transform.eulerAngles = new Vector3(0, 0, this.transform.eulerAngles.z);
                        if (ammoNoClipScript)
                        {
                            ammoNoClipScript.SetNoCollision(weaponColliderRight);
                        }
                        currentCannonFiring = 1;
                        playerRB.AddTorque(-600); // recoil
                        break;
                }
            }
        }
        else if (coolingDown)
        {
            if (finishedFiringCounter <= 0)
            {
                // finished firing, retract 
                float moveDiff = leftLauncher.transform.localPosition.x - leftLauncherOrigPos.x;
                tempVector = leftLauncher.transform.localPosition;
                tempVector.x = leftLauncherOrigPos.x + moveDiff * 0.9f;
                leftLauncher.transform.localPosition = tempVector;

                moveDiff = rightLauncher.transform.localPosition.x - rightLauncherOrigPos.x;
                tempVector = rightLauncher.transform.localPosition;
                tempVector.x = rightLauncherOrigPos.x + moveDiff * 0.9f;
                rightLauncher.transform.localPosition = tempVector;

                float farMoveDiff = farLeftLauncher.transform.localPosition.x - farLeftLauncherOrigPos.x;
                tempVector = farLeftLauncher.transform.localPosition;
                tempVector.x = farLeftLauncherOrigPos.x + farMoveDiff * 0.9f;
                farLeftLauncher.transform.localPosition = tempVector;

                farMoveDiff = farRightLauncher.transform.localPosition.x - farRightLauncherOrigPos.x;
                tempVector = farRightLauncher.transform.localPosition;
                tempVector.x = farRightLauncherOrigPos.x + farMoveDiff * 0.9f;
                farRightLauncher.transform.localPosition = tempVector;
                if (moveDiff <= 0.03f && farMoveDiff <= 0.03f)
                {
                    // close enough to original position, snap it to proper orig position
                    leftLauncher.transform.localPosition = leftLauncherOrigPos;
                    rightLauncher.transform.localPosition = rightLauncherOrigPos;
                    farLeftLauncher.transform.localPosition = farLeftLauncherOrigPos;
                    farRightLauncher.transform.localPosition = farRightLauncherOrigPos;
                    engScript.setWeaponTurnMult(this.moveMultiplier);
                    engScript.setWeaponMoveMult(this.moveMultiplier);

                    coolingDown = false;
                    currReload = reloadDuration;
                    finishedFiringCounter = finishedFiringFullCounter;
                }
            } else
            {
                finishedFiringCounter -= Time.timeScale;
            }
        }
        else
        {
            // reloading ammo
            currReload = Mathf.Max(0, currReload - Time.timeScale);
            if (currReload <= 0)
            {
                if (clipSize == 0)
                {
                    clipSize = 8;
                    currReload = reloadDuration;

                    leftLauncherTip.transform.localPosition = leftLauncherTipOrigPos;
                    rightLauncherTip.transform.localPosition = rightLauncherTipOrigPos;
                }
                else if (clipSize == 8)
                {
                    clipSize = 16;
                    farLeftLauncherTip.transform.localPosition = farLeftLauncherTipOrigPos;
                    farRightLauncherTip.transform.localPosition = farRightLauncherTipOrigPos;
                }
            }
            if (isActivated && clipSize > 0 && !warmingUp && !firing & !coolingDown)
            {
                // player just pressed fire button
                warmingUp = true;
                engScript.setWeaponTurnMult(this.turnMultiplier * 0.1f);
                engScript.setWeaponMoveMult(this.moveMultiplier * 0.15f);
            }
        }
    }

    public override void activateWeapon(Rigidbody2D playerRigidBody)
    {
        // What happens when the fire button is pressed
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
        firing = true;
        //engScript.setWeaponTurnMult(this.turnMultiplier);
        //engScript.setWeaponMoveMult(this.moveMultiplier);

        /*
        if 
        ammoPos.x = -facingDirX * 0.25f;
        ammoPos.y = -facingDirY * 0.25f;
        ammoPos = ammoPos + this.transform.position;
        ammoPos.z = 0;
        ammo.transform.position = ammoPos;
        GameObject newAmmo = Instantiate(ammo);
        tempNoCollision ammoNoClipScript = newAmmo.GetComponent<tempNoCollision>();
        if (ammoNoClipScript)
        {
            ammoNoClipScript.SetNoCollision(weaponColliderLeft);
        }
        newAmmo.GetComponent<Rigidbody2D>().velocity = playerRB.velocity + aimDir * fireVel;
        playerRB.AddForce(aimDir * -fireVel, ForceMode2D.Impulse);//.velocity = playerRB.velocity + (new Vector2(facingDirX, facingDirY) * -fireVel * 0.1f);
        */
    }

}
