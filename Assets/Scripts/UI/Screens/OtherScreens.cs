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
        private Transform contentParent;
        private System.Collections.Generic.List<RelationshipData> eligibleChildren;
        private DeathReason deathReason;

        public GameOverScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("GameOver", null, null, new Color(0.05f, 0.05f, 0.08f, 1f));

            // Event dinleyicisi ekle
            EventBus.Subscribe<LegacyTransitionEvent>(OnLegacyTransition);
        }

        private void OnLegacyTransition(LegacyTransitionEvent evt)
        {
            eligibleChildren = evt.EligibleChildren;
            deathReason = evt.DeathReason;
        }

        public override void Show()
        {
            base.Show();
            Refresh();
        }

        public override void Refresh()
        {
            // Eski içeriği temizle
            foreach (Transform child in rootObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Başlık
            var titleText = uiManager.TextFactory.Create("Hayatın Sona Erdi", rootObject.transform, 42, new Color(0.8f, 0.2f, 0.2f));
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 350);

            // Karakter bilgisi
            var nameText = uiManager.TextFactory.Create($"{character.Name} {character.Surname}", rootObject.transform, 28, Color.white);
            nameText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 280);

            var ageText = uiManager.TextFactory.Create($"{character.BirthYear} - {character.BirthYear + character.Age} ({character.Age} yaşında)", rootObject.transform, 20, new Color(0.7f, 0.7f, 0.7f));
            ageText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 240);

            // Net servet
            float netWorth = GameManager.Instance.LegacySystem.CalculateNetWorth(character);
            var worthText = uiManager.TextFactory.Create($"Net Servet: ₺{netWorth:N0}", rootObject.transform, 22, new Color(0.3f, 0.8f, 0.3f));
            worthText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 190);

            // Nesil bilgisi
            var genText = uiManager.TextFactory.Create($"Nesil: {character.Legacy.Generation}", rootObject.transform, 18, new Color(0.6f, 0.6f, 0.8f));
            genText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150);

            float buttonY = 50;

            // Çocuk varsa devam seçeneği göster
            if (eligibleChildren != null && eligibleChildren.Count > 0)
            {
                var continueLabel = uiManager.TextFactory.Create("Çocuklarından biri olarak devam et:", rootObject.transform, 18, new Color(0.8f, 0.8f, 0.5f));
                continueLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonY);
                buttonY -= 40;

                foreach (var child in eligibleChildren)
                {
                    string childInfo = $"{child.Name} ({child.Age} yaş)";
                    RelationshipData capturedChild = child;

                    var childBtn = uiManager.ButtonFactory.Create(childInfo, rootObject.transform, () =>
                    {
                        GameManager.Instance.ContinueAsChild(capturedChild, deathReason);
                    }, new Vector2(250, 45), new Color(0.3f, 0.6f, 0.4f));
                    childBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonY);
                    buttonY -= 55;
                }

                buttonY -= 20;
            }

            // Aile geçmişi butonu
            if (character.Legacy.Ancestors.Count > 0)
            {
                var historyBtn = uiManager.ButtonFactory.Create("Aile Geçmişi", rootObject.transform, () =>
                {
                    ShowFamilyHistory(character);
                }, new Vector2(200, 45), new Color(0.4f, 0.4f, 0.6f));
                historyBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonY);
                buttonY -= 55;
            }

            // Yeni oyun butonu
            var newGameBtn = uiManager.ButtonFactory.Create("Yeni Oyun", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.NewGame);
            }, new Vector2(200, 45), new Color(0.3f, 0.5f, 0.7f));
            newGameBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonY);
            buttonY -= 55;

            // Ana menü butonu
            var menuBtn = uiManager.ButtonFactory.Create("Ana Menü", rootObject.transform, () =>
            {
                GameManager.Instance.ReturnToMainMenu();
            }, new Vector2(200, 45), new Color(0.5f, 0.3f, 0.3f));
            menuBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonY);
        }

        private void ShowFamilyHistory(CharacterData character)
        {
            var popup = uiManager.PanelFactory.Create("FamilyHistory", new Vector2(700, 800));
            popup.GetComponent<UnityEngine.UI.Image>().color = new Color(0.1f, 0.1f, 0.15f, 0.98f);

            var title = uiManager.TextFactory.Create($"{character.Surname} Ailesi Geçmişi", popup.transform, 28, Color.white);
            title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 350);

            // Scroll view
            var scrollView = uiManager.ScrollViewFactory.Create("HistoryScroll", popup.transform, new Vector2(650, 550));
            scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30);
            var content = uiManager.ScrollViewFactory.GetContent(scrollView);

            // Ataları listele
            foreach (var ancestor in character.Legacy.Ancestors)
            {
                var panel = uiManager.PanelFactory.CreateWithLayout("Ancestor", new Vector2(600, 120), content, true, 5);
                panel.GetComponent<UnityEngine.UI.Image>().color = new Color(0.2f, 0.2f, 0.25f, 0.9f);

                uiManager.TextFactory.Create($"{ancestor.Name} {ancestor.Surname}", panel.transform, 20, Color.white);
                uiManager.TextFactory.Create($"{ancestor.BirthYear} - {ancestor.DeathYear} ({ancestor.DeathAge} yaş)", panel.transform, 16, new Color(0.7f, 0.7f, 0.7f));
                uiManager.TextFactory.Create($"Meslek: {ancestor.Occupation}", panel.transform, 14, new Color(0.6f, 0.6f, 0.8f));
                uiManager.TextFactory.Create($"Servet: ₺{ancestor.NetWorth:N0}", panel.transform, 14, new Color(0.3f, 0.8f, 0.3f));

                if (ancestor.Achievements.Count > 0)
                {
                    string achievements = string.Join(", ", ancestor.Achievements);
                    uiManager.TextFactory.Create($"Başarılar: {achievements}", panel.transform, 12, new Color(0.8f, 0.8f, 0.3f));
                }
            }

            // Kapat butonu
            var closeBtn = uiManager.ButtonFactory.Create("Kapat", popup.transform, () =>
            {
                GameObject.Destroy(popup);
            }, new Vector2(150, 45), new Color(0.5f, 0.3f, 0.3f));
            closeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -360);
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

    /// <summary>
    /// Evcil hayvanlar ekranı
    /// </summary>
    public class PetsScreen : BaseScreen
    {
        private Transform contentParent;

        public PetsScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Pets", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            var titleText = uiManager.TextFactory.Create("Evcil Hayvanlar", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            var scrollView = uiManager.ScrollViewFactory.Create("PetsScroll", rootObject.transform, new Vector2(500, 500));
            scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            contentParent = uiManager.ScrollViewFactory.GetContent(scrollView);

            // Hayvan sahiplen butonu
            var adoptBtn = uiManager.ButtonFactory.Create("Hayvan Sahiplen", rootObject.transform, () =>
            {
                ShowAdoptionMenu();
            }, new Vector2(200, 50), new Color(0.3f, 0.6f, 0.4f));
            adoptBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -320);

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

            var alivePets = GameManager.Instance.PetSystem.GetAlivePets(character);

            if (alivePets.Count == 0)
            {
                uiManager.TextFactory.Create("Henüz evcil hayvanın yok.", contentParent, 20, new Color(0.6f, 0.6f, 0.6f));
            }
            else
            {
                foreach (var pet in alivePets)
                {
                    CreatePetItem(pet);
                }
            }
        }

        private void CreatePetItem(PetData pet)
        {
            string healthColor = pet.Health > 50 ? "white" : (pet.Health > 25 ? "yellow" : "red");
            string info = $"{pet.Name} ({pet.GetTypeName()}) - Yaş: {pet.Age}";

            var btn = uiManager.ButtonFactory.Create(info, contentParent, () =>
            {
                ShowPetDetails(pet);
            }, new Vector2(450, 60), new Color(0.3f, 0.4f, 0.5f));
        }

        private void ShowPetDetails(PetData pet)
        {
            // Detay popup'ı oluştur
            var popup = uiManager.PanelFactory.Create("PetDetail", new Vector2(600, 500));
            popup.GetComponent<UnityEngine.UI.Image>().color = new Color(0.15f, 0.15f, 0.2f, 0.98f);

            float y = 180;

            // Başlık
            var title = uiManager.TextFactory.Create($"{pet.Name} - {pet.GetTypeName()}", popup.transform, 28, Color.white);
            title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

            y -= 50;

            // Statlar
            uiManager.TextFactory.Create($"Yaş: {pet.Age} / {pet.GetMaxAge()}", popup.transform, 18, new Color(0.8f, 0.8f, 0.8f));
            var ageText = popup.transform.GetChild(popup.transform.childCount - 1);
            ageText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

            y -= 30;

            Color healthColor = pet.Health > 50 ? new Color(0.3f, 0.8f, 0.3f) : (pet.Health > 25 ? new Color(0.8f, 0.8f, 0.3f) : new Color(0.8f, 0.3f, 0.3f));
            uiManager.TextFactory.Create($"Sağlık: {pet.Health:F0}%", popup.transform, 18, healthColor);
            popup.transform.GetChild(popup.transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

            y -= 30;

            uiManager.TextFactory.Create($"Mutluluk: {pet.Happiness:F0}%", popup.transform, 18, new Color(0.8f, 0.8f, 0.3f));
            popup.transform.GetChild(popup.transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

            y -= 30;

            uiManager.TextFactory.Create($"İlişki: {pet.RelationshipLevel:F0}%", popup.transform, 18, new Color(0.3f, 0.6f, 0.8f));
            popup.transform.GetChild(popup.transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

            y -= 50;

            // Aksiyonlar
            var playBtn = uiManager.ButtonFactory.Create("Oyna", popup.transform, () =>
            {
                GameManager.Instance.PetSystem.PlayWithPet(GameManager.Instance.CurrentCharacter, pet.Id);
                GameObject.Destroy(popup);
                Refresh();
            }, new Vector2(120, 45), new Color(0.3f, 0.5f, 0.7f));
            playBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-130, y);

            var feedBtn = uiManager.ButtonFactory.Create("Besle", popup.transform, () =>
            {
                GameManager.Instance.PetSystem.FeedPet(GameManager.Instance.CurrentCharacter, pet.Id);
                GameObject.Destroy(popup);
                Refresh();
            }, new Vector2(120, 45), new Color(0.5f, 0.6f, 0.3f));
            feedBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

            var vetBtn = uiManager.ButtonFactory.Create("Veteriner", popup.transform, () =>
            {
                GameManager.Instance.PetSystem.TakeToVet(GameManager.Instance.CurrentCharacter, pet.Id);
                GameObject.Destroy(popup);
                Refresh();
            }, new Vector2(120, 45), new Color(0.6f, 0.4f, 0.6f));
            vetBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(130, y);

            y -= 60;

            // Başka birine ver
            var giveAwayBtn = uiManager.ButtonFactory.Create("Başkasına Ver", popup.transform, () =>
            {
                GameManager.Instance.PetSystem.GiveAwayPet(GameManager.Instance.CurrentCharacter, pet.Id);
                GameObject.Destroy(popup);
                Refresh();
            }, new Vector2(200, 45), new Color(0.6f, 0.5f, 0.3f));
            giveAwayBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

            y -= 60;

            // Kapat butonu
            var closeBtn = uiManager.ButtonFactory.Create("Kapat", popup.transform, () =>
            {
                GameObject.Destroy(popup);
            }, new Vector2(150, 45), new Color(0.5f, 0.3f, 0.3f));
            closeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
        }

        private void ShowAdoptionMenu()
        {
            var popup = uiManager.PanelFactory.Create("AdoptPet", new Vector2(600, 600));
            popup.GetComponent<UnityEngine.UI.Image>().color = new Color(0.15f, 0.15f, 0.2f, 0.98f);

            var title = uiManager.TextFactory.Create("Hayvan Sahiplen", popup.transform, 28, Color.white);
            title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 250);

            var petSystem = GameManager.Instance.PetSystem;
            var petTypes = petSystem.GetAvailablePetTypes();

            float y = 180;

            foreach (var petType in petTypes)
            {
                string typeName = petSystem.GetPetTypeName(petType);
                float price = petSystem.GetPetPrice(petType);
                string btnText = $"{typeName} - ₺{price:N0}";

                PetType capturedType = petType;
                var btn = uiManager.ButtonFactory.Create(btnText, popup.transform, () =>
                {
                    ShowNameInputForPet(capturedType, popup);
                }, new Vector2(250, 45), new Color(0.3f, 0.5f, 0.6f));
                btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

                y -= 55;
            }

            // Kapat butonu
            var closeBtn = uiManager.ButtonFactory.Create("İptal", popup.transform, () =>
            {
                GameObject.Destroy(popup);
            }, new Vector2(150, 45), new Color(0.5f, 0.3f, 0.3f));
            closeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -250);
        }

        private void ShowNameInputForPet(PetType petType, GameObject adoptPopup)
        {
            GameObject.Destroy(adoptPopup);

            var popup = uiManager.PanelFactory.Create("NamePet", new Vector2(500, 300));
            popup.GetComponent<UnityEngine.UI.Image>().color = new Color(0.15f, 0.15f, 0.2f, 0.98f);

            string typeName = GameManager.Instance.PetSystem.GetPetTypeName(petType);
            var title = uiManager.TextFactory.Create($"{typeName} için isim seç", popup.transform, 24, Color.white);
            title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);

            // İsim input'u
            var inputField = uiManager.InputFactory.Create("PetNameInput", popup.transform, "İsim girin...", new Vector2(300, 50));
            inputField.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);

            // Onayla butonu
            var confirmBtn = uiManager.ButtonFactory.Create("Sahiplen", popup.transform, () =>
            {
                var input = inputField.GetComponent<TMPro.TMP_InputField>();
                string petName = string.IsNullOrEmpty(input.text) ? GetRandomPetName(petType) : input.text;

                bool success = GameManager.Instance.PetSystem.AdoptPet(
                    GameManager.Instance.CurrentCharacter,
                    petType,
                    petName
                );

                if (success)
                {
                    uiManager.ShowPopup("Tebrikler!", $"{petName} adlı {typeName} sahiplendin!", null, null);
                }
                else
                {
                    uiManager.ShowPopup("Hata", "Hayvan sahiplenemedi. Yeterli paran olmayabilir.", null, null);
                }

                GameObject.Destroy(popup);
                Refresh();
            }, new Vector2(150, 45), new Color(0.3f, 0.6f, 0.4f));
            confirmBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80, -80);

            // İptal butonu
            var cancelBtn = uiManager.ButtonFactory.Create("İptal", popup.transform, () =>
            {
                GameObject.Destroy(popup);
            }, new Vector2(150, 45), new Color(0.5f, 0.3f, 0.3f));
            cancelBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(80, -80);
        }

        private string GetRandomPetName(PetType type)
        {
            string[] dogNames = { "Karabaş", "Pamuk", "Çomar", "Boncuk", "Duman", "Findik", "Tarçın", "Şanslı" };
            string[] catNames = { "Minnoş", "Tekir", "Boncuk", "Pamuk", "Şeker", "Tarçın", "Maviş", "Kedik" };
            string[] birdNames = { "Maviş", "Civciv", "Muhabbet", "Kanarya", "Şeker", "Tatlı" };
            string[] fishNames = { "Pırıl", "Balık", "Nemo", "Goldi", "Şans" };
            string[] hamsterNames = { "Fıstık", "Ceviz", "Pofuduk", "Mırnav", "Topik" };
            string[] rabbitNames = { "Pamuk", "Havuç", "Zıpzıp", "Tavşi", "Minnoş" };
            string[] turtleNames = { "Tospik", "Yavaş", "Kabuk", "Ninja", "Tosbik" };
            string[] parrotNames = { "Papağan", "Konuşkan", "Renkli", "Tüylü", "Şamata" };

            return type switch
            {
                PetType.Dog => dogNames[Random.Range(0, dogNames.Length)],
                PetType.Cat => catNames[Random.Range(0, catNames.Length)],
                PetType.Bird => birdNames[Random.Range(0, birdNames.Length)],
                PetType.Fish => fishNames[Random.Range(0, fishNames.Length)],
                PetType.Hamster => hamsterNames[Random.Range(0, hamsterNames.Length)],
                PetType.Rabbit => rabbitNames[Random.Range(0, rabbitNames.Length)],
                PetType.Turtle => turtleNames[Random.Range(0, turtleNames.Length)],
                PetType.Parrot => parrotNames[Random.Range(0, parrotNames.Length)],
                _ => "Dostum"
            };
        }
    }

    /// <summary>
    /// Miras/Nesil ekranı
    /// </summary>
    public class LegacyScreen : BaseScreen
    {
        private Transform contentParent;

        public LegacyScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Legacy", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            var titleText = uiManager.TextFactory.Create("Aile Mirası", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            var scrollView = uiManager.ScrollViewFactory.Create("LegacyScroll", rootObject.transform, new Vector2(500, 550));
            scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20);
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

            // Nesil bilgisi
            var genPanel = uiManager.PanelFactory.CreateWithLayout("GenInfo", new Vector2(450, 120), contentParent, true, 5);
            genPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0.2f, 0.2f, 0.3f, 0.9f);

            uiManager.TextFactory.Create($"{character.Surname} Ailesi", genPanel.transform, 24, Color.white);
            uiManager.TextFactory.Create($"Nesil: {character.Legacy.Generation}", genPanel.transform, 18, new Color(0.6f, 0.6f, 0.8f));
            uiManager.TextFactory.Create($"Toplam Aile Serveti: ₺{character.Legacy.TotalFamilyWealth:N0}", genPanel.transform, 16, new Color(0.3f, 0.8f, 0.3f));

            // Miras aldıysa göster
            if (character.Legacy.HasReceivedInheritance)
            {
                var inheritPanel = uiManager.PanelFactory.CreateWithLayout("Inherit", new Vector2(450, 80), contentParent, true, 5);
                inheritPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0.3f, 0.3f, 0.2f, 0.9f);

                uiManager.TextFactory.Create("Alınan Miras", inheritPanel.transform, 18, new Color(0.8f, 0.8f, 0.3f));
                uiManager.TextFactory.Create($"₺{character.Legacy.InheritedAmount:N0}", inheritPanel.transform, 16, new Color(0.3f, 0.8f, 0.3f));
            }

            // Vasiyetname butonu
            var willBtn = uiManager.ButtonFactory.Create("Vasiyetname Düzenle", contentParent, () =>
            {
                uiManager.ShowScreen(ScreenType.Will);
            }, new Vector2(250, 50), new Color(0.4f, 0.4f, 0.6f));

            // Atalar
            if (character.Legacy.Ancestors.Count > 0)
            {
                var ancestorsLabel = uiManager.TextFactory.Create("=== Atalar ===", contentParent, 20, Color.white);

                foreach (var ancestor in character.Legacy.Ancestors)
                {
                    var panel = uiManager.PanelFactory.CreateWithLayout("Ancestor", new Vector2(450, 100), contentParent, true, 3);
                    panel.GetComponent<UnityEngine.UI.Image>().color = new Color(0.15f, 0.15f, 0.2f, 0.9f);

                    uiManager.TextFactory.Create($"{ancestor.Name} {ancestor.Surname}", panel.transform, 18, Color.white);
                    uiManager.TextFactory.Create($"{ancestor.BirthYear} - {ancestor.DeathYear}", panel.transform, 14, new Color(0.6f, 0.6f, 0.6f));
                    uiManager.TextFactory.Create($"Servet: ₺{ancestor.NetWorth:N0}", panel.transform, 14, new Color(0.3f, 0.7f, 0.3f));
                }
            }
        }
    }

    /// <summary>
    /// Vasiyetname ekranı
    /// </summary>
    public class WillScreen : BaseScreen
    {
        private Transform contentParent;

        public WillScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Will", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            var titleText = uiManager.TextFactory.Create("Vasiyetname", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            var scrollView = uiManager.ScrollViewFactory.Create("WillScroll", rootObject.transform, new Vector2(500, 500));
            scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            contentParent = uiManager.ScrollViewFactory.GetContent(scrollView);

            var backBtn = uiManager.ButtonFactory.Create("Geri", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Legacy);
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

            var legacySystem = GameManager.Instance.LegacySystem;
            float netWorth = legacySystem.CalculateNetWorth(character);

            // Net servet bilgisi
            var worthText = uiManager.TextFactory.Create($"Net Servetiniz: ₺{netWorth:N0}", contentParent, 20, new Color(0.3f, 0.8f, 0.3f));

            // Tahmini vergi
            float tax = legacySystem.CalculateInheritanceTax(netWorth);
            uiManager.TextFactory.Create($"Tahmini Miras Vergisi: ₺{tax:N0}", contentParent, 16, new Color(0.8f, 0.5f, 0.3f));

            // Mevcut vasiyetname durumu
            if (character.Legacy.Will.HasWill)
            {
                var statusPanel = uiManager.PanelFactory.CreateWithLayout("Status", new Vector2(450, 100), contentParent, true, 5);
                statusPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0.2f, 0.3f, 0.2f, 0.9f);

                uiManager.TextFactory.Create("Vasiyetname Mevcut", statusPanel.transform, 18, new Color(0.3f, 0.8f, 0.3f));
                uiManager.TextFactory.Create($"Son Güncelleme: {character.Legacy.Will.LastUpdatedYear}", statusPanel.transform, 14, new Color(0.6f, 0.6f, 0.6f));
                uiManager.TextFactory.Create($"Dağıtım: {GetDistributionName(character.Legacy.Will.DistributionType)}", statusPanel.transform, 14, Color.white);
            }
            else
            {
                uiManager.TextFactory.Create("Henüz vasiyetname oluşturmadınız.", contentParent, 16, new Color(0.8f, 0.5f, 0.5f));
            }

            // Dağıtım seçenekleri
            uiManager.TextFactory.Create("=== Dağıtım Şekli ===", contentParent, 18, Color.white);

            // Eşit dağıtım butonu
            var equalBtn = uiManager.ButtonFactory.Create("Eşit Dağıt", contentParent, () =>
            {
                GameManager.Instance.LegacySystem.CreateDefaultWill(character);
                uiManager.ShowPopup("Vasiyetname", "Vasiyetnameniz eşit dağıtım olarak güncellendi.", null, null);
                Refresh();
            }, new Vector2(250, 45), new Color(0.3f, 0.5f, 0.6f));

            // İlk çocuğa
            var firstChildBtn = uiManager.ButtonFactory.Create("İlk Çocuğa Bırak", contentParent, () =>
            {
                CreateFirstChildWill(character);
                Refresh();
            }, new Vector2(250, 45), new Color(0.5f, 0.4f, 0.6f));

            // Eşe bırak
            var spouseBtn = uiManager.ButtonFactory.Create("Eşe Bırak", contentParent, () =>
            {
                CreateSpouseWill(character);
                Refresh();
            }, new Vector2(250, 45), new Color(0.6f, 0.4f, 0.5f));

            // Hayır kurumuna
            var charityBtn = uiManager.ButtonFactory.Create("Hayır Kurumuna (%20)", contentParent, () =>
            {
                CreateCharityWill(character, 20);
                Refresh();
            }, new Vector2(250, 45), new Color(0.4f, 0.6f, 0.4f));
        }

        private string GetDistributionName(WillDistribution dist)
        {
            return dist switch
            {
                WillDistribution.Equal => "Eşit",
                WillDistribution.Custom => "Özel",
                WillDistribution.FirstChild => "İlk Çocuk",
                WillDistribution.Spouse => "Eş",
                WillDistribution.Charity => "Hayır Kurumu",
                _ => "Bilinmiyor"
            };
        }

        private void CreateFirstChildWill(CharacterData character)
        {
            var children = character.Relationships.FindAll(r => r.IsAlive && r.RelationType == RelationType.Child);
            if (children.Count == 0)
            {
                uiManager.ShowPopup("Hata", "Çocuğunuz bulunmuyor.", null, null);
                return;
            }

            var firstChild = children[0];
            var beneficiaries = new System.Collections.Generic.List<WillBeneficiary>
            {
                new WillBeneficiary
                {
                    RelationshipId = firstChild.Id,
                    Name = firstChild.Name,
                    RelationType = RelationType.Child,
                    Percentage = 100
                }
            };

            GameManager.Instance.LegacySystem.CreateOrUpdateWill(character, WillDistribution.FirstChild, beneficiaries);
            uiManager.ShowPopup("Vasiyetname", $"Mirasınız {firstChild.Name}'a bırakıldı.", null, null);
        }

        private void CreateSpouseWill(CharacterData character)
        {
            var spouse = character.Relationships.Find(r => r.IsAlive && r.RelationType == RelationType.Spouse);
            if (spouse == null)
            {
                uiManager.ShowPopup("Hata", "Eşiniz bulunmuyor.", null, null);
                return;
            }

            var beneficiaries = new System.Collections.Generic.List<WillBeneficiary>
            {
                new WillBeneficiary
                {
                    RelationshipId = spouse.Id,
                    Name = spouse.Name,
                    RelationType = RelationType.Spouse,
                    Percentage = 100
                }
            };

            GameManager.Instance.LegacySystem.CreateOrUpdateWill(character, WillDistribution.Spouse, beneficiaries);
            uiManager.ShowPopup("Vasiyetname", $"Mirasınız eşiniz {spouse.Name}'a bırakıldı.", null, null);
        }

        private void CreateCharityWill(CharacterData character, float charityPercentage)
        {
            var beneficiaries = new System.Collections.Generic.List<WillBeneficiary>();
            var livingFamily = character.Relationships.FindAll(r => r.IsAlive &&
                (r.RelationType == RelationType.Spouse || r.RelationType == RelationType.Child));

            float remainingPercentage = 100 - charityPercentage;
            if (livingFamily.Count > 0)
            {
                float perPerson = remainingPercentage / livingFamily.Count;
                foreach (var family in livingFamily)
                {
                    beneficiaries.Add(new WillBeneficiary
                    {
                        RelationshipId = family.Id,
                        Name = family.Name,
                        RelationType = family.RelationType,
                        Percentage = perPerson
                    });
                }
            }

            GameManager.Instance.LegacySystem.CreateOrUpdateWill(
                character,
                WillDistribution.Charity,
                beneficiaries,
                charityPercentage,
                "Türk Kızılayı"
            );
            uiManager.ShowPopup("Vasiyetname", $"Mirasınızın %{charityPercentage}'i hayır kurumuna bağışlanacak.", null, null);
        }
    }
}
