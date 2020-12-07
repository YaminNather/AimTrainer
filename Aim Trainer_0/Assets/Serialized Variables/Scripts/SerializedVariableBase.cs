using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SerializedVariableBase<T> : ScriptableObject
{
    [SerializeField] private T m_InitialValue;
    [SerializeField] private T m_SharedValue;
    public System.Action<T> m_OnChangedE;

    public void SetValueToInitialValue_F() => m_SharedValue = m_InitialValue;

    public T GetInitialValue_F() => m_InitialValue;

    public T GetValue_F() => m_SharedValue;
    public void SetValue_F(T value)
    {
        m_SharedValue = value;
        m_OnChangedE?.Invoke(m_SharedValue);
    }
}
