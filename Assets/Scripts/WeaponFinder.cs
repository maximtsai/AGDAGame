using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponFinder : MonoBehaviour {
    public GameObject listOfWeapons;
    public GameObject pickUpIndicator;
    public Text indicatorTextDisplay;
    public KeyCode equipWeapButton = KeyCode.F;
    public float weaponGrabDist = 3;
    float closestWeaponDist = 99;
    Vector3 indicatorPos = new Vector3(0,0,-5);
    float prevRotation = 0;
    float prevPrevRotation = 0;
    float rotationDiff = 0;
    private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    bool hasWeapon = false;
    bool keyDown = false; // indicates if equip key is down, allows grabbing weapons on the fly
    bool prevKeyDown = false;
    bool justPickedUpWeapon = false; // prevents you from dropping weapon right after you pick it up
    bool needToRealignWeapon = false;
    Transform weaponTransform; // thing used to control position of weapon that you pick up
    Rigidbody2D playerRB;
	// Use this for initialization
	void Start () {
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
                    hasWeapon = false;
                    foreach (Transform child in this.transform)
                    {
                        if (child.CompareTag("Weapon"))
                        {
                            // throw weapon away
                            child.SetParent(listOfWeapons.transform);
                            Rigidbody2D childRB = child.gameObject.AddComponent<Rigidbody2D>();
                            childRB.gravityScale = 0;
                            childRB.drag = 0.75f;
                            childRB.angularDrag = 1;
                            Vector2 throwVelocity = playerRB.GetPointVelocity(child.position) * 1.6f;
                            // also need to take into account player rotation velocity
                            float velX = 0;
                            float velY = 0;
                            if (rotationDiff > 0)
                            {
                                // counterclockwise fling
                                velX = Mathf.Cos((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 0.55f;
                                velY = Mathf.Sin((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 0.55f;
                            }
                            else
                            {
                                // clockwise fling
                                velX = Mathf.Cos((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 0.55f;
                                velY = Mathf.Sin((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 0.55f;
                            }
                            throwVelocity.x += velX;
                            throwVelocity.y += velY;

                            child.gameObject.GetComponent<Rigidbody2D>().velocity = throwVelocity;
                            child.gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationDiff * 3);
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
                    // found weapon that's close enough, is potential thing to pick up
                    closestWeaponIndex = tempIndex;
                    closestWeaponDist = distToWeapon;
                }
                closestWeaponDist = weaponGrabDist;
            }

            if (closestWeaponIndex > -1)
            {
                // Got weapon, ready to make grab indicator show up
                weaponTransform = listOfWeapons.transform.GetChild(closestWeaponIndex);
                // have weapon indicator show up and set it to active
                indicatorPos.x = weaponTransform.position.x;
                indicatorPos.y = weaponTransform.position.y;
                pickUpIndicator.transform.position = indicatorPos;
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
                    // going to right position in front of player. This should offset some of the stuff
                    float equipOffsetX = Mathf.Cos(parentRB.rotation / Mathf.Rad2Deg) * parentRB.velocity.x*Time.deltaTime;
                    float equipOffsetY = -Mathf.Sin(parentRB.rotation / Mathf.Rad2Deg) * parentRB.velocity.y*Time.deltaTime;
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
    private void LateUpdate()
    {
        if (needToRealignWeapon)
        {
            //Debug.Log("realign");
            needToRealignWeapon = false;
            WeaponData wd = weaponTransform.gameObject.GetComponent<WeaponData>();
            //SpriteRenderer sprite = weaponTransform.gameObject.GetComponent<SpriteRenderer>();
            //Debug.Log(sprite.bounds.size.x);
            weaponTransform.localPosition = new Vector3(wd.weaponOffset * weaponTransform.gameObject.transform.localScale.y + this.transform.localScale.y*0.5f, 0, 0);
            weaponTransform.localEulerAngles = new Vector3(0, 0, -90f);
        }
    }
}
