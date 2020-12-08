using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomUI.Bar
{
    public interface IBar
    {
        float GetValue_F();
        void SetValue_F(float value);

        MinMax<float> GetValueRange_F();
        void SetValueRange_F(MinMax<float> valueRange);

        void Refill_F();
        void Refresh_F();
    }
}
