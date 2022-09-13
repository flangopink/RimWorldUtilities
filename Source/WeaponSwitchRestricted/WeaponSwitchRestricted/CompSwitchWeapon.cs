using System.Collections.Generic;
using RimWorld;
using Verse;

namespace WeaponSwitchRestricted
{
    public class CompSwitchWeapon : ThingComp
	{
		private CompEquippable compEquippable;

		public Dictionary<ThingDef, Thing> generatedWeapons;

		private List<ThingDef> thingDefs;

		private List<Thing> things;

		public CompProperties_SwitchWeapon Props => props as CompProperties_SwitchWeapon;

		private CompEquippable CompEquippable
		{
			get
			{
				if (compEquippable == null)
				{
					compEquippable = parent.GetComp<CompEquippable>();
				}
				return compEquippable;
			}
		}

		public Pawn Pawn
		{
			get
			{
				if (CompEquippable.ParentHolder is Pawn_EquipmentTracker pawn_EquipmentTracker && pawn_EquipmentTracker.pawn != null)
				{
					return pawn_EquipmentTracker.pawn;
				}
				return null;
			}
		}

		public IEnumerable<Gizmo> SwitchWeaponOptions()
		{
			foreach (ThingDef weaponDef in Props.weaponsToSwitch)
			{
				yield return new Command_Action
				{
					defaultLabel = weaponDef.LabelCap,
					defaultDesc = weaponDef.LabelCap,
					activateSound = SoundDefOf.Click,
					icon = weaponDef.uiIcon,
					action = delegate
					{
						Pawn pawn = Pawn;
						if (generatedWeapons == null)
						{
							generatedWeapons = new Dictionary<ThingDef, Thing>();
						}
						if (!generatedWeapons.TryGetValue(weaponDef, out var value))
						{
							value = ThingMaker.MakeThing(weaponDef);
							generatedWeapons[weaponDef] = value;
							value.TryGetComp<CompQuality>().SetQuality(parent.TryGetComp<CompQuality>().Quality, 0);
						}
						generatedWeapons[parent.def] = parent;
						value.TryGetComp<CompSwitchWeapon>().generatedWeapons = generatedWeapons;
						pawn.equipment.Remove(parent);
						pawn.equipment.AddEquipment(value as ThingWithComps);
					}
				};
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			generatedWeapons?.Remove(parent.def);
			Scribe_Collections.Look(ref generatedWeapons, "generatedWeapons", LookMode.Def, LookMode.Deep, ref thingDefs, ref things);
		}
	}
}
