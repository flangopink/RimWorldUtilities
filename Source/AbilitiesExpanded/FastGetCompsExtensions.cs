using Verse;

namespace AbilitiesExtended
{
    public static class FastGetCompsExtensions
    {

        // Token: 0x060017F8 RID: 6136 RVA: 0x0008820C File Offset: 0x0008640C
        public static T TryGetCompFast<T>(this Thing thing) where T : ThingComp
        {
            if (!(thing is ThingWithComps thingWithComps))
            {
                return default;
            }
            return thingWithComps.GetCompFast<T>();
        }
        private static T GetCompFast<T>(this ThingWithComps thing) where T : ThingComp
        {
            var type = typeof(T);
            var comps = thing.AllComps;
            for (int i = 0, count = comps.Count; i < count; i++)
            {
                var comp = comps[i];
                if (comp.GetType() == type)
                    return (T)comp;
            }
            return null;
        }

        public static T TryGetCompFast<T>(this Hediff hd) where T : HediffComp
        {
            if (!(hd is HediffWithComps hediffWithComps))
            {
                return default;
            }
            if (hediffWithComps.comps != null)
            {
                var type = typeof(T);
                var comps = hediffWithComps.comps;
                for (int i = 0; i < comps.Count; i++)
                {
                    var comp = comps[i];
                    if (comp.GetType() == type)
                        return (T)comp;
                }
            }
            return default;
        }

    }
}
