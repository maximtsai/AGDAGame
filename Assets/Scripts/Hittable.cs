using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour {
    private ushort hittableId;

    public ushort HittableId
    {
        get
        {
            return hittableId;
        }

        set
        {
            hittableId = value;
        }
    }
}
