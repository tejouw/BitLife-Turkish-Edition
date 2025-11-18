using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BitLifeTR.Core;
using BitLifeTR.Systems;
using BitLifeTR.Utils;

namespace BitLifeTR.UI.Screens
{
    /// <summary>
    /// Death screen showing life summary.
    /// </summary>
    public class DeathScreen : UIScreen
    {
        public override string ScreenId => "Death";

        protected override void CreateUI()
        {
            // Background
            UIFactory.CreateFullScreenPanel("Background", transform, UITheme.BackgroundDark);

            // Main container
            var container = UIFactory.CreateContainer("Container", transform);
            UIFactory.SetFullStretch(container);
            UIFactory.AddVerticalLayout(container, UITheme.SpacingLarge, null, TextAnchor.MiddleCenter);

            // RIP text
            var ripText = UIFactory.CreateText("RIP", container, "Huzur İçinde Yat",
                UITheme.FontSizeTitle, UITheme.TextSecondary, TextAlignmentOptions.Center, FontStyles.Bold);
            UIFactory.AddLayoutElement(ripText.rectTransform, -1, 80);
        }

        protected override void OnShow()
        {
            Refresh();
        }

        public override void Refresh()
        {
            var character = CharacterManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Clear existing content
            var container = transform.Find("Container");
            if (container != null)
            {
                container.DestroyAllChildren();
            }
            else
            {
                return;
            }

            var rect = container.GetComponent<RectTransform>();

            // RIP text
            var ripText = UIFactory.CreateText("RIP", rect, "Huzur İçinde Yat",
                UITheme.FontSizeTitle, UITheme.TextSecondary, TextAlignmentOptions.Center, FontStyles.Bold);
            UIFactory.AddLayoutElement(ripText.rectTransform, -1, 80);

            // Name
            var nameText = UIFactory.CreateText("Name", rect, character.FullName,
                UITheme.FontSizeLarge, UITheme.TextPrimary, TextAlignmentOptions.Center, FontStyles.Bold);
            UIFactory.AddLayoutElement(nameText.rectTransform, -1, 50);

            // Years
            var yearsText = UIFactory.CreateText("Years", rect,
                $"{character.BirthYear} - {character.BirthYear + character.DeathAge}",
                UITheme.FontSizeMedium, UITheme.TextSecondary);
            UIFactory.AddLayoutElement(yearsText.rectTransform, -1, 40);

            // Cause of death
            var causeText = UIFactory.CreateText("Cause", rect,
                $"Ölüm Sebebi: {character.DeathCause}",
                UITheme.FontSizeNormal, UITheme.TextSecondary);
            UIFactory.AddLayoutElement(causeText.rectTransform, -1, 30);

            // Stats panel
            var statsPanel = UIFactory.CreatePanel("StatsPanel", rect, UITheme.BackgroundPanel);
            statsPanel.sizeDelta = new Vector2(300, 200);
            UIFactory.AddVerticalLayout(statsPanel, UITheme.SpacingSmall, new RectOffset(20, 20, 15, 15));
            UIFactory.AddLayoutElement(statsPanel, 300, 200);

            // Life stats
            CreateStatLine(statsPanel, "Yaşanan Yıllar", character.DeathAge.ToString());
            CreateStatLine(statsPanel, "Net Değer", character.NetWorth.ToTurkishLira());
            CreateStatLine(statsPanel, "Çocuk Sayısı", character.ChildrenCount.ToString());
            CreateStatLine(statsPanel, "Karma", Mathf.RoundToInt(character.Karma).ToString());

            if (character.Achievements.Count > 0)
            {
                CreateStatLine(statsPanel, "Başarılar", character.Achievements.Count.ToString());
            }

            // Spacer
            UIFactory.CreateSpacer(rect, 30);

            // Buttons
            var buttonsContainer = UIFactory.CreateContainer("Buttons", rect);
            buttonsContainer.sizeDelta = new Vector2(300, 130);
            UIFactory.AddVerticalLayout(buttonsContainer, UITheme.SpacingNormal);

            // New life button
            var newLifeBtn = UIFactory.CreatePrimaryButton("NewLifeBtn", buttonsContainer, "Yeni Hayat", OnNewLife);
            UIFactory.AddLayoutElement(newLifeBtn.GetComponent<RectTransform>(), 280, 55);

            // Main menu button
            var menuBtn = UIFactory.CreateSecondaryButton("MenuBtn", buttonsContainer, "Ana Menü", OnMainMenu);
            UIFactory.AddLayoutElement(menuBtn.GetComponent<RectTransform>(), 280, 55);
        }

        private void CreateStatLine(RectTransform parent, string label, string value)
        {
            var line = UIFactory.CreateContainer(label + "Line", parent);
            line.sizeDelta = new Vector2(260, 30);
            UIFactory.AddHorizontalLayout(line, UITheme.SpacingSmall, new RectOffset(0, 0, 0, 0));
            UIFactory.AddLayoutElement(line, -1, 30);

            var labelText = UIFactory.CreateText(label + "Label", line, label,
                UITheme.FontSizeSmall, UITheme.TextSecondary, TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(labelText.rectTransform, -1, -1, -1, -1, 1);

            var valueText = UIFactory.CreateText(label + "Value", line, value,
                UITheme.FontSizeSmall, UITheme.TextPrimary, TextAlignmentOptions.Right);
            UIFactory.AddLayoutElement(valueText.rectTransform, 100, -1);
        }

        private void OnNewLife()
        {
            GameManager.Instance.StartNewGame();
            UIManager.Instance.ShowScreen<CharacterCreationScreen>();
        }

        private void OnMainMenu()
        {
            GameManager.Instance.ReturnToMainMenu();
            UIManager.Instance.SetRootScreen<MainMenuScreen>();
        }

        public override bool OnBackPressed()
        {
            OnMainMenu();
            return true;
        }
    }
}
