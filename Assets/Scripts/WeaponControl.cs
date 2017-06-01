using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// WeaponControl class enables the player to pick up weapons and drop/throw weapons
/// with whatever equipWeapButton is mapped to
/// </summary>
public class WeaponControl : MonoBehaviour {
    GameObject listOfWeapons;
    GameObject pickUpIndicator;
    public KeyCode equipWeapButton = KeyCode.F;
    public float weaponGrabDist = 4;
    public TutorialKeysManager tutKeyManager; // causes various hud elements to disappear 

    float closestWeaponDist;
    Vector3 indicatorPos = new Vector3(0,0,-5); // where the indicator will show up at
    Vector3 indicatorRot = new Vector3(0, 0, -90); // where the indicator will show up at
    float prevRotation = 0;
    float prevPrevRotation = 0;
    float rotationDiff = 0;
    const float DEG_TO_RAD = Mathf.PI / 180.0f;
    bool hasWeapon = false;
    bool equipKeyDown = false; // indicates if equip key is down, allows grabbing weapons on the fly
    bool activateKeyDown = false; // indicates if equip key is down, allows grabbing weapons on the fly
    bool justPickedUpWeapon = false; // prevents you from dropping weapon right after you pick it up
    bool needToRealignWeapon = false;
    Transform weaponTransform; // thing used to control position of weapon that you pick up
    WeaponScript currentWeaponScript; // script of currently held weapon, we call functions from this
    EngineScript currentEngineScript; // script of object's engine, used to control movement
    Rigidbody2D playerRB;
    bool usable = false;
	// Use this for initialization
	void Start () {
        // first search for some required world objects
        listOfWeapons = GameObject.Find("ListOfWeapons");
        foreach (Transform childTrans in this.transform)
        {
            if (childTrans.CompareTag("WeaponPickupIndicator"))
            {
                pickUpIndicator = childTrans.gameObject;
                Transform indicatorCanvasTrans = pickUpIndicator.transform.FindChild("Canvas");
            }
            if (childTrans.CompareTag("Engine"))
            {
                currentEngineScript = childTrans.gameObject.GetComponent<EngineScript>();
            }
        }

        if (listOfWeapons && pickUpIndicator)
        {
            usable = true;
        }

        closestWeaponDist = weaponGrabDist;
        playerRB = this.GetComponent<Rigidbody2D>();
        prevRotation = playerRB.rotation;
        prevPrevRotation = playerRB.rotation;
        if (!usable)
        {
            Debug.Log("WeaponControl.cs not initialized due to missing elements");
            // missing certain elements so terminate script
            Destroy(this);
        }
    }
    void FixedUpdate() {

        float currRotation = playerRB.rotation;
        rotationDiff = currRotation - prevPrevRotation;
        int tempIndex = -1;
        int closestWeaponIndex = -1;
        if (!hasWeapon)
        {
            // No weapon in hand. Look at all weapons, calculate distance to each, and determine
            // if they can be picked up
            foreach (Transform childTrans in listOfWeapons.transform)
            {
                tempIndex++;
                // minor preliminary optimizations
                float xDistDiff = Mathf.Abs(this.transform.position.x - childTrans.position.x);
                if (xDistDiff > weaponGrabDist)
                {
                    continue;
                }
                float yDistDiff = Mathf.Abs(this.transform.position.y - childTrans.position.y);
                if (yDistDiff > weaponGrabDist)
                {
                    continue;
                }
                if (childTrans.position.z > 0)
                {
                    continue;
                }

                float distToWeapon = Vector2.Distance(this.transform.position, childTrans.position);
                // if there are multiple weapons nearby, always grab the closest weapon
                if (distToWeapon < closestWeaponDist)
                {
                    //Debug.Log(closestWeaponDist);
                    // found weapon that's close enough, is potential thing to pick up
                    closestWeaponIndex = tempIndex;
                    closestWeaponDist = distToWeapon;
                }
            }
            closestWeaponDist = weaponGrabDist;

            if (closestWeaponIndex > -1)
            {
                // Got weapon, ready to make grab indicator show up
                weaponTransform = listOfWeapons.transform.GetChild(closestWeaponIndex);
                // have weapon indicator show up and set it to active
                indicatorPos.x = weaponTransform.position.x;
                indicatorPos.y = weaponTransform.position.y;
                pickUpIndicator.SetActive(true);
                // picking up weapon
                if (equipKeyDown)
                {
                    currentWeaponScript = weaponTransform.GetComponent<WeaponScript>();
                    justPickedUpWeapon = true;
                    hasWeapon = true;
                    if (this.name == "Player1")
                    {
                        tutKeyManager.P1EquipPressed();
                    }
                    else if (this.name == "Player2")
                    {
                        tutKeyManager.P2EquipPressed();
                    }

                    // grab weapon, put it in front of player
                    Destroy(weaponTransform.GetComponent<Rigidbody2D>());
                    weaponTransform.SetParent(this.transform);
                    // Due to physics engine behavior, grabbing a weapon while at high speeds will result in weapon not
                    // going to right position in front of player.

                    needToRealignWeapon = true;
                    if (pickUpIndicator.activeSelf)
                    {
                        pickUpIndicator.SetActive(false);
                    }
                    // add movement multipliers/penalties and link the engine with the weapon
                    currentWeaponScript.equipWeaponExtra(playerRB);

                    currentWeaponScript.setEngineScript(currentEngineScript);
                    currentEngineScript.setWeaponTurnMult(currentWeaponScript.turnMultiplier);
                    currentEngineScript.setWeaponMoveMult(currentWeaponScript.moveMultiplier);
                }
            } else if(pickUpIndicator.activeSelf)
            {
                // no weapon nearby, indicator is hidden
                pickUpIndicator.SetActive(false);
            }
        }

        prevPrevRotation = prevRotation;
        prevRotation = currRotation;
    }
    void LateUpdate()
    {
        if (pickUpIndicator.activeSelf)
        {
            // realign indicator so it's not rotating incorrectly
            pickUpIndicator.transform.position = indicatorPos;
            pickUpIndicator.transform.eulerAngles = indicatorRot;
        }
        if (needToRealignWeapon)
        {
            // center weapon in front of player
            needToRealignWeapon = false;
            WeaponScript ws = weaponTransform.gameObject.GetComponent<WeaponScript>();
            //SpriteRenderer sprite = weaponTransform.gameObject.GetComponent<SpriteRenderer>();
            weaponTransform.localPosition = new Vector3(0, ws.weaponOffset * weaponTransform.gameObject.transform.localScale.y + this.transform.localScale.y * 0.5f, 0);
            weaponTransform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
    public void pressEquipKey()
    {
        equipKeyDown = true;
    }
    public void releaseEquipKey()
    {
        equipKeyDown = false;
        float currRotation = playerRB.rotation;
        rotationDiff = currRotation - prevPrevRotation;
        if (justPickedUpWeapon)
        {
            justPickedUpWeapon = false;
        } else if (hasWeapon)
        {
            // if you already have a weapon, throw it away
            hasWeapon = false;
            if (this.name == "Player1")
            {
                tutKeyManager.P1EquipPressed();
            }
            else if (this.name == "Player2")
            {
                tutKeyManager.P2EquipPressed();
            }
            foreach (Transform childTrans in this.transform)
            {
                if (childTrans.CompareTag("Weapon") || childTrans.CompareTag("SoftWeapon"))
                {
                    // throw weapon away
                    childTrans.SetParent(listOfWeapons.transform);
                    Rigidbody2D childRB = childTrans.gameObject.AddComponent<Rigidbody2D>();

                    Vector3 weaponPos = childTrans.position;
                    weaponPos.z = 30;
                    childTrans.position = weaponPos;

                    childRB.gravityScale = 0;
                    childRB.drag = 0.75f;
                    childRB.angularDrag = 1;
                    Vector2 throwVelocity = playerRB.GetPointVelocity(childTrans.position) * 2f;
                    // also need to take into account player rotation velocity
                    float velX = 0;
                    float velY = 0;
                    if (rotationDiff > 0)
                    {
                        // counterclockwise fling
                        velX = Mathf.Cos((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 1.25f;
                        velY = Mathf.Sin((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 1.25f;
                    }
                    else
                    {
                        // clockwise fling
                        velX = Mathf.Cos((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 1.25f;
                        velY = Mathf.Sin((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 1.25f;
                    }
                    float aimDirMultiplier = 5 + playerRB.velocity.magnitude * 0.2f;
                    throwVelocity.x += velX - Mathf.Sin(currRotation * Mathf.Deg2Rad) * aimDirMultiplier;
                    throwVelocity.y += velY + Mathf.Cos(currRotation * Mathf.Deg2Rad) * aimDirMultiplier;

                    childTrans.gameObject.GetComponent<Rigidbody2D>().velocity = throwVelocity;
                    childTrans.gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationDiff * 4);
                    break;
                }
            }
            // reset movement multipliers
            currentEngineScript.setWeaponTurnMult(1);
            currentEngineScript.setWeaponMoveMult(1);
            currentWeaponScript.unequipWeaponExtra();
            currentWeaponScript = null;

        }
    }
    public void throwWeaponAway()
    {
        hasWeapon = false;
        foreach (Transform childTrans in this.transform)
        {
            if (childTrans.CompareTag("Weapon") || childTrans.CompareTag("SoftWeapon"))
            {
                // throw weapon away
                childTrans.SetParent(listOfWeapons.transform);
                Rigidbody2D childRB = childTrans.gameObject.AddComponent<Rigidbody2D>();

                Vector3 weaponPos = childTrans.position;
                weaponPos.z = 30;
                childTrans.position = weaponPos;

                childRB.gravityScale = 0;
                childRB.drag = 0.75f;
                childRB.angularDrag = 1;
                Vector2 throwVelocity = playerRB.GetPointVelocity(childTrans.position) * 2f;
                // also need to take into account player rotation velocity
                float velX = 0;
                float velY = 0;
                if (rotationDiff > 0)
                {
                    // counterclockwise fling
                    velX = Mathf.Cos((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 0.75f;
                    velY = Mathf.Sin((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 0.75f;
                }
                else
                {
                    // clockwise fling
                    velX = Mathf.Cos((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 0.75f;
                    velY = Mathf.Sin((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 0.75f;
                }
                throwVelocity.x += velX;
                throwVelocity.y += velY;

                childTrans.gameObject.GetComponent<Rigidbody2D>().velocity = throwVelocity;
                childTrans.gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationDiff * 4);
                break;
            }
        }
        // reset movement multipliers
        currentEngineScript.setWeaponTurnMult(1);
        currentEngineScript.setWeaponMoveMult(1);
        currentWeaponScript.unequipWeaponExtra();
        currentWeaponScript = null;
    }

    public void dropWeapon()
    {
        // similar to throwWeaponAway except without making the weapon launch out.
        hasWeapon = false;
        foreach (Transform childTrans in this.transform)
        {
            if (childTrans.CompareTag("Weapon") || childTrans.CompareTag("SoftWeapon"))
            {
                // throw weapon away
                childTrans.SetParent(listOfWeapons.transform);
                Rigidbody2D childRB = childTrans.gameObject.AddComponent<Rigidbody2D>();

                Vector3 weaponPos = childTrans.position;
                weaponPos.z = 30;
                childTrans.position = weaponPos;

                childRB.gravityScale = 0;
                childRB.drag = 0.75f;
                childRB.angularDrag = 1;
                break;
            }
        }
        // reset movement multipliers
        currentEngineScript.setWeaponTurnMult(1);
        currentEngineScript.setWeaponMoveMult(1);
        currentWeaponScript.unequipWeaponExtra();
        currentWeaponScript = null;
    }
    public void pressActivateKey()
    {
        if (currentWeaponScript)
        {
            currentWeaponScript.activateWeapon(playerRB);
            if (this.name == "Player1")
            {
                tutKeyManager.P1ActivatePressed();
            }
            else if (this.name == "Player2")
            {
                tutKeyManager.P2ActivatePressed();
            }
        }
    }
    public void releaseActivateKey()
    {
        if (currentWeaponScript)
        {
            currentWeaponScript.deactivateWeapon();
        }
    }
    public bool hasWeaponEquipped()
    {
        return hasWeapon;
    }
}
