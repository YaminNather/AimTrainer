using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ludiq.PeekCore.ReflectionMagic;
using TargetStuff.MovementComponents;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class CircularHumanoidMC : MovementComponentBase
{
    #region Variables
    [SerializeField] private Vector3 m_CenterPoint;
    
    [Header("Circular Movement Stuff")]
    [SerializeField] private MinMax<float> m_CircularSpeedRange;
    [SerializeField] private MinMax<float> m_CircularTimeRange;
    
    [Header("Forward Movement Stuff")]
    [SerializeField] private MinMax<float> m_ForwardSpeedRange;
    [SerializeField] private MinMax<float> m_ForwardTimeRange;
    
    [Header("Vertical Movement Stuff")]
    [SerializeField] private float m_Gravity;
    
    private Vector3 m_Velocity;
    private Tweener m_CircularMovementT;
    private Tweener m_ForwardMovementT;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if(m_IsMovementEnabled)
        {
            m_Velocity.y += m_Gravity * Time.deltaTime;
            ApplyCircularVelocity_F();
            LookAtCenterPoint_F();
            ApplyVerticalVelocity_F();
            ApplyForwardVelocity_F();
        }
    }

    private void ApplyCircularVelocity_F()
    {
        Color rayColor = Color.green;
        Vector3 deltaPos = CalcPosInAngle_F(CalcCurAngle_F() + m_Velocity.x * Time.deltaTime) - transform.position;
        if (!m_Rigidbody.SweepTest(deltaPos.normalized, out _, deltaPos.magnitude))
            transform.position = transform.position + deltaPos;
        else
            rayColor = Color.red;
        Debug.DrawRay(transform.position, deltaPos.normalized * 5.0f, rayColor);
    }

    private void ApplyVerticalVelocity_F()
    {
        Vector3 deltaPos = Vector3.up * m_Velocity.y * Time.deltaTime;

        if (!m_Rigidbody.SweepTest(deltaPos.normalized, out _, deltaPos.magnitude))
        {
            transform.position += deltaPos;
        }
    }

    private void ApplyForwardVelocity_F()
    {
        Vector3 deltaPos = transform.forward * m_Velocity.z * Time.deltaTime;

        if (!m_Rigidbody.SweepTest(deltaPos.normalized, out _, deltaPos.magnitude))
        {
            transform.position += deltaPos;
        }
    }

    private void LookAtCenterPoint_F()
    {
        transform.LookAt(m_CenterPoint);
        transform.eulerAngles = transform.eulerAngles.With(x:0.0f);
    }

    private float CalcCurAngle_F()
    {
        Vector3 centerPointToPlayerVector = transform.position.With(y: m_CenterPoint.y) - m_CenterPoint;

        float angle = Vector3.Angle(Vector3.forward, centerPointToPlayerVector);
        Vector3 cross = Vector3.Cross(Vector3.forward, centerPointToPlayerVector);
        if (cross == Vector3.zero)
            return 0.0f;
        else if (Mathf.Sign(cross.y) > 0.0f)
            return angle;
        else
            return 360.0f - angle;
    }

    private void SetAngle_F(float angle) => transform.position = CalcPosInAngle_F(angle);

    private Vector3 CalcPosInAngle_F(float angle)
    {
        float radius = CalcRadius_F();
        Vector3 pos = m_CenterPoint + Quaternion.Euler(0.0f, angle, 0.0f) * (Vector3.forward * radius);
        pos = pos.With(y: transform.position.y);
        return pos;
    }

    private float CalcRadius_F()
    {
        return (transform.position.With(y: m_CenterPoint.y) - m_CenterPoint).magnitude;
    }

    public override void EnableMovement_F()
    {
        base.EnableMovement_F();

        setCircularVel_F();
        setForwardVel_F();

        void setCircularVel_F()
        {
            CheckAndKillMovementT_F(m_CircularMovementT);

            float dir = (Random.Range(0, 2) == 0) ? -1 : 1;
            m_Velocity.x = m_CircularSpeedRange.GetRandomValueWithinRange_F() * dir;

            m_CircularMovementT = DOTween.To(() => 0, val => { }, 0, m_CircularTimeRange.GetRandomValueWithinRange_F()).
                OnComplete(setCircularVel_F);
        }

        void setForwardVel_F()
        {
            CheckAndKillMovementT_F(m_ForwardMovementT);
            float dir = (Random.Range(0, 2) == 0) ? -1 : 1;
            m_Velocity.z = m_ForwardSpeedRange.GetRandomValueWithinRange_F() * dir;

            m_CircularMovementT = DOTween.To(() => 0, val => { }, 0, m_ForwardTimeRange.GetRandomValueWithinRange_F()).
                OnComplete(setCircularVel_F);
        }
    }

    public override void DisableMovement_F()
    {
        base.DisableMovement_F();
        CheckAndKillMovementT_F(m_CircularMovementT);
        CheckAndKillMovementT_F(m_ForwardMovementT);
    }

    private void CheckAndKillMovementT_F(Tweener tween)
    {
        if(tween.IsActive()) tween.Kill();
    }
}
