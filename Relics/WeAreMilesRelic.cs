using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using SwarmTheSpire.Data;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Relics
{
    public sealed class WeAreMilesRelic : MilesRelic
    {
        public override RelicRarity Rarity => RelicRarity.Ancient;

        public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
            ICombatState combatState)
        {
            if (side != Owner.Creature.Side)
                return;

            Flash();
            await PowerCmd.Apply<MilesPower>(choiceContext, combatState.HittableEnemies,
                DynamicVars["MilesPower"].BaseValue, Owner.Creature, null);

            if (combatState.RoundNumber <= 1)
            {
                CatchesData.Instance.CurrentCombatCatches = 0;
                InvokeDisplayAmountChanged();
            }
        }

        public override Task AfterObtained()
        {
            InvokeDisplayAmountChanged();
            return Task.CompletedTask;
        }
    }
}
