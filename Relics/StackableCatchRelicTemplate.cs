using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace SwarmTheSpire.Relics
{
    public abstract class StackableCatchRelicTemplate : SwarmRelicTemplate
    {
        [SavedProperty]
        public int CatchStacks
        {
            get;
            set
            {
                AssertMutable();
                field = Math.Max(1, value);
                InvokeDisplayAmountChanged();
            }
        } = 1;

        public override bool ShowCounter => CatchStacks > 1;

        public override int DisplayAmount => CatchStacks;

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
                MergeStateFromDuplicate(duplicate);
                await RelicCmd.Remove(duplicate);
            }
        }

        /// <summary>
        ///     合并时由保留实例（新获得）吸收旧实例；在移除旧遗物前调用，子类可在此迁移 <see cref="SavedProperties.SavedProperty{T}" /> 等状态。
        /// </summary>
        /// <param name="absorbed">即将被移除的同类旧遗物。</param>
        protected virtual void MergeStateFromDuplicate(StackableCatchRelicTemplate absorbed)
        {
        }
    }
}
