using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// WeaponFinder class enables the player to pick up weapons and drop/throw weapons
/// with whatever equipWeapButton is mapped to
/// </summary>
public class WeaponFinder : MonoBehaviour {
    GameObject listOfWeapons;
    GameObject pickUpIndicator;
    Text indicatorTextDisplay;
    public KeyCode equipWeapButton = KeyCode.F;
    public float weaponGrabDist = 4;

    float closestWeaponDist;
    Vector3 indicatorPos = new Vector3(0,0,-5); // where the indicator will show up at
    Vector3 indicatorRot = new Vector3(0, 0, -90); // where the indicator will show up at
    float prevRotation = 0;
    float prevPrevRotation = 0;
    float rotationDiff = 0;
    const float DEG_TO_RAD = Mathf.PI / 180.0f;
    bool hasWeapon = false;
    bool keyDown = false; // indicates if equip key is down, allows grabbing weapons on the fly
    bool prevKeyDown = false;
    bool justPickedUpWeapon = false; // prevents you from dropping weapon right after you pick it up
    bool needToRealignWeapon = false;
    Transform weaponTransform; // thing used to control position of weapon that you pick up
    WeaponScript currentWeaponScript; // script of currently held weapon, we call functions from this
    Rigidbody2D playerRB;
	// Use this for initialization
	void Start () {
        // first search for some required world objects
        listOfWeapons = GameObject.Find("ListOfWeapons");
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("WeaponPickupIndicator"))
            {
                pickUpIndicator = child.gameObject;
                break;
            }
        }
        Transform indicatorCanvasTrans = pickUpIndicator.transform.FindChild("Canvas");
        indicatorTextDisplay = indicatorCanvasTrans.FindChild("Text").GetComponent<Text>();

        closestWeaponDist = weaponGrabDist;
        indicatorTextDisplay.text = equipWeapButton.ToString();
        playerRB = this.GetComponent<Rigidbody2D>();
        prevRotation = playerRB.rotation;
        prevPrevRotation = playerRB.rotation;
    }
    void FixedUpdate() {
        if (Input.GetKeyDown(equipWeapButton))
        {
            keyDown = true;
        }
        else if (Input.GetKeyUp(equipWeapButton))
        {
            keyDown = false;
        }
        float currRotation = playerRB.rotation;
        rotationDiff = currRotation - prevPrevRotation;
        int tempIndex = -1;
        int closestWeaponIndex = -1;
        if (hasWeapon)
        {
            // if you already have a weapon, throw it away
            if (Input.GetKeyUp(equipWeapButton) || (!keyDown && prevKeyDown))
            {
                if (justPickedUpWeapon)
                {
                    // don't want to throw away weapon first time we pick it up.
                    //    Debug.Log(keyDown);
                    justPickedUpWeapon = false;
                }
                else
                {
                    // throw weapon away
                    hasWeapon = false;
                    foreach (Transform child in this.transform)
                    {
                        if (child.CompareTag("Weapon") || child.CompareTag("SoftWeapon"))
                        {
                            // throw weapon away
                            child.SetParent(listOfWeapons.transform);
                            Rigidbody2D childRB = child.gameObject.AddComponent<Rigidbody2D>();
                            childRB.gravityScale = 0;
                            childRB.drag = 0.75f;
                            childRB.angularDrag = 1;
                            Vector2 throwVelocity = playerRB.GetPointVelocity(child.position) * 2f;
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

                            child.gameObject.GetComponent<Rigidbody2D>().velocity = throwVelocity;
                            child.gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationDiff * 4);
                            break;
                        }
                    }
                }
            }
        } else
        {
            // No weapon in hand. Look at all weapons, calculate distance to each, and determine
            // if they can be picked up
            foreach (Transform child in listOfWeapons.transform)
            {
                tempIndex++;
                // minor preliminary optimizations
                float xDistDiff = Mathf.Abs(this.transform.position.x - child.position.x);
                if (xDistDiff > weaponGrabDist)
                {
                    continue;
                }
                float yDistDiff = Mathf.Abs(this.transform.position.y - child.position.y);
                if (yDistDiff > weaponGrabDist)
                {
                    continue;
                }

                float distToWeapon = Vector2.Distance(this.transform.position, child.position);
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
                if (keyDown)
                {
                    justPickedUpWeapon = true;
                    hasWeapon = true;
                    // grab weapon, put it in front of player
                    Destroy(weaponTransform.GetComponent<Rigidbody2D>());
                    weaponTransform.SetParent(this.transform);
                    Rigidbody2D parentRB = this.GetComponent<Rigidbody2D>();
                    // Due to physics engine behavior, grabbing a weapon while at high speeds will result in weapon not
                    // going to right position in front of player.

                    needToRealignWeapon = true;
                    //weaponTransform.localPosition = new Vector3(weaponOffsetDist + equipOffsetX, equipOffsetY, 0);
                    //weaponTransform.localEulerAngles = new Vector3(0, 0, -90f);
                    if (pickUpIndicator.activeSelf)
                    {
                        pickUpIndicator.SetActive(false);
                    }
                }
            } else
            {
                // no weapon nearby, indicator is hidden
                if (pickUpIndicator.activeSelf)
                {
                    pickUpIndicator.SetActive(false);
                }
            }
        }
        prevPrevRotation = prevRotation;
        prevRotation = currRotation;
        prevKeyDown = keyDown;
    }
    void LateUpdate()
    {
        if (pickUpIndicator.activeSelf)
        {
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
}
