using UnityEngine;
using UnityEngine.UI;
using BitLifeTR.Core;
using BitLifeTR.Systems;

namespace BitLifeTR.UI.Screens
{
    /// <summary>
    /// Settings screen for game options.
    /// </summary>
    public class SettingsScreen : UIScreen
    {
        public override string ScreenId => "Settings";
        public override int SortingOrder => 20;

        private GameSettings settings;
        private Slider musicSlider;
        private Slider sfxSlider;
        private Toggle autoSaveToggle;
        private Toggle vibrationToggle;

        protected override void CreateUI()
        {
            // Semi-transparent background
            UIFactory.CreateFullScreenPanel("Background", transform, UITheme.WithAlpha(Color.black, 0.7f));

            // Panel
            var panel = UIFactory.CreatePanel("Panel", transform, UITheme.BackgroundPanel);
            UIFactory.SetCenterAnchors(panel);
            panel.sizeDelta = new Vector2(350, 450);
            UIFactory.AddVerticalLayout(panel, UITheme.SpacingNormal, new RectOffset(20, 20, 20, 20));

            // Title
            var title = UIFactory.CreateTitle("Title", panel, "Ayarlar");
            UIFactory.AddLayoutElement(title.rectTransform, -1, 50);

            // Music volume
            CreateSliderSetting(panel, "Müzik", out musicSlider);

            // SFX volume
            CreateSliderSetting(panel, "Ses Efektleri", out sfxSlider);

            // Auto save
            CreateToggleSetting(panel, "Otomatik Kayıt", out autoSaveToggle);

            // Vibration
            CreateToggleSetting(panel, "Titreşim", out vibrationToggle);

            // Spacer
            UIFactory.CreateSpacer(panel, 20);

            // Save button
            var saveBtn = UIFactory.CreatePrimaryButton("SaveBtn", panel, "Kaydet", OnSave);
            UIFactory.AddLayoutElement(saveBtn.GetComponent<RectTransform>(), -1, 50);

            // Close button
            var closeBtn = UIFactory.CreateSecondaryButton("CloseBtn", panel, "İptal", () => Hide());
            UIFactory.AddLayoutElement(closeBtn.GetComponent<RectTransform>(), -1, 50);
        }

        private void CreateSliderSetting(RectTransform parent, string label, out Slider slider)
        {
            var container = UIFactory.CreateContainer(label + "Container", parent);
            container.sizeDelta = new Vector2(310, 50);
            UIFactory.AddVerticalLayout(container, UITheme.SpacingTiny, new RectOffset(0, 0, 0, 0));
            UIFactory.AddLayoutElement(container, -1, 55);

            // Label
            var labelText = UIFactory.CreateText(label + "Label", container, label,
                UITheme.FontSizeSmall, UITheme.TextSecondary, TMPro.TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(labelText.rectTransform, -1, 20);

            // Slider background
            var sliderGO = new GameObject(label + "Slider", typeof(RectTransform));
            var sliderRect = sliderGO.GetComponent<RectTransform>();
            sliderRect.SetParent(container, false);
            UIFactory.AddLayoutElement(sliderRect, -1, 25);

            slider = sliderGO.AddComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 1;

            // Background
            var bgGO = new GameObject("Background", typeof(RectTransform));
            bgGO.transform.SetParent(sliderGO.transform, false);
            var bgRect = bgGO.GetComponent<RectTransform>();
            UIFactory.SetFullStretch(bgRect);
            var bgImage = bgGO.AddComponent<Image>();
            bgImage.color = UITheme.BackgroundDark;

            // Fill area
            var fillAreaGO = new GameObject("FillArea", typeof(RectTransform));
            fillAreaGO.transform.SetParent(sliderGO.transform, false);
            var fillAreaRect = fillAreaGO.GetComponent<RectTransform>();
            UIFactory.SetFullStretch(fillAreaRect);

            var fillGO = new GameObject("Fill", typeof(RectTransform));
            fillGO.transform.SetParent(fillAreaGO.transform, false);
            var fillRect = fillGO.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(0, 1);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            var fillImage = fillGO.AddComponent<Image>();
            fillImage.color = UITheme.PrimaryColor;

            slider.fillRect = fillRect;
            slider.targetGraphic = bgImage;
        }

        private void CreateToggleSetting(RectTransform parent, string label, out Toggle toggle)
        {
            var container = UIFactory.CreateContainer(label + "Container", parent);
            container.sizeDelta = new Vector2(310, 40);
            UIFactory.AddHorizontalLayout(container, UITheme.SpacingNormal, new RectOffset(0, 0, 5, 5));
            UIFactory.AddLayoutElement(container, -1, 45);

            // Label
            var labelText = UIFactory.CreateText(label + "Label", container, label,
                UITheme.FontSizeNormal, UITheme.TextPrimary, TMPro.TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(labelText.rectTransform, -1, -1, -1, -1, 1);

            // Toggle
            var toggleGO = new GameObject(label + "Toggle", typeof(RectTransform));
            var toggleRect = toggleGO.GetComponent<RectTransform>();
            toggleRect.SetParent(container, false);
            toggleRect.sizeDelta = new Vector2(50, 30);
            UIFactory.AddLayoutElement(toggleRect, 50, 30);

            toggle = toggleGO.AddComponent<Toggle>();

            // Background
            var bgImage = UIFactory.CreateImage("Background", toggleRect, null, UITheme.BackgroundDark);
            bgImage.rectTransform.sizeDelta = new Vector2(50, 30);

            // Checkmark
            var checkmark = UIFactory.CreateImage("Checkmark", bgImage.transform, null, UITheme.PrimaryColor);
            checkmark.rectTransform.anchorMin = new Vector2(0.1f, 0.1f);
            checkmark.rectTransform.anchorMax = new Vector2(0.9f, 0.9f);
            checkmark.rectTransform.offsetMin = Vector2.zero;
            checkmark.rectTransform.offsetMax = Vector2.zero;

            toggle.targetGraphic = bgImage;
            toggle.graphic = checkmark;
            toggle.isOn = true;
        }

        protected override void OnShow()
        {
            // Load current settings
            settings = SaveManager.Instance.LoadSettings();

            musicSlider.value = settings.MusicVolume;
            sfxSlider.value = settings.SfxVolume;
            autoSaveToggle.isOn = settings.AutoSave;
            vibrationToggle.isOn = settings.Vibration;
        }

        private void OnSave()
        {
            settings.MusicVolume = musicSlider.value;
            settings.SfxVolume = sfxSlider.value;
            settings.AutoSave = autoSaveToggle.isOn;
            settings.Vibration = vibrationToggle.isOn;

            SaveManager.Instance.SaveSettings(settings);
            Hide();
        }

        public override bool OnBackPressed()
        {
            Hide();
            return true;
        }
    }
}
