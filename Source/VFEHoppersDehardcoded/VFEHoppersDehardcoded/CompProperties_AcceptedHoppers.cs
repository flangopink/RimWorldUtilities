using RimWorld;
using Verse;
using System.Collections.Generic;

namespace VEFHoppersDehardcoded
{
    public class CompProperties_AcceptedHoppers : CompProperties
    {
        public List<ThingDef> thingDefs;

        public CompProperties_AcceptedHoppers() => compClass = typeof(CompAcceptedHoppers);
    }
}
