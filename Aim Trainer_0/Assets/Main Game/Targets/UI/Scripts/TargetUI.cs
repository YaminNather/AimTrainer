using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUI : MonoBehaviour
{
    private void Start()
    {
        m_Camera = Camera.main;
    }

    private void Update()
    {
        FaceCamera_F();
    }

    private void FaceCamera_F()
    {
        transform.LookAt(m_Camera.transform);
        transform.localRotation *= Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }


    #region Variables
    private Camera m_Camera;
    #endregion
}
