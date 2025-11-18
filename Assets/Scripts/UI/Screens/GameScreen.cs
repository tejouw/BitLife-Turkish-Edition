using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using BitLifeTR.Core;
using BitLifeTR.Systems;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.UI.Screens
{
    /// <summary>
    /// Main game screen showing character status, events, and actions.
    /// </summary>
    public class GameScreen : UIScreen
    {
        public override string ScreenId => "Game";

        // Header
        private TextMeshProUGUI nameText;
        private TextMeshProUGUI ageText;
        private TextMeshProUGUI moneyText;

        // Stat bars
        private Image healthBar;
        private Image happinessBar;
        private Image intelligenceBar;
        private Image looksBar;

        // Event display
        private RectTransform eventPanel;
        private TextMeshProUGUI eventTitle;
        private TextMeshProUGUI eventDescription;
        private RectTransform choicesContainer;

        // Bottom navigation
        private Button ageUpButton;
        private Button activitiesButton;
        private Button relationshipsButton;

        protected override void CreateUI()
        {
            // Background
            UIFactory.CreateFullScreenPanel("Background", transform, UITheme.BackgroundDark);

            // Main container with vertical layout
            var container = UIFactory.CreateContainer("Container", transform);
            UIFactory.SetFullStretch(container);
            UIFactory.AddVerticalLayout(container, 0, new RectOffset(0, 0, 0, 0), TextAnchor.UpperCenter, true, false);

            // Create UI sections
            CreateHeader(container);
            CreateStatBars(container);
            CreateEventPanel(container);
            CreateBottomNav(container);
        }

        private void CreateHeader(RectTransform parent)
        {
            var header = UIFactory.CreatePanel("Header", parent, UITheme.BackgroundMedium);
            UIFactory.AddLayoutElement(header, -1, 100);
            UIFactory.AddHorizontalLayout(header, UITheme.SpacingNormal, new RectOffset(20, 20, 15, 15));

            // Left side - name and age
            var leftContainer = UIFactory.CreateContainer("Left", header);
            UIFactory.AddVerticalLayout(leftContainer, UITheme.SpacingTiny, new RectOffset(0, 0, 0, 0), TextAnchor.UpperLeft);
            UIFactory.AddLayoutElement(leftContainer, -1, -1, -1, -1, 1);

            nameText = UIFactory.CreateText("Name", leftContainer, "İsim Soyisim",
                UITheme.FontSizeMedium, UITheme.TextPrimary, TextAlignmentOptions.Left, FontStyles.Bold);
            UIFactory.AddLayoutElement(nameText.rectTransform, -1, 35);

            ageText = UIFactory.CreateText("Age", leftContainer, "0 yaşında",
                UITheme.FontSizeNormal, UITheme.TextSecondary, TextAlignmentOptions.Left);
            UIFactory.AddLayoutElement(ageText.rectTransform, -1, 25);

            // Right side - money
            var rightContainer = UIFactory.CreateContainer("Right", header);
            UIFactory.AddVerticalLayout(rightContainer, UITheme.SpacingTiny, new RectOffset(0, 0, 0, 0), TextAnchor.UpperRight);
            UIFactory.AddLayoutElement(rightContainer, 150, -1);

            moneyText = UIFactory.CreateText("Money", rightContainer, "0 TL",
                UITheme.FontSizeMedium, UITheme.StatMoney, TextAlignmentOptions.Right, FontStyles.Bold);
            UIFactory.AddLayoutElement(moneyText.rectTransform, -1, 35);
        }

        private void CreateStatBars(RectTransform parent)
        {
            var statsPanel = UIFactory.CreatePanel("Stats", parent, UITheme.BackgroundPanel);
            UIFactory.AddLayoutElement(statsPanel, -1, 80);
            UIFactory.AddHorizontalLayout(statsPanel, UITheme.SpacingSmall, new RectOffset(15, 15, 10, 10));

            // Health
            healthBar = CreateStatBarVertical(statsPanel, "Sağlık", UITheme.StatHealth);

            // Happiness
            happinessBar = CreateStatBarVertical(statsPanel, "Mutluluk", UITheme.StatHappiness);

            // Intelligence
            intelligenceBar = CreateStatBarVertical(statsPanel, "Zeka", UITheme.StatIntelligence);

            // Looks
            looksBar = CreateStatBarVertical(statsPanel, "Görünüm", UITheme.StatLooks);
        }

        private Image CreateStatBarVertical(RectTransform parent, string label, Color color)
        {
            var container = UIFactory.CreateContainer(label, parent);
            UIFactory.AddVerticalLayout(container, 2, new RectOffset(0, 0, 0, 0), TextAnchor.MiddleCenter);
            UIFactory.AddLayoutElement(container, -1, -1, -1, -1, 1);

            // Bar background
            var barBg = UIFactory.CreateImage("BarBg", container, null, UITheme.BackgroundDark);
            barBg.rectTransform.sizeDelta = new Vector2(40, 30);
            UIFactory.AddLayoutElement(barBg.rectTransform, 40, 30);

            // Fill
            var fill = UIFactory.CreateImage("Fill", barBg.transform, null, color);
            fill.rectTransform.anchorMin = Vector2.zero;
            fill.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            fill.rectTransform.offsetMin = Vector2.zero;
            fill.rectTransform.offsetMax = Vector2.zero;

            // Label
            var labelText = UIFactory.CreateText("Label", container, label[0].ToString(),
                UITheme.FontSizeSmall, color);
            UIFactory.AddLayoutElement(labelText.rectTransform, -1, 20);

            return fill;
        }

        private void CreateEventPanel(RectTransform parent)
        {
            // Scrollable event area
            var (scroll, content) = UIFactory.CreateScrollView("EventScroll", parent);
            UIFactory.AddLayoutElement(scroll.GetComponent<RectTransform>(), -1, -1, -1, -1, -1, 1);

            // Event panel
            eventPanel = UIFactory.CreatePanel("EventPanel", content, UITheme.BackgroundPanel);
            eventPanel.sizeDelta = new Vector2(0, 400);
            UIFactory.AddVerticalLayout(eventPanel, UITheme.SpacingNormal, new RectOffset(20, 20, 20, 20), TextAnchor.UpperCenter);

            // Event title
            eventTitle = UIFactory.CreateText("EventTitle", eventPanel, "Olay Başlığı",
                UITheme.FontSizeMedium, UITheme.TextPrimary, TextAlignmentOptions.Center, FontStyles.Bold);
            eventTitle.enableWordWrapping = true;
            UIFactory.AddLayoutElement(eventTitle.rectTransform, -1, -1, -1, 40);

            // Event description
            eventDescription = UIFactory.CreateText("EventDesc", eventPanel, "Olay açıklaması burada görünecek.",
                UITheme.FontSizeNormal, UITheme.TextPrimary, TextAlignmentOptions.Center);
            eventDescription.enableWordWrapping = true;
            UIFactory.AddLayoutElement(eventDescription.rectTransform, -1, -1, -1, 60);

            // Choices container
            choicesContainer = UIFactory.CreateContainer("Choices", eventPanel);
            choicesContainer.sizeDelta = new Vector2(320, 200);
            UIFactory.AddVerticalLayout(choicesContainer, UITheme.SpacingSmall);
            UIFactory.AddContentSizeFitter(choicesContainer);
        }

        private void CreateBottomNav(RectTransform parent)
        {
            var bottomNav = UIFactory.CreatePanel("BottomNav", parent, UITheme.BackgroundMedium);
            UIFactory.AddLayoutElement(bottomNav, -1, 80);
            UIFactory.AddHorizontalLayout(bottomNav, UITheme.SpacingNormal, new RectOffset(15, 15, 10, 10));

            // Activities button
            activitiesButton = UIFactory.CreateSecondaryButton("ActivitiesBtn", bottomNav, "Aktiviteler", OnActivities);
            UIFactory.AddLayoutElement(activitiesButton.GetComponent<RectTransform>(), -1, 60, -1, -1, 1);

            // Age up button (center, prominent)
            ageUpButton = UIFactory.CreatePrimaryButton("AgeUpBtn", bottomNav, "Yaşlan", OnAgeUp);
            UIFactory.AddLayoutElement(ageUpButton.GetComponent<RectTransform>(), -1, 60, -1, -1, 1);

            // Relationships button
            relationshipsButton = UIFactory.CreateSecondaryButton("RelationsBtn", bottomNav, "İlişkiler", OnRelationships);
            UIFactory.AddLayoutElement(relationshipsButton.GetComponent<RectTransform>(), -1, 60, -1, -1, 1);
        }

        protected override void OnShow()
        {
            Refresh();
            EventBus.Subscribe<StatChangedEvent>(OnStatChanged);
            EventBus.Subscribe<MoneyChangedEvent>(OnMoneyChanged);
        }

        protected override void OnHide()
        {
            EventBus.Unsubscribe<StatChangedEvent>(OnStatChanged);
            EventBus.Unsubscribe<MoneyChangedEvent>(OnMoneyChanged);
        }

        public override void Refresh()
        {
            var character = CharacterManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Update header
            nameText.text = character.FullName;
            ageText.text = $"{character.Age} yaşında - {character.LifeStage.ToTurkish()}";
            moneyText.text = character.Money.ToTurkishLira();

            // Update stat bars
            UpdateStatBar(healthBar, character.Health);
            UpdateStatBar(happinessBar, character.Happiness);
            UpdateStatBar(intelligenceBar, character.Intelligence);
            UpdateStatBar(looksBar, character.Looks);

            // Update event display
            UpdateEventDisplay();
        }

        private void UpdateStatBar(Image bar, float value)
        {
            bar.rectTransform.anchorMax = new Vector2(value / 100f, 1f);
        }

        private void UpdateEventDisplay()
        {
            var currentEvent = GameLoop.Instance.GetCurrentEvent();

            if (currentEvent != null)
            {
                eventTitle.text = currentEvent.Title;
                eventDescription.text = currentEvent.Description;
                ShowChoices(currentEvent);
                ageUpButton.interactable = false;
            }
            else
            {
                eventTitle.text = "Hayatın devam ediyor...";
                eventDescription.text = "Yeni bir yıla geçmek için 'Yaşlan' butonuna bas.";
                ClearChoices();
                ageUpButton.interactable = true;
            }
        }

        private void ShowChoices(GameEvent gameEvent)
        {
            ClearChoices();

            foreach (var choice in gameEvent.Choices)
            {
                var button = UIFactory.CreateChoiceButton("Choice", choicesContainer, choice.Text, () => OnChoiceSelected(gameEvent, choice));
                UIFactory.AddLayoutElement(button.GetComponent<RectTransform>(), 300, 50);

                if (!choice.IsAvailable)
                {
                    button.interactable = false;
                }
            }
        }

        private void ClearChoices()
        {
            choicesContainer.DestroyAllChildren();
        }

        private void OnChoiceSelected(GameEvent gameEvent, EventChoice choice)
        {
            int index = gameEvent.Choices.IndexOf(choice);
            var outcome = EventManager.Instance.ExecuteChoice(index);

            if (outcome != null)
            {
                // Show outcome briefly
                eventDescription.text = outcome.Description;
            }

            // Move to next event or end year
            GameLoop.Instance.NextEvent();
            Refresh();

            // Check if character died
            var character = CharacterManager.Instance.CurrentCharacter;
            if (character != null && !character.IsAlive)
            {
                UIManager.Instance.ShowScreen<DeathScreen>();
            }
        }

        private void OnAgeUp()
        {
            GameLoop.Instance.AdvanceYear();
            Refresh();

            // Check if character died
            var character = CharacterManager.Instance.CurrentCharacter;
            if (character != null && !character.IsAlive)
            {
                UIManager.Instance.ShowScreen<DeathScreen>();
            }
        }

        private void OnActivities()
        {
            UIManager.Instance.ShowScreen<ActivitiesScreen>(false);
        }

        private void OnRelationships()
        {
            UIManager.Instance.ShowScreen<RelationshipsScreen>(false);
        }

        private void OnStatChanged(StatChangedEvent e)
        {
            Refresh();
        }

        private void OnMoneyChanged(MoneyChangedEvent e)
        {
            var character = CharacterManager.Instance.CurrentCharacter;
            if (character != null)
            {
                moneyText.text = character.Money.ToTurkishLira();
            }
        }

        public override bool OnBackPressed()
        {
            // Show pause/menu
            GameManager.Instance.PauseGame();
            return true;
        }
    }
}
