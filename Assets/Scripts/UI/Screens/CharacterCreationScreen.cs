using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BitLifeTR.Core;
using BitLifeTR.Systems;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.UI.Screens
{
    /// <summary>
    /// Character creation screen for starting a new game.
    /// </summary>
    public class CharacterCreationScreen : UIScreen
    {
        public override string ScreenId => "CharacterCreation";

        private CharacterData previewCharacter;
        private TextMeshProUGUI nameText;
        private TextMeshProUGUI genderText;
        private TextMeshProUGUI cityText;
        private TextMeshProUGUI healthText;
        private TextMeshProUGUI intelligenceText;
        private TextMeshProUGUI looksText;
        private TextMeshProUGUI happinessText;
        private Image healthBar;
        private Image intelligenceBar;
        private Image looksBar;
        private Image happinessBar;

        protected override void CreateUI()
        {
            // Background
            UIFactory.CreateFullScreenPanel("Background", transform, UITheme.BackgroundDark);

            // Main container
            var container = UIFactory.CreateContainer("Container", transform);
            UIFactory.SetFullStretch(container);
            UIFactory.AddVerticalLayout(container, UITheme.SpacingNormal, null, TextAnchor.UpperCenter);

            // Title
            var title = UIFactory.CreateTitle("Title", container, "Yeni Hayat");
            UIFactory.AddLayoutElement(title.rectTransform, -1, 80);

            // Character info panel
            var infoPanel = UIFactory.CreatePanel("InfoPanel", container, UITheme.BackgroundPanel);
            infoPanel.sizeDelta = new Vector2(350, 200);
            UIFactory.AddVerticalLayout(infoPanel, UITheme.SpacingSmall,
                new RectOffset(20, 20, 20, 20), TextAnchor.UpperLeft);
            UIFactory.AddLayoutElement(infoPanel, 350, 200);

            // Name
            nameText = UIFactory.CreateText("Name", infoPanel, "İsim: -", UITheme.FontSizeNormal,
                UITheme.TextPrimary, TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(nameText.rectTransform, -1, 30);

            // Gender
            genderText = UIFactory.CreateText("Gender", infoPanel, "Cinsiyet: -", UITheme.FontSizeNormal,
                UITheme.TextPrimary, TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(genderText.rectTransform, -1, 30);

            // City
            cityText = UIFactory.CreateText("City", infoPanel, "Şehir: -", UITheme.FontSizeNormal,
                UITheme.TextPrimary, TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(cityText.rectTransform, -1, 30);

            // Stats panel
            var statsPanel = UIFactory.CreatePanel("StatsPanel", container, UITheme.BackgroundPanel);
            statsPanel.sizeDelta = new Vector2(350, 250);
            UIFactory.AddVerticalLayout(statsPanel, UITheme.SpacingSmall,
                new RectOffset(20, 20, 20, 20), TextAnchor.UpperLeft);
            UIFactory.AddLayoutElement(statsPanel, 350, 250);

            // Stats title
            var statsTitle = UIFactory.CreateText("StatsTitle", statsPanel, "İstatistikler",
                UITheme.FontSizeMedium, UITheme.TextPrimary);
            UIFactory.AddLayoutElement(statsTitle.rectTransform, -1, 35);

            // Health stat
            CreateStatRow(statsPanel, "Sağlık", UITheme.StatHealth, out healthText, out healthBar);

            // Intelligence stat
            CreateStatRow(statsPanel, "Zeka", UITheme.StatIntelligence, out intelligenceText, out intelligenceBar);

            // Looks stat
            CreateStatRow(statsPanel, "Görünüm", UITheme.StatLooks, out looksText, out looksBar);

            // Happiness stat
            CreateStatRow(statsPanel, "Mutluluk", UITheme.StatHappiness, out happinessText, out happinessBar);

            // Spacer
            UIFactory.CreateSpacer(container, 20);

            // Buttons
            var buttonsContainer = UIFactory.CreateContainer("Buttons", container);
            buttonsContainer.sizeDelta = new Vector2(350, 120);
            UIFactory.AddVerticalLayout(buttonsContainer, UITheme.SpacingNormal);

            // Randomize button
            var randomBtn = UIFactory.CreateSecondaryButton("RandomBtn", buttonsContainer, "Yeniden Oluştur", OnRandomize);
            UIFactory.AddLayoutElement(randomBtn.GetComponent<RectTransform>(), 300, 50);

            // Start button
            var startBtn = UIFactory.CreatePrimaryButton("StartBtn", buttonsContainer, "Hayata Başla", OnStartLife);
            UIFactory.AddLayoutElement(startBtn.GetComponent<RectTransform>(), 300, 60);
        }

        private void CreateStatRow(RectTransform parent, string label, Color color,
            out TextMeshProUGUI valueText, out Image fillBar)
        {
            var row = UIFactory.CreateContainer(label + "Row", parent);
            row.sizeDelta = new Vector2(310, 35);
            UIFactory.AddHorizontalLayout(row, UITheme.SpacingSmall, new RectOffset(0, 0, 0, 0));
            UIFactory.AddLayoutElement(row, -1, 35);

            // Label
            var labelText = UIFactory.CreateText(label + "Label", row, label, UITheme.FontSizeSmall, UITheme.TextSecondary, TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(labelText.rectTransform, 80, -1);

            // Bar background
            var barBg = UIFactory.CreateImage(label + "BarBg", row, null, UITheme.BackgroundDark);
            UIFactory.AddLayoutElement(barBg.rectTransform, -1, 20, -1, 20, 1);

            // Bar fill
            var fill = UIFactory.CreateImage(label + "Fill", barBg.transform, null, color);
            fill.rectTransform.anchorMin = Vector2.zero;
            fill.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            fill.rectTransform.offsetMin = Vector2.zero;
            fill.rectTransform.offsetMax = Vector2.zero;
            fillBar = fill;

            // Value text
            valueText = UIFactory.CreateText(label + "Value", row, "50", UITheme.FontSizeSmall, UITheme.TextPrimary, TextAlignmentOptions.Right);
            UIFactory.AddLayoutElement(valueText.rectTransform, 40, -1);
        }

        protected override void OnShow()
        {
            // Generate initial character
            OnRandomize();
        }

        private void OnRandomize()
        {
            // Create new random character
            previewCharacter = CharacterManager.Instance.CreateNewCharacter();

            // Update UI
            UpdateCharacterDisplay();
        }

        private void UpdateCharacterDisplay()
        {
            if (previewCharacter == null) return;

            // Update text
            nameText.text = $"İsim: {previewCharacter.FullName}";
            genderText.text = $"Cinsiyet: {previewCharacter.Gender.ToTurkish()}";
            cityText.text = $"Şehir: {previewCharacter.BirthCity}";

            // Update stats
            UpdateStat(healthText, healthBar, previewCharacter.Health);
            UpdateStat(intelligenceText, intelligenceBar, previewCharacter.Intelligence);
            UpdateStat(looksText, looksBar, previewCharacter.Looks);
            UpdateStat(happinessText, happinessBar, previewCharacter.Happiness);
        }

        private void UpdateStat(TextMeshProUGUI text, Image bar, float value)
        {
            text.text = Mathf.RoundToInt(value).ToString();
            bar.rectTransform.anchorMax = new Vector2(value / 100f, 1f);
        }

        private void OnStartLife()
        {
            if (previewCharacter == null) return;

            Debug.Log($"[CharacterCreationScreen] Starting life as {previewCharacter.FullName}");

            // Start the game
            GameManager.Instance.BeginLife();
            UIManager.Instance.ShowScreen<GameScreen>();

            // Start first year
            GameLoop.Instance.StartYear();
        }

        public override bool OnBackPressed()
        {
            UIManager.Instance.ShowScreen<MainMenuScreen>();
            return true;
        }
    }
}
