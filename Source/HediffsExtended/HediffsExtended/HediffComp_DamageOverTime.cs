using RimWorld;
using Verse;
using UnityEngine;

namespace HediffsExtended
{
    public class HediffComp_DamageOverTime : HediffComp
    {
		public int ticksCounter;

		public HediffCompProperties_DamageOverTime Props => (HediffCompProperties_DamageOverTime)props;

		public override void CompExposeData()
		{
			Scribe_Values.Look(ref ticksCounter, "ticksCounter", 0);
		}

		public override void CompPostTick(ref float severityAdjustment)
		{
			ticksCounter++;
			if (ticksCounter > Props.damageIntervalTicks)
			{
				CompHediffImmunity chi = parent.pawn.TryGetComp<CompHediffImmunity>();
				if (chi != null)
				{
					if (chi.Props.hediffDefs.Contains(this.Def))
					{
						ticksCounter = 0;
						parent.pawn.health.RemoveHediff(parent);
						if (chi.Props.throwText) 
							MoteMaker.ThrowText(parent.pawn.Position.ToVector3(), parent.pawn.Map, "HE_Immune".Translate(Def.label), chi.Props.textColor, chi.Props.textDuration);
						return;
					}
				}

				parent.pawn.TakeDamage(new DamageInfo(Props.damageDef, Props.damageAmount, Props.armorPenetration));
				ticksCounter = 0;
			}
		}

        public override void CompTended(float quality, float maxQuality, int batchPosition = 0)
        {
			parent.pawn.health.RemoveHediff(parent);
        }
    }
}
