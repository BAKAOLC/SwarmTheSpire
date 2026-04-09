using Godot;
using STS2RitsuLib.Scaffolding.Content;

namespace SwarmTheSpire.Character
{
    public sealed class EvilCardPool : TypeListCardPoolModel
    {
        public override string Title => "Evil";

        public override string EnergyColorName => Const.EnergyColorName;

        public override string CardFrameMaterialPath => "card_frame_red";

        public override Color DeckEntryCardColor => EvilCharacter.Color;

        public override Color EnergyOutlineColor => EvilCharacter.EnergyOutlineColor;

        public override bool IsColorless => false;
    }
}
