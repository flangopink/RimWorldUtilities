using Verse;
using RimWorld;

namespace Immunities
{
    public class CompHediffImmunities : ThingComp
    {
		public CompProperties_HediffImmunities Props => (CompProperties_HediffImmunities)props;

		public override void CompTick()
		{
			base.CompTick();
			if (parent.IsHashIntervalTick(Props.tickInterval))
			{
				TryRemoveHediff();
			}
		}

		public void TryRemoveHediff()
		{
			Pawn pawn = parent as Pawn;
			if (pawn.health == null || pawn.health.hediffSet == null)
			{
				return;
			}
			foreach (HediffDef hed in Props.hediffDefs)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hed);
				if (firstHediffOfDef != null)
				{
					pawn.health.RemoveHediff(firstHediffOfDef); 
					if (Props.throwText)
						MoteMaker.ThrowText(pawn.Position.ToVector3(), pawn.Map, "HE_Immune".Translate(hed.label), Props.textColor, Props.textDuration);

				}
			}
		}
	}
}
