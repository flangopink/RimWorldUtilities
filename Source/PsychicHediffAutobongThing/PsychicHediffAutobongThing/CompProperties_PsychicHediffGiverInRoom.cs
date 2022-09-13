using Verse;

namespace PsychicHediffAutobongThing
{
    public class CompProperties_PsychicHediffGiverInRoom : CompProperties
    {
        public HediffDef hediff;

        public float minRadius = 999f;

        public float severity = -1f;

        public bool resetLastRecreationalDrugTick;

        public CompProperties_PsychicHediffGiverInRoom()
        {
            compClass = typeof(CompPsychicGiveHediffSeverity);
        }
    }
}
