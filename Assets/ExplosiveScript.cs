using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveScript : MonoBehaviour
{
    public GameObject listOfShrapnel;
    public float detonationImpactVelocity = 3;
    public float detonationDelay = 3;
    public GameObject shrapnel;
    public int numberOfShrapnel = 10;
    int shrapnelInstantiated = 0;
    float startTime;
    // Use this for initialization
    private void Awake()
    {
        listOfShrapnel = GameObject.Find("ListOfShrapnel");
        if (!listOfShrapnel)
        {
            Debug.Log("Warning, no ListOfShrapnel object found");
            Destroy(this);
        }
    }
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint2D contact = collision.contacts[0];
            // compare relative velocity to collision normal - so we don't explode from a fast but gentle glancing collision

            float velocityAlongCollisionNormal = Mathf.Abs(Vector2.Dot(contact.normal, collision.relativeVelocity));

            if (velocityAlongCollisionNormal > detonationImpactVelocity)
            {
                Explode();
            }
        }
        if (Time.time > startTime + detonationDelay)
        {
            Explode();
        }
    }
    private void Explode()
    {
        for (int i = 0; i < numberOfShrapnel; i++)
        {
            createShrapnel(this.transform.position);
        }
        Time.timeScale = Mathf.Min(Time.timeScale, 0.95f);
        Destroy(this.gameObject);
    }

    private void createShrapnel(Vector2 shrapnelPos)
    {
        Vector2 tempVec = new Vector2(0, 0);
        bool shrapnelHandled = false;
        foreach (Transform childShrapnel in listOfShrapnel.transform)
        {
            GameObject shrapnelObj = childShrapnel.gameObject;
            // use an existing shrapnel object if it is available
            if (!shrapnelObj.activeSelf)
            {
                shrapnelHandled = true;
                shrapnelObj.SetActive(true);
                shrapnelObj.transform.position = shrapnelPos;
                shrapnelObj.transform.localScale = new Vector3(0.3f, 0.3f, 1);
                Rigidbody2D rb = shrapnelObj.GetComponent<Rigidbody2D>();
                // reset disappear timer
                shrapnelObj.GetComponent<DisappearAfterTime>().resetDisappearTime(); 
                tempVec.x = Random.Range(-65, 65);
                tempVec.y = Random.Range(-65, 65);
                rb.velocity = tempVec;
                tempVec.x *= 0.01f;
                tempVec.y *= 0.01f;
                tempVec = tempVec + rb.position;
                rb.position = tempVec;
                break;
            }
        }
        if (!shrapnelHandled)
        {
            // create new shrapnel object
            GameObject newShrapnel = Instantiate(shrapnel, listOfShrapnel.transform, true);
            newShrapnel.transform.position = shrapnelPos;
            Rigidbody2D rb = newShrapnel.GetComponent<Rigidbody2D>();
            tempVec.x = Random.Range(-65, 65);
            tempVec.y = Random.Range(-65, 65);
            rb.velocity = tempVec;
            tempVec.x *= 0.01f;
            tempVec.y *= 0.01f;
            tempVec = tempVec + rb.position;
        }
        shrapnelHandled = false;
    }
}
