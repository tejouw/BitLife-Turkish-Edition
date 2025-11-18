using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BitLifeTR.Core;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Yeni oyun oluşturma ekranı
    /// </summary>
    public class NewGameScreen : BaseScreen
    {
        private TMP_InputField nameInput;
        private Gender selectedGender = Gender.Male;
        private Button maleButton;
        private Button femaleButton;

        public NewGameScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("NewGame", null, null, new Color(0.05f, 0.1f, 0.15f, 1f));

            // Başlık
            var titleText = uiManager.TextFactory.Create("Yeni Hayat", rootObject.transform, 48, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 350);

            // İsim girişi
            var nameLabel = uiManager.TextFactory.Create("Adın:", rootObject.transform, 24, Color.white);
            nameLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 200);

            var nameInputObj = uiManager.InputFactory.Create("İsim girin...", rootObject.transform, new Vector2(400, 60));
            nameInputObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 140);
            nameInput = nameInputObj.GetComponent<TMP_InputField>();

            // Cinsiyet seçimi
            var genderLabel = uiManager.TextFactory.Create("Cinsiyet:", rootObject.transform, 24, Color.white);
            genderLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);

            // Erkek butonu
            var maleBtn = uiManager.ButtonFactory.Create("Erkek", rootObject.transform, () =>
            {
                SelectGender(Gender.Male);
            }, new Vector2(150, 50), new Color(0.3f, 0.5f, 0.8f));
            maleBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, -20);
            maleButton = maleBtn.GetComponent<Button>();

            // Kadın butonu
            var femaleBtn = uiManager.ButtonFactory.Create("Kadın", rootObject.transform, () =>
            {
                SelectGender(Gender.Female);
            }, new Vector2(150, 50), new Color(0.8f, 0.4f, 0.6f));
            femaleBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -20);
            femaleButton = femaleBtn.GetComponent<Button>();

            // Rastgele isim butonu
            var randomBtn = uiManager.ButtonFactory.Create("Rastgele", rootObject.transform, () =>
            {
                GenerateRandomName();
            }, new Vector2(200, 50), new Color(0.5f, 0.5f, 0.5f));
            randomBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);

            // Başlat butonu
            var startBtn = uiManager.ButtonFactory.Create("Hayata Başla!", rootObject.transform, () =>
            {
                StartGame();
            }, new Vector2(300, 60), new Color(0.2f, 0.7f, 0.3f));
            startBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);

            // Geri butonu
            var backBtn = uiManager.ButtonFactory.Create("Geri", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.MainMenu);
            }, new Vector2(150, 50), new Color(0.5f, 0.3f, 0.3f));
            backBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);

            // Varsayılan seçim
            SelectGender(Gender.Male);
        }

        private void SelectGender(Gender gender)
        {
            selectedGender = gender;

            // Buton renklerini güncelle
            var maleColors = maleButton.colors;
            var femaleColors = femaleButton.colors;

            if (gender == Gender.Male)
            {
                maleColors.normalColor = new Color(0.2f, 0.6f, 0.9f);
                femaleColors.normalColor = new Color(0.4f, 0.4f, 0.4f);
            }
            else
            {
                maleColors.normalColor = new Color(0.4f, 0.4f, 0.4f);
                femaleColors.normalColor = new Color(0.9f, 0.4f, 0.6f);
            }

            maleButton.colors = maleColors;
            femaleButton.colors = femaleColors;
        }

        private void GenerateRandomName()
        {
            string[] maleNames = { "Ahmet", "Mehmet", "Ali", "Mustafa", "Emre", "Burak", "Can", "Efe", "Mert", "Kaan" };
            string[] femaleNames = { "Ayşe", "Fatma", "Zeynep", "Elif", "Merve", "Selin", "Ceren", "Ece", "Naz", "İrem" };

            string name = selectedGender == Gender.Male
                ? maleNames[Random.Range(0, maleNames.Length)]
                : femaleNames[Random.Range(0, femaleNames.Length)];

            nameInput.text = name;
        }

        private void StartGame()
        {
            string characterName = nameInput.text.Trim();

            if (string.IsNullOrEmpty(characterName))
            {
                uiManager.ShowPopup("Hata", "Lütfen bir isim girin!", null, null);
                return;
            }

            GameManager.Instance.StartNewGame(characterName, selectedGender);
        }

        public override void Show()
        {
            base.Show();
            // İsim alanını temizle
            if (nameInput != null)
            {
                nameInput.text = "";
            }
        }
    }
}
