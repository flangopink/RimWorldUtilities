using HarmonyLib;
using RimWorld;
using Verse;
using System;

namespace WeaponSwitchRestricted
{
	[HarmonyPatch(typeof(Verb_LaunchProjectile), "Available")]
	public static class Patch_Verb_LaunchProjectile_Available
	{
		[HarmonyPrefix]
		public static bool Prefix(Verb_LaunchProjectile __instance, ref Thing ___caster)
		{
			if (__instance.EquipmentSource.def.HasComp(typeof(CompSwitchWeapon)))
            {
				return EquipmentUtility.CanEquip(__instance.EquipmentSource, (Pawn)___caster);
            }
			return true;
		}
	}

}
