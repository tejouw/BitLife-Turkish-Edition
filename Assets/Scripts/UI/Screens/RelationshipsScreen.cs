using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BitLifeTR.Core;
using BitLifeTR.Systems;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.UI.Screens
{
    /// <summary>
    /// Screen showing all relationships.
    /// </summary>
    public class RelationshipsScreen : UIScreen
    {
        public override string ScreenId => "Relationships";
        public override int SortingOrder => 10;

        private RectTransform contentContainer;

        protected override void CreateUI()
        {
            // Semi-transparent background
            UIFactory.CreateFullScreenPanel("Background", transform, UITheme.WithAlpha(Color.black, 0.7f));

            // Panel
            var panel = UIFactory.CreatePanel("Panel", transform, UITheme.BackgroundPanel);
            UIFactory.SetCenterAnchors(panel);
            panel.sizeDelta = new Vector2(350, 550);
            UIFactory.AddVerticalLayout(panel, UITheme.SpacingNormal, new RectOffset(20, 20, 20, 20));

            // Title
            var title = UIFactory.CreateTitle("Title", panel, "İlişkiler");
            UIFactory.AddLayoutElement(title.rectTransform, -1, 50);

            // Scroll view
            var (scroll, content) = UIFactory.CreateScrollView("Scroll", panel);
            UIFactory.AddLayoutElement(scroll.GetComponent<RectTransform>(), -1, 400);
            contentContainer = content;

            // Close button
            var closeBtn = UIFactory.CreateSecondaryButton("CloseBtn", panel, "Kapat", () => Hide());
            UIFactory.AddLayoutElement(closeBtn.GetComponent<RectTransform>(), -1, 50);
        }

        protected override void OnShow()
        {
            Refresh();
        }

        public override void Refresh()
        {
            contentContainer.DestroyAllChildren();

            var character = CharacterManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Aile
            CreateCategoryHeader("Aile");
            foreach (var rel in character.Relationships.FindAll(r =>
                r.Type == RelationType.Anne || r.Type == RelationType.Baba || r.Type == RelationType.Kardes))
            {
                CreateRelationshipRow(rel);
            }

            // Eş/Sevgili
            var romantic = character.Relationships.FindAll(r => r.Type == RelationType.Es || r.Type == RelationType.Sevgili);
            if (romantic.Count > 0)
            {
                CreateCategoryHeader("Romantik");
                foreach (var rel in romantic)
                {
                    CreateRelationshipRow(rel);
                }
            }

            // Çocuklar
            var children = character.Relationships.FindAll(r => r.Type == RelationType.Cocuk);
            if (children.Count > 0)
            {
                CreateCategoryHeader("Çocuklar");
                foreach (var rel in children)
                {
                    CreateRelationshipRow(rel);
                }
            }

            // Arkadaşlar
            var friends = character.Relationships.FindAll(r => r.Type == RelationType.Arkadas);
            if (friends.Count > 0)
            {
                CreateCategoryHeader("Arkadaşlar");
                foreach (var rel in friends)
                {
                    CreateRelationshipRow(rel);
                }
            }
        }

        private void CreateCategoryHeader(string category)
        {
            var header = UIFactory.CreateText("Header_" + category, contentContainer, category,
                UITheme.FontSizeSmall, UITheme.PrimaryColor, TMPro.TextAlignmentOptions.Left, TMPro.FontStyles.Bold);
            UIFactory.AddLayoutElement(header.rectTransform, -1, 30);
        }

        private void CreateRelationshipRow(RelationshipData rel)
        {
            var row = UIFactory.CreatePanel("Rel_" + rel.Id, contentContainer, UITheme.BackgroundMedium);
            row.sizeDelta = new Vector2(300, 60);
            UIFactory.AddHorizontalLayout(row, UITheme.SpacingSmall, new RectOffset(10, 10, 5, 5));
            UIFactory.AddLayoutElement(row, -1, 65);

            // Info container
            var info = UIFactory.CreateContainer("Info", row);
            UIFactory.AddVerticalLayout(info, 2, new RectOffset(0, 0, 0, 0), TextAnchor.MiddleLeft);
            UIFactory.AddLayoutElement(info, -1, -1, -1, -1, 1);

            // Name
            var nameText = UIFactory.CreateText("Name", info, rel.Name, UITheme.FontSizeSmall,
                rel.IsAlive ? UITheme.TextPrimary : UITheme.TextDisabled, TMPro.TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(nameText.rectTransform, -1, 20);

            // Details
            string details = $"{GetRelationTypeName(rel.Type)} - {rel.Age} yaşında";
            if (!rel.IsAlive) details += " (Vefat)";
            var detailsText = UIFactory.CreateText("Details", info, details, UITheme.FontSizeSmall,
                UITheme.TextSecondary, TMPro.TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(detailsText.rectTransform, -1, 18);

            // Relationship bar
            var barContainer = UIFactory.CreateContainer("Bar", row);
            UIFactory.AddLayoutElement(barContainer, 60, -1);

            var barBg = UIFactory.CreateImage("BarBg", barContainer, null, UITheme.BackgroundDark);
            barBg.rectTransform.anchorMin = new Vector2(0, 0.3f);
            barBg.rectTransform.anchorMax = new Vector2(1, 0.7f);
            barBg.rectTransform.offsetMin = Vector2.zero;
            barBg.rectTransform.offsetMax = Vector2.zero;

            var fill = UIFactory.CreateImage("Fill", barBg.transform, null, GetRelationshipColor(rel.RelationshipLevel));
            fill.rectTransform.anchorMin = Vector2.zero;
            fill.rectTransform.anchorMax = new Vector2(rel.RelationshipLevel / 100f, 1f);
            fill.rectTransform.offsetMin = Vector2.zero;
            fill.rectTransform.offsetMax = Vector2.zero;
        }

        private string GetRelationTypeName(RelationType type)
        {
            return type switch
            {
                RelationType.Anne => "Anne",
                RelationType.Baba => "Baba",
                RelationType.Kardes => "Kardeş",
                RelationType.Cocuk => "Çocuk",
                RelationType.Es => "Eş",
                RelationType.Sevgili => "Sevgili",
                RelationType.Arkadas => "Arkadaş",
                _ => type.ToString()
            };
        }

        private Color GetRelationshipColor(int level)
        {
            if (level >= 75) return UITheme.Success;
            if (level >= 50) return UITheme.Warning;
            if (level >= 25) return UITheme.AccentColor;
            return UITheme.Error;
        }

        public override bool OnBackPressed()
        {
            Hide();
            return true;
        }
    }
}
