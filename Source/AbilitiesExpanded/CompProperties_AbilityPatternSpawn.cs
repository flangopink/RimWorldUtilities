using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;

namespace AbilitiesExtended
{
    public class CompProperties_AbilityPatternSpawn : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityPatternSpawn()
        {
            compClass = typeof(CompAbilityEffect_PatternSpawn);
        }

        public List<IntVec2> pattern;

        public bool despawnAffectedThings = true;
        public bool dontCareIfOccupied = false;

        public ThingDef thingToSpawn;

        public bool throwDust = true;
        public Color dustColor = new Color(0.55f, 0.55f, 0.55f, 4f);
    }
}
