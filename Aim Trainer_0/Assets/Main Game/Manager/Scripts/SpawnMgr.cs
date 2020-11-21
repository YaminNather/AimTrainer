using System.Collections;
using System.Collections.Generic;
using TargetStuff.ShapeComponents;
using UnityEngine;

namespace MainGameMgrStuff
{
    [DefaultExecutionOrder(-9)]
    public class SpawnMgr : MonoBehaviour
    {
        #region Variables
        private SpawnLocatorBase[] m_Spawners;
        [SerializeField] private TargetMgrBase m_TargetPrefab;

        [SerializeField] private int m_InitialCount; 
        #endregion

        private void Awake()
        {
            m_Spawners = FindObjectsOfType<SpawnLocatorBase>();
        }

        private void Start()
        {
            for (int i = 0; i < m_InitialCount; i++)
            {
                SpawnTarget_F();
            }
        }

        private void SpawnTarget_F()
        {
            SpawnLocatorBase spawner = m_Spawners[Random.Range(0, m_Spawners.Length)];

            TargetMgrBase target = Instantiate(m_TargetPrefab.gameObject, Vector3.zero, Quaternion.identity)
                .GetComponent<TargetMgrBase>();
            spawner.SetTargetPosAndRot_F(target);
            target.OnSpawn_F();

            void onTargetKilledE(TargetMgrBase caller)
            {
                caller.m_OnKilledE -= onTargetKilledE;
                SpawnTarget_F(0.1f);
            }

            target.m_OnKilledE += onTargetKilledE;
        }

        public void SpawnTarget_F(float delay)
        {
            StartCoroutine(spawnTarget_IEF());

            IEnumerator spawnTarget_IEF()
            {
                yield return new WaitForSeconds(delay);

                SpawnTarget_F();
            }
        }
    }
}
