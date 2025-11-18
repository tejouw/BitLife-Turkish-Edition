using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BitLifeTR.Core;
using BitLifeTR.Systems;

namespace BitLifeTR.UI.Screens
{
    /// <summary>
    /// Main menu screen with new game, continue, and settings options.
    /// </summary>
    public class MainMenuScreen : UIScreen
    {
        public override string ScreenId => "MainMenu";

        private Button newGameButton;
        private Button continueButton;
        private Button settingsButton;
        private Button quitButton;

        protected override void CreateUI()
        {
            // Background
            var background = UIFactory.CreateFullScreenPanel("Background", transform, UITheme.BackgroundDark);

            // Main container
            var container = UIFactory.CreateContainer("Container", transform);
            UIFactory.SetFullStretch(container);
            UIFactory.AddVerticalLayout(container, UITheme.SpacingLarge, null, TextAnchor.MiddleCenter);

            // Title
            var title = UIFactory.CreateText("Title", container, "BitLife", UITheme.FontSizeTitle,
                UITheme.PrimaryColor, TMPro.TextAlignmentOptions.Center, TMPro.FontStyles.Bold);
            UIFactory.AddLayoutElement(title.rectTransform, -1, 100);

            // Subtitle
            var subtitle = UIFactory.CreateText("Subtitle", container, "Türkiye", UITheme.FontSizeLarge,
                UITheme.AccentColor);
            UIFactory.AddLayoutElement(subtitle.rectTransform, -1, 60);

            // Spacer
            UIFactory.CreateSpacer(container, 50);

            // Buttons container
            var buttonsContainer = UIFactory.CreateContainer("Buttons", container);
            buttonsContainer.sizeDelta = new Vector2(300, 300);
            UIFactory.AddVerticalLayout(buttonsContainer, UITheme.SpacingNormal);

            // New Game button
            newGameButton = UIFactory.CreatePrimaryButton("NewGameBtn", buttonsContainer, "Yeni Oyun", OnNewGame);
            UIFactory.AddLayoutElement(newGameButton.GetComponent<RectTransform>(), 280, 60);

            // Continue button
            continueButton = UIFactory.CreateSecondaryButton("ContinueBtn", buttonsContainer, "Devam Et", OnContinue);
            UIFactory.AddLayoutElement(continueButton.GetComponent<RectTransform>(), 280, 60);

            // Settings button
            settingsButton = UIFactory.CreateSecondaryButton("SettingsBtn", buttonsContainer, "Ayarlar", OnSettings);
            UIFactory.AddLayoutElement(settingsButton.GetComponent<RectTransform>(), 280, 60);

            // Quit button
            quitButton = UIFactory.CreateButton("QuitBtn", buttonsContainer, "Çıkış", OnQuit,
                new Vector2(280, 60), UITheme.BackgroundLight);
            UIFactory.AddLayoutElement(quitButton.GetComponent<RectTransform>(), 280, 60);

            // Version text
            var version = UIFactory.CreateText("Version", container, $"v{Constants.GAME_VERSION}",
                UITheme.FontSizeSmall, UITheme.TextSecondary);
            UIFactory.AddLayoutElement(version.rectTransform, -1, 30);
        }

        protected override void OnShow()
        {
            // Check if there's a save to continue
            bool hasSave = SaveManager.Instance.SaveExists(Constants.DEFAULT_SAVE_SLOT);
            continueButton.interactable = hasSave;

            var continueText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            if (continueText != null)
            {
                continueText.color = hasSave ? UITheme.TextPrimary : UITheme.TextDisabled;
            }
        }

        private void OnNewGame()
        {
            Debug.Log("[MainMenuScreen] New Game clicked");
            GameManager.Instance.StartNewGame();
            UIManager.Instance.ShowScreen<CharacterCreationScreen>();
        }

        private void OnContinue()
        {
            Debug.Log("[MainMenuScreen] Continue clicked");

            if (CharacterManager.Instance.LoadSavedCharacter())
            {
                GameManager.Instance.BeginLife();
                UIManager.Instance.ShowScreen<GameScreen>();
            }
        }

        private void OnSettings()
        {
            Debug.Log("[MainMenuScreen] Settings clicked");
            UIManager.Instance.ShowScreen<SettingsScreen>(false);
        }

        private void OnQuit()
        {
            Debug.Log("[MainMenuScreen] Quit clicked");
            GameManager.Instance.QuitGame();
        }
    }
}
