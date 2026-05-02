using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class Fishy1()
        : SwarmTokenPoolCard(-1, CardType.Skill, CardRarity.Token, TargetType.Self, true), FishyValor.IChoosable
    {
        public override bool CanBeGeneratedInCombat => false;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<VigorPower>(-3m),
            new DynamicVar("Gold", 6m),
        ];

        public async Task OnChosen()
        {
            await PlayerCmd.GainGold(DynamicVars["Gold"].IntValue, Owner, false);
            await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
                DynamicVars["VigorPower"].BaseValue, Owner.Creature, this, false);
        }

        protected override void OnUpgrade() => DynamicVars["Gold"].UpgradeValueBy(4m);
    }
}
