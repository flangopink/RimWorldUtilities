using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace TransformCreature
{
    public class Ability_Transform : Ability
    {
        Ability_TransformDef AbilityDef => (Ability_TransformDef)this.def;

        public Ability_Transform() : base() {}
        public Ability_Transform(Pawn pawn) : base(pawn) {}
        public Ability_Transform(Pawn pawn, AbilityDef def) : base(pawn, def) {}
        public Ability_Transform(Pawn pawn, Precept sourcePrecept) : base(pawn, sourcePrecept) {}
        public Ability_Transform(Pawn pawn, Precept sourcePrecept, AbilityDef def) : base(pawn, sourcePrecept, def) {}

        public Texture2D OptionsIcon => ContentFinder<Texture2D>.Get(AbilityDef.optionsIconPath);
        public Texture2D ResetIcon => ContentFinder<Texture2D>.Get("UI/ResetIcon");

        public int MaxCastingTicks => (int)(AbilityDef.cooldown * GenTicks.TicksPerRealSecond);
        private int TicksUntilCasting = -5;
        public int CooldownTicksLeft
        {
            get => TicksUntilCasting;
            set => TicksUntilCasting = value;
        } //Log.Message(value.ToString()); } }

        public override void ExposeData()
        {
            Scribe_Defs.Look<AbilityDef>(ref this.def, "def");
            if (this.def == null)
            {
                return;
            }
            Scribe_Values.Look<int>(ref this.Id, "Id", -1, false);
            if (Scribe.mode == LoadSaveMode.LoadingVars && this.Id == -1)
            {
                this.Id = Find.UniqueIDsManager.GetNextAbilityID();
            }
            Scribe_References.Look<Precept>(ref this.sourcePrecept, "sourcePrecept", false);
            /*Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
            {
                this
            });
            Scribe_Values.Look<int>(ref this.cooldownTicks, "cooldownTicks", 0, false);
            Scribe_Values.Look<int>(ref this.cooldownTicksDuration, "cooldownTicksDuration", 0, false);*/
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                this.Initialize();
            }
            Scribe_Values.Look(ref TicksUntilCasting, "EquipmentAbilityTicksUntilcasting", -5);
        }

        public override IEnumerable<Command> GetGizmos()
        {
            if (gizmo == null)
            {
                //var command = (Command_EquipmentAbility)Activator.CreateInstance(def.gizmoClass, this);
                var command = new Command_Transform(this)
                {
                    defaultLabel = AbilityDef.LabelCap,
                    order = def.uiOrder,
                    curTicks = CooldownTicksLeft
                };

                if (!CanCastPowerCheck("Player", out string reason))
                    command.Disable(reason);
                this.gizmo = command;

                if (this.CooldownTicksLeft == -5) // Min is -1, but this will do a one-time cooldown upon equipping
                {
                    this.CooldownTicksLeft = this.MaxCastingTicks;
                    this.StartCooldown(MaxCastingTicks);
                    this.gizmo.Disable("AbilityOnCooldown".Translate(this.TicksUntilCasting.ToStringSecondsFromTicks()));
                }
            }
            yield return gizmo;

            // Swapping ability with another from floating menu
            if (this.TryGetCompFast<CompEffect_AbilityTransform>().Props.transformOptions is List<TransformOutcomeOptions> options && options.Count > 1)
            {
                yield return new Command_Action()
                {
                    defaultLabel = "TransformOptions".Translate(),
                    defaultDesc = "TransformOptionsDesc".Translate(),
                    icon = OptionsIcon,
                    order = def.uiOrder + 1,
                    action = () =>
                    {
                        List<FloatMenuOption> list = new List<FloatMenuOption>();

                        foreach(var option in options)
                        {
                            list.Add(new FloatMenuOption(option.label, delegate
                            {
                                gizmo.defaultLabel = option.label;
                                gizmo.icon = option.Icon;
                                this.TryGetCompFast<CompEffect_AbilityTransform>().Props.thingToSpawn = option.thingDef;
                            }));
                        }
                        Find.WindowStack.Add(new FloatMenu(list));
                    }
                };
            }

            // Reset cooldown if devmode is on
            if (Prefs.DevMode && this.CooldownTicksLeft > 0)
            {
                Command_Action command_Action = new Command_Action
                {
                    defaultLabel = "DEV: Reset cooldown",
                    order = def.uiOrder + 2,
                    icon = ResetIcon,
                    action = delegate
                    {
                        CooldownTicksLeft = 0;
                        this.StartCooldown(0);
                    }
                };
                yield return command_Action;
            }

        }

        public virtual bool CanCastPowerCheck(string context, out string reason)
        {
            reason = "";

            if (context == "Player" && this.pawn.Faction != Faction.OfPlayer)
            {
                reason = "CannotOrderNonControlled".Translate();
                return false;
            }
            if (this.pawn.story.DisabledWorkTagsBackstoryAndTraits.HasFlag(WorkTags.Violent) && AbilityDef.verbProperties.violent)
            {
                reason = "AbilityDisabled_IncapableOfWorkTag".Translate(this.pawn.Named("PAWN"), WorkTags.Violent.LabelTranslated());
                return false;
            }
            if (CooldownTicksLeft > 0)
            {
                reason = "AU_PawnAbilityRecharging".Translate(this.pawn.NameShortColored);
                return false;
            }
            //else if (!Verb.CasterPawn.drafter.Drafted)
            //{
            //    reason = "IsNotDrafted".Translate(new object[]
            //    {
            //        Verb.CasterPawn.Name.ToStringShort
            //    });
            //}

            return true;
        }

        public override void QueueCastingJob(LocalTargetInfo target, LocalTargetInfo destination)
        {
            base.QueueCastingJob(target, destination);
            CooldownTicksLeft = MaxCastingTicks;
            this.StartCooldown(MaxCastingTicks);
        }

        public override void AbilityTick()
        {
            if (RimWorld.Planet.WorldPawnsUtility.IsWorldPawn(pawn)) return;

            base.AbilityTick();

            if (CooldownTicksLeft > -1 && !Find.TickManager.Paused)
            {
                CooldownTicksLeft--;
                //Log.Error("fuck you");

                if (!this.gizmo.disabled)
                {
                    this.gizmo.Disable("AbilityOnCooldown".Translate(this.CooldownTicksLeft.ToStringSecondsFromTicks()));
                }
            }
            else
            {
                if (!Find.TickManager.Paused)
                {
                    if (this.gizmo != null)
                    {
                        if (this.gizmo.disabled)
                        {
                            this.gizmo.disabled = false;
                        }
                    }
                }
            }
        }
    }
}
