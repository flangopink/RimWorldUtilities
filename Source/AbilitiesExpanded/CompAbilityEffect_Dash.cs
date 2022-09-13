using RimWorld;
using Verse;

namespace AbilitiesExtended
{
	public class CompAbilityEffect_Dash : CompAbilityEffect_WithDest
	{
		public CompProperties_AbilityDash CompProp => (CompProperties_AbilityDash)props;

		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Map map = Caster.Map;
			DashingPawn jumpingPawn = (DashingPawn)PawnFlyer.MakeFlyer(DefDatabase<ThingDef>.GetNamed("DashingPawn"), base.CasterPawn, target.Cell);
			jumpingPawn.ability = this.parent;

			if (target.Thing == null)
				jumpingPawn.target = target.CenterVector3;
			else jumpingPawn.target = target.Thing.InteractionCell.ToVector3();

			if (CompProp.rope) jumpingPawn.rope = true;

			GenSpawn.Spawn(jumpingPawn, base.Caster.Position, map);
			base.Apply(target, dest);
		}
	}
}
