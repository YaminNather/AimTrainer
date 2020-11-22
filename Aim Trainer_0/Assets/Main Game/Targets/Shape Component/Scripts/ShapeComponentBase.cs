using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetStuff.ShapeComponents
{
    public abstract class ShapeComponentBase : MonoBehaviour
    {
        public abstract bool CheckForOverlaps_F();
    }
}
