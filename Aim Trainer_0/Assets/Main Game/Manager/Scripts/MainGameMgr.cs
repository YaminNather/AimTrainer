using System.Collections;
using System.Collections.Generic;
using TargetStuff.ShapeComponents;
using UnityEditor;
using UnityEngine;

namespace MainGameMgrStuff
{
    [DefaultExecutionOrder(-10)]
    public partial class MainGameMgr : MonoBehaviour
    {
        #region Variables
        private SpawnMgr m_SpawnMgr;
        #endregion

        private void Start()
        {
            m_SpawnMgr = GetComponentInChildren<SpawnMgr>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha9))
                m_SpawnMgr.SpawnTarget_F(0.0f);

            if (Input.GetKeyDown(KeyCode.Space))
                KillRandomTarget_F();
        }

        private static void KillRandomTarget_F()
        {
            TargetMgrBase[] targets = FindObjectsOfType<TargetMgrBase>();
            targets[Random.Range(0, targets.Length)].TakeDamage(200);
        }
    }

    public partial class MainGameMgr : MonoBehaviour
    {
        [MenuItem("Custom/Cycle Play Mode")]
        static private void CyclePlayMode_F() 
            => EditorSettings.enterPlayModeOptionsEnabled = !EditorSettings.enterPlayModeOptionsEnabled;
    }
}
