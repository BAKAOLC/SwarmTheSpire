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
using STS2RitsuLib.Interop.AutoRegistration;
using SwarmTheSpire.Character;
using SwarmTheSpire.Data;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Relics
{
    [RegisterCharacterStarterRelic(typeof(EvilCharacter))]
    [RegisterTouchOfOrobasRefinement(typeof(WeAreMilesRelic))]
    public class MilesRelic : SwarmRelicTemplate
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        public override bool ShowCounter => true;

        public override int DisplayAmount =>
            CatchesData.Instance.GlobalCatchesCount + CatchesData.Instance.CurrentCombatCatches;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<MilesPower>(1m)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<MilesPower>()];

        [SavedProperty]
        public int SavedCatchCount { get; set; }

        public static void TryIncrementCatch(Player player)
        {
            CatchesData.Instance.CurrentCombatCatches++;
            var relic = player.GetRelic<MilesRelic>();
            relic?.Flash();
            relic?.InvokeDisplayAmountChanged();
        }

        public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
            ICombatState combatState)
        {
            if (side != Owner.Creature.Side || combatState.RoundNumber > 1)
                return;

            CatchesData.Instance.CurrentCombatCatches = 0;
            InvokeDisplayAmountChanged();
            Flash();
            await PowerCmd.Apply<MilesPower>(choiceContext, combatState.HittableEnemies,
                DynamicVars["MilesPower"].BaseValue, Owner.Creature, null);
        }

        public override async Task AfterObtained()
        {
            CatchesData.Instance.GlobalCatchesCount = 0;
            InvokeDisplayAmountChanged();
        }

        public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
        {
            if (player != Owner || room is null)
                return false;

            var currentCombatCatches = CatchesData.Instance.CurrentCombatCatches;
            if (currentCombatCatches <= 0)
                return false;

            CatchesData.Instance.GlobalCatchesCount += currentCombatCatches;
            CatchesData.Instance.CurrentCombatCatches = 0;
            SavedCatchCount = CatchesData.Instance.GlobalCatchesCount;
            InvokeDisplayAmountChanged();

            for (var i = 0; i < currentCombatCatches; i++)
            {
                var list = new List<RelicModel>();
                switch (room.RoomType)
                {
                    case RoomType.Monster:
                        list.AddRange(
                        [
                            ModelDb.Relic<RottenFleshRelic>().ToMutable(),
                            ModelDb.Relic<BoneRelic>().ToMutable(),
                            ModelDb.Relic<StringRelic>().ToMutable(),
                            ModelDb.Relic<TropicalFishRelic>().ToMutable(),
                            ModelDb.Relic<RawCodRelic>().ToMutable(),
                            ModelDb.Relic<RawSalmonRelic>().ToMutable(),
                            ModelDb.Relic<SwordOfStone>().ToMutable(),
                        ]);
                        break;
                    case RoomType.Elite:
                        list.AddRange(
                        [
                            ModelDb.Relic<Anchor>().ToMutable(),
                            ModelDb.Relic<HornCleat>().ToMutable(),
                            ModelDb.Relic<BeatingRemnant>().ToMutable(),
                            ModelDb.Relic<NautilusShellRelic>().ToMutable(),
                            ModelDb.Relic<TropicalFishRelic>().ToMutable(),
                            ModelDb.Relic<RawSalmonRelic>().ToMutable(),
                            ModelDb.Relic<RottenFleshRelic>().ToMutable(),
                            ModelDb.Relic<CaptainsWheel>().ToMutable(),
                        ]);
                        break;
                    case RoomType.Boss:
                        list.AddRange(
                        [
                            ModelDb.Relic<WhiteStar>().ToMutable(),
                            ModelDb.Relic<TungstenRod>().ToMutable(),
                            ModelDb.Relic<OldCoin>().ToMutable(),
                        ]);
                        break;
                    default:
                        continue;
                }

                var allowed = list.Where(r => r.IsAllowed(player.RunState)).ToList();
                if (allowed.Count == 0)
                    continue;

                player.PlayerRng.Rewards.Shuffle(allowed);
                Flash();
                var take = Math.Min(1, allowed.Count);
                rewards.AddRange(allowed.Take(take).Select(r => new RelicReward(r, player)));
            }

            return true;
        }

        public override async Task AfterRoomEntered(AbstractRoom room)
        {
            if (CatchesData.Instance.GlobalCatchesCount < SavedCatchCount)
                CatchesData.Instance.GlobalCatchesCount = SavedCatchCount;

            InvokeDisplayAmountChanged();

            switch (room.RoomType)
            {
                case RoomType.Monster:
                    Flash();
                    await PowerCmd.Apply<MonsterPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1m,
                        Owner.Creature, null);
                    break;
                case RoomType.Elite:
                    Flash();
                    await PowerCmd.Apply<ElitePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1m,
                        Owner.Creature, null);
                    break;
                case RoomType.Boss:
                    Flash();
                    await PowerCmd.Apply<BossPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1m,
                        Owner.Creature, null);
                    break;
            }
        }
    }
}
