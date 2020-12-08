using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetStuff.HealthComponents
{
    public class HealthComponent : MonoBehaviour
    {
        private void Awake()
        {
            m_CurHealth = ScriptableObject.CreateInstance<SerializedFloat>();
            RestoreMaxHealth_F();
        }

        public void AddHealth_F(int amount)
        {
            m_CurHealth.SetValue_F(m_CurHealth.GetValue_F() + amount);
            ClampHealth_F();
        }

        private void ClampHealth_F() => m_CurHealth.SetValue_F(Mathf.Clamp(m_CurHealth.GetValue_F(), 0.0f, m_MaxHealth));

        public void ReduceHealth_F(int amount)
        {
            m_CurHealth.OffsetValue_F(-amount);

            ClampHealth_F();

            if(m_CurHealth.GetValue_F() <= 0)
                m_OnHealthZeroE?.Invoke();
        }
        
        public void RestoreMaxHealth_F() => m_CurHealth.SetValue_F(m_MaxHealth);

        private void OnDestroy()
        {
            Destroy(m_CurHealth);
        }


        #region Variables
        [SerializeField] private int m_MaxHealth;
        private SerializedFloat m_CurHealth;
        public System.Action m_OnHealthZeroE;
        #endregion

        public SerializedFloat GetCurHealthSharedValue_F() => m_CurHealth;

        public float GetMaxHealth_F() => m_MaxHealth;
    }
}
