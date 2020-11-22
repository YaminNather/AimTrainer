using System.Collections;
using System.Collections.Generic;
using TargetStuff.ShapeComponents;
using UnityEngine;

public class BoxSCMgr : ShapeComponentBase
{
    public override bool CheckForOverlaps_F()
    {
        BoxCollider collider = GetComponentInChildren<BoxCollider>();

        Vector3 halfExtents = transform.localScale / 2.0f;
        Vector3 center = transform.position + (transform.up * halfExtents.y);
        Collider[] result = Physics.OverlapBox(center, halfExtents, transform.rotation);
        if (result.Length < 1)
        {
            return false;
        }
        else if (result.Length == 1)
        {
            if (result[0] != collider)
                return true;

            return false;
        }
        else
            return true;

    }
}
