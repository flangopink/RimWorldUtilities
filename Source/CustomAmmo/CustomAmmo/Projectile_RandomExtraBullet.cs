using RimWorld;
using Verse;
using System.Collections.Generic;

namespace CustomAmmo
{
    public class Projectile_RandomExtraBullet : Bullet
    {
        public ThingDef_RandomExtraBullet Def => (ThingDef_RandomExtraBullet)def;

        #region Overrides
        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);

            // null checks
            if (Def != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                float roll = Rand.Value;
                foreach (ExtraWithChance h in Def.Extras)
                {
                    if (roll < h.AddExtraChance)
                    {
                        if (h.ExtraDamage != null)
                        {
                            DamageInfo dinfo2 = new DamageInfo(h.ExtraDamage.def, h.ExtraDamage.amount, h.ExtraDamage.AdjustedArmorPenetration(), ExactRotation.eulerAngles.y, launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, intendedTarget.Thing);
                            hitThing.TakeDamage(dinfo2).AssociateWithLog(new BattleLogEntry_RangedImpact(launcher, hitThing, intendedTarget.Thing, equipmentDef, def, targetCoverDef));
                        }

                        if (h.SetOnFire)
                        {
                            bool canSetOnFire = true;

                            if (!h.IgnoreShields)
                            {
                                if (hitPawn.apparel != null)
                                {
                                    List<Apparel> wornApparel = hitPawn.apparel.WornApparel;
                                    if (wornApparel != null)
                                    {
                                        for (int i = 0; i < wornApparel.Count; i++)
                                        {
                                            if (wornApparel[i] is ShieldBelt shield && hitPawn.Drafted)
                                            {
                                                if (shield.Energy > 0) 
                                                    canSetOnFire = false;
                                                break;
                                            }
                                            else if (!h.AffectedShields.NullOrEmpty() && h.AffectedShields.Contains(wornApparel[i].def) && hitPawn.Drafted)
                                            {
                                                canSetOnFire = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (canSetOnFire) FireUtility.TryAttachFire(hitPawn, h.FireSize);
                        }
                        break;
                    }
                    else
                    {
                        roll -= h.AddExtraChance;
                    }
                }
            }
        }
        #endregion Overrides
    }
}
