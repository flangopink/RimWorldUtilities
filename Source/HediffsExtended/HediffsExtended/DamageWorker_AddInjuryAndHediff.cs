using RimWorld;
using Verse;

namespace HediffsExtended
{
    public class DamageWorker_AddInjuryAndHediff : DamageWorker_AddInjury
    {
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside);
		}
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageResult result)
		{
			HediffMaker.MakeHediff(((DamageDefWithExtraHediff)def).additionalHediffOnHit, pawn, dinfo.HitPart);
		}
	}
}
