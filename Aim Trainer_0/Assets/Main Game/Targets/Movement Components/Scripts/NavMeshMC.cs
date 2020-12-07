using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TargetStuff.MovementComponents;
using TargetStuff.ShapeComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public partial class NavMeshMC : MovementComponentBase
{
    protected override void Awake()
    {
        base.Awake();
        m_NavMeshSurface = FindObjectOfType<NavMeshSurface>();

        m_NavMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        m_NavMeshAgent.acceleration = 120.0f;
        m_NavMeshAgent.enabled = false;
    }

    private void Update()
    {
        if (m_IsMovementEnabled)
        {
            if (m_NavMeshAgent.remainingDistance < 0.01f)
            {
                SetDestination_F();
                //Debug.Log("Set new destination cuz the object reached point");
            }
        }
    }

    [ContextMenu("Enable Movement")]
    public override void EnableMovement_F()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        base.EnableMovement_F();
        m_NavMeshAgent.enabled = true;
        SetDestination_F();

    }

    private void SetDestination_F()
    {
        if (calcRandomMovePoint_F(2000.0f, out Vector3 point))
        {
            //Debug.Log($"Found Point; Point = {point}");
            m_NavMeshAgent.SetDestination(point);
            m_MoveToPoint = point;
            m_NavMeshAgent.speed = m_SpeedRange.CalcRandomValueWithinRange_F();
        }
        
        CheckAndKillMovementT_F();
        m_MovementT = DOTween.To(() => 0.0f, val => {}, 0.0f,
            m_TimeRange.CalcRandomValueWithinRange_F()).OnComplete(SetDestination_F);
        
        
        bool calcRandomMovePoint_F(float radius, out Vector3 point)
    {
        point = Vector3.zero;
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(randomDirection, out navMeshHit, radius, 1))
        {
            point = navMeshHit.position;
            return true;
        }

        return false;
    }
    }

    [ContextMenu("Disable Movement")]
    public override void DisableMovement_F()
    {
        base.DisableMovement_F();
        m_NavMeshAgent.enabled = false;
        CheckAndKillMovementT_F();
    }

    private void CheckAndKillMovementT_F()
    {
        if(m_MovementT.IsActive()) m_MovementT.Kill();
    }



    #region Variables
    private NavMeshSurface m_NavMeshSurface;
    private NavMeshAgent m_NavMeshAgent;

    [SerializeField] private MinMax<float> m_TimeRange;
    [SerializeField] private MinMax<float> m_SpeedRange;

    private Tweener m_MovementT;
    #endregion
}

#if UNITY_EDITOR
public partial class NavMeshMC : MovementComponentBase
{
    private void OnDrawGizmos()
    {
        if (m_IsMovementEnabled)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(m_MoveToPoint, 0.5f);
        }
    }
    
    
    
    private Vector3 m_MoveToPoint;
}
#endif
