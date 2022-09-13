using System.Collections.Generic;
using Verse;

namespace CustomAmmo
{
    public class ExtraWithChance
    {
        /*public DamageDef DamageDef;
        public float Damage = 10;
        public float ArmorPenetration = -1;*/

        public float AddExtraChance = 0.5f;

        public ExtraDamage ExtraDamage;
        // <ExtraDamage>
        //   <def>Burn</def>
        //   <amount>10</amount>
        //   <armorPenetration>-1</armorPenetration>
        // </ExtraDamage>

        public bool SetOnFire;
        public float FireSize = 0.5f;

        public bool IgnoreShields = true;
        public List<ThingDef> AffectedShields;
    }
}
