using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class Advent()
        : SwarmEvilPoolCard(0, CardType.Skill, CardRarity.Ancient, TargetType.Self, true)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromPower<MilesPower>(),
            HoverTipFactory.FromPower<ArtifactPower>(),
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new DynamicVar("Birthday", 1m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Heal(Owner.Creature, DynamicVars["Birthday"].BaseValue, true);
            await PlayerCmd.GainEnergy(DynamicVars["Birthday"].BaseValue, Owner);
            await CardPileCmd.Draw(choiceContext, DynamicVars["Birthday"].BaseValue, Owner, false);
            await PowerCmd.Apply<ArtifactPower>(choiceContext, Owner.Creature, DynamicVars["Birthday"].BaseValue,
                Owner.Creature, this, false);

            foreach (var enemy in CombatState!.HittableEnemies)
            {
                if (enemy.IsAlive)
                {
                    await PowerCmd.Apply<MilesPower>(choiceContext, enemy, DynamicVars["Birthday"].BaseValue,
                        Owner.Creature, this, false);
                }
            }
        }

        protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
    }
}
