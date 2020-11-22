using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using TargetStuff.MovementComponents;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomXYPlaneMC : MovementComponentBase
{
    #region Variables
    [SerializeField] private MinMax<float> m_SpeedRange;
    [SerializeField] private MinMax<float> m_TimeRange;

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

    private float CalcRandomSpeed_F() => Random.Range(m_SpeedRange.GetMin_F(), m_SpeedRange.GetMax_F());

    private static float CalcRandomRotation_F() => Random.Range(0.0f, 360f);

    private float CalcRandomTime_F() => Random.Range(m_TimeRange.GetMin_F(), m_TimeRange.GetMax_F());
    
    private void CheckAndKillMovementT_F()
    {
        if (m_MovementT.IsActive()) m_MovementT.Kill();
    }
}

