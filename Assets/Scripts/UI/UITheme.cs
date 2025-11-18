using UnityEngine;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Defines the visual theme for the entire UI.
    /// All colors, sizes, and styles are defined here.
    /// </summary>
    public static class UITheme
    {
        #region Colors

        // Primary colors
        public static readonly Color PrimaryColor = new Color(0.2f, 0.6f, 0.86f, 1f);      // Mavi
        public static readonly Color SecondaryColor = new Color(0.18f, 0.8f, 0.44f, 1f);   // Yeşil
        public static readonly Color AccentColor = new Color(0.95f, 0.77f, 0.06f, 1f);     // Sarı

        // Background colors
        public static readonly Color BackgroundDark = new Color(0.1f, 0.1f, 0.12f, 1f);
        public static readonly Color BackgroundMedium = new Color(0.15f, 0.15f, 0.18f, 1f);
        public static readonly Color BackgroundLight = new Color(0.2f, 0.2f, 0.24f, 1f);
        public static readonly Color BackgroundPanel = new Color(0.25f, 0.25f, 0.3f, 0.95f);

        // Text colors
        public static readonly Color TextPrimary = Color.white;
        public static readonly Color TextSecondary = new Color(0.7f, 0.7f, 0.7f, 1f);
        public static readonly Color TextDisabled = new Color(0.5f, 0.5f, 0.5f, 1f);
        public static readonly Color TextHighlight = AccentColor;

        // Button colors
        public static readonly Color ButtonNormal = PrimaryColor;
        public static readonly Color ButtonHover = new Color(0.25f, 0.65f, 0.9f, 1f);
        public static readonly Color ButtonPressed = new Color(0.15f, 0.5f, 0.75f, 1f);
        public static readonly Color ButtonDisabled = new Color(0.4f, 0.4f, 0.4f, 1f);

        // Stat colors
        public static readonly Color StatHealth = new Color(0.9f, 0.3f, 0.3f, 1f);        // Kırmızı
        public static readonly Color StatHappiness = new Color(0.95f, 0.77f, 0.06f, 1f);  // Sarı
        public static readonly Color StatIntelligence = new Color(0.2f, 0.6f, 0.86f, 1f); // Mavi
        public static readonly Color StatLooks = new Color(0.91f, 0.46f, 0.85f, 1f);      // Mor
        public static readonly Color StatMoney = new Color(0.18f, 0.8f, 0.44f, 1f);       // Yeşil
        public static readonly Color StatFame = new Color(0.95f, 0.5f, 0.1f, 1f);         // Turuncu
        public static readonly Color StatKarma = new Color(0.6f, 0.4f, 0.8f, 1f);         // Lavanta

        // Status colors
        public static readonly Color Success = new Color(0.18f, 0.8f, 0.44f, 1f);
        public static readonly Color Warning = new Color(0.95f, 0.77f, 0.06f, 1f);
        public static readonly Color Error = new Color(0.9f, 0.3f, 0.3f, 1f);
        public static readonly Color Info = new Color(0.2f, 0.6f, 0.86f, 1f);

        #endregion

        #region Sizes

        // Font sizes
        public const float FontSizeSmall = 14f;
        public const float FontSizeNormal = 18f;
        public const float FontSizeMedium = 24f;
        public const float FontSizeLarge = 32f;
        public const float FontSizeXLarge = 48f;
        public const float FontSizeTitle = 64f;

        // Spacing
        public const float SpacingTiny = 4f;
        public const float SpacingSmall = 8f;
        public const float SpacingNormal = 16f;
        public const float SpacingMedium = 24f;
        public const float SpacingLarge = 32f;
        public const float SpacingXLarge = 48f;

        // Padding
        public const float PaddingSmall = 8f;
        public const float PaddingNormal = 16f;
        public const float PaddingLarge = 24f;

        // Border radius (for UI elements)
        public const float BorderRadiusSmall = 4f;
        public const float BorderRadiusNormal = 8f;
        public const float BorderRadiusLarge = 16f;
        public const float BorderRadiusRound = 9999f;

        // Button sizes
        public static readonly Vector2 ButtonSizeSmall = new Vector2(120f, 40f);
        public static readonly Vector2 ButtonSizeNormal = new Vector2(200f, 50f);
        public static readonly Vector2 ButtonSizeLarge = new Vector2(280f, 60f);
        public static readonly Vector2 ButtonSizeWide = new Vector2(350f, 50f);

        // Icon sizes
        public const float IconSizeSmall = 24f;
        public const float IconSizeNormal = 32f;
        public const float IconSizeLarge = 48f;

        // Stat bar
        public static readonly Vector2 StatBarSize = new Vector2(200f, 20f);
        public const float StatBarHeight = 8f;

        #endregion

        #region Animation

        public const float AnimationFast = 0.15f;
        public const float AnimationNormal = 0.3f;
        public const float AnimationSlow = 0.5f;

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get color for a specific stat type.
        /// </summary>
        public static Color GetStatColor(string statName)
        {
            return statName.ToLower() switch
            {
                "health" or "sağlık" or "saglik" => StatHealth,
                "happiness" or "mutluluk" => StatHappiness,
                "intelligence" or "zeka" => StatIntelligence,
                "looks" or "görünüm" or "gorunum" => StatLooks,
                "money" or "para" => StatMoney,
                "fame" or "şöhret" or "sohret" => StatFame,
                "karma" => StatKarma,
                _ => TextPrimary
            };
        }

        /// <summary>
        /// Get a color with modified alpha.
        /// </summary>
        public static Color WithAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Lerp between two colors.
        /// </summary>
        public static Color Lerp(Color a, Color b, float t)
        {
            return Color.Lerp(a, b, t);
        }

        #endregion
    }
}
