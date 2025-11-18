using UnityEngine;
using System.Collections;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Base class for all UI screens.
    /// All screens must inherit from this and implement UI creation in code.
    /// </summary>
    public abstract class UIScreen : MonoBehaviour
    {
        protected RectTransform rootTransform;
        protected Canvas canvas;
        protected CanvasGroup canvasGroup;

        /// <summary>
        /// Unique identifier for this screen.
        /// </summary>
        public abstract string ScreenId { get; }

        /// <summary>
        /// Whether this screen is currently visible.
        /// </summary>
        public bool IsVisible { get; private set; }

        /// <summary>
        /// Whether this screen blocks input to screens below it.
        /// </summary>
        public virtual bool BlocksInput => true;

        /// <summary>
        /// Sorting order for this screen's canvas.
        /// </summary>
        public virtual int SortingOrder => 0;

        /// <summary>
        /// Initialize the screen. Called once when screen is created.
        /// </summary>
        public void Initialize()
        {
            // Create root container
            rootTransform = gameObject.GetComponent<RectTransform>();
            if (rootTransform == null)
            {
                rootTransform = gameObject.AddComponent<RectTransform>();
            }

            // Add canvas group for fading
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

            // Set up as full screen
            UIFactory.SetFullStretch(rootTransform);

            // Create the UI
            CreateUI();

            // Start hidden
            SetVisible(false, false);

            OnInitialize();
        }

        /// <summary>
        /// Override this to create the screen's UI elements.
        /// </summary>
        protected abstract void CreateUI();

        /// <summary>
        /// Called after initialization is complete.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when the screen becomes visible.
        /// </summary>
        protected virtual void OnShow() { }

        /// <summary>
        /// Called when the screen becomes hidden.
        /// </summary>
        protected virtual void OnHide() { }

        /// <summary>
        /// Called every frame while visible.
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        /// Called when data needs to be refreshed.
        /// </summary>
        public virtual void Refresh() { }

        /// <summary>
        /// Show this screen.
        /// </summary>
        public void Show(bool animate = true)
        {
            SetVisible(true, animate);
        }

        /// <summary>
        /// Hide this screen.
        /// </summary>
        public void Hide(bool animate = true)
        {
            SetVisible(false, animate);
        }

        /// <summary>
        /// Toggle screen visibility.
        /// </summary>
        public void Toggle(bool animate = true)
        {
            SetVisible(!IsVisible, animate);
        }

        private void SetVisible(bool visible, bool animate)
        {
            if (IsVisible == visible) return;

            IsVisible = visible;

            if (animate)
            {
                StartCoroutine(AnimateVisibility(visible));
            }
            else
            {
                canvasGroup.alpha = visible ? 1f : 0f;
                canvasGroup.interactable = visible;
                canvasGroup.blocksRaycasts = visible;
                gameObject.SetActive(visible);

                if (visible) OnShow();
                else OnHide();
            }
        }

        private IEnumerator AnimateVisibility(bool visible)
        {
            float duration = UITheme.AnimationFast;
            float startAlpha = canvasGroup.alpha;
            float endAlpha = visible ? 1f : 0f;
            float elapsed = 0f;

            if (visible)
            {
                gameObject.SetActive(true);
                canvasGroup.interactable = false;
            }

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }

            canvasGroup.alpha = endAlpha;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;

            if (!visible)
            {
                gameObject.SetActive(false);
                OnHide();
            }
            else
            {
                OnShow();
            }
        }

        private void Update()
        {
            if (IsVisible)
            {
                OnUpdate();
            }
        }

        /// <summary>
        /// Handle back button press. Return true if handled.
        /// </summary>
        public virtual bool OnBackPressed()
        {
            return false;
        }
    }
}
