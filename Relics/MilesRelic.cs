using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Relics
{
    public class MilesRelic : SwarmRelicTemplate
    {
        private int _catchesCount;

        public override RelicRarity Rarity => RelicRarity.Starter;

        public override bool ShowCounter => true;

        public override int DisplayAmount => CatchesCount;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<MilesPower>(1m)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<MilesPower>()];

        [SavedProperty]
        public int CatchesCount
        {
            get => _catchesCount;
            set
            {
                AssertMutable();
                _catchesCount = value;
                InvokeDisplayAmountChanged();
            }
        }

        public static void TryIncrementCatch(Player player)
        {
            var relic = player.GetRelic<MilesRelic>();
            if (relic is null)
                return;

            relic.Flash();
            relic.CatchesCount++;
        }

        public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
            CombatState combatState)
        {
            if (side == Owner.Creature.Side && combatState.RoundNumber <= 1)
            {
                Flash();
                await PowerCmd.Apply<MilesPower>(combatState.HittableEnemies, DynamicVars["MilesPower"].BaseValue,
                    Owner.Creature, null);
            }
        }

        public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
        {
            if (player != Owner || room is null || CatchesCount <= 0)
                return false;

            for (var i = 0; i < CatchesCount; i++)
            {
                Flash();
                switch (room.RoomType)
                {
                    case RoomType.Monster:
                    {
                        var candidates = new List<RelicModel>
                        {
                            ModelDb.Relic<RottenFleshRelic>().ToMutable(),
                            ModelDb.Relic<BoneRelic>().ToMutable(),
                            ModelDb.Relic<StringRelic>().ToMutable(),
                        }.Where(relic => relic.IsAllowed(player.RunState)).ToList();

                        if (candidates.Count == 0)
                            break;

                        player.PlayerRng.Rewards.Shuffle(candidates);
                        rewards.Add(new RelicReward(candidates[0], player));
                        break;
                    }
                    case RoomType.Elite:
                    {
                        var candidates = new List<RelicModel>
                        {
                            ModelDb.Relic<Anchor>().ToMutable(),
                            ModelDb.Relic<HornCleat>().ToMutable(),
                            ModelDb.Relic<SwordOfStone>().ToMutable(),
                        }.Where(relic => relic.IsAllowed(player.RunState)).ToList();

                        if (candidates.Count == 0)
                            break;

                        player.PlayerRng.Rewards.Shuffle(candidates);
                        rewards.Add(new RelicReward(candidates[0], player));
                        break;
                    }
                    case RoomType.Boss:
                    {
                        var candidates = new List<RelicModel>
                        {
                            ModelDb.Relic<CaptainsWheel>().ToMutable(),
                            ModelDb.Relic<WhiteStar>().ToMutable(),
                            ModelDb.Relic<BeatingRemnant>().ToMutable(),
                            ModelDb.Relic<TungstenRod>().ToMutable(),
                            ModelDb.Relic<OldCoin>().ToMutable(),
                        }.Where(relic => relic.IsAllowed(player.RunState)).ToList();

                        if (candidates.Count == 0)
                            break;

                        player.PlayerRng.Rewards.Shuffle(candidates);
                        rewards.Add(new RelicReward(candidates[0], player));
                        break;
                    }
                }
            }

            CatchesCount = 0;
            return true;
        }

        public override async Task AfterRoomEntered(AbstractRoom room)
        {
            switch (room.RoomType)
            {
                case RoomType.Monster:
                    Flash();
                    await PowerCmd.Apply<MonsterPower>(Owner.Creature, 1m, Owner.Creature, null);
                    break;
                case RoomType.Elite:
                    Flash();
                    await PowerCmd.Apply<ElitePower>(Owner.Creature, 1m, Owner.Creature, null);
                    break;
                case RoomType.Boss:
                    Flash();
                    await PowerCmd.Apply<BossPower>(Owner.Creature, 1m, Owner.Creature, null);
                    break;
            }
        }

    }
}
