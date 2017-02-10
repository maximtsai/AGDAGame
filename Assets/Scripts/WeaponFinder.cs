using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponFinder : MonoBehaviour {
    public GameObject listOfWeapons;
    public GameObject pickUpIndicator;
    public Text indicatorTextDisplay;
    public KeyCode pickUpWeaponButton = KeyCode.F;
    public float weaponGrabDist = 3;
    public float weaponOffsetDist = 1.9f;
    float closestWeaponDist = 99;
    Vector3 indicatorPos = new Vector3(0,0,-5);
    float prevRotation = 0;
    float prevPrevRotation = 0;
    float rotationDiff = 0;
    private const float DEG_TO_RAD = Mathf.PI / 180.0f;
    Rigidbody2D playerRB;
	// Use this for initialization
	void Start () {
        closestWeaponDist = weaponGrabDist;
        indicatorTextDisplay.text = pickUpWeaponButton.ToString();
        playerRB = this.GetComponent<Rigidbody2D>();
        prevRotation = playerRB.rotation;
        prevPrevRotation = playerRB.rotation;
    }

    // Update is called once per frame
    void Update () {
        float currRotation = playerRB.rotation;
        rotationDiff = currRotation - prevPrevRotation;
        int i = -1;
        int closestWeaponIndex = -1;
        foreach (Transform child in listOfWeapons.transform)
        {
            i++;
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
            if (distToWeapon < closestWeaponDist)
            {
                // found weapon that's close enough, is potential thing to pick up
                closestWeaponIndex = i;
                closestWeaponDist = distToWeapon;
            }
            closestWeaponDist = weaponGrabDist;
        }
        if (closestWeaponIndex > -1)
        {
            Transform weaponTransform = listOfWeapons.transform.GetChild(closestWeaponIndex);
            // have pickup weapon indicator show up and set it to active
            indicatorPos.x = weaponTransform.position.x;
            indicatorPos.y = weaponTransform.position.y + 1.6f;
            pickUpIndicator.transform.position = indicatorPos;
            pickUpIndicator.SetActive(true);
            // picking up weapon
            if (Input.GetKeyUp(pickUpWeaponButton))
            {
                bool hasWeapon = false;
                foreach (Transform child in this.transform)
                {
                    if (child.CompareTag("Weapon"))
                    {
                        // has a weapon, throw it away
                        hasWeapon = true;
                        child.SetParent(listOfWeapons.transform);
                        child.gameObject.AddComponent<Rigidbody2D>();
                        child.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    }
                }
                if (!hasWeapon)
                {
                    // grab weapon, put it in front of player
                    Destroy(weaponTransform.GetComponent<Rigidbody2D>());
                    weaponTransform.SetParent(this.transform);
                    weaponTransform.localPosition = new Vector3(weaponOffsetDist, 0, 0);
                    weaponTransform.localEulerAngles = new Vector3(0, 0, -90f);
                }
            }
        } else
        {
            pickUpIndicator.SetActive(false);
            if (Input.GetKeyUp(pickUpWeaponButton))
            {
                foreach (Transform child in this.transform)
                {
                    if (child.CompareTag("Weapon"))
                    {
                        // has a weapon, throw it away
                        child.SetParent(listOfWeapons.transform);
                        Rigidbody2D childRB = child.gameObject.AddComponent<Rigidbody2D>();
                        childRB.gravityScale = 0;
                        childRB.drag = 0.75f;
                        childRB.angularDrag = 1;
                        Vector2 throwVelocity = playerRB.GetPointVelocity(child.position) * 1.8f;
                        // also need to take into account player rotation velocity
                        float velX = 0;
                        float velY = 0;
                        if (rotationDiff > 0)
                        {
                            // counterclockwise fling
                            velX = Mathf.Cos((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 2f;
                            velY = Mathf.Sin((prevRotation + 90) * DEG_TO_RAD) * rotationDiff * 2f;
                        }
                        else
                        {
                            // clockwise fling
                            velX = Mathf.Cos((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 2f;
                            velY = Mathf.Sin((prevRotation - 90) * DEG_TO_RAD) * -rotationDiff * 2f;
                        }
                        throwVelocity.x += velX;
                        throwVelocity.y += velY;

                        child.gameObject.GetComponent<Rigidbody2D>().velocity = throwVelocity;
                        child.gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationDiff*3);

                    }
                }
            }
        }
        prevPrevRotation = prevRotation;
        prevRotation = currRotation;
    }
}
