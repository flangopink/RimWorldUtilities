using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace HediffsExtended
{
    public class CompProperties_HediffImmunities : CompProperties
    {
        public List<HediffDef> hediffDefs;
        public bool throwText = true;
        public Color textColor = Color.white;
        public float textDuration = 3f;
        public CompProperties_HediffImmunities()
        {
            compClass = typeof(CompHediffImmunity);
        }
    }
}
