using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AbilitiesExtended
{
    // Token: 0x02000024 RID: 36
    public class VerbProperties_EquipmentAbility : VerbProperties
    {
        public bool RapidFire = false;

        public bool TyranidBurstBodySize = false;

        public bool GetsHot = false;
        public bool HotDamageWeapon = false;
        public float HotDamage = 0f;
        public bool GetsHotCrit = false;
        public float GetsHotCritChance = 0f;
        public bool GetsHotCritExplosion = false;
        public float GetsHotCritExplosionChance = 0f;

        public bool TwinLinked = false;
        public bool Multishot = false;

        public bool EffectsUser = false;
        public float EffectsUserChance = 0f;
        public StatDef ResistEffectStat = null;
        public HediffDef UserEffect = null;
        public bool Rending = false;
        public float RendingChance = 0.167f;

        public DamageDef ForceWeaponEffect = null;
        public HediffDef ForceWeaponHediff = null;
        public float ForceWeaponKillChance = 0f;
        public SoundDef ForceWeaponTriggerSound = null;

        public int ScattershotCount = 0;
        public ResearchProjectDef requiredResearch = null;
        public VerbProperties VerbProps;
        public List<string> UserEffectImmuneList = new List<string>();
    }
}
