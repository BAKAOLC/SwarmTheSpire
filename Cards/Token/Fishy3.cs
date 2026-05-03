using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class Fishy3()
        : SwarmTokenPoolCard(-1, CardType.Skill, CardRarity.Token, TargetType.Self, true), FishyValor.IChoosable
    {
        public override bool CanBeGeneratedInCombat => false;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<ChargePower>(15m),
            new DynamicVar("Gold", -50m),
        ];

        public async Task OnChosen()
        {
            await PlayerCmd.GainGold(DynamicVars["Gold"].IntValue, Owner, false);
            await PowerCmd.Apply<ChargePower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
                DynamicVars["ChargePower"].BaseValue, Owner.Creature, this, false);
        }

        protected override void OnUpgrade() => DynamicVars["ChargePower"].UpgradeValueBy(10m);
    }
}
