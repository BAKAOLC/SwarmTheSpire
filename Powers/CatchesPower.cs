using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace SwarmTheSpire.Powers
{
    public sealed class CatchesPower : SwarmPowerTemplate
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        private bool HasMonsterPower => CombatManager.Instance.IsInProgress && Owner.HasPower<MonsterPower>();

        private bool HasElitePower => CombatManager.Instance.IsInProgress && Owner.HasPower<ElitePower>();

        private bool HasBossPower => CombatManager.Instance.IsInProgress && Owner.HasPower<BossPower>();

        public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (Amount <= 0)
                return;

            var player = Owner.Player;
            if (player is null)
                return;

            await PowerCmd.Decrement(this);

            if (HasMonsterPower)
            {
                var rewards = new PowerModel[]
                {
                    ModelDb.Power<CatchCommonCardPower>().ToMutable(0),
                    ModelDb.Power<CatchCommonPotionPower>().ToMutable(0),
                    ModelDb.Power<CatchCommonTokenPower>().ToMutable(0),
                };
                player.RunState.Rng.CombatCardGeneration.Shuffle(rewards);
                await PowerCmd.Apply(rewards[0], Owner, 1m, Owner, null);
            }

            if (HasElitePower)
                await PlayerCmd.GainGold(1m, player);

            if (HasBossPower)
                await PlayerCmd.GainStars(1m, player);
        }
    }
}
