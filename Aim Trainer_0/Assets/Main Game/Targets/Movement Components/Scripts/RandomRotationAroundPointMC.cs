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
        base.Awake();
        m_Rigidbody.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (m_IsMovementEnabled)
            SetAngle_F(GetAngle_F() + m_Velocity * Time.deltaTime);
    }

    public override void EnableMovement_F()
    {
        base.EnableMovement_F();
        m_CenterPoint = m_CenterPoint.With(y:transform.position.y);
        m_Radius = (transform.position - m_CenterPoint).magnitude;
        //SetKinematic_F(false);
        setVel_F();

        void setVel_F()
        {
            CheckAndKillMovementT_F();
            m_Velocity = CalcRandomDir_F() * m_SpeedRange.GetRandomValueWithinRange_F();
            m_MovementT = DOTween.To(() => 0.0f, val => { },
                0.0f, m_TimeRange.GetRandomValueWithinRange_F()).OnComplete(setVel_F);
        }
    }

    public override void DisableMovement_F()
    {
        base.DisableMovement_F();
        CheckAndKillMovementT_F();
        m_Velocity = 0.0f;
    }

    private float CalcRandomDir_F() => ((Random.Range(0, 2) == 0) ? -1 : 1);

    private void SetAngle_F(float angle)
    {
        //m_Rigidbody.MovePosition(m_CenterPoint + Quaternion.Euler(0.0f, angle, 0.0f) * (Vector3.forward * m_Radius));
        //transform.position = m_CenterPoint + Quaternion.Euler(0.0f, angle, 0.0f) * (Vector3.forward * m_Radius);

        Vector3 toPos = m_CenterPoint + Quaternion.Euler(0.0f, angle, 0.0f) * (Vector3.forward * m_Radius);

        bool blocked = false;
        Vector3 deltaPos = toPos - transform.position;
        if (!m_Rigidbody.SweepTest(deltaPos.normalized, out _, deltaPos.magnitude))
        {
            Debug.Log("Moved");
            transform.position = toPos;
        }
        else
        {
            Debug.Log("Blocked");
            blocked = true;
        }

        Debug.DrawRay(transform.position, deltaPos.normalized * 5.0f, (blocked) ? Color.red : Color.green);
        
        LookAtPoint_F();
    }

    private float GetAngle_F()
    {
        Vector3 centerPointToPlayerVector = transform.position.With(y:m_CenterPoint.y) - m_CenterPoint;

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