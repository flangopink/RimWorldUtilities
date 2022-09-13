using RimWorld;
using Verse;
using System.Collections.Generic;

namespace WeaponSwitchRestricted
{
    public class CompProperties_SwitchWeapon : CompProperties
	{
		public List<ThingDef> weaponsToSwitch;

		public CompProperties_SwitchWeapon()
		{
			compClass = typeof(CompSwitchWeapon);
		}
	}
}
