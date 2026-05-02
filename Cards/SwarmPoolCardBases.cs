using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using SwarmTheSpire.Character;

namespace SwarmTheSpire.Cards;

[RegisterCard(typeof(EvilCardPool), Inherit = true)]
public abstract class SwarmEvilPoolCard(
    int baseCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    bool shouldShowInCardLibrary)
    : SwarmCardTemplate(baseCost, type, rarity, targetType, shouldShowInCardLibrary);

[RegisterCard(typeof(TokenCardPool), Inherit = true)]
public abstract class SwarmTokenPoolCard(
    int baseCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    bool shouldShowInCardLibrary)
    : SwarmCardTemplate(baseCost, type, rarity, targetType, shouldShowInCardLibrary);
