using RimWorld;
using Verse;
using UnityEngine;

namespace AbilitiesExtended
{
    public class CompProperties_AbilityDash : CompProperties_EffectWithDest
    {
        public CompProperties_AbilityDash()
        {
            compClass = typeof(CompAbilityEffect_Dash);
        }

        public bool rope;

        /*public AbilityEffectDest dest;
        public FleckDef tickFleck;
        public float tickFleckScale = 1f;
        public Color rangeColor = Color.white;*/
    }
}
