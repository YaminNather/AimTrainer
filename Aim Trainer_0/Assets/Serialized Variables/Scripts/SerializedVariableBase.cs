using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SerializedVariableBase<T> : ScriptableObject
{
    [SerializeField] protected T m_InitialValue;
    [SerializeField] protected T m_Value;
    public System.Action<T> m_OnChangedE;

    public void SetValueToInitialValue_F() => m_Value = m_InitialValue;

    public T GetInitialValue_F() => m_InitialValue;

    public T GetValue_F() => m_Value;
    public void SetValue_F(T value)
    {
        m_Value = value;
        m_OnChangedE?.Invoke(m_Value);
    }
}

public class SerializedNumberVariableBase<T> : SerializedVariableBase<T>
{
    public void OffsetValue_F(T value) => m_Value += (dynamic)value;
}
