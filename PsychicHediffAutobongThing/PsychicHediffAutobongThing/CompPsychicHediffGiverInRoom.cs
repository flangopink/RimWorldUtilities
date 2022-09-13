using RimWorld;
using Verse;

namespace PsychicHediffAutobongThing
{
	public class CompPsychicHediffGiverInRoom : ThingComp
	{
		private const int CheckInterval = 60;

		private CompProperties_PsychicHediffGiverInRoom Props => (CompProperties_PsychicHediffGiverInRoom)props;

		public override void CompTick()
		{
			if (!parent.Spawned || !parent.IsHashIntervalTick(60) || !parent.IsRitualTarget())
			{
				return;
			}
			Room room = parent.GetRoom();
			if (room == null || room.TouchesMapEdge)
			{
				return;
			}
			foreach (Pawn item in parent.Map.mapPawns.AllPawnsSpawned)
			{
				if (item.RaceProps.IsFlesh && item.needs.mood != null && item.GetStatValue(StatDefOf.PsychicSensitivity) > 0 && item.GetRoom() == room && parent.Position.InHorDistOf(item.Position, Props.minRadius))
				{
					Hediff hediff = HediffMaker.MakeHediff(Props.hediff, item);
					item.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource).TryRandomElement(out BodyPartRecord part);

					if (Props.severity > 0f)
					{
						hediff.Severity = Props.severity * item.GetStatValue(StatDefOf.PsychicSensitivity);
					}
					item.health.AddHediff(hediff, part);
					if (Props.resetLastRecreationalDrugTick && item.mindState != null)
					{
						item.mindState.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame;
					}
				}
			}
		}
	}
}
