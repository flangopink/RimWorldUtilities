using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld.Planet;
using System.Text;

namespace AbilitiesExtended
{
    public class Command_EquipmentPsycast : Command_Ability
    {
        public Command_EquipmentPsycast(EquipmentAbility ability) : base(ability)
        {
            shrinkable = true;
        }

        public int curTicks = -1;
        public new EquipmentAbility ability => (EquipmentAbility)base.ability;

        public override string Label
        {
            get
            {
                if (ability.pawn.IsCaravanMember())
                {
                    Pawn pawn = ability.pawn;
                    Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
                    StringBuilder stringBuilder = new StringBuilder(base.Label + " (" + pawn.LabelShort);
                    if (ability.def.PsyfocusCost > float.Epsilon)
                    {
                        stringBuilder.Append(", " + "PsyfocusLetter".Translate() + ":" + psychicEntropy.CurrentPsyfocus.ToStringPercent("0"));
                    }

                    if (ability.def.EntropyGain > float.Epsilon)
                    {
                        if (ability.def.PsyfocusCost > float.Epsilon)
                        {
                            stringBuilder.Append(",");
                        }

                        stringBuilder.Append((string)(" " + "NeuralHeatLetter".Translate() + ":") + Mathf.Round(psychicEntropy.EntropyValue));
                    }

                    stringBuilder.Append(")");
                    return stringBuilder.ToString();
                }

                return base.Label;
            }
        }

        public override string TopRightLabel
        {
            get
            {
                AbilityDef def = ability.def;
                string text = "";
                if (def.EntropyGain > float.Epsilon)
                {
                    text += "NeuralHeatLetter".Translate() + ": " + def.EntropyGain.ToString() + "\n";
                }

                if (def.PsyfocusCost > float.Epsilon)
                {
                    string text2 = "";
                    text2 = ((!def.AnyCompOverridesPsyfocusCost) ? def.PsyfocusCostPercent : ((!(def.PsyfocusCostRange.Span > float.Epsilon)) ? def.PsyfocusCostPercentMax : (def.PsyfocusCostRange.min * 100f + "-" + def.PsyfocusCostPercentMax)));
                    text += "PsyfocusLetter".Translate() + ": " + text2;
                }

                return text.TrimEndNewlines();
            }
        }

        protected override void DisabledCheck()
        {
            AbilityDef def = ability.def;
            Pawn pawn = ability.pawn;
            disabled = false;
            if (def.EntropyGain > float.Epsilon)
            {
                if (pawn.GetPsylinkLevel() < def.level)
                {
                    DisableWithReason("CommandPsycastHigherLevelPsylinkRequired".Translate(def.level));
                }
                else if (pawn.psychicEntropy.WouldOverflowEntropy(def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(pawn)))
                {
                    DisableWithReason("CommandPsycastWouldExceedEntropy".Translate(def.label));
                }
            }

            base.DisabledCheck();
        }
    }
}
