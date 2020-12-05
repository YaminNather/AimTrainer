using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TargetStuff.MovementComponents;
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
        m_NavMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    [ContextMenu("Enable Movement")]
    public override void EnableMovement_F()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        base.EnableMovement_F();

        setDestination_F();

        void setDestination_F()
        {
            if (CalcRandomMovePoint_F(2000.0f, out Vector3 point))
            {
                Debug.Log($"Found Point; Point = {point}");
                m_NavMeshAgent.SetDestination(point);
                m_MoveToPoint = point;
            }
            
            CheckAndKillMovementT_F();
            m_MovementT = DOTween.To(() => 0.0f, val => {}, 0.0f,
                m_TimeRange.CalcRandomValueWithinRange_F()).OnComplete(setDestination_F);
        }
    }

    [ContextMenu("Disable Movement")]
    public override void DisableMovement_F()
    {
        base.DisableMovement_F();
        CheckAndKillMovementT_F();
    }

    private bool CalcRandomMovePoint_F(float radius, out Vector3 point)
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
