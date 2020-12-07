using System.Collections;
using System.Collections.Generic;
using Ludiq.PeekCore.ReflectionMagic;
using UnityEngine;

public partial class BoxSpawnLocator : SpawnLocatorBase
{

    public override Vector3 CalcRandomSpawnPoint_F()
    {
        Vector3 relOffset = new Vector3(
            Random.Range(0.0f, transform.localScale.x),
            Random.Range(0.0f, transform.localScale.y),
            Random.Range(0.0f, transform.localScale.z)
            );

        Vector3 r = transform.position + transform.rotation * relOffset;
        return r;
    }

    protected override void DrawVisualizer_F()
    {
        Matrix4x4 defaultMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Vector3 relOffset = new Vector3(transform.localScale.x / 2.0f,
            transform.localScale.y / 2.0f,
            transform.localScale.z / 2.0f);

        Gizmos.DrawWireCube(relOffset,
            transform.localScale);

        Gizmos.matrix = defaultMatrix;
    }
}
