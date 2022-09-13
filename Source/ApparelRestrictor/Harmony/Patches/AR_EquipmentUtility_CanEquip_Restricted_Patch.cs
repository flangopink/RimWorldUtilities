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

namespace ApparelRestrictor.HarmonyInstance
{

    public static class AR_EquipmentUtility_CanEquip_Restricted_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Thing thing, Pawn pawn, ref string cantReason, ref bool __result)
        {
            if (__result)
            {
                if (thing.def.IsApparel || thing.def.equipmentType == EquipmentType.Primary)
                {
                    if (thing.def.HasModExtension<ApparelRestrictionDefExtension>())
                    {
                        ApparelRestrictionDefExtension defExtension = thing.def.GetModExtension<ApparelRestrictionDefExtension>();
                        if (defExtension != null)
                        {
                            bool gender = defExtension.gender == Gender.None || pawn.gender == defExtension.gender;
                            bool race = defExtension.RaceDefs.NullOrEmpty();
                            bool apparel = defExtension.ApparelDefs.NullOrEmpty();
                            bool hediff = defExtension.HediffDefs.NullOrEmpty();
                            bool trait = defExtension.TraitDefs.NullOrEmpty();
                            bool worktag = defExtension.MustBeCapableOf.NullOrEmpty();
                            bool skillreqs = defExtension.SkillRequirements.NullOrEmpty();
                            //bool memes = defExtension.MemeDefs.NullOrEmpty();

                            // Race
                            if (!defExtension.RaceDefs.NullOrEmpty())
                            {
                                race = defExtension.RaceDefs.Contains(pawn.def);
                                if (race && defExtension.Any)
                                {
                                    __result = true;
                                    return;
                                }
                                cantReason = "CantBeUsedByThisRace".Translate();
                            }

                            // Apparel
                            if (!defExtension.ApparelDefs.NullOrEmpty())
                            {
                                if (pawn.apparel?.WornApparelCount > 0)
                                {
                                    foreach (var item in defExtension.ApparelDefs)
                                    {
                                        apparel = pawn.apparel.WornApparel.Any(x => x.def == item);
                                        if (apparel)
                                        {
                                            break;
                                        }
                                    }
                                    if (apparel && defExtension.Any)
                                    {
                                        __result = true;
                                        return;
                                    }
                                }
                                cantReason = "MissingRequiredApparel".Translate();
                            }

                            // Hediffs
                            if (!defExtension.HediffDefs.NullOrEmpty())
                            {
                                hediff = pawn.health.hediffSet.hediffs.Any(x => defExtension.HediffDefs.Contains(x.def));

                                if (hediff && defExtension.Any)
                                {
                                    __result = true;
                                    return;
                                }
                                cantReason = "MissingRequiredImplant".Translate();
                            }

                            // Traits
                            if (!defExtension.TraitDefs.NullOrEmpty())
                            {
                                trait = pawn.story.traits.allTraits.Any(x => defExtension.TraitDefs.Contains(x.def));

                                if (trait && defExtension.Any)
                                {
                                    __result = true;
                                    return;
                                }
                                cantReason = "MissingRequiredTrait".Translate();
                            }

                            // Must be capable of
                            if (!defExtension.MustBeCapableOf.NullOrEmpty())
                            {
                                var incapableof = defExtension.MustBeCapableOf.FirstOrDefault(flag => pawn.story.DisabledWorkTagsBackstoryAndTraits.HasFlag(flag)); // not disabled
                                
                                //Log.Message("Incapable of: " + incapableof.ToString());

                                if (!pawn.story.DisabledWorkTagsBackstoryAndTraits.HasFlag(incapableof) && defExtension.Any)
                                {
                                    //Log.Message("Passed | Any True");
                                    __result = true;
                                    return;
                                }
                                else if (incapableof != WorkTags.None && pawn.story.DisabledWorkTagsBackstoryAndTraits.HasFlag(incapableof))
                                {
                                    //Log.Message("Not Passed");
                                    cantReason = "AbilityDisabled_IncapableOfWorkTag".Translate(pawn.Named("PAWN"), incapableof.LabelTranslated());
                                }
                                else
                                {
                                    //Log.Message("Passed | Any False");
                                    worktag = true;
                                }
                            }

                            // Skill Requirements
                            if (!defExtension.SkillRequirements.NullOrEmpty())
                            {
                                if (pawn.skills == null)
                                {
                                    __result = false; return;
                                }

                                SkillRequirement s = null;

                                for (int i = 0; i < defExtension.SkillRequirements.Count; i++)
                                {
                                    skillreqs = true;
                                    if (!defExtension.SkillRequirements[i].PawnSatisfies(pawn))
                                    {
                                        skillreqs = false;
                                        s = defExtension.SkillRequirements[i];
                                        break;
                                    }
                                }

                                if (skillreqs && defExtension.Any)
                                {
                                    __result = true;
                                    return;
                                }

                                if (!skillreqs) 
                                {
                                    cantReason = "SkillTooLow".Translate(s.skill.skillLabel, pawn.skills.GetSkill(s.skill).Level, s.minLevel);
                                }
                            }

                            /*// Memes! The DNA of the soul!
                            if (!defExtension.MemeDefs.NullOrEmpty())
                            {
                                hediff = pawn.health.hediffSet.hediffs.Any(x => defExtension.HediffDefs.Contains(x.def));

                                if (hediff && defExtension.Any)
                                {
                                    __result = true;
                                    return;
                                }
                                cantReason = "MissingRequiredImplant".Translate();
                            }*/



                            __result = gender && race && hediff && trait && apparel && worktag && skillreqs;

                            if (!__result) return;
                        }
                    }
                }

            }
        }
    }

    [HarmonyPatch(typeof(Pawn_ApparelTracker), "Notify_ApparelRemoved")]
    public static class AR_Pawn_ApparelTracker_Notify_ApparelRemoved_Restricted_Patch
    {
        [HarmonyPostfix] // Apparel apparel apparel
        public static void Notify_ApparelRemovedPostfix(Pawn_ApparelTracker __instance, Apparel apparel)
        {
            for (int i = 0; i < __instance.WornApparelCount; i++)
            {
                Apparel worn = __instance.WornApparel[i];
                if (worn.def.GetModExtension<ApparelRestrictionDefExtension>() is ApparelRestrictionDefExtension ext2)
                {
                    if (ext2.ApparelDefs.Contains(apparel.def))
                    {
                        Log.Message($"Tried to drop {worn} as it requires {apparel}");
                        __instance.TryDrop(worn, out Apparel dropped, __instance.pawn.Position.RandomAdjacentCell8Way());
                    }
                }
            }
        }
    }
}
