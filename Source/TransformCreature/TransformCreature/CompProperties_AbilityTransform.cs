using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TransformCreature
{
    public class CompProperties_AbilityTransform : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityTransform()
        {
            compClass = typeof(CompEffect_AbilityTransform);
        }

        public List<ThingDef> canApplyTo;
        public List<TransformOutcomeOptions> transformOptions;
        public ThingDef thingToSpawn;
        public bool makeSmoke;
        public bool makeSparks;
        public bool makeGlow;
    }
}
