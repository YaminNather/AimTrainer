using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class InvisibleWall : MonoBehaviour
{

}

#if UNITY_EDITOR
public partial class InvisibleWall : MonoBehaviour
{
    [MenuItem("GameObject/Custom/Invisible Wall")]
    static private bool CreateInvisibleWallValidator_F()
    {
        if (Selection.gameObjects.Length > 1)
            return false;
        return true;
    }

    [MenuItem("GameObject/Custom/Invisible Wall")]
    static private void CreateInvisibleWall_F()
    {
        GameObject gObj = new GameObject("Invisible Wall", typeof(InvisibleWall));
        BoxCollider boxCollider = gObj.AddComponent<BoxCollider>();
        boxCollider.center = Vector3.one * 0.5f;
        GameObjectUtility.SetParentAndAlign(gObj, Selection.activeGameObject);
        Undo.RegisterCreatedObjectUndo(gObj, "Created Invisible Wall");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Vector3 center = new Vector3(transform.localScale.x / 2.0f, transform.localScale.y / 2.0f, transform.localScale.z / 2.0f);
        Gizmos.DrawWireCube(center, transform.localScale);
    }
}
#endif
