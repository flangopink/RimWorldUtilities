using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;

namespace CustomAmmo
{
    public class CompProperties_ProjectileEffects : CompProperties
    {
        public bool rotating;
        public bool counterClockwise = true;
        public float rotationSpeed = 5f;

        public bool emitFlecks;
        public List<FleckProps> flecks;

        public CompProperties_ProjectileEffects()
        {
            compClass = typeof(CompProjectileEffects);
        }
    }

    public class CompProjectileEffects : ThingComp
    {
        public CompProperties_ProjectileEffects Props => (CompProperties_ProjectileEffects)props;

        public override void CompTick()
        {
            base.CompTick();

            if (Props.emitFlecks)
            {
                foreach (FleckProps fleck in Props.flecks)
                {
                    if (parent.Map != null && Find.TickManager.TicksGame % fleck.intervalTicks == 0)
                    {
                        Map map = parent.Map;
                        FleckCreationData dataStatic = FleckMaker.GetDataStatic(parent.DrawPos + fleck.offset, map, fleck.fleckDef, fleck.scaleRange.RandomInRange);
                        if (fleck.randomRotation) dataStatic.rotation = Rand.Range(0f, 360f);
                        map.flecks.CreateFleck(dataStatic);
                    }
                }
            }
        }
    }
}
