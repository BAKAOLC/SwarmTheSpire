using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Audio;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public class PipeDrop() : SwarmEvilPoolCard(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies, true)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
        {
            new PowerVar<WeakPower>(2m),
            new PowerVar<VulnerablePower>(2m),
        };

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromPower<MilesPower>(),
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.FromPower<VulnerablePower>(),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            Sts2SfxAlignedFmod.PlayOneShot(Const.Sfx.MetalPipeFalling);

            var combatState = CombatState;
            ArgumentNullException.ThrowIfNull(combatState);
            var hittableEnemies = combatState.HittableEnemies;
            foreach (var enemy in hittableEnemies)
            {
                await PowerCmd.Apply<WeakPower>(choiceContext, enemy, DynamicVars["WeakPower"].BaseValue,
                    Owner.Creature, this);
                if (enemy.HasPower<MilesPower>())
                    await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy,
                        DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
