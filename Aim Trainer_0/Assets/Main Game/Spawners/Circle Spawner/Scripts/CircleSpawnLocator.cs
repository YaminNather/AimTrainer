using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CircleSpawnLocator : SpawnLocatorBase
{
    public override Vector3 CalcRandomSpawnPoint_F()
    {
        float angle = Random.Range(0.0f, 360.0f);
        Vector3 offset = Quaternion.AngleAxis(angle, transform.up) * transform.right;
        float radius = Random.Range(m_InnerRingRadius, m_OuterRingRadius);
        offset = offset * radius;
        offset = offset + (transform.up * 0.5f);
        Vector3 r = transform.position + offset;
        return r;
    }

    protected override void DrawVisualizer_F()
    {
        float brightness = 0.5f;
        Handles.color = new Color(brightness, brightness, brightness);
        Handles.DrawWireDisc(transform.position, transform.up, m_InnerRingRadius);
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, transform.up, m_OuterRingRadius);

    }

    private void OnValidate()
    {
        if (m_InnerRingRadius > m_OuterRingRadius)
            SwapValues_F(ref m_InnerRingRadius, ref m_OuterRingRadius);
    }

    private void SwapValues_F<T>(ref T var0, ref T var1)
    {
        T temp = var0;
        var0 = var1;
        var1 = temp;
    }



    [SerializeField] private float m_InnerRingRadius;
    [SerializeField] private float m_OuterRingRadius;
}
