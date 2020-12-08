using System.Collections;
using System.Collections.Generic;
using TargetStuff.ShapeComponents;
using UnityEngine;

public class AimComponent : MonoBehaviour
{
    #region Variables
    private Camera m_Camera;

    [SerializeField] private float m_Sensitivity = 0.022f;
    [SerializeField] private float m_SensitivityMultiplier = 1.0f;

    [SerializeField] private float m_Firerate = 0.1f;
    private float m_FirerateTimer;

    [SerializeField] private int m_DamagePerShot = 20;
    #endregion

    private void Awake()
    {
        m_Camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (m_FirerateTimer > 0.0f) m_FirerateTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && m_FirerateTimer <= 0.0f)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            int layerMask = LayerMask.GetMask("Player");
            if (Physics.Raycast(ray, out RaycastHit hitInfo,20.0f,~layerMask))
            {
                Debug.Log($"Hit objects name = {hitInfo.transform.name}");
                TargetMgrBase target = hitInfo.transform.GetComponentInParent<TargetMgrBase>();
                if (target != null)
                    target.TakeDamage(m_DamagePerShot);
            }

            m_FirerateTimer = m_Firerate;
            Debug.DrawRay(transform.position, transform.forward.normalized * 20.0f, Color.red, 2.0f);
        }
    }

    //private void LateUpdate()
    //{
    //    Rotate_F();
    //    m_InputVector = Vector3.zero;
    //}

    //public void AddInputVector_F(Vector3 deltaMouse)
    //{
    //    m_InputVector += deltaMouse;
    //    m_InputVector.x = Mathf.Clamp(m_InputVector.x, -1.0f, 1.0f);
    //    m_InputVector.y = Mathf.Clamp(m_InputVector.y, -1.0f, 1.0f);
    //    m_InputVector.z = Mathf.Clamp(m_InputVector.z, -1.0f, 1.0f);

    //    //Debug.Log($"Input Vector = {m_InputVector}");
    //}

    public void Rotate_F(Vector3 deltaMouse)
    {
        Vector3 rotateAmount = (new Vector3(deltaMouse.y * -1.0f, deltaMouse.x) / m_Sensitivity) * m_SensitivityMultiplier;
        //Debug.Log($"RotateAmount = {rotateAmount}");
        m_Camera.transform.localEulerAngles += rotateAmount * Time.deltaTime;
    }
}
