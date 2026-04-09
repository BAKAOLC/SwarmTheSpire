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
    public class SwarmPower : SwarmPowerTemplate
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override string CustomPackedIconPath =>
            "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override string CustomBigIconPath => "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
        {
            new("DamageDecrease", 0.79m),
        };

        public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props,
            Creature? dealer, CardModel? cardSource)
        {
            if (dealer != Owner) return 1m;
            return !props.IsPoweredAttack() ? 1m : DynamicVars["DamageDecrease"].BaseValue;
        }

        public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if ((int)side == 2) await PowerCmd.TickDownDuration(this);
        }
    }
}
