using RimWorld;
using Verse;

namespace HediffsExtended
{
    public class HediffCompProperties_DamageOverTime : HediffCompProperties
    {
		public int damageIntervalTicks = 50;

		public float damageAmount = 1f;

		public float armorPenetration = 0f;

		public DamageDef damageDef;

		public HediffCompProperties_DamageOverTime()
		{
			compClass = typeof(HediffComp_DamageOverTime);
		}
	}
}
