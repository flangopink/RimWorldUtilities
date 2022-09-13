using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AbilitiesExtended
{
    public class CompAbilityEffect_Shoot : CompAbilityEffect_WithDest
    {
        public new CompProperties_EffectWithDest Props => (CompProperties_EffectWithDest)props;
    }
}
