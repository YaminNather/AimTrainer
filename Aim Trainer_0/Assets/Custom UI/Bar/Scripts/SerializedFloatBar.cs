using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CustomUI.Bar
{
    public partial class SerializedFloatBar : MonoBehaviour, IBar
    {
        private void Awake()
        {
            setupUI_F();

            if (m_Value == null)
                SetSharedValue_F(null);

            Refresh_F();

            void setupUI_F()
            {
                RectTransform rTrans = m_BackgroundImage.GetComponent<RectTransform>();
                if (m_BackgroundImage != null)
                    rTrans.pivot = rTrans.pivot.With(x: 0.0f);
                else
                    Debug.LogError("Background Image not Available for SerializedFloatBar", gameObject);

                if (m_ValueImage != null)
                    rTrans.pivot = rTrans.pivot.With(x: 0.0f);
                else
                    Debug.LogError("Value Image not Available for SerializedFloatBar", gameObject);

            }
        }

        private void OnValueChanged_EF(float value)
        {
            Refresh_F();
        }

        public void Refresh_F()
        {
            float multiplier;
            if (m_Value != null)
            {
                multiplier = Mathf.InverseLerp(m_ValueRange.GetMin_F(), m_ValueRange.GetMax_F(),
                    m_Value.GetValue_F());
            }
            else
                multiplier = 0.5f;

            m_ValueImage.transform.localScale = new Vector3(multiplier, 1.0f, 1.0f);
        }

        public void Refill_F()
        {
            m_Value.SetValue_F(m_ValueRange.GetMax_F());
            Refresh_F();
        }

        private void OnDestroy()
        {
            if (m_Value != null)
            {
                m_Value.m_OnChangedE -= OnValueChanged_EF;
                Destroy(m_Value);
            }
        }


        #region Variables
        [SerializeField] Image m_BackgroundImage;
        [SerializeField] private Image m_ValueImage;
        [SerializeField] private SerializedFloat m_Value;
        [SerializeField] private MinMax<float> m_ValueRange;
        #endregion
        
        public SerializedFloat GetSharedValue_F() => m_Value;
        public void SetSharedValue_F(SerializedFloat serializedFloat)
        {
            if (m_Value != null)
            {
                m_Value.m_OnChangedE -= OnValueChanged_EF;
                Destroy(m_Value);
            }

            if (serializedFloat == null)
                serializedFloat = ScriptableObject.CreateInstance<SerializedFloat>();

            m_Value = serializedFloat;
            m_Value.m_OnChangedE += OnValueChanged_EF;

            Refresh_F();
        }
        
        public float GetValue_F() => m_Value.GetValue_F();
        public void SetValue_F(float value) { }

        public MinMax<float> GetValueRange_F() => m_ValueRange;
        public void SetValueRange_F(MinMax<float> valueRange) => m_ValueRange = valueRange;
        public void SetMinValueRange_F(float value)
        {
            if (value > m_ValueRange.GetMax_F())
                value = m_ValueRange.GetMax_F();
            
            m_ValueRange.SetMin_F(value);
        }
        public void SetMaxValueRange_F(float value)
        {
            if (value < m_ValueRange.GetMin_F())
                value = m_ValueRange.GetMin_F();
            m_ValueRange.SetMax_F(value);
        }
    }


#if UNITY_EDITOR
    public partial class SerializedFloatBar : MonoBehaviour, IBar
    {
        [MenuItem("GameObject/Custom/UI/Bar/SerializedFloat Bar", false, priority = 10)]
        private static void CreateBar_F(MenuCommand menuCommand)
        {
            //GameObject gObj = new GameObject("Serialized Bar", new Type[]
            //{
            //    typeof(RectTransform), typeof(CanvasRenderer), typeof()
            //});

            Debug.Log($"Context = {menuCommand.context}");
            GameObject gObj =
                Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Custom UI/Bar/Prefabs/Bar.prefab"));
            GameObjectUtility.SetParentAndAlign(gObj, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(gObj, "Created Bar");
        }

        private void OnValidate()
        {
            Debug.Log("OnValidate called", gameObject);

            m_Value?.SetValue_F(m_ValueRange.ClampWithinRange_F(m_Value.GetValue_F()));

            if (m_BackgroundImage != null && m_ValueImage != null)
            {
                RectTransform valueImageRTrans = m_ValueImage.GetComponent<RectTransform>();
                RectTransform backgroundImageRTrans = m_BackgroundImage.GetComponent<RectTransform>();
                backgroundImageRTrans.pivot = valueImageRTrans.pivot = backgroundImageRTrans.pivot.With(x: 0.0f);
                valueImageRTrans.position = backgroundImageRTrans.position;
                valueImageRTrans.anchorMax = backgroundImageRTrans.anchorMax;
                valueImageRTrans.anchorMin = backgroundImageRTrans.anchorMin;

                Refresh_F();
            }

        }
    }
#endif
}

