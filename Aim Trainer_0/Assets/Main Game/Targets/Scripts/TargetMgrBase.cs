using System;
using System.Collections;
using System.Collections.Generic;
using CustomUI.Bar;
using TargetStuff.HealthComponents;
using TargetStuff.MovementComponents;
using UnityEngine;

namespace TargetStuff.ShapeComponents
{
    public abstract class TargetMgrBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            m_ShapeComponentBase = GetComponentInChildren<ShapeComponentBase>();
            m_HealthComponent = GetComponentInChildren<HealthComponent>();
            m_MovementComponent = GetComponentInChildren<MovementComponentBase>();
            m_HealthBar = GetComponentInChildren<SerializedFloatBar>();
        }

        protected virtual void Start()
        {
            if (m_HealthBar != null)
            {
                m_HealthBar.SetValueRange_F(new MinMax<float>(0, m_HealthComponent.GetMaxHealth_F()));
                m_HealthBar.SetSharedValue_F(m_HealthComponent.GetCurHealthSharedValue_F());
            }
        }

        public virtual void OnSpawn_F()
        {
            gameObject.SetActive(true);
            m_HealthComponent.RestoreMaxHealth_F();
            m_HealthComponent.m_OnHealthZeroE += Kill_EF;
            m_MovementComponent.EnableMovement_F();
        }

        public virtual void TakeDamage(int amount)
        {
            m_HealthComponent.ReduceHealth_F(amount);
        }

        protected virtual void Kill_EF()
        {
            m_OnKilledE?.Invoke(this);
            Despawn_F();
        }

        public virtual void Despawn_F()
        {
            m_MovementComponent.DisableMovement_F();
            m_HealthComponent.m_OnHealthZeroE -= Kill_EF;
            Destroy(gameObject);
        }

        public ShapeComponentBase GetShapeComponent_F() => m_ShapeComponentBase;
        
        
        #region Variables
        private ShapeComponentBase m_ShapeComponentBase;
        private MovementComponentBase m_MovementComponent;
        private HealthComponent m_HealthComponent;
        private SerializedFloatBar m_HealthBar;

        public System.Action<TargetMgrBase> m_OnKilledE;
        #endregion
    }
}
