using System.Collections.Generic;
using TargetStuff.ShapeComponents;
using UnityEngine;

public  abstract partial class SpawnLocatorBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        m_SpawnPoints = new List<Vector3>();
    }

    public virtual void SetTargetPosAndRot_F(TargetMgrBase target)
    {
        Vector3 spawnPoint;
        do
        {
            spawnPoint = CalcRandomSpawnPoint_F();
            target.transform.position = spawnPoint;
            target.transform.rotation = transform.rotation;
        } while (target.GetShapeComponent_F().CheckForOverlaps_F());

#if UNITY_EDITOR
        if(m_SpawnPoints.Count > 5)
            m_SpawnPoints.RemoveAt(0);
        m_SpawnPoints.Add(spawnPoint);
#endif
    }

    public abstract Vector3 CalcRandomSpawnPoint_F();
}

#if UNITY_EDITOR
public abstract partial class SpawnLocatorBase : MonoBehaviour
{
    protected abstract void DrawVisualizer_F();
    
    protected virtual void OnDrawGizmos()
    {
        DrawVisualizer_F();

        if(Application.isPlaying)
            DrawSpawnPoints_F();
    }

    private void DrawSpawnPoints_F()
    {
        Gizmos.color = new Color32(255, 20, 147, 255);

        foreach (Vector3 spawnPoint in m_SpawnPoints)
            Gizmos.DrawWireCube(spawnPoint, Vector3.one * 0.5f);
    }



    #region Variables
    private List<Vector3> m_SpawnPoints;
    #endregion
}
#endif
