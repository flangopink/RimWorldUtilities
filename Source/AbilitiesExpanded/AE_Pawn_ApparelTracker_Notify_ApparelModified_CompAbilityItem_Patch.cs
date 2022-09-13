using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;

namespace AbilitiesExtended.HarmonyInstance
{

    [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelRemoved")]
    public static class AE_Pawn_ApparelTracker_Notify_ApparelRemoved_CompAbilityItem_Patch
    {
        [HarmonyPostfix] // Apparel apparel
        public static void Notify_ApparelRemovedPostfix(Pawn_EquipmentTracker __instance, Apparel apparel)
        {
            bool abilityitem = apparel.TryGetCompFast<AbilitiesExtended.CompAbilityItem>() != null;
            if (abilityitem)
            {
                Pawn pawn = __instance.pawn;
                if (!pawn.RaceProps.Humanlike)
                {
                    return;
                }
                foreach (AbilitiesExtended.CompAbilityItem compAbilityItem in apparel.GetComps<AbilitiesExtended.CompAbilityItem>())
                {
                    foreach (AbilityDef abilityDef in compAbilityItem.Props.Abilities)
                    {
                        __instance.pawn.abilities.TryRemoveEquipmentAbility(abilityDef, apparel);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelAdded")]
    public static class AE_Pawn_ApparelTracker_Notify_ApparelAdded_CompAbilityItem_Patch
    {
        [HarmonyPostfix] // Apparel apparel
        public static void Notify_Notify_ApparelAddedPostfix(Pawn_EquipmentTracker __instance, Apparel apparel)
        {
            if (apparel.TryGetCompFast<CompAbilityItem>() != null && apparel.TryGetCompFast<CompAbilityItem>() is CompAbilityItem abilityItem)
            {
                Pawn pawn = __instance.pawn;
                if (!pawn.RaceProps.Humanlike)
                {
                    return;
                }
                if (!abilityItem.Props.Abilities.NullOrEmpty())
                {
                    //bool dirty = false;
                    foreach (EquipmentAbilityDef def in abilityItem.Props.Abilities)
                    {
                        if (!__instance.pawn.abilities.abilities.Any(x => x.def == def))
                        {
                            __instance.pawn.abilities.TryGainEquipmentAbility(def, apparel);
                        }
                    }
                }
            }
        }
    }

}
