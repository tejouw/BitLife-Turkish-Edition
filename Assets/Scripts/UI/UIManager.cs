using UnityEngine;
using System.Collections.Generic;
using BitLifeTR.Core;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Manages all UI screens and navigation.
    /// Creates and controls all UI programmatically.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        private Canvas mainCanvas;
        private RectTransform screenContainer;
        private Dictionary<string, UIScreen> screens = new Dictionary<string, UIScreen>();
        private Stack<UIScreen> screenStack = new Stack<UIScreen>();

        public Canvas MainCanvas => mainCanvas;
        public UIScreen CurrentScreen => screenStack.Count > 0 ? screenStack.Peek() : null;

        protected override void OnSingletonAwake()
        {
            CreateMainCanvas();
            Debug.Log("[UIManager] Initialized");
        }

        private void CreateMainCanvas()
        {
            // Create main canvas
            mainCanvas = UIFactory.CreateCanvas("MainCanvas", 0);
            mainCanvas.transform.SetParent(transform);

            // Create screen container
            screenContainer = UIFactory.CreateContainer("Screens", mainCanvas.transform);
            UIFactory.SetFullStretch(screenContainer);
        }

        /// <summary>
        /// Register a screen with the UI system.
        /// </summary>
        public T CreateScreen<T>() where T : UIScreen
        {
            var screenGO = new GameObject(typeof(T).Name);
            screenGO.transform.SetParent(screenContainer, false);

            var screen = screenGO.AddComponent<T>();
            screen.Initialize();

            screens[screen.ScreenId] = screen;
            Debug.Log($"[UIManager] Created screen: {screen.ScreenId}");

            return screen;
        }

        /// <summary>
        /// Get a screen by type.
        /// </summary>
        public T GetScreen<T>() where T : UIScreen
        {
            foreach (var screen in screens.Values)
            {
                if (screen is T typedScreen)
                {
                    return typedScreen;
                }
            }
            return null;
        }

        /// <summary>
        /// Get a screen by ID.
        /// </summary>
        public UIScreen GetScreen(string screenId)
        {
            return screens.TryGetValue(screenId, out var screen) ? screen : null;
        }

        /// <summary>
        /// Show a screen by type (pushes onto stack).
        /// </summary>
        public void ShowScreen<T>(bool hideCurrent = true) where T : UIScreen
        {
            var screen = GetScreen<T>();
            if (screen != null)
            {
                ShowScreen(screen, hideCurrent);
            }
            else
            {
                Debug.LogWarning($"[UIManager] Screen not found: {typeof(T).Name}");
            }
        }

        /// <summary>
        /// Show a screen by ID.
        /// </summary>
        public void ShowScreen(string screenId, bool hideCurrent = true)
        {
            if (screens.TryGetValue(screenId, out var screen))
            {
                ShowScreen(screen, hideCurrent);
            }
            else
            {
                Debug.LogWarning($"[UIManager] Screen not found: {screenId}");
            }
        }

        private void ShowScreen(UIScreen screen, bool hideCurrent)
        {
            // Hide current screen if requested
            if (hideCurrent && screenStack.Count > 0)
            {
                var current = screenStack.Peek();
                current.Hide();
            }

            // Remove from stack if already there (will be re-added at top)
            var tempStack = new Stack<UIScreen>();
            while (screenStack.Count > 0)
            {
                var s = screenStack.Pop();
                if (s != screen)
                {
                    tempStack.Push(s);
                }
            }
            while (tempStack.Count > 0)
            {
                screenStack.Push(tempStack.Pop());
            }

            // Push and show
            screenStack.Push(screen);
            screen.transform.SetAsLastSibling();
            screen.Show();
            screen.Refresh();

            Debug.Log($"[UIManager] Showing screen: {screen.ScreenId}");
        }

        /// <summary>
        /// Go back to previous screen.
        /// </summary>
        public void GoBack()
        {
            if (screenStack.Count <= 1)
            {
                Debug.Log("[UIManager] Cannot go back - at root screen");
                return;
            }

            // Let current screen handle back if it wants
            var current = screenStack.Peek();
            if (current.OnBackPressed())
            {
                return;
            }

            // Pop and hide current
            screenStack.Pop();
            current.Hide();

            // Show previous
            if (screenStack.Count > 0)
            {
                var previous = screenStack.Peek();
                previous.Show();
                previous.Refresh();
                Debug.Log($"[UIManager] Going back to: {previous.ScreenId}");
            }
        }

        /// <summary>
        /// Clear all screens and show only the specified one.
        /// </summary>
        public void SetRootScreen<T>() where T : UIScreen
        {
            // Hide and clear all
            while (screenStack.Count > 0)
            {
                var screen = screenStack.Pop();
                screen.Hide(false);
            }

            // Show new root
            ShowScreen<T>(false);
        }

        /// <summary>
        /// Refresh the current screen.
        /// </summary>
        public void RefreshCurrentScreen()
        {
            CurrentScreen?.Refresh();
        }

        /// <summary>
        /// Refresh all screens.
        /// </summary>
        public void RefreshAllScreens()
        {
            foreach (var screen in screens.Values)
            {
                screen.Refresh();
            }
        }

        private void Update()
        {
            // Handle Android back button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoBack();
            }
        }

        /// <summary>
        /// Show a simple notification/toast message.
        /// </summary>
        public void ShowNotification(string title, string message, float duration = 3f)
        {
            EventBus.Publish(new ShowNotificationEvent
            {
                Title = title,
                Message = message,
                Duration = duration
            });
        }
    }
}
