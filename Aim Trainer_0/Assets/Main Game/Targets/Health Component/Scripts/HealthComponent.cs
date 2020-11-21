using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetStuff.HealthComponents
{
    public class HealthComponent : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int m_MaxHealth;
        private int m_CurHealth;
        public System.Action m_OnHealthZeroE;
        #endregion

        private void Awake()
        {
            RestoreMaxHealth_F();
        }

        public void AddHealth_F(int amount)
        {
            m_CurHealth += amount;
            ClampHealth_F();
        }

        private void ClampHealth_F() => m_CurHealth = Mathf.Clamp(m_CurHealth, 0, m_MaxHealth);

        public void ReduceHealth_F(int amount)
        {
            m_CurHealth -= amount;

            ClampHealth_F();

            if(m_CurHealth == 0)
                m_OnHealthZeroE?.Invoke();
        }
        
        public void RestoreMaxHealth_F()
        {
            m_CurHealth = m_MaxHealth;
        }
    }
}
