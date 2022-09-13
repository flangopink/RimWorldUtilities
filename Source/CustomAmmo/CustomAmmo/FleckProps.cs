using RimWorld;
using Verse;
using UnityEngine;

namespace CustomAmmo
{
    public class FleckProps 
    {
        public FleckDef fleckDef;
        public int intervalTicks = 1;
        public FloatRange scaleRange = new FloatRange(1, 1); // 1~1
        public Vector3 offset = Vector3.zero;
        public bool randomRotation;
    }
}
