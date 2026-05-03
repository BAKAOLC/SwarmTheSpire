using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Content;
using SwarmTheSpire.Powers;
using SwarmTheSpire;
using SwarmTheSpire.Relics;

namespace SwarmTheSpire.Cards;

public sealed class Harpoon()
    : SwarmTokenPoolCard(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy, false)
{
    protected override IEnumerable<string> RegisteredKeywordIds =>
        [ModContentRegistry.GetQualifiedKeywordId(Const.ModId, "harpoon")];

    protected override IEnumerable<string> RegisteredCardTagIds => [SwarmCardTagIds.Harpoon];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Retain];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(1m, DamageProps.cardHpLoss)];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    private bool HasQueenPower => CombatManager.Instance.IsInProgress && Owner.Creature.HasPower<QueenPower>();

    public override TargetType TargetType => HasQueenPower ? TargetType.AllEnemies : TargetType.AnyEnemy;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var shouldTriggerFatal = cardPlay.Target.Powers.All(static power => power.ShouldOwnerDeathTriggerFatal());
        var combatState = CombatState;
        var dv = DynamicVars.Damage;
        var primary = await CreatureCmd.Damage(choiceContext, cardPlay.Target, dv.BaseValue, dv.Props, Owner.Creature,
            this);
        TryIncrementCatch(shouldTriggerFatal, primary);

        if (!HasQueenPower)
            return;

        ArgumentNullException.ThrowIfNull(combatState);
        var queenPowerCount = Owner.Creature.GetPowerAmount<QueenPower>();
        for (var i = 0; i < queenPowerCount; i++)
        {
            foreach (var hittableEnemy in combatState.HittableEnemies)
            {
                var followUp = await CreatureCmd.Damage(choiceContext, hittableEnemy, dv.BaseValue, dv.Props,
                    Owner.Creature, this);
                TryIncrementCatch(shouldTriggerFatal, followUp);
            }
        }

        return;

        void TryIncrementCatch(bool canTriggerFatal, IEnumerable<DamageResult> damageResults)
        {
            if (!canTriggerFatal ||
                !damageResults.Any(static result => result is { OverkillDamage: 0, WasTargetKilled: true }))
                return;

            MilesRelic.TryIncrementCatch(Owner);
        }
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
