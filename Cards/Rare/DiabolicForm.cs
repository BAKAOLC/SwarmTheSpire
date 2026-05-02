using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class DiabolicForm()
        : SwarmEvilPoolCard(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromPower<StrengthPower>(),
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromPower<MilesPower>(),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, 6m, Owner.Creature, this, false);
            await PowerCmd.Apply<DiabolicFormPower>(choiceContext, Owner.Creature, 6m, Owner.Creature, this, false);

            foreach (var enemy in CombatState!.HittableEnemies)
            {
                if (!enemy.IsAlive)
                    continue;

                await PowerCmd.Apply<MilesPower>(choiceContext, enemy, 6m, Owner.Creature, this, false);
                await PowerCmd.Apply<WeakPower>(choiceContext, enemy, 6m, Owner.Creature, this, false);
                await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, 6m, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Ethereal);
    }
}
