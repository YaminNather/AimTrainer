using System.Collections;
using System.Collections.Generic;
using Ludiq.PeekCore.FullSerializer;
using UnityEngine;

public class PlayerMgr : MonoBehaviour
{
    #region Variables
    private Camera m_Camera;
    private CharacterController m_CharacterController;
    [SerializeField] private float m_MovementSpeed = 5.0f;

    private AimComponent m_AimComponent;
    private Vector3 m_CameraOffset;
    #endregion

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        
        m_Camera = GetComponentInChildren<Camera>();
        m_Camera.transform.parent = null;
        m_CameraOffset = m_Camera.transform.position - transform.position;
        
        m_AimComponent = m_Camera.GetComponent<AimComponent>();
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 deltaMousePos = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0.0f);
        m_AimComponent.Rotate_F(deltaMousePos);

        transform.rotation = Quaternion.Euler(0.0f, m_Camera.transform.eulerAngles.y, 0.0f);

        Vector3 deltaPos = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        deltaPos *= m_MovementSpeed;
        m_CharacterController.Move(deltaPos * Time.deltaTime);
    }

    private void LateUpdate()
    {
        m_Camera.transform.position = transform.position + m_CameraOffset;
    }
}
