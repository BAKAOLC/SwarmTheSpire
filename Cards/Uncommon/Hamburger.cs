using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class Hamburger() : SwarmCardTemplate(6, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(7m)];

        private bool HasCrewMemberPower =>
            CombatManager.Instance.IsInProgress && Owner.Creature.HasPower<CrewmemberPower>();

        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            return Task.CompletedTask;
        }

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card,
            bool causedByEthereal)
        {
            if (card == this)
            {
                await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
                await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
            }
        }

        public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            if (card != this) return Task.CompletedTask;
            EnergyCost.AddThisCombat(-3);
            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Heal.UpgradeValueBy(4m);
        }
    }
}
