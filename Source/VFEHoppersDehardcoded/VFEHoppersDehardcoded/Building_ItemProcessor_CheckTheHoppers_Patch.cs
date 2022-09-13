using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using ItemProcessor;
using UnityEngine;

namespace VEFHoppersDehardcoded
{
    [HarmonyPatch(typeof(Building_ItemProcessor), "CheckTheHoppers")]
    public class Building_ItemProcessor_CheckTheHoppers_Patch
    {
		public static bool Prefix(Building_ItemProcessor __instance, string itemToCheckFor, ref int ExpectedAmountXIngredient, ref int CurrentAmountXIngredient, ref bool XIngredientComplete)
		{
			if ((__instance.compItemProcessor.Props.noPowerDestroysProgress && __instance.compPowerTrader != null && !__instance.compPowerTrader.PowerOn) || (__instance.compItemProcessor.Props.noPowerDestroysProgress && __instance.compFuelable != null && !__instance.compFuelable.HasFuel))
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < __instance.compItemProcessor.Props.inputSlots.Count; i++)
			{
				if (flag)
				{
					continue;
				}
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = (__instance.Position + __instance.compItemProcessor.Props.inputSlots[i].RotatedBy(__instance.Rotation)).GetThingList(__instance.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (DefDatabase<CombinationDef>.GetNamed(__instance.thisRecipe).isCategoryRecipe)
					{
						if (thing3.def.IsWithinCategory(ThingCategoryDef.Named(itemToCheckFor)))
						{
							thing = thing3;
						}
					}
					else if (thing3.def.defName == itemToCheckFor)
					{
						thing = thing3;
					}
					
					if (__instance.GetComp<CompAcceptedHoppers>() == null || __instance.GetComp<CompAcceptedHoppers>().Props.thingDefs.NullOrEmpty())
					{
						thing3.def = ThingDefOf.Hopper;
						thing2 = thing3;
					}
					else if (__instance.GetComp<CompAcceptedHoppers>().Props.thingDefs.Contains(thing3.def))
                    {
						thing2 = thing3;
					}
				}
				if (thing == null || thing2 == null)
				{
					continue;
				}
				flag = true;
				if (ExpectedAmountXIngredient != 0)
				{
					int num = ExpectedAmountXIngredient - CurrentAmountXIngredient;
					if (thing.stackCount - num > 0)
					{
						CurrentAmountXIngredient += num;
						if (CurrentAmountXIngredient >= ExpectedAmountXIngredient)
						{
							XIngredientComplete = true;
						}
						__instance.firstItemSwallowedForButchery = thing.def.defName;
						__instance.Notify_StartProcessing();
						thing.stackCount -= num;
						if (thing.stackCount <= 0)
						{
							thing.Destroy();
						}
					}
					else if (thing.stackCount - num <= 0 && !XIngredientComplete)
					{
						CurrentAmountXIngredient += thing.stackCount;
						__instance.firstItemSwallowedForButchery = thing.def.defName;
						__instance.Notify_StartProcessing();
						thing.Destroy();
					}
				}
				else
				{
					__instance.Notify_StartProcessing();
					thing.Destroy();
				}
			}
			return false;
		}
	}
}
