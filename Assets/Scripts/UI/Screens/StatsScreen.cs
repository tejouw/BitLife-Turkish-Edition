using UnityEngine;
using TMPro;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.UI
{
    /// <summary>
    /// İstatistikler ekranı
    /// </summary>
    public class StatsScreen : BaseScreen
    {
        private Transform contentParent;

        public StatsScreen(UIManager manager) : base(manager) { }

        protected override void Initialize()
        {
            rootObject = uiManager.PanelFactory.Create("Stats", null, null, new Color(0.08f, 0.08f, 0.12f, 1f));

            // Başlık
            var titleText = uiManager.TextFactory.Create("İstatistikler", rootObject.transform, 36, Color.white);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 380);

            // ScrollView
            var scrollView = uiManager.ScrollViewFactory.Create("StatsScroll", rootObject.transform, new Vector2(500, 600));
            scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
            contentParent = uiManager.ScrollViewFactory.GetContent(scrollView);

            // Geri butonu
            var backBtn = uiManager.ButtonFactory.Create("Geri", rootObject.transform, () =>
            {
                uiManager.ShowScreen(ScreenType.Game);
            }, new Vector2(150, 50), new Color(0.5f, 0.3f, 0.3f));
            backBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -380);
        }

        public override void Refresh()
        {
            // Eski içeriği temizle
            foreach (Transform child in contentParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Temel statlar
            CreateStatItem("Sağlık", character.Stats.Health, new Color(0.8f, 0.2f, 0.2f));
            CreateStatItem("Mutluluk", character.Stats.Happiness, new Color(0.2f, 0.8f, 0.2f));
            CreateStatItem("Zeka", character.Stats.Intelligence, new Color(0.2f, 0.6f, 0.9f));
            CreateStatItem("Görünüm", character.Stats.Appearance, new Color(0.9f, 0.6f, 0.2f));
            CreateStatItem("Şöhret", character.Stats.Fame, new Color(0.9f, 0.9f, 0.2f));
            CreateStatItem("Karma", character.Stats.Karma, new Color(0.8f, 0.4f, 0.8f));

            // Ayırıcı
            CreateSeparator();

            // Ek bilgiler
            CreateInfoItem("Para", $"₺{character.Stats.Money:N0}");
            CreateInfoItem("Yaş", character.Age.ToString());
            CreateInfoItem("Yaş Dönemi", GameManager.Instance.AgeSystem.GetAgePeriodName(character.GetAgePeriod()));
            CreateInfoItem("Doğum Yeri", character.BirthCity);

            CreateSeparator();

            // Eğitim
            CreateInfoItem("Eğitim", GetEducationLevelName(character.Education.HighestLevel));
            if (character.Education.IsEnrolled)
            {
                CreateInfoItem("Mevcut Okul", character.Education.CurrentSchool ?? "Bilinmiyor");
            }

            CreateSeparator();

            // Kariyer
            if (character.Career.IsEmployed)
            {
                CreateInfoItem("Meslek", character.Career.CurrentJob);
                CreateInfoItem("Maaş", $"₺{character.Career.Salary:N0}/ay");
                CreateInfoItem("İş Performansı", $"%{character.Career.JobPerformance:F0}");
            }
            else if (character.Career.IsRetired)
            {
                CreateInfoItem("Durum", "Emekli");
                CreateInfoItem("Emekli Maaşı", $"₺{character.Finance.MonthlyIncome:N0}/ay");
            }
            else
            {
                CreateInfoItem("Durum", "İşsiz");
            }

            CreateSeparator();

            // Sağlık detayları
            CreateInfoItem("Fitness", $"%{character.Health.Fitness:F0}");
            CreateInfoItem("Sigorta", character.Health.HasInsurance ? "Var" : "Yok");

            if (character.Health.IsSmoker)
                CreateInfoItem("", "Sigara içiyor");
            if (character.Health.IsDrinker)
                CreateInfoItem("", "Alkol kullanıyor");

            // Hastalıklar
            if (character.Health.CurrentDiseases.Count > 0)
            {
                CreateSeparator();
                CreateInfoItem("Hastalıklar", "");
                foreach (var disease in character.Health.CurrentDiseases)
                {
                    CreateInfoItem("", $"• {disease.Name}");
                }
            }

            // Borç
            if (character.Finance.Debt > 0)
            {
                CreateSeparator();
                CreateInfoItem("Toplam Borç", $"₺{character.Finance.Debt:N0}");
            }
        }

        private void CreateStatItem(string label, float value, Color color)
        {
            var panel = uiManager.PanelFactory.Create($"StatItem_{label}", new Vector2(450, 50), contentParent, Color.clear);

            var labelText = uiManager.TextFactory.Create(label, panel.transform, 20, Color.white, TMPro.TextAlignmentOptions.Left);
            var labelRect = labelText.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0.5f);
            labelRect.anchorMax = new Vector2(0, 0.5f);
            labelRect.pivot = new Vector2(0, 0.5f);
            labelRect.anchoredPosition = new Vector2(10, 0);

            var valueText = uiManager.TextFactory.Create($"{value:F0}%", panel.transform, 20, color, TMPro.TextAlignmentOptions.Right);
            var valueRect = valueText.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(1, 0.5f);
            valueRect.anchorMax = new Vector2(1, 0.5f);
            valueRect.pivot = new Vector2(1, 0.5f);
            valueRect.anchoredPosition = new Vector2(-10, 0);
        }

        private void CreateInfoItem(string label, string value)
        {
            var panel = uiManager.PanelFactory.Create($"InfoItem_{label}", new Vector2(450, 40), contentParent, Color.clear);

            if (!string.IsNullOrEmpty(label))
            {
                var labelText = uiManager.TextFactory.Create(label, panel.transform, 18, new Color(0.7f, 0.7f, 0.7f), TMPro.TextAlignmentOptions.Left);
                var labelRect = labelText.GetComponent<RectTransform>();
                labelRect.anchorMin = new Vector2(0, 0.5f);
                labelRect.anchorMax = new Vector2(0, 0.5f);
                labelRect.pivot = new Vector2(0, 0.5f);
                labelRect.anchoredPosition = new Vector2(10, 0);
            }

            var valueText = uiManager.TextFactory.Create(value, panel.transform, 18, Color.white, TMPro.TextAlignmentOptions.Right);
            var valueRect = valueText.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(1, 0.5f);
            valueRect.anchorMax = new Vector2(1, 0.5f);
            valueRect.pivot = new Vector2(1, 0.5f);
            valueRect.anchoredPosition = new Vector2(-10, 0);
        }

        private void CreateSeparator()
        {
            var sep = uiManager.ImageFactory.Create("Separator", null, contentParent, new Vector2(400, 2), new Color(0.3f, 0.3f, 0.3f));
        }

        private string GetEducationLevelName(EducationLevel level)
        {
            return level switch
            {
                EducationLevel.None => "Eğitimsiz",
                EducationLevel.PrimarySchool => "İlkokul",
                EducationLevel.MiddleSchool => "Ortaokul",
                EducationLevel.HighSchool => "Lise",
                EducationLevel.University => "Üniversite",
                EducationLevel.Masters => "Yüksek Lisans",
                EducationLevel.Doctorate => "Doktora",
                _ => "Bilinmiyor"
            };
        }
    }
}
