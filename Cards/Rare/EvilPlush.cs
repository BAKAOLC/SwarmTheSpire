using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class EvilPlush() : SwarmCardTemplate(0, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies, true)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new PowerVar<MilesPower>(1m)];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<MilesPower>()];

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

        public override bool HasTurnEndInHandEffect => true;

        public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
        {
            if (card != this)
            {
                modifiedCost = originalCost;
                return false;
            }

            var combatState = CombatState;
            if (combatState == null)
            {
                modifiedCost = originalCost;
                return false;
            }

            var num = combatState.Enemies.Where(c => c.IsAlive).Sum(c => c.GetPowerAmount<MilesPower>());
            modifiedCost = num;
            return true;
        }

        public override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
        {
            var combatState = CombatState;
            ArgumentNullException.ThrowIfNull(combatState);
            var hittableEnemies = combatState.HittableEnemies;
            foreach (var item in hittableEnemies)
                if (item.IsAlive)
                    await PowerCmd.Apply<MilesPower>(item, DynamicVars["MilesPower"].BaseValue, Owner.Creature,
                        this);
        }

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card,
            bool causedByEthereal)
        {
            var resolved = EnergyCost.GetResolved();
            if (card == this)
            {
                var combatState = CombatState;
                ArgumentNullException.ThrowIfNull(combatState);
                await PowerCmd.Apply<MilesPower>(combatState.HittableEnemies, resolved, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["MilesPower"].UpgradeValueBy(1m);
        }
    }
}
