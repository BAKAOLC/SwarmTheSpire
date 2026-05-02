using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SwarmTheSpire.Powers
{
    public sealed class PipesPower : SwarmPowerTemplate
    {
        public override string CustomPackedIconPath =>
            "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override string CustomBigIconPath => "res://SwarmTheSpire/images/powers/SwarmTheSpire-miles_power.png";

        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Single;

        public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext,
            ICombatState combatState)
        {
            Flash();
            await PowerCmd.Apply<RingingPower>(choiceContext, Owner, 1m, Owner, null);
            await PowerCmd.Remove(this);
        }
    }
}
