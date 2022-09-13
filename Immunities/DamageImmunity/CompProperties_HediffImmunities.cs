using UnityEngine;
using Verse;
using System.Collections.Generic;

namespace Immunities
{
    public class CompProperties_HediffImmunities : CompProperties
    {
        public List<HediffDef> hediffDefs;
        public bool throwText = true;
        public Color textColor = Color.white;
        public float textDuration = 3f;
        public int tickInterval = 60;

        public CompProperties_HediffImmunities()
        {
            compClass = typeof(CompHediffImmunities);
        }
    }
}
