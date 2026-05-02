using System.Collections.Generic;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Powers
{
    public sealed class NeuroPlayPower : SwarmPowerTemplate
    {
        private sealed class Data
        {
            public readonly Dictionary<CardModel, int> AmountsForPlayedCards = [];
        }

        private const int MaxCardsToPlay = 21;

        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;

        public override string CustomPackedIconPath => Const.Paths.NeuroPowerIcon;

        public override string CustomBigIconPath => Const.Paths.NeuroPowerIcon;

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.Static(StaticHoverTip.Block)];

        protected override object InitInternalData() => new Data();

        public override Task BeforeCardPlayed(CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner.Creature != Owner)
                return Task.CompletedTask;

            GetInternalData<Data>().AmountsForPlayedCards[cardPlay.Card] = 2;
            return Task.CompletedTask;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner.Creature != Owner)
                return;

            if (!GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var value) || value <= 0)
                return;

            await CreatureCmd.GainBlock(Owner, value, ValueProp.Move, null);
        }

        public override async Task AfterAutoPrePlayPhaseEnteredLate(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != Owner.Player)
                return;

            var combatState = player.Creature.CombatState;
            if (combatState is null || Owner.Player is null)
                return;

            Flash();
            var cardsPlayed = 0;
            using (CardSelectCmd.PushSelector(new VakuuCardSelector()))
            {
                for (; cardsPlayed < MaxCardsToPlay; cardsPlayed++)
                {
                    if (CombatManager.Instance.IsOverOrEnding)
                        break;

                    if (CombatManager.Instance.IsPlayerReadyToEndTurn(player))
                        break;

                    var playable = PileType.Hand.GetPile(player).Cards.Where(c => c.CanPlay()).ToList();
                    if (playable.Count == 0)
                        break;

                    var skills = playable.Where(c => c.Type == CardType.Skill).ToList();
                    var rng = Owner.Player.RunState.Rng.CombatCardSelection;
                    var card = skills.Count > 0
                        ? rng.NextItem(skills)
                        : rng.NextItem(playable);

                    if (card is null)
                        break;

                    var target = GetTarget(card, combatState);
                    await card.SpendResources();
                    await CardCmd.AutoPlay(choiceContext, card, target, AutoPlayType.Default, true, false);
                }

                if (cardsPlayed == 0)
                    return;
            }

            TalkCmd.Play(
                cardsPlayed >= MaxCardsToPlay
                    ? new LocString("cards", "SWARMTHESPIRE-NEURO_PLAY.warning")
                    : new LocString("cards", "SWARMTHESPIRE-NEURO_PLAY.approval"),
                Owner,
                VfxColor.Purple,
                VfxDuration.Forever);
        }

        public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext,
            ICombatState combatState)
        {
            if (player == Owner.Player && AmountOnTurnStart >= 1)
                await PlayerCmd.GainEnergy(1m, Owner.Player);
        }

        private Creature? GetTarget(CardModel card, ICombatState combatState)
        {
            var ownerPlayer = Owner.Player;
            if (ownerPlayer is null)
                return null;

            var rng = ownerPlayer.RunState.Rng.CombatTargets;
            return card.TargetType switch
            {
                TargetType.AnyEnemy => combatState.HittableEnemies.FirstOrDefault(),
                TargetType.AnyAlly => rng.NextItem(combatState.Allies.Where(c =>
                    c is { IsAlive: true, IsPlayer: true } && c != Owner)),
                TargetType.Self => Owner,
                _ => null,
            };
        }
    }
}
