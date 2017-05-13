using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreSpecificObj : MonoBehaviour {
    public bool ignorePlayers = false;
    public bool ignoreWeapons = false;
    public bool ignoreWalls = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
