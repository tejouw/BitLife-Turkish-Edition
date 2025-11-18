using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using BitLifeTR.Core;

namespace BitLifeTR.UI
{
    /// <summary>
    /// UI yönetim sistemi - Tüm ekranları kontrol eder
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        private Canvas mainCanvas;
        private Dictionary<ScreenType, BaseScreen> screens;
        private ScreenType currentScreen;
        private Stack<ScreenType> screenHistory;

        // Factory'ler
        public ButtonFactory ButtonFactory { get; private set; }
        public PanelFactory PanelFactory { get; private set; }
        public TextFactory TextFactory { get; private set; }
        public ImageFactory ImageFactory { get; private set; }
        public InputFactory InputFactory { get; private set; }
        public ScrollViewFactory ScrollViewFactory { get; private set; }

        private void Awake()
        {
            screens = new Dictionary<ScreenType, BaseScreen>();
            screenHistory = new Stack<ScreenType>();
            InitializeCanvas();
            InitializeFactories();
            CreateAllScreens();
        }

        private void InitializeCanvas()
        {
            // Ana Canvas oluştur
            GameObject canvasObj = new GameObject("MainCanvas");
            mainCanvas = canvasObj.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            mainCanvas.sortingOrder = 0;

            // Canvas Scaler
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            // Graphic Raycaster
            canvasObj.AddComponent<GraphicRaycaster>();

            // Event System
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            DontDestroyOnLoad(canvasObj);
        }

        private void InitializeFactories()
        {
            ButtonFactory = new ButtonFactory(mainCanvas.transform);
            PanelFactory = new PanelFactory(mainCanvas.transform);
            TextFactory = new TextFactory(mainCanvas.transform);
            ImageFactory = new ImageFactory(mainCanvas.transform);
            InputFactory = new InputFactory(mainCanvas.transform);
            ScrollViewFactory = new ScrollViewFactory(mainCanvas.transform);
        }

        private void CreateAllScreens()
        {
            // Tüm ekranları oluştur
            screens[ScreenType.MainMenu] = new MainMenuScreen(this);
            screens[ScreenType.NewGame] = new NewGameScreen(this);
            screens[ScreenType.Game] = new GameScreen(this);
            screens[ScreenType.Stats] = new StatsScreen(this);
            screens[ScreenType.Relationships] = new RelationshipsScreen(this);
            screens[ScreenType.Career] = new CareerScreen(this);
            screens[ScreenType.Education] = new EducationScreen(this);
            screens[ScreenType.Activities] = new ActivitiesScreen(this);
            screens[ScreenType.Assets] = new AssetsScreen(this);
            screens[ScreenType.Settings] = new SettingsScreen(this);
            screens[ScreenType.GameOver] = new GameOverScreen(this);
            screens[ScreenType.Event] = new EventScreen(this);

            // Tüm ekranları gizle
            foreach (var screen in screens.Values)
            {
                screen.Hide();
            }
        }

        public void ShowScreen(ScreenType screenType)
        {
            // Mevcut ekranı gizle
            if (screens.ContainsKey(currentScreen))
            {
                screens[currentScreen].Hide();
            }

            // Geçmişe ekle
            if (currentScreen != screenType)
            {
                screenHistory.Push(currentScreen);
            }

            // Yeni ekranı göster
            currentScreen = screenType;
            if (screens.ContainsKey(screenType))
            {
                screens[screenType].Show();
                screens[screenType].Refresh();
            }

            EventBus.Publish(new ScreenChangedEvent(screenType));
        }

        public void GoBack()
        {
            if (screenHistory.Count > 0)
            {
                ScreenType previousScreen = screenHistory.Pop();
                ShowScreen(previousScreen);
            }
        }

        public BaseScreen GetScreen(ScreenType screenType)
        {
            return screens.ContainsKey(screenType) ? screens[screenType] : null;
        }

        public Canvas GetMainCanvas()
        {
            return mainCanvas;
        }

        public void ShowPopup(string title, string message, System.Action onConfirm = null, System.Action onCancel = null)
        {
            // Popup paneli oluştur
            var popupPanel = PanelFactory.Create("Popup", new Vector2(800, 400));
            popupPanel.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.95f);

            // Başlık
            var titleText = TextFactory.Create(title, popupPanel.transform, 32);
            titleText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 120);

            // Mesaj
            var messageText = TextFactory.Create(message, popupPanel.transform, 24);
            messageText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);

            // Butonlar
            if (onConfirm != null)
            {
                var confirmBtn = ButtonFactory.Create("Tamam", popupPanel.transform, () =>
                {
                    onConfirm?.Invoke();
                    Destroy(popupPanel);
                });
                confirmBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, -120);
            }

            if (onCancel != null)
            {
                var cancelBtn = ButtonFactory.Create("İptal", popupPanel.transform, () =>
                {
                    onCancel?.Invoke();
                    Destroy(popupPanel);
                });
                cancelBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -120);
            }
            else if (onConfirm == null)
            {
                var okBtn = ButtonFactory.Create("Tamam", popupPanel.transform, () =>
                {
                    Destroy(popupPanel);
                });
                okBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120);
            }
        }

        public void ShowEventPopup(GameEvent gameEvent, System.Action<int> onChoice)
        {
            var eventScreen = screens[ScreenType.Event] as EventScreen;
            eventScreen?.ShowEvent(gameEvent, onChoice);
            ShowScreen(ScreenType.Event);
        }
    }
}
