using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct MinMax<T>
{
    [SerializeField] private T m_Min;
    [SerializeField] private T m_Max;

    public MinMax(T m_Min, T m_Max)
    {
        this.m_Min = m_Min;
        this.m_Max = m_Max;
    }

    public T GetRandomValueWithinRange_F()
    {
        if (typeof(T) == typeof(int))
            return Random.Range((dynamic) m_Min, (dynamic) m_Max + 1);
        else if (typeof(T) == typeof(float))
            return Random.Range((dynamic)m_Min, (dynamic)m_Max);
        else
            return default(T);
    }
    
    public T ClampWithinRange_F(T value) => Mathf.Clamp((dynamic)value, (dynamic)m_Min, (dynamic)m_Max);

    public T GetMidPoint_F() => ((dynamic)m_Max - (dynamic)m_Min) / 2.0f;

    public T GetMin_F() => m_Min;
    public void SetMin_F(T value) => m_Min = value;

    public T GetMax_F() => m_Max;
    public T SetMax_F(T value) => m_Max = value;

}

[CustomPropertyDrawer(typeof(MinMax<>))]
public class MinMaxPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, label);

        float fieldWidth = 55.0f;
        position.width = fieldWidth;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("m_Min"), GUIContent.none);

        position.x += fieldWidth + 5.0f;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("m_Max"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}
