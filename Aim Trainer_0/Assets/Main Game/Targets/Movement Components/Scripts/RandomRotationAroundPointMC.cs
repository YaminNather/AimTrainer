using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TargetStuff.MovementComponents;
using UnityEditor;
using UnityEngine;

public partial class RandomRotationAroundPointMC : MovementComponentBase
{
    #region Variables

    private bool m_IsKinematic;

    [SerializeField] private Vector3 m_CenterPoint;
    private float m_Radius;

    private float m_Velocity;

    [SerializeField] private MinMax<float> m_SpeedRange;
    [SerializeField] private MinMax<float> m_TimeRange;

    private Tweener m_MovementT;

    #endregion

    protected override void Awake()
    {
        m_IsKinematic = true;
    }

    private void Update()
    {
        if (!m_IsKinematic)
        {
            SetAngle_F(GetAngle_F() + m_Velocity * Time.deltaTime);
        }
    }

    public override void EnableMovement_F()
    {
        base.EnableMovement_F();
        m_CenterPoint = m_CenterPoint.With(y:transform.position.y);
        m_Radius = (transform.position - m_CenterPoint).magnitude;
        m_IsKinematic = false;
        setVel_F();

        void setVel_F()
        {
            CheckAndKillMovementT_F();
            m_Velocity = CalcRandomDir_F() * CalcRandomSpeed_F();
            m_MovementT = DOTween.To(() => 0.0f, val => { },
                0.0f, CalcRandomTime_F()).OnComplete(setVel_F);
        }
    }

    public override void DisableMovement_F()
    {
        CheckAndKillMovementT_F();
        m_Velocity = 0.0f;
        m_IsKinematic = true;
    }

    private float CalcRandomDir_F() => ((Random.Range(0, 2) == 0) ? -1 : 1);

    private float CalcRandomSpeed_F() => Random.Range(m_SpeedRange.GetMin_F(), m_SpeedRange.GetMax_F());

    private float CalcRandomTime_F() => Random.Range(m_TimeRange.GetMin_F(), m_TimeRange.GetMax_F());

    private void SetAngle_F(float angle)
    {
        transform.position = m_CenterPoint + Quaternion.Euler(0.0f, angle, 0.0f) * (Vector3.forward * m_Radius);
        LookAtPoint_F();
    }

    private float GetAngle_F()
    {
        Vector3 centerPointToPlayerVector = transform.position - m_CenterPoint;

        float angle = Vector3.Angle(Vector3.forward, centerPointToPlayerVector);
        Vector3 cross = Vector3.Cross(Vector3.forward, centerPointToPlayerVector);
        if (cross == Vector3.zero)
            return 0.0f;
        else if (Mathf.Sign(cross.y) > 0.0f)
            return angle;
        else
            return 360.0f - angle;
    }

    private void LookAtPoint_F() => transform.LookAt(m_CenterPoint, Vector3.up);

    private void CheckAndKillMovementT_F()
    {
        if (m_MovementT.IsActive()) m_MovementT.Kill();
    }
}

[CustomEditor(typeof(RandomRotationAroundPointMC))]
public partial class RandomRotationAroundPointMCEditor : Editor
{
    private void OnSceneGUI()
    {
        serializedObject.Update();
        Vector3 centerPoint = serializedObject.FindProperty("m_CenterPoint").vector3Value;
        serializedObject.FindProperty("m_CenterPoint").vector3Value = Handles.PositionHandle(centerPoint, Quaternion.identity);
        serializedObject.ApplyModifiedProperties();
    }
}