using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace SwarmTheSpire.Relics
{
    public abstract class StackableCatchRelicTemplate : SwarmRelicTemplate
    {
        private int _catchStacks = 1;

        [SavedProperty]
        public int CatchStacks
        {
            get => _catchStacks;
            set
            {
                AssertMutable();
                _catchStacks = Math.Max(1, value);
                InvokeDisplayAmountChanged();
            }
        }

        public override bool ShowCounter => CatchStacks > 1;

        public override int DisplayAmount => CatchStacks;

        protected decimal StackMultiplier => CatchStacks;

        protected async Task MergeDuplicateIntoSingleSlot()
        {
            var duplicates = Owner.Relics
                .OfType<StackableCatchRelicTemplate>()
                .Where(relic => relic.GetType() == GetType() && relic != this)
                .ToList();

            if (duplicates.Count == 0)
            {
                if (CatchStacks < 1)
                    CatchStacks = 1;
                return;
            }

            var totalStacks = CatchStacks + duplicates.Sum(static relic => relic.CatchStacks);
            CatchStacks = totalStacks;

            foreach (var duplicate in duplicates)
            {
                await RelicCmd.Remove(duplicate);
            }
        }
    }
}
