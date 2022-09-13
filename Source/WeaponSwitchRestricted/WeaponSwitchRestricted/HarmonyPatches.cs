using HarmonyLib;
using Verse;

namespace WeaponSwitchRestricted
{
	[StaticConstructorOnStartup]
	public static class HarmonyPatches
	{
		static HarmonyPatches()
		{
			new Harmony("flangopink.WeaponSwitchRestricted").PatchAll();
		}
	}
}
