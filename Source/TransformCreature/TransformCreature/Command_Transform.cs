using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace TransformCreature
{
    public class Command_Transform : Command_Ability
    {
        public Command_Transform(Ability_Transform ability) : base(ability)
        {
        }

        public int curTicks = -1;
        public new Ability_Transform Ability => (Ability_Transform)base.ability;
    }
}
