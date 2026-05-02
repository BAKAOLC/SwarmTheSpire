using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Cards
{
    public sealed class FrenchFries()
        : SwarmEvilPoolCard(6, CardType.Attack, CardRarity.Uncommon, TargetType.Self, true)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
        {
            new DamageVar(28m, ValueProp.Move),
            new PowerVar<WeakPower>(2m),
        };

        public override List<CardKeyword> CanonicalKeywords
        {
            get
            {
                const int num = 1;
                var list = new List<CardKeyword>(num);
                CollectionsMarshal.SetCount(list, num);
                CollectionsMarshal.AsSpan(list)[0] = CardKeyword.Exhaust;
                return list;
            }
        }

        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            return Task.CompletedTask;
        }

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card,
            bool causedByEthereal)
        {
            if (card == this)
            {
                var combatState = CombatState;
                ArgumentNullException.ThrowIfNull(combatState);
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                    .TargetingAllOpponents(combatState)
                    .Execute(choiceContext);
                await PowerCmd.Apply<WeakPower>(choiceContext, combatState.HittableEnemies,
                    DynamicVars.Weak.BaseValue, Owner.Creature, this);
            }
        }

        public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            if (card != this) return Task.CompletedTask;
            EnergyCost.AddThisCombat(-2);
            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(8m);
            DynamicVars.Weak.UpgradeValueBy(1m);
        }
    }
}
