using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Powers
{
    public sealed class NeuroFansPower : SwarmPowerTemplate
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override string CustomPackedIconPath => Const.Paths.SharedPowerIcon;

        public override string CustomBigIconPath => Const.Paths.SharedPowerIcon;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DynamicVar("DamageDecrease", 0.79m)];

        public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props,
            Creature? dealer, CardModel? cardSource)
        {
            if (dealer != Owner)
                return 1m;

            if (!props.IsPoweredAttack())
                return 1m;

            return DynamicVars["DamageDecrease"].BaseValue;
        }

        public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side == CombatSide.Enemy)
                await PowerCmd.TickDownDuration(this);
        }
    }
}
