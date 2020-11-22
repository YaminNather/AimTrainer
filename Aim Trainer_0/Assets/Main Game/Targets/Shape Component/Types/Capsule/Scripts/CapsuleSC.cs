using System.Collections;
using System.Collections.Generic;
using TargetStuff.ShapeComponents;
using UnityEngine;

public class CapsuleSC : ShapeComponentBase
{
    public override bool CheckForOverlaps_F()
    {
        CapsuleCollider collider = GetComponentInChildren<CapsuleCollider>();

        float length = (transform.localScale.y * 2.0f);
        Vector3 centerPoint = transform.position + 
                              transform.up * (length / 2.0f);
        float radius = transform.localScale.x;
        float cylinderHeight = length - (radius * 2.0f);
        Vector3 p0 = centerPoint + (-transform.up * cylinderHeight / 2.0f);
        Vector3 p1 = centerPoint + (transform.up * cylinderHeight / 2.0f);

        Collider[] result = Physics.OverlapCapsule(p0, p1, radius);
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
