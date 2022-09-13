using RimWorld;
using Verse;

namespace PsychicHediffAutobongThing
{
    public class CompProperties_PsychicGiveHediffSeverity : CompProperties
    {
		public HediffDef hediff;

		public float range;

		public float severityPerSecond;

		public bool drugExposure;

		public ChemicalDef chemical;

		public bool allowMechs = true;

		public CompProperties_PsychicGiveHediffSeverity()
		{
			compClass = typeof(CompPsychicGiveHediffSeverity);
		}
	}
}
