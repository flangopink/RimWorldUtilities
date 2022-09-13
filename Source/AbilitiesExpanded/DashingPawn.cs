using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace AbilitiesExtended
{
	[StaticConstructorOnStartup]
	public class DashingPawn : AbilityPawnFlyer
	{
		public bool rope;
		private static readonly string RopeTexPath = "UI/Overlays/Rope";
		private static readonly Material RopeLineMat = MaterialPool.MatFrom(RopeTexPath, ShaderDatabase.Transparent, GenColor.FromBytes(99, 70, 41));

		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.FlyingPawn.DrawAt(GetDrawPos(), flip);

			if (rope && position != Vector3.zero) GenDraw.DrawLineBetween(position, target, AltitudeLayer.PawnRope.AltitudeFor(), RopeLineMat);
		}

		public override void Tick()
		{
			base.Tick();

			if (base.MapHeld != null && Find.TickManager.TicksGame % 2 == 0)
			{
				Map map = base.MapHeld;
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(GetDrawPos(), map, FleckDefOf.DustPuffThick, 1f);
				dataStatic.rotation = Rand.Range(0f, 360f);
				map.flecks.CreateFleck(dataStatic);
			}
		}

		private Vector3 GetDrawPos()
		{
			float num = (float)ticksFlying / (float)ticksFlightTime;
			Vector3 vector = position;
			//vector.y = AltitudeLayer.Skyfaller.AltitudeFor();

			return vector + Vector3.forward * (num - Mathf.Pow(num, 2f)) * 1.5f;// * 15f;
		}

		protected override void RespawnPawn()
		{
			Pawn flyingPawn = base.FlyingPawn;
			if (flyingPawn.drafter != null)
			{
				pawnCanFireAtWill = flyingPawn.drafter.FireAtWill;
			}
			base.RespawnPawn();
			DefDatabase<SoundDef>.GetNamed("Dash_Whoosh").PlayOneShot(flyingPawn);

			//FleckMaker.ThrowSmoke(flyingPawn.DrawPos, flyingPawn.Map, 1f);
			FleckMaker.ThrowDustPuffThick(flyingPawn.DrawPos, flyingPawn.Map, 2f, new Color(1f, 1f, 1f, 2.5f));
			flyingPawn.meleeVerbs.TryMeleeAttack(new LocalTargetInfo(target.ToIntVec3()).Pawn, null, surpriseAttack: true);


			//GenExplosion.DoExplosion(target.ToIntVec3(), flyingPawn.Map, 50, DamageDefOf.Flame, flyingPawn);
		}
	}

}
