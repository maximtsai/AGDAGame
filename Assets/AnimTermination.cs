using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTermination : MonoBehaviour
{
    // Use this for initialization
    float damageRadius = 0;
    void Start()
    {
        damageRadius = this.transform.localScale.x;
    }

    void DestroyGameObject()
    {
        gameObject.transform.position.Set(-1000, 0, 0);
        gameObject.SetActive(false);
        //Destroy (gameObject);
    }
}
