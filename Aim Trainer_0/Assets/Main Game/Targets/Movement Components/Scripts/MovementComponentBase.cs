using System.Collections;
using System.Collections.Generic;
using TargetStuff.ShapeComponents;
using UnityEngine;

namespace TargetStuff.MovementComponents
{
    public abstract class MovementComponentBase : MonoBehaviour
    {
        #region Variables
        protected TargetMgrBase m_Target;
        
        protected Rigidbody m_Rigidbody;
        protected bool m_IsMovementEnabled;
        #endregion

        protected virtual void Awake()
        {
            m_Target = GetComponent<TargetMgrBase>();
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        public virtual void EnableMovement_F()
        {
            if (m_IsMovementEnabled)
                return;

            m_IsMovementEnabled = true;
        }

        public virtual void DisableMovement_F()
        {
            if (!m_IsMovementEnabled)
                return;

            m_IsMovementEnabled = false;
        }

        public bool GetIsMovementEnabled_F() => m_IsMovementEnabled;
    }
}
