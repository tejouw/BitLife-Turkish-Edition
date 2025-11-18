using UnityEngine;
using UnityEngine.UI;
using BitLifeTR.Core;
using BitLifeTR.Systems;

namespace BitLifeTR.UI.Screens
{
    /// <summary>
    /// Activities screen for quick actions.
    /// </summary>
    public class ActivitiesScreen : UIScreen
    {
        public override string ScreenId => "Activities";
        public override int SortingOrder => 10;

        protected override void CreateUI()
        {
            // Semi-transparent background
            var bg = UIFactory.CreateFullScreenPanel("Background", transform, UITheme.WithAlpha(Color.black, 0.7f));

            // Panel
            var panel = UIFactory.CreatePanel("Panel", transform, UITheme.BackgroundPanel);
            UIFactory.SetCenterAnchors(panel);
            panel.sizeDelta = new Vector2(350, 500);
            UIFactory.AddVerticalLayout(panel, UITheme.SpacingNormal, new RectOffset(20, 20, 20, 20));

            // Title
            var title = UIFactory.CreateTitle("Title", panel, "Aktiviteler");
            UIFactory.AddLayoutElement(title.rectTransform, -1, 50);

            // Scroll view for activities
            var (scroll, content) = UIFactory.CreateScrollView("Scroll", panel);
            UIFactory.AddLayoutElement(scroll.GetComponent<RectTransform>(), -1, 350);

            // Sağlık kategorisi
            CreateCategoryHeader(content, "Sağlık");
            CreateActivityButton(content, "Spor Salonu", "gym", "Sağlık ve görünümünü geliştir");
            CreateActivityButton(content, "Doktora Git", "doctor", "Sağlığını kontrol ettir (500 TL)");
            CreateActivityButton(content, "Meditasyon", "meditation", "Mutluluğunu artır");
            CreateActivityButton(content, "Estetik Operasyon", "plastic_surgery", "Görünümünü iyileştir (10.000 TL)");

            // Eğitim kategorisi
            CreateCategoryHeader(content, "Zihin");
            CreateActivityButton(content, "Kütüphane", "library", "Zekânı geliştir");

            // Eğlence kategorisi
            CreateCategoryHeader(content, "Eğlence");
            CreateActivityButton(content, "Tatile Git", "vacation", "Mutluluğunu artır (5.000 TL)");

            // Tehlikeli kategorisi
            CreateCategoryHeader(content, "Suç");
            CreateActivityButton(content, "Hırsızlık", "crime", "Riskli ama para kazanabilirsin");

            // Close button
            var closeBtn = UIFactory.CreateSecondaryButton("CloseBtn", panel, "Kapat", () => Hide());
            UIFactory.AddLayoutElement(closeBtn.GetComponent<RectTransform>(), -1, 50);
        }

        private void CreateCategoryHeader(RectTransform parent, string category)
        {
            var header = UIFactory.CreateText("Header_" + category, parent, category,
                UITheme.FontSizeSmall, UITheme.TextSecondary, TMPro.TextAlignmentOptions.Left, TMPro.FontStyles.Bold);
            UIFactory.AddLayoutElement(header.rectTransform, -1, 30);
        }

        private void CreateActivityButton(RectTransform parent, string name, string activityId, string description)
        {
            var button = UIFactory.CreateChoiceButton("Activity_" + activityId, parent, name, () => OnActivitySelected(activityId));
            UIFactory.AddLayoutElement(button.GetComponent<RectTransform>(), -1, 45);
        }

        private void OnActivitySelected(string activityId)
        {
            GameLoop.Instance.PerformActivity(activityId);
            UIManager.Instance.RefreshCurrentScreen();
            Hide();
        }

        public override bool OnBackPressed()
        {
            Hide();
            return true;
        }
    }
}
