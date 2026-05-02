using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class NeuroPlay()
        : SwarmEvilPoolCard(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new BlockVar(21m, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
            await PowerCmd.Apply<NeuroPlayPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this, false);
            PlayerCmd.EndTurn(Owner, false, null);
        }

        protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
    }
}
