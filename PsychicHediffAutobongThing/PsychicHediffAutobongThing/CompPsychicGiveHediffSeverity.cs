using RimWorld;
using Verse;
using System.Collections.Generic;

namespace PsychicHediffAutobongThing
{
	public class CompPsychicGiveHediffSeverity : ThingComp
	{
		private const int TickInterval = 87;

		private const float ChemicalSatisfactionPerSecond = 0.1f;

		private CompProperties_PsychicGiveHediffSeverity Props => (CompProperties_PsychicGiveHediffSeverity)props;

		private bool AppliesTo(Pawn pawn)
		{
			if (pawn.GetRoom() != parent.GetRoom())
			{
				return false;
			}
			if (!Props.allowMechs && pawn.RaceProps.IsMechanoid)
			{
				return false;
			}
			if (pawn.GetStatValue(StatDefOf.PsychicSensitivity) <= 0 || pawn.needs.mood == null)
			{
				return false;
			}
			return true;
		}

		public override void CompTick()
		{
			if (!parent.Spawned || Find.TickManager.TicksGame % 87 != 0)
			{
				return;
			}
			CompRefuelable compRefuelable = parent.TryGetComp<CompRefuelable>();
			CompPowerTrader compPowerTrader = parent.TryGetComp<CompPowerTrader>();
			if ((compRefuelable != null && !compRefuelable.HasFuel) || (compPowerTrader != null && !compPowerTrader.PowerOn))
			{
				return;
			}
			int num = GenRadial.NumCellsInRadius(Props.range);
			for (int i = 0; i < num; i++)
			{
				List<Thing> thingList = (parent.Position + GenRadial.RadialPattern[i]).GetThingList(parent.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (!(thingList[j] is Pawn pawn) || !AppliesTo(pawn))
					{
						continue;
					}
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediff);
					float num2 = Props.severityPerSecond * 1.45f * pawn.GetStatValue(StatDefOf.PsychicSensitivity); // Psych Sens scaling
					if (firstHediffOfDef != null)
					{
						firstHediffOfDef.Severity += num2;
					}
					else
					{
						pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource).TryRandomElement(out BodyPartRecord part);
						pawn.health.AddHediff(Props.hediff, part).Severity = num2;
					}
					if (!Props.drugExposure)
					{
						continue;
					}
					pawn.mindState.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame;
					if (pawn.needs?.drugsDesire != null)
					{
						pawn.needs.drugsDesire.CurLevel += 0.145000011f;
					}
					if (Props.chemical != null)
					{
						HediffDef addictionHediffDef = Props.chemical.addictionHediff;
						Need need = pawn.needs.AllNeeds.Find((Need x) => x.def == addictionHediffDef.causesNeed);
						if (need != null)
						{
							need.CurLevel += 0.145000011f;
						}
					}
				}
			}
		}
	}
}
