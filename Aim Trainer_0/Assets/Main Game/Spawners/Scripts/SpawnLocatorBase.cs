using TargetStuff.ShapeComponents;
using UnityEngine;

public abstract class SpawnLocatorBase : MonoBehaviour
{

    public virtual void SetTargetPosAndRot_F(TargetMgrBase target)
    {
        do
        {
            target.transform.position = CalcRandomSpawnPoint_F();
            target.transform.rotation = transform.rotation;
        } while (target.GetShapeComponent_F().CheckForOverlaps_F());

    }

    public abstract Vector3 CalcRandomSpawnPoint_F();

    protected abstract void DrawVisualizer_F();

    private void OnDrawGizmos()
    {
        DrawVisualizer_F();
    }
}
