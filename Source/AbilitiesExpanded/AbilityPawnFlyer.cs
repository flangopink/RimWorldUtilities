using System.Collections;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace AbilitiesExtended
{
    public class AbilityPawnFlyer : PawnFlyer
    {
		public Ability ability;

		protected Vector3 position;

		public Vector3 target;

		public Rot4 direction;

		public bool pawnCanFireAtWill = true;

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			direction = ((startVec.x > (float)target.ToIntVec3().x) ? Rot4.West : ((startVec.x < (float)target.ToIntVec3().x) ? Rot4.East : ((startVec.y < (float)target.ToIntVec3().y) ? Rot4.North : Rot4.South)));
		}

		public override void Tick()
		{
			float num = (float)ticksFlying / (float)ticksFlightTime;

			position = Vector3.Lerp(startVec, target, num); // removed the parabola here

			var pairs = base.FlyingPawn.Drawer.renderer.effecters.pairs;
			foreach (PawnStatusEffecters.LiveEffecter p in pairs)
				p.effecter.EffectTick(new TargetInfo(position.ToIntVec3(), MapHeld), TargetInfo.Invalid);

			base.Tick();
		}

		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.FlyingPawn.Drawer.renderer.RenderPawnAt(position, direction);
		}

		protected override void RespawnPawn()
		{
			base.Position = target.ToIntVec3();
			base.RespawnPawn();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look(ref ability, "ability");
			Scribe_Values.Look(ref direction, "direction");
		}
	}
}
