using UnityEngine;
using UnityEngine.UI;
using BitLifeTR.Core;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Ana menü ekranı
    /// </summary>
    public class MainMenuScreen : BaseScreen
    {
        public MainMenuScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            // Ana panel
            rootObject = uiManager.PanelFactory.Create("MainMenu", null, null, new Color(0.05f, 0.05f, 0.15f, 1f));

            // Başlık
            var titleText = uiManager.TextFactory.Create("BitLife", rootObject.transform, 72, Color.white);
            var titleRect = titleText.GetComponent<RectTransform>();
            titleRect.anchoredPosition = new Vector2(0, 400);

            // Alt başlık
            var subtitleText = uiManager.TextFactory.Create("Türk Versiyonu", rootObject.transform, 32, new Color(0.7f, 0.7f, 0.7f));
            var subtitleRect = subtitleText.GetComponent<RectTransform>();
            subtitleRect.anchoredPosition = new Vector2(0, 320);

            // Butonlar
            CreateMenuButtons();
        }

        private void CreateMenuButtons()
        {
            float buttonY = 100;
            float buttonSpacing = 80;

            // Yeni Oyun butonu
            var newGameBtn = uiManager.ButtonFactory.Create("Yeni Hayat", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.NewGame);
            }, new Vector2(300, 60), new Color(0.2f, 0.7f, 0.3f));
            newGameBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonY);

            // Devam Et butonu
            buttonY -= buttonSpacing;
            var continueBtn = uiManager.ButtonFactory.Create("Devam Et", rootObject.transform, () =>
            {
                GameManager.Instance.LoadGame();
            }, new Vector2(300, 60), new Color(0.3f, 0.5f, 0.8f));
            var continueRect = continueBtn.GetComponent<RectTransform>();
            continueRect.anchoredPosition = new Vector2(0, buttonY);

            // Kayıt yoksa butonu devre dışı bırak
            if (!GameManager.Instance.SaveManager.HasSaveFile())
            {
                continueBtn.GetComponent<Button>().interactable = false;
            }

            // Ayarlar butonu
            buttonY -= buttonSpacing;
            var settingsBtn = uiManager.ButtonFactory.Create("Ayarlar", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Settings);
            }, new Vector2(300, 60), new Color(0.5f, 0.5f, 0.5f));
            settingsBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonY);

            // Çıkış butonu
            buttonY -= buttonSpacing;
            var exitBtn = uiManager.ButtonFactory.Create("Çıkış", rootObject.transform, () =>
            {
                Application.Quit();
            }, new Vector2(300, 60), new Color(0.8f, 0.3f, 0.3f));
            exitBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonY);

            // Versiyon bilgisi
            var versionText = uiManager.TextFactory.Create("v1.0.0", rootObject.transform, 18, new Color(0.5f, 0.5f, 0.5f));
            var versionRect = versionText.GetComponent<RectTransform>();
            versionRect.anchoredPosition = new Vector2(0, -400);
        }

        public override void Refresh()
        {
            // Devam et butonunu güncelle
            // Her gösterimde kayıt durumunu kontrol et
        }
    }
}
