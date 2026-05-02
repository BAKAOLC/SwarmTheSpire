using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using SwarmTheSpire.Character;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    [RegisterCharacterStarterCard(typeof(EvilCharacter))]
    public class EvilStream() : SwarmEvilPoolCard(1, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy, true)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<MilesPower>(2m)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<MilesPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<MilesPower>(choiceContext, cardPlay.Target, DynamicVars["MilesPower"].BaseValue,
                Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
