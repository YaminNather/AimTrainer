using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TargetStuff.MovementComponents;
using UnityEngine;

public class RandomRotationAroundPointMC : MovementComponentBase
{
    #region Variables
    private bool m_IsKinematic;

    [SerializeField] private Vector3 m_CenterPoint;
    private float m_Radius;

    private float m_Velocity;

    [SerializeField] private float[] m_SpeedRange;
    [SerializeField] private float[] m_TimeRange;

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

    private float CalcRandomDir_F() => ((Random.Range(0, 1) == 0) ? -1 : 1);

    private float CalcRandomSpeed_F() => Random.Range(m_SpeedRange[0], m_SpeedRange[1]);

    private float CalcRandomTime_F() => Random.Range(m_TimeRange[0], m_TimeRange[1]);

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
        if(m_MovementT.IsActive()) m_MovementT.Kill();
    }
}
