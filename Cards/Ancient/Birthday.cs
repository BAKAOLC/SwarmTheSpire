using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SwarmTheSpire.Powers;

namespace SwarmTheSpire.Cards
{
    public sealed class Birthday() : SwarmEvilPoolCard(0, CardType.Skill, CardRarity.Ancient, TargetType.Self, true)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>
        {
            CardKeyword.Exhaust,
            CardKeyword.Sly,
        };

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<MilesPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
            [new HealVar(7m)];

        protected override void OnUpgrade()
        {
        }
    }
}
