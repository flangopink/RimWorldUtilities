using RimWorld;
using Verse;

namespace CustomAmmo
{
    public class Projectile_RandomHediffBullet : Bullet
    {
        public ThingDef_RandomHediffBullet Def => (ThingDef_RandomHediffBullet)def;

        #region Overrides
        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);

            // null checks
            if (Def != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                float roll = Rand.Value;
                foreach (HediffWithChance h in Def.Hediffs)
                {
                    if (roll < h.AddHediffChance)
                    {
                        foreach (HediffWithChance heds in Def.Hediffs)
                        {
                            if (hitPawn.health.hediffSet.HasHediff(heds.HediffToAdd))
                            {
                                Hediff hed = hitPawn.health.hediffSet.GetFirstHediffOfDef(heds.HediffToAdd);
                                hitPawn.health?.RemoveHediff(hed); 
                            }
                        }
                        Hediff hediff = HediffMaker.MakeHediff(h.HediffToAdd, hitPawn);
                        hediff.Severity = h.AddHediffSeverity;
                        hitPawn.health.AddHediff(hediff);
                        break;
                    }
                    else
                    {
                        roll -= h.AddHediffChance;
                    }
                }
            }
        }
        #endregion Overrides
    }
}
