using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using TargetStuff.MovementComponents;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomXYPlaneMC : MovementComponentBase
{
    #region Variables
    [SerializeField] private float[] m_SpeedRange;
    [SerializeField] private float[] m_TimeRange;

    private Tweener m_MovementT;
    #endregion

    private void Update()
    {
        Debug.DrawRay(transform.position, m_Rigidbody.velocity.normalized * 5.0f, Color.green);
    }

    public override void EnableMovement_F()
    {
        base.EnableMovement_F();

        setVel_F();

        void setVel_F()
        {
            CheckAndKillMovementT_F();

            m_Rigidbody.velocity = calcRandomDirVector_F() * CalcRandomSpeed_F();
            m_MovementT = DOTween.To(() => 0.0f, val => { },
                0.0f, CalcRandomTime_F()).OnComplete(setVel_F);
        }

        Vector3 calcRandomDirVector_F()
        {
            return Quaternion.AngleAxis(CalcRandomRotation_F(), transform.forward) 
                   * transform.right;
        }
    }

    public override void DisableMovement_F()
    {
        CheckAndKillMovementT_F();
        m_Rigidbody.velocity = Vector3.zero;
        base.DisableMovement_F();
    }

    private float CalcRandomSpeed_F() => Random.Range(m_SpeedRange[0], m_SpeedRange[1]);

    private static float CalcRandomRotation_F() => Random.Range(0.0f, 360f);

    private float CalcRandomTime_F() => Random.Range(m_TimeRange[0], m_TimeRange[1]);
    
    private void CheckAndKillMovementT_F()
    {
        if (m_MovementT.IsActive()) m_MovementT.Kill();
    }
}



//[Serializable]
//public struct MinMax<T>
//{
//    [SerializeField] private T m_Min;
//    [SerializeField] private T m_Max;

//    public MinMax(T m_Min, T m_Max)
//    {
//        this.m_Min = m_Min;
//        this.m_Max = m_Max;
//    }

//    public T GetMin_F() => m_Min;
//    public void SetMin_F(T value) => m_Min = value;

//    public T GetMax_F() => m_Max;
//    public T SetMax_F(T value) => m_Max = value;
//}

//[CustomPropertyDrawer(typeof(MinMax<>))]
//public class MinMaxPropertyDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        base.OnGUI(position, property, label);

//        EditorGUILayout.MinMaxSlider();

//    }
//}

