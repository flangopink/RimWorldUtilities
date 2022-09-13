using RimWorld;
using System;
using System.Linq;
using Verse;

namespace AbilitiesExtended
{
	public static class AbilityTracker_Extentions
	{
        public static void TryGainEquipmentAbility(this Pawn_AbilityTracker tracker, AbilityDef abilityDef, ThingWithComps thing)
		{
            if (abilityDef is EquipmentAbilityDef def)
            {
                if (!(tracker.abilities.FirstOrFallback(x => x.def == def && x is EquipmentAbility y && y.sourceEquipment == thing) is EquipmentAbility ab))
                {
                    ab = Activator.CreateInstance(def.abilityClass, new object[]
                    {
                    tracker.pawn,
                    def,
                    thing
                    }) as EquipmentAbility;
                    ab.sourceEquipment = thing;
                    tracker.abilities.Add(ab);
                    tracker.Notify_TemporaryAbilitiesChanged();
                }
            }
		}

		public static void TryRemoveEquipmentAbility(this Pawn_AbilityTracker tracker, AbilityDef def, ThingWithComps thing)
		{
            if (!(def is EquipmentAbilityDef))
            {
				return;
            }
            if (tracker.abilities.FirstOrFallback(x => x.def == def && x is EquipmentAbility y && y.sourceEquipment == thing) is EquipmentAbility ab)
            {
                tracker.abilities.Remove(ab);
                tracker.Notify_TemporaryAbilitiesChanged();
            }
        }

		public static void GainAbility(this Pawn_AbilityTracker tracker, AbilityDef def, Thing source)
		{
			if (!tracker.abilities.Any((Ability a) => a.def == def))
            {
                tracker.abilities.Add(Activator.CreateInstance(def.abilityClass, new object[]
				{
					tracker.pawn,
					def,
					source
				}) as Ability);
            }
		}

	}
}
