using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace Immunities
{
    public class CompProperties_DamageImmunities : CompProperties
    {
        public List<DamageDef> damageDefs;
        public bool throwText = true;
        public Color textColor = Color.white;
        public float textDuration = 3f;

        public CompProperties_DamageImmunities()
        {
            compClass = typeof(CompDamageImmunities);
        }
    }
}
