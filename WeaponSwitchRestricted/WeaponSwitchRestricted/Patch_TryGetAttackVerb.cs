using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace WeaponSwitchRestricted
{
	[HarmonyPatch(typeof(Pawn), "TryGetAttackVerb")]
	public static class Patch_TryGetAttackVerb
	{
		public static void Prefix(Pawn __instance, Thing target, bool allowManualCastWeapons = false)
		{
			if (ModLister.HasActiveModWithName("Combat Extended") || __instance.Faction == Faction.OfPlayer)
			{
				return;
			}
			CompSwitchWeapon compSwitchWeapon = __instance.equipment?.Primary.TryGetComp<CompSwitchWeapon>();
			if (compSwitchWeapon == null)
			{
				return;
			}
			if (compSwitchWeapon.generatedWeapons == null)
			{
				compSwitchWeapon.generatedWeapons = new Dictionary<ThingDef, Thing>();
			}
			foreach (ThingDef item in compSwitchWeapon.Props.weaponsToSwitch)
			{
				if (item != __instance.equipment?.Primary.def && !compSwitchWeapon.generatedWeapons.ContainsKey(item))
				{
					Thing value = ThingMaker.MakeThing(item);
					compSwitchWeapon.generatedWeapons[item] = value;
				}
			}
			if (!__instance.equipment.PrimaryEq.PrimaryVerb.CanHitTarget(target))
			{
				foreach (KeyValuePair<ThingDef, Thing> item2 in compSwitchWeapon.generatedWeapons.OrderBy((KeyValuePair<ThingDef, Thing> x) => x.Value.TryGetComp<CompEquippable>().PrimaryVerb.verbProps.range))
				{
					Verb primaryVerb = item2.Value.TryGetComp<CompEquippable>().PrimaryVerb;
					primaryVerb.caster = __instance;
					if (primaryVerb.CanHitTargetFrom(__instance.Position, target))
					{
						compSwitchWeapon.generatedWeapons[__instance.equipment.Primary.def] = __instance.equipment.Primary;
						__instance.equipment.Remove(__instance.equipment.Primary);
						__instance.equipment.AddEquipment(item2.Value as ThingWithComps);
						break;
					}
				}
				return;
			}
			if (!Rand.Chance(0.1f))
			{
				return;
			}
			foreach (KeyValuePair<ThingDef, Thing> item3 in compSwitchWeapon.generatedWeapons.InRandomOrder())
			{
				Verb primaryVerb2 = item3.Value.TryGetComp<CompEquippable>().PrimaryVerb;
				primaryVerb2.caster = __instance;
				if (primaryVerb2.CanHitTargetFrom(__instance.Position, target))
				{
					compSwitchWeapon.generatedWeapons[__instance.equipment.Primary.def] = __instance.equipment.Primary;
					__instance.equipment.Remove(__instance.equipment.Primary);
					__instance.equipment.AddEquipment(item3.Value as ThingWithComps);
					break;
				}
			}
		}
	}

}
