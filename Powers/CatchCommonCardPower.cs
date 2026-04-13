using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace SwarmTheSpire.Powers
{
    public sealed class CatchCommonCardPower : SwarmPowerTemplate
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
        {
            if (player != Owner.Player || room?.RoomType != RoomType.Monster)
                return false;

            for (var i = 0; i < Amount; i++)
            {
                rewards.Add(new CardReward(CardCreationOptions.ForRoom(Owner.Player, RoomType.Monster), 3, Owner.Player));
            }

            return true;
        }
    }
}
