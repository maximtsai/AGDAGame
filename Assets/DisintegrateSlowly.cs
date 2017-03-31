using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisintegrateSlowly : MonoBehaviour {
    int duration = 0;
    float initialXSize;
    float initialYSize;
    Vector3 nextScale;
    private Rigidbody2D rb;
    private bool asdf = true;
    // Use this for initialization
    void Start () {
        duration = 150 + (int)Random.Range(0, 120);
        initialXSize = this.transform.localScale.x * Random.Range(0.5f, 2f);
        initialYSize = this.transform.localScale.y * Random.Range(0.5f, 2f);
        this.gameObject.transform.localScale = new Vector2(initialXSize, initialYSize);
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        if (asdf)
        {
            asdf = false;
            Vector2 speed = new Vector2((float)Random.Range(-160, 160), (float)Random.Range(-160, 160));
            if (speed.magnitude > 140)
            {
                speed *= 0.7f;
            }
            rb.velocity = speed;

        }
        duration--;
        if (duration < 0)
        {
            nextScale.x = this.transform.localScale.x - initialXSize * 0.05f;
            nextScale.y = this.transform.localScale.y - initialYSize * 0.05f;
            this.transform.localScale = nextScale;
            if (this.transform.localScale.x <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
