using System.Collections;
using System.Collections.Generic;
using TargetStuff.ShapeComponents;
using UnityEngine;

public class BallShapeComponentMgr : ShapeComponentBase
{
    public override bool CheckForOverlaps_F()
    {
        SphereCollider collider = GetComponentInChildren<SphereCollider>();

        Vector3 relOrigin = transform.rotation * 
                            new Vector3(0.0f, transform.localScale.y / 2.0f, 0.0f);
        Collider[] result = Physics.OverlapSphere(transform.position + relOrigin,
            collider.bounds.extents.x);
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
