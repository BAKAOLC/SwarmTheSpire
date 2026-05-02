using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace SwarmTheSpire.Cards
{
    public sealed class PirateEvil()
        : SwarmEvilPoolCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
    {
        protected override HashSet<CardTag> CanonicalTags => [];

        protected override bool ShouldGlowGoldInternal
        {
            get
            {
                var combatState = CombatState;
                return combatState != null && combatState.HittableEnemies.Any(e => e.HasPower<WeakPower>());
            }
        }

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.Static(StaticHoverTip.Fatal),
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(10m, ValueProp.Move),
            new CardsVar(1),
            new("Gold", 5m),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            var wasTargetWeak = cardPlay.Target.HasPower<WeakPower>();
            var shouldTriggerFatal = cardPlay.Target.Powers.All(p => p.ShouldOwnerDeathTriggerFatal());
            Vector2? monsterPos = null;
            if (TestMode.IsOff)
            {
                var creatureNode = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
                monsterPos = creatureNode?.VfxSpawnPosition;
            }

            var attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
            if (wasTargetWeak) await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            if (shouldTriggerFatal && attackCommand.Results.Any(r => r.WasTargetKilled))
            {
                if (monsterPos.HasValue)
                    VfxCmd.PlayVfx(monsterPos.Value, "vfx/vfx_coin_explosion_regular",
                        NCombatRoom.Instance?.CombatVfxContainer);
                await PlayerCmd.GainGold(DynamicVars["Gold"].IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
            DynamicVars.Cards.UpgradeValueBy(1m);
            DynamicVars["Gold"].UpgradeValueBy(3m);
        }
    }
}
