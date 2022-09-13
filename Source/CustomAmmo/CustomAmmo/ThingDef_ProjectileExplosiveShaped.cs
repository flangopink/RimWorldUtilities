using RimWorld;
using Verse;
using System.Collections.Generic;

namespace CustomAmmo
{
    public class ThingDef_ProjectileExplosiveShaped : ThingDef
    {
        public ExplosionShape ExplosionShape = 0;
    }

    public enum ExplosionShape
    {
        Normal,
        Star,
        Ring,
        RandomAdjacent
    }
}
