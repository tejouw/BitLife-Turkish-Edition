using UnityEngine;
using TMPro;
using BitLifeTR.Core;
using BitLifeTR.Data;
using BitLifeTR.Systems;

namespace BitLifeTR.UI
{
    /// <summary>
    /// İlişkiler ekranı
    /// </summary>
    public class RelationshipsScreen : BaseScreen
    {
        private Transform contentParent;

        public RelationshipsScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Relationships", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            var titleText = uiManager.TextFactory.Create("İlişkiler", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            var scrollView = uiManager.ScrollViewFactory.Create("RelScroll", rootObject.transform, new Vector2(500, 600));
            scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
            contentParent = uiManager.ScrollViewFactory.GetContent(scrollView);

            var backBtn = uiManager.ButtonFactory.Create("Geri", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Game);
            }, new Vector2(150, 50), new Color(0.5f, 0.3f, 0.3f));
            backBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -380);
        }

        public override void Refresh()
        {
            foreach (Transform child in contentParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            foreach (var rel in character.Relationships)
            {
                CreateRelationshipItem(rel);
            }
        }

        private void CreateRelationshipItem(RelationshipData rel)
        {
            string status = rel.IsAlive ? "" : " (Vefat)";
            string info = $"{rel.Name}{status} - {GetRelationTypeName(rel.RelationType)}";

            var btn = uiManager.ButtonFactory.Create(info, contentParent, () =>
            {
                // İlişki detayları
            }, new Vector2(450, 60), rel.IsAlive ? new Color(0.3f, 0.4f, 0.5f) : new Color(0.3f, 0.3f, 0.3f));
        }

        private string GetRelationTypeName(RelationType type)
        {
            return type switch
            {
                RelationType.Parent => "Ebeveyn",
                RelationType.Sibling => "Kardeş",
                RelationType.Child => "Çocuk",
                RelationType.Spouse => "Eş",
                RelationType.Friend => "Arkadaş",
                RelationType.Romantic => "Sevgili",
                _ => "Diğer"
            };
        }
    }

    /// <summary>
    /// Kariyer ekranı
    /// </summary>
    public class CareerScreen : BaseScreen
    {
        private Transform contentParent;

        public CareerScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Career", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            var titleText = uiManager.TextFactory.Create("Kariyer", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            var scrollView = uiManager.ScrollViewFactory.Create("CareerScroll", rootObject.transform, new Vector2(500, 600));
            scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
            contentParent = uiManager.ScrollViewFactory.GetContent(scrollView);

            var backBtn = uiManager.ButtonFactory.Create("Geri", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Game);
            }, new Vector2(150, 50), new Color(0.5f, 0.3f, 0.3f));
            backBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -380);
        }

        public override void Refresh()
        {
            foreach (Transform child in contentParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Mevcut iş durumu
            if (character.Career.IsEmployed)
            {
                uiManager.TextFactory.Create($"Mevcut İş: {character.Career.CurrentJob}", contentParent, 20, Color.white);
                uiManager.TextFactory.Create($"Maaş: ₺{character.Career.Salary:N0}", contentParent, 18, new Color(0.3f, 0.8f, 0.3f));
            }
            else
            {
                uiManager.TextFactory.Create("İşsizsin", contentParent, 20, new Color(0.8f, 0.3f, 0.3f));
            }

            // İş ara butonu
            var searchBtn = uiManager.ButtonFactory.Create("İş Ara", contentParent, () =>
            {
                ShowJobListings(character);
            }, new Vector2(200, 50), new Color(0.3f, 0.5f, 0.7f));
        }

        private void ShowJobListings(CharacterData character)
        {
            var jobs = GameManager.Instance.CareerSystem.GetAvailableJobs(character);
            // İş listesini göster
        }
    }

    /// <summary>
    /// Eğitim ekranı
    /// </summary>
    public class EducationScreen : BaseScreen
    {
        public EducationScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Education", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            var titleText = uiManager.TextFactory.Create("Eğitim", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            var backBtn = uiManager.ButtonFactory.Create("Geri", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Game);
            }, new Vector2(150, 50), new Color(0.5f, 0.3f, 0.3f));
            backBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -380);
        }
    }

    /// <summary>
    /// Aktiviteler ekranı
    /// </summary>
    public class ActivitiesScreen : BaseScreen
    {
        public ActivitiesScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Activities", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            var titleText = uiManager.TextFactory.Create("Aktiviteler", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            // Aktivite butonları
            float y = 200;
            CreateActivityButton("Egzersiz Yap", ActivityType.Exercise, y);
            CreateActivityButton("Kitap Oku", ActivityType.ReadBook, y - 70);
            CreateActivityButton("Meditasyon", ActivityType.Meditation, y - 140);
            CreateActivityButton("Parti", ActivityType.Party, y - 210);
            CreateActivityButton("Seyahat", ActivityType.Travel, y - 280);

            var backBtn = uiManager.ButtonFactory.Create("Geri", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Game);
            }, new Vector2(150, 50), new Color(0.5f, 0.3f, 0.3f));
            backBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -380);
        }

        private void CreateActivityButton(string label, ActivityType activity, float y)
        {
            var btn = uiManager.ButtonFactory.Create(label, rootObject.transform, () =>
            {
                var character = GameManager.Instance.CurrentCharacter;
                GameManager.Instance.StatSystem.ApplyActivity(character, activity);
                uiManager.ShowPopup("Aktivite", $"{label} yaptın!", null, null);
            }, new Vector2(300, 50), new Color(0.3f, 0.5f, 0.6f));
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
        }
    }

    /// <summary>
    /// Varlıklar ekranı
    /// </summary>
    public class AssetsScreen : BaseScreen
    {
        public AssetsScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Assets", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            var titleText = uiManager.TextFactory.Create("Varlıklar", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            var backBtn = uiManager.ButtonFactory.Create("Geri", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Game);
            }, new Vector2(150, 50), new Color(0.5f, 0.3f, 0.3f));
            backBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -380);
        }
    }

    /// <summary>
    /// Ayarlar ekranı
    /// </summary>
    public class SettingsScreen : BaseScreen
    {
        public SettingsScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Settings", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            var titleText = uiManager.TextFactory.Create("Ayarlar", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            // Ana menüye dön
            var menuBtn = uiManager.ButtonFactory.Create("Ana Menü", rootObject.transform, () =>
            {
                GameManager.Instance.ReturnToMainMenu();
            }, new Vector2(200, 50), new Color(0.5f, 0.3f, 0.3f));
            menuBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);

            // Kaydet
            var saveBtn = uiManager.ButtonFactory.Create("Kaydet", rootObject.transform, () =>
            {
                GameManager.Instance.SaveGame();
                uiManager.ShowPopup("Kayıt", "Oyun kaydedildi!", null, null);
            }, new Vector2(200, 50), new Color(0.3f, 0.5f, 0.3f));
            saveBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);

            var backBtn = uiManager.ButtonFactory.Create("Geri", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Game);
            }, new Vector2(150, 50), new Color(0.5f, 0.3f, 0.3f));
            backBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -380);
        }
    }

    /// <summary>
    /// Oyun sonu ekranı
    /// </summary>
    public class GameOverScreen : BaseScreen
    {
        public GameOverScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("GameOver", null, null, new Color(0.05f, 0.05f, 0.08f, 1f));

            var titleText = uiManager.TextFactory.Create("Hayatın Sona Erdi", rootObject.transform, 42, new Color(0.8f, 0.2f, 0.2f));
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);

            var menuBtn = uiManager.ButtonFactory.Create("Ana Menü", rootObject.transform, () =>
            {
                GameManager.Instance.ReturnToMainMenu();
            }, new Vector2(200, 50), new Color(0.5f, 0.5f, 0.5f));
            menuBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
        }
    }

    /// <summary>
    /// Olay ekranı
    /// </summary>
    public class EventScreen : BaseScreen
    {
        private GameEvent currentEvent;
        private System.Action<int> onChoiceCallback;
        private Transform choicesParent;
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI descText;

        public EventScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Event", null, null, new Color(0.1f, 0.1f, 0.15f, 0.98f));

            var titleObj = uiManager.TextFactory.Create("Olay", rootObject.transform, 32, Color.white);
            titleObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);
            titleText = titleObj.GetComponent<TextMeshProUGUI>();

            var descObj = uiManager.TextFactory.Create("Açıklama", rootObject.transform, 24, new Color(0.8f, 0.8f, 0.8f));
            var descRect = descObj.GetComponent<RectTransform>();
            descRect.anchoredPosition = new Vector2(0, 180);
            descRect.sizeDelta = new Vector2(500, 100);
            descText = descObj.GetComponent<TextMeshProUGUI>();

            // Seçenekler için parent
            var choicesPanel = uiManager.PanelFactory.CreateWithLayout("Choices", new Vector2(400, 300), rootObject.transform, true, 15);
            choicesPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
            choicesParent = choicesPanel.transform;
        }

        public void ShowEvent(GameEvent evt, System.Action<int> callback)
        {
            currentEvent = evt;
            onChoiceCallback = callback;
            Refresh();
        }

        public override void Refresh()
        {
            if (currentEvent == null) return;

            titleText.text = currentEvent.Title;
            descText.text = currentEvent.Description;

            // Eski seçenekleri temizle
            foreach (Transform child in choicesParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            // Yeni seçenekleri oluştur
            for (int i = 0; i < currentEvent.Choices.Count; i++)
            {
                int index = i;
                var choice = currentEvent.Choices[i];

                var btn = uiManager.ButtonFactory.Create(choice.Text, choicesParent, () =>
                {
                    onChoiceCallback?.Invoke(index);
                    uiManager.ShowScreen(ScreenType.Game);
                }, new Vector2(350, 50), new Color(0.3f, 0.5f, 0.6f));
            }
        }
    }
}
