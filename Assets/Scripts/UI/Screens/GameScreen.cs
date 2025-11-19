using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Ana oyun ekranı
    /// </summary>
    public class GameScreen : BaseScreen
    {
        private TextMeshProUGUI nameText;
        private TextMeshProUGUI ageText;
        private TextMeshProUGUI moneyText;

        // Stat barları
        private Image healthBar;
        private Image happinessBar;
        private Image intelligenceBar;
        private Image appearanceBar;

        public GameScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Game", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            CreateHeader();
            CreateStatBars();
            CreateActionButtons();
            CreateBottomNav();
        }

        private void CreateHeader()
        {
            // Üst bilgi paneli
            var headerPanel = uiManager.PanelFactory.Create("Header", new Vector2(0, 200), rootObject.transform, new Color(0.1f, 0.1f, 0.15f, 1f));
            var headerRect = headerPanel.GetComponent<RectTransform>();
            headerRect.anchorMin = new Vector2(0, 1);
            headerRect.anchorMax = new Vector2(1, 1);
            headerRect.pivot = new Vector2(0.5f, 1);
            headerRect.anchoredPosition = Vector2.zero;
            headerRect.sizeDelta = new Vector2(0, 200);

            // İsim
            var nameObj = uiManager.TextFactory.Create("İsim", headerPanel.transform, 32, Color.white);
            nameObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 60);
            nameText = nameObj.GetComponent<TextMeshProUGUI>();

            // Yaş
            var ageObj = uiManager.TextFactory.Create("Yaş: 0", headerPanel.transform, 24, Color.white);
            ageObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);
            ageText = ageObj.GetComponent<TextMeshProUGUI>();

            // Para
            var moneyObj = uiManager.TextFactory.Create("₺0", headerPanel.transform, 24, new Color(0.3f, 0.8f, 0.3f));
            moneyObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20);
            moneyText = moneyObj.GetComponent<TextMeshProUGUI>();
        }

        private void CreateStatBars()
        {
            float startY = 250;
            float barSpacing = 60;

            // Sağlık
            healthBar = CreateStatBar("Sağlık", new Color(0.8f, 0.2f, 0.2f), startY);

            // Mutluluk
            happinessBar = CreateStatBar("Mutluluk", new Color(0.2f, 0.8f, 0.2f), startY - barSpacing);

            // Zeka
            intelligenceBar = CreateStatBar("Zeka", new Color(0.2f, 0.6f, 0.9f), startY - barSpacing * 2);

            // Görünüm
            appearanceBar = CreateStatBar("Görünüm", new Color(0.9f, 0.6f, 0.2f), startY - barSpacing * 3);
        }

        private Image CreateStatBar(string label, Color color, float yPosition)
        {
            // Label
            var labelObj = uiManager.TextFactory.Create(label, rootObject.transform, 18, Color.white, TMPro.TextAlignmentOptions.Left);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 1);
            labelRect.anchorMax = new Vector2(0, 1);
            labelRect.pivot = new Vector2(0, 0.5f);
            labelRect.anchoredPosition = new Vector2(30, -yPosition);
            labelRect.sizeDelta = new Vector2(150, 30);

            // Bar background
            var bgObj = uiManager.ImageFactory.Create($"Bar_BG_{label}", null, rootObject.transform, new Vector2(200, 20), new Color(0.2f, 0.2f, 0.2f));
            var bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0, 1);
            bgRect.anchorMax = new Vector2(0, 1);
            bgRect.pivot = new Vector2(0, 0.5f);
            bgRect.anchoredPosition = new Vector2(150, -yPosition);

            // Bar fill
            var fillObj = new GameObject($"Bar_Fill_{label}");
            fillObj.transform.SetParent(bgObj.transform, false);

            var fillRect = fillObj.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(0, 1);
            fillRect.pivot = new Vector2(0, 0.5f);
            fillRect.anchoredPosition = Vector2.zero;
            fillRect.sizeDelta = new Vector2(100, 0);

            var fillImage = fillObj.AddComponent<Image>();
            fillImage.color = color;

            return fillImage;
        }

        private void CreateActionButtons()
        {
            float buttonY = -50;
            float buttonSpacing = 70;

            // Yaş İlerlet butonu
            var ageBtn = uiManager.ButtonFactory.Create("Yaş +1", rootObject.transform, () =>
            {
                GameManager.Instance.AdvanceYear();
            }, new Vector2(200, 60), new Color(0.3f, 0.6f, 0.3f));
            var ageRect = ageBtn.GetComponent<RectTransform>();
            ageRect.anchoredPosition = new Vector2(0, buttonY);

            // Aktiviteler
            buttonY -= buttonSpacing;
            var actBtn = uiManager.ButtonFactory.Create("Aktiviteler", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Activities);
            }, new Vector2(200, 50), new Color(0.4f, 0.5f, 0.6f));
            actBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonY);
        }

        private void CreateBottomNav()
        {
            // Alt navigasyon paneli
            var navPanel = uiManager.PanelFactory.Create("BottomNav", new Vector2(0, 120), rootObject.transform, new Color(0.1f, 0.1f, 0.15f, 1f));
            var navRect = navPanel.GetComponent<RectTransform>();
            navRect.anchorMin = new Vector2(0, 0);
            navRect.anchorMax = new Vector2(1, 0);
            navRect.pivot = new Vector2(0.5f, 0);
            navRect.anchoredPosition = Vector2.zero;
            navRect.sizeDelta = new Vector2(0, 120);

            // Horizontal layout
            var layout = navPanel.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 10;
            layout.padding = new RectOffset(20, 20, 10, 10);
            layout.childAlignment = TextAnchor.MiddleCenter;

            // Nav butonları
            CreateNavButton("İş", navPanel.transform, ScreenType.Career);
            CreateNavButton("İlişki", navPanel.transform, ScreenType.Relationships);
            CreateNavButton("Miras", navPanel.transform, ScreenType.Legacy);
            CreateNavButton("Hayvan", navPanel.transform, ScreenType.Pets);
            CreateNavButton("Menü", navPanel.transform, ScreenType.Settings);
        }

        private void CreateNavButton(string text, Transform parent, ScreenType targetScreen)
        {
            var btn = uiManager.ButtonFactory.Create(text, parent, () =>
            {
                uiManager.ShowScreen(targetScreen);
            }, new Vector2(80, 80), new Color(0.3f, 0.3f, 0.4f));
        }

        public override void Refresh()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Bilgileri güncelle
            nameText.text = character.Name;
            ageText.text = $"Yaş: {character.Age}";
            moneyText.text = $"₺{character.Stats.Money:N0}";

            // Stat barlarını güncelle
            UpdateStatBar(healthBar, character.Stats.Health);
            UpdateStatBar(happinessBar, character.Stats.Happiness);
            UpdateStatBar(intelligenceBar, character.Stats.Intelligence);
            UpdateStatBar(appearanceBar, character.Stats.Appearance);
        }

        private void UpdateStatBar(Image bar, float value)
        {
            float percentage = value / 100f;
            var rect = bar.GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(percentage, 1);
        }
    }
}
