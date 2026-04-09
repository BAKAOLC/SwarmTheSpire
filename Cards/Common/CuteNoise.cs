using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public class CuteNoise() : SwarmCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies, true)
    {
        private const string StrengthLossKey = "StrengthLoss";

        public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
        {
            new PowerVar<MilesPower>(1m),
            new("StrengthLoss", 4m),
        };

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromPower<MilesPower>(),
            HoverTipFactory.FromPower<StrengthPower>(),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = CombatState;
            ArgumentNullException.ThrowIfNull(combatState);
            var hittableEnemies = combatState.HittableEnemies;
            foreach (var enemy in hittableEnemies)
            {
                await PowerCmd.Apply<MilesPower>(enemy, DynamicVars["MilesPower"].BaseValue, Owner.Creature,
                    this);
                await PowerCmd.Apply<PiercingWailPower>(enemy, DynamicVars["StrengthLoss"].BaseValue,
                    Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["MilesPower"].UpgradeValueBy(1m);
        }
    }
}
