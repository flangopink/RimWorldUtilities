using RimWorld;
using Verse;

namespace TransformCreature
{
    public static class FastGetCompsExtensions
    {
        public static T TryGetCompFast<T>(this Ability_Transform ab) where T : CompAbilityEffect
        {
            if (ab.comps != null)
            {
                var type = typeof(T);
                var comps = ab.comps;
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
