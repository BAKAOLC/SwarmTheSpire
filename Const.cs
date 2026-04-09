using System.Text;

namespace SwarmTheSpire
{
    public static class Const
    {
        public const string ModId = "SwarmTheSpire";
        public const string Name = "蜂群尖塔";
        public const string Version = "1.0.0";
        public const string EnergyColorName = "ironclad";

        public static string ToSnakeCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var builder = new StringBuilder(value.Length + 8);

            for (var index = 0; index < value.Length; index++)
            {
                var current = value[index];
                var hasPrevious = index > 0;
                var hasNext = index + 1 < value.Length;

                if (hasPrevious && char.IsUpper(current))
                {
                    var previous = value[index - 1];
                    var nextIsLower = hasNext && char.IsLower(value[index + 1]);

                    if (char.IsLower(previous) || char.IsDigit(previous) || nextIsLower)
                        builder.Append('_');
                }

                builder.Append(char.ToLowerInvariant(current));
            }

            return builder.ToString();
        }

        public static class Paths
        {
            public const string Root = "res://SwarmTheSpire";

            public const string CharacterVisualsScene = Root + "/scenes/evil/evil_character.tscn";
            public const string CharacterIconTexture = Root + "/images/evil_icon.png";
            public const string CharacterIconScene = Root + "/scenes/evil/evil_icon.tscn";
            public const string CharacterMerchantScene = Root + "/scenes/merchant/characters/evil_merchant.tscn";
            public const string CharacterRestSiteScene = Root + "/scenes/rest_site/characters/evil_rest_site.tscn";
            public const string CharacterSelectBgScene = Root + "/scenes/evil/evil_bg.tscn";
            public const string CharacterSelectIcon = Root + "/images/char_select_evil.png";
            public const string CharacterSelectLockedIcon = Root + "/images/char_select_evil_locked.png";
            public const string ArmPointingTexture = Root + "/images/ui/hands/multiplayer_hand_evil_point.png";
            public const string SharedPowerIcon = Root + "/images/powers/SwarmTheSpire-miles_power.png";
            public const string CharacterTransitionSfx = "event:/sfx/ui/wipe_ironclad";

            public static string Card(string typeName)
            {
                return $"{Root}/images/cards/SwarmTheSpire-{ToSnakeCase(typeName)}.png";
            }

            public static string Relic(string typeName)
            {
                return $"{Root}/images/relics/SwarmTheSpire-{ToSnakeCase(typeName)}.png";
            }
        }
    }
}
