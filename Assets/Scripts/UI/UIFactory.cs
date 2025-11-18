using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Factory class for creating UI elements programmatically.
    /// All UI creation goes through this class - no manual prefabs or inspector assignments.
    /// </summary>
    public static class UIFactory
    {
        #region Canvas

        /// <summary>
        /// Create a new Canvas with standard settings.
        /// </summary>
        public static Canvas CreateCanvas(string name = "Canvas", int sortingOrder = 0)
        {
            var canvasGO = new GameObject(name);

            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            canvasGO.AddComponent<GraphicRaycaster>();

            return canvas;
        }

        #endregion

        #region Containers

        /// <summary>
        /// Create an empty RectTransform container.
        /// </summary>
        public static RectTransform CreateContainer(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            return rect;
        }

        /// <summary>
        /// Create a panel with background.
        /// </summary>
        public static RectTransform CreatePanel(string name, Transform parent, Color? backgroundColor = null)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            var image = go.AddComponent<Image>();
            image.color = backgroundColor ?? UITheme.BackgroundPanel;

            return rect;
        }

        /// <summary>
        /// Create a full-screen panel.
        /// </summary>
        public static RectTransform CreateFullScreenPanel(string name, Transform parent, Color? backgroundColor = null)
        {
            var panel = CreatePanel(name, parent, backgroundColor);
            SetFullStretch(panel);
            return panel;
        }

        #endregion

        #region Text

        /// <summary>
        /// Create a TextMeshPro text element.
        /// </summary>
        public static TextMeshProUGUI CreateText(
            string name,
            Transform parent,
            string text = "",
            float fontSize = UITheme.FontSizeNormal,
            Color? color = null,
            TextAlignmentOptions alignment = TextAlignmentOptions.Center,
            FontStyles fontStyle = FontStyles.Normal)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.color = color ?? UITheme.TextPrimary;
            tmp.alignment = alignment;
            tmp.fontStyle = fontStyle;
            tmp.enableWordWrapping = true;
            tmp.overflowMode = TextOverflowModes.Ellipsis;

            return tmp;
        }

        /// <summary>
        /// Create a title text.
        /// </summary>
        public static TextMeshProUGUI CreateTitle(string name, Transform parent, string text)
        {
            return CreateText(name, parent, text, UITheme.FontSizeLarge, UITheme.TextPrimary,
                TextAlignmentOptions.Center, FontStyles.Bold);
        }

        /// <summary>
        /// Create a subtitle text.
        /// </summary>
        public static TextMeshProUGUI CreateSubtitle(string name, Transform parent, string text)
        {
            return CreateText(name, parent, text, UITheme.FontSizeMedium, UITheme.TextSecondary);
        }

        /// <summary>
        /// Create a body text.
        /// </summary>
        public static TextMeshProUGUI CreateBodyText(string name, Transform parent, string text)
        {
            var tmp = CreateText(name, parent, text, UITheme.FontSizeNormal, UITheme.TextPrimary,
                TextAlignmentOptions.Left);
            tmp.enableWordWrapping = true;
            return tmp;
        }

        #endregion

        #region Buttons

        /// <summary>
        /// Create a button with text.
        /// </summary>
        public static Button CreateButton(
            string name,
            Transform parent,
            string text,
            UnityAction onClick = null,
            Vector2? size = null,
            Color? backgroundColor = null,
            Color? textColor = null)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.sizeDelta = size ?? UITheme.ButtonSizeNormal;

            // Background image
            var image = go.AddComponent<Image>();
            image.color = backgroundColor ?? UITheme.ButtonNormal;

            // Button component
            var button = go.AddComponent<Button>();
            button.targetGraphic = image;

            // Set up color transitions
            var colors = button.colors;
            colors.normalColor = backgroundColor ?? UITheme.ButtonNormal;
            colors.highlightedColor = UITheme.ButtonHover;
            colors.pressedColor = UITheme.ButtonPressed;
            colors.disabledColor = UITheme.ButtonDisabled;
            colors.fadeDuration = 0.1f;
            button.colors = colors;

            // Add click handler
            if (onClick != null)
            {
                button.onClick.AddListener(onClick);
            }

            // Text
            var tmp = CreateText("Text", rect.transform, text, UITheme.FontSizeNormal,
                textColor ?? UITheme.TextPrimary);
            SetFullStretch(tmp.rectTransform);

            return button;
        }

        /// <summary>
        /// Create a primary action button.
        /// </summary>
        public static Button CreatePrimaryButton(string name, Transform parent, string text, UnityAction onClick = null)
        {
            return CreateButton(name, parent, text, onClick, UITheme.ButtonSizeLarge, UITheme.PrimaryColor);
        }

        /// <summary>
        /// Create a secondary action button.
        /// </summary>
        public static Button CreateSecondaryButton(string name, Transform parent, string text, UnityAction onClick = null)
        {
            return CreateButton(name, parent, text, onClick, UITheme.ButtonSizeNormal, UITheme.BackgroundLight);
        }

        /// <summary>
        /// Create a choice/decision button.
        /// </summary>
        public static Button CreateChoiceButton(string name, Transform parent, string text, UnityAction onClick = null)
        {
            var button = CreateButton(name, parent, text, onClick, UITheme.ButtonSizeWide, UITheme.BackgroundMedium);

            var tmp = button.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.alignment = TextAlignmentOptions.Left;
                tmp.margin = new Vector4(UITheme.PaddingNormal, 0, UITheme.PaddingNormal, 0);
            }

            return button;
        }

        #endregion

        #region Images

        /// <summary>
        /// Create an Image element.
        /// </summary>
        public static Image CreateImage(string name, Transform parent, Sprite sprite = null, Color? color = null)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);

            var image = go.AddComponent<Image>();
            image.sprite = sprite;
            image.color = color ?? Color.white;
            image.preserveAspect = true;

            return image;
        }

        /// <summary>
        /// Create a colored rectangle.
        /// </summary>
        public static Image CreateColoredRect(string name, Transform parent, Color color, Vector2 size)
        {
            var image = CreateImage(name, parent, null, color);
            image.rectTransform.sizeDelta = size;
            return image;
        }

        #endregion

        #region Progress Bars

        /// <summary>
        /// Create a progress/stat bar.
        /// </summary>
        public static (RectTransform container, Image background, Image fill) CreateProgressBar(
            string name,
            Transform parent,
            Color fillColor,
            Vector2? size = null)
        {
            var containerSize = size ?? UITheme.StatBarSize;

            // Container
            var container = CreateContainer(name, parent);
            container.sizeDelta = containerSize;

            // Background
            var bgImage = CreateImage("Background", container, null, UITheme.BackgroundDark);
            SetFullStretch(bgImage.rectTransform);

            // Fill
            var fillGO = new GameObject("Fill", typeof(RectTransform));
            var fillRect = fillGO.GetComponent<RectTransform>();
            fillRect.SetParent(container, false);
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(0.5f, 1f); // Start at 50%
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            var fillImage = fillGO.AddComponent<Image>();
            fillImage.color = fillColor;

            return (container, bgImage, fillImage);
        }

        /// <summary>
        /// Create a stat bar with label.
        /// </summary>
        public static (RectTransform container, TextMeshProUGUI label, Image fill, TextMeshProUGUI valueText) CreateStatBar(
            string name,
            Transform parent,
            string labelText,
            Color statColor)
        {
            // Container
            var container = CreateContainer(name, parent);
            container.sizeDelta = new Vector2(300f, 40f);

            // Label
            var label = CreateText("Label", container, labelText, UITheme.FontSizeSmall, UITheme.TextSecondary,
                TextAlignmentOptions.Left);
            label.rectTransform.anchorMin = new Vector2(0, 0.5f);
            label.rectTransform.anchorMax = new Vector2(0.3f, 1f);
            label.rectTransform.offsetMin = Vector2.zero;
            label.rectTransform.offsetMax = Vector2.zero;

            // Progress bar
            var (barContainer, _, fill) = CreateProgressBar("Bar", container, statColor, new Vector2(150f, 12f));
            barContainer.anchorMin = new Vector2(0.3f, 0.25f);
            barContainer.anchorMax = new Vector2(0.85f, 0.75f);
            barContainer.offsetMin = Vector2.zero;
            barContainer.offsetMax = Vector2.zero;

            // Value text
            var valueText = CreateText("Value", container, "50", UITheme.FontSizeSmall, UITheme.TextPrimary,
                TextAlignmentOptions.Right);
            valueText.rectTransform.anchorMin = new Vector2(0.85f, 0);
            valueText.rectTransform.anchorMax = Vector2.one;
            valueText.rectTransform.offsetMin = Vector2.zero;
            valueText.rectTransform.offsetMax = Vector2.zero;

            return (container, label, fill, valueText);
        }

        #endregion

        #region Layouts

        /// <summary>
        /// Add a Vertical Layout Group to a RectTransform.
        /// </summary>
        public static VerticalLayoutGroup AddVerticalLayout(
            RectTransform rect,
            float spacing = UITheme.SpacingNormal,
            RectOffset padding = null,
            TextAnchor childAlignment = TextAnchor.UpperCenter,
            bool controlWidth = true,
            bool controlHeight = false)
        {
            var layout = rect.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = padding ?? new RectOffset(
                (int)UITheme.PaddingNormal,
                (int)UITheme.PaddingNormal,
                (int)UITheme.PaddingNormal,
                (int)UITheme.PaddingNormal);
            layout.childAlignment = childAlignment;
            layout.childControlWidth = controlWidth;
            layout.childControlHeight = controlHeight;
            layout.childForceExpandWidth = controlWidth;
            layout.childForceExpandHeight = false;

            return layout;
        }

        /// <summary>
        /// Add a Horizontal Layout Group to a RectTransform.
        /// </summary>
        public static HorizontalLayoutGroup AddHorizontalLayout(
            RectTransform rect,
            float spacing = UITheme.SpacingNormal,
            RectOffset padding = null,
            TextAnchor childAlignment = TextAnchor.MiddleCenter,
            bool controlWidth = false,
            bool controlHeight = true)
        {
            var layout = rect.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = padding ?? new RectOffset(
                (int)UITheme.PaddingNormal,
                (int)UITheme.PaddingNormal,
                (int)UITheme.PaddingNormal,
                (int)UITheme.PaddingNormal);
            layout.childAlignment = childAlignment;
            layout.childControlWidth = controlWidth;
            layout.childControlHeight = controlHeight;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = controlHeight;

            return layout;
        }

        /// <summary>
        /// Add a Grid Layout Group to a RectTransform.
        /// </summary>
        public static GridLayoutGroup AddGridLayout(
            RectTransform rect,
            Vector2 cellSize,
            Vector2 spacing,
            RectOffset padding = null,
            GridLayoutGroup.Corner startCorner = GridLayoutGroup.Corner.UpperLeft,
            GridLayoutGroup.Axis startAxis = GridLayoutGroup.Axis.Horizontal)
        {
            var layout = rect.gameObject.AddComponent<GridLayoutGroup>();
            layout.cellSize = cellSize;
            layout.spacing = spacing;
            layout.padding = padding ?? new RectOffset(
                (int)UITheme.PaddingNormal,
                (int)UITheme.PaddingNormal,
                (int)UITheme.PaddingNormal,
                (int)UITheme.PaddingNormal);
            layout.startCorner = startCorner;
            layout.startAxis = startAxis;
            layout.childAlignment = TextAnchor.UpperLeft;

            return layout;
        }

        /// <summary>
        /// Add a Content Size Fitter to auto-size based on content.
        /// </summary>
        public static ContentSizeFitter AddContentSizeFitter(
            RectTransform rect,
            ContentSizeFitter.FitMode horizontalFit = ContentSizeFitter.FitMode.Unconstrained,
            ContentSizeFitter.FitMode verticalFit = ContentSizeFitter.FitMode.PreferredSize)
        {
            var fitter = rect.gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = horizontalFit;
            fitter.verticalFit = verticalFit;
            return fitter;
        }

        /// <summary>
        /// Add a Layout Element for size control.
        /// </summary>
        public static LayoutElement AddLayoutElement(
            RectTransform rect,
            float preferredWidth = -1,
            float preferredHeight = -1,
            float minWidth = -1,
            float minHeight = -1,
            float flexibleWidth = -1,
            float flexibleHeight = -1)
        {
            var element = rect.gameObject.AddComponent<LayoutElement>();
            element.preferredWidth = preferredWidth;
            element.preferredHeight = preferredHeight;
            element.minWidth = minWidth;
            element.minHeight = minHeight;
            element.flexibleWidth = flexibleWidth;
            element.flexibleHeight = flexibleHeight;
            return element;
        }

        #endregion

        #region ScrollView

        /// <summary>
        /// Create a ScrollView.
        /// </summary>
        public static (ScrollRect scrollRect, RectTransform content) CreateScrollView(
            string name,
            Transform parent,
            bool horizontal = false,
            bool vertical = true)
        {
            // ScrollView container
            var scrollGO = new GameObject(name, typeof(RectTransform));
            var scrollRect = scrollGO.GetComponent<RectTransform>();
            scrollRect.SetParent(parent, false);

            var scroll = scrollGO.AddComponent<ScrollRect>();
            scrollGO.AddComponent<Image>().color = Color.clear; // Needed for raycasting
            var mask = scrollGO.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            // Viewport
            var viewport = CreateContainer("Viewport", scrollRect);
            SetFullStretch(viewport);

            // Content
            var content = CreateContainer("Content", viewport);
            content.anchorMin = new Vector2(0, 1);
            content.anchorMax = new Vector2(1, 1);
            content.pivot = new Vector2(0.5f, 1);
            content.sizeDelta = new Vector2(0, 0);

            // Add vertical layout to content
            AddVerticalLayout(content, UITheme.SpacingSmall);
            AddContentSizeFitter(content, ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize);

            // Configure scroll rect
            scroll.viewport = viewport;
            scroll.content = content;
            scroll.horizontal = horizontal;
            scroll.vertical = vertical;
            scroll.movementType = ScrollRect.MovementType.Elastic;
            scroll.elasticity = 0.1f;
            scroll.inertia = true;
            scroll.decelerationRate = 0.135f;
            scroll.scrollSensitivity = 20f;

            return (scroll, content);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Set RectTransform to stretch to fill parent.
        /// </summary>
        public static void SetFullStretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// Set RectTransform anchors to center.
        /// </summary>
        public static void SetCenterAnchors(RectTransform rect)
        {
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
        }

        /// <summary>
        /// Set RectTransform anchors to top.
        /// </summary>
        public static void SetTopAnchors(RectTransform rect)
        {
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
        }

        /// <summary>
        /// Set RectTransform anchors to bottom.
        /// </summary>
        public static void SetBottomAnchors(RectTransform rect)
        {
            rect.anchorMin = new Vector2(0.5f, 0f);
            rect.anchorMax = new Vector2(0.5f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
        }

        /// <summary>
        /// Create a horizontal divider line.
        /// </summary>
        public static Image CreateDivider(Transform parent, Color? color = null, float height = 1f)
        {
            var divider = CreateImage("Divider", parent, null, color ?? UITheme.BackgroundLight);
            divider.rectTransform.sizeDelta = new Vector2(0, height);

            var layout = AddLayoutElement(divider.rectTransform, -1, height, -1, height);
            layout.flexibleWidth = 1;

            return divider;
        }

        /// <summary>
        /// Create a spacer for layouts.
        /// </summary>
        public static RectTransform CreateSpacer(Transform parent, float height)
        {
            var spacer = CreateContainer("Spacer", parent);
            AddLayoutElement(spacer, -1, height, -1, height);
            return spacer;
        }

        #endregion
    }
}
