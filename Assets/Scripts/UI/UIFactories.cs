using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using BitLifeTR.Core;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Buton oluşturma factory'si
    /// </summary>
    public class ButtonFactory
    {
        private Transform parent;

        public ButtonFactory(Transform parent)
        {
            this.parent = parent;
        }

        public GameObject Create(string text, Transform customParent = null, Action onClick = null,
            Vector2? size = null, Color? backgroundColor = null)
        {
            Transform targetParent = customParent ?? parent;

            // Button GameObject
            GameObject buttonObj = new GameObject($"Button_{text}");
            buttonObj.transform.SetParent(targetParent, false);

            // RectTransform
            RectTransform rect = buttonObj.AddComponent<RectTransform>();
            rect.sizeDelta = size ?? new Vector2(Constants.DEFAULT_BUTTON_WIDTH, Constants.DEFAULT_BUTTON_HEIGHT);

            // Image (background)
            Image image = buttonObj.AddComponent<Image>();
            image.color = backgroundColor ?? new Color(0.3f, 0.6f, 0.9f, 1f);

            // Button component
            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = image;

            // Button colors
            ColorBlock colors = button.colors;
            colors.normalColor = backgroundColor ?? new Color(0.3f, 0.6f, 0.9f, 1f);
            colors.highlightedColor = new Color(0.4f, 0.7f, 1f, 1f);
            colors.pressedColor = new Color(0.2f, 0.5f, 0.8f, 1f);
            colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            button.colors = colors;

            // Text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = Constants.DEFAULT_FONT_SIZE;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;

            // Click event
            if (onClick != null)
            {
                button.onClick.AddListener(() => onClick());
            }

            return buttonObj;
        }

        public GameObject CreateIconButton(Sprite icon, Transform customParent = null, Action onClick = null,
            Vector2? size = null)
        {
            Transform targetParent = customParent ?? parent;

            GameObject buttonObj = new GameObject("IconButton");
            buttonObj.transform.SetParent(targetParent, false);

            RectTransform rect = buttonObj.AddComponent<RectTransform>();
            rect.sizeDelta = size ?? new Vector2(60, 60);

            Image image = buttonObj.AddComponent<Image>();
            image.sprite = icon;
            image.type = Image.Type.Simple;
            image.preserveAspect = true;

            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = image;

            if (onClick != null)
            {
                button.onClick.AddListener(() => onClick());
            }

            return buttonObj;
        }
    }

    /// <summary>
    /// Panel oluşturma factory'si
    /// </summary>
    public class PanelFactory
    {
        private Transform parent;

        public PanelFactory(Transform parent)
        {
            this.parent = parent;
        }

        public GameObject Create(string name, Vector2? size = null, Transform customParent = null,
            Color? backgroundColor = null)
        {
            Transform targetParent = customParent ?? parent;

            GameObject panelObj = new GameObject($"Panel_{name}");
            panelObj.transform.SetParent(targetParent, false);

            RectTransform rect = panelObj.AddComponent<RectTransform>();

            if (size.HasValue)
            {
                rect.sizeDelta = size.Value;
            }
            else
            {
                // Tam ekran
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.sizeDelta = Vector2.zero;
            }

            Image image = panelObj.AddComponent<Image>();
            image.color = backgroundColor ?? new Color(0.1f, 0.1f, 0.1f, 0.9f);

            return panelObj;
        }

        public GameObject CreateWithLayout(string name, Vector2? size = null, Transform customParent = null,
            bool vertical = true, float spacing = 10f, RectOffset padding = null)
        {
            GameObject panel = Create(name, size, customParent);

            if (vertical)
            {
                VerticalLayoutGroup layout = panel.AddComponent<VerticalLayoutGroup>();
                layout.spacing = spacing;
                layout.padding = padding ?? new RectOffset(20, 20, 20, 20);
                layout.childAlignment = TextAnchor.UpperCenter;
                layout.childControlHeight = false;
                layout.childControlWidth = true;
                layout.childForceExpandHeight = false;
                layout.childForceExpandWidth = true;
            }
            else
            {
                HorizontalLayoutGroup layout = panel.AddComponent<HorizontalLayoutGroup>();
                layout.spacing = spacing;
                layout.padding = padding ?? new RectOffset(20, 20, 20, 20);
                layout.childAlignment = TextAnchor.MiddleCenter;
                layout.childControlHeight = true;
                layout.childControlWidth = false;
                layout.childForceExpandHeight = true;
                layout.childForceExpandWidth = false;
            }

            ContentSizeFitter fitter = panel.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            return panel;
        }
    }

    /// <summary>
    /// Text oluşturma factory'si
    /// </summary>
    public class TextFactory
    {
        private Transform parent;

        public TextFactory(Transform parent)
        {
            this.parent = parent;
        }

        public GameObject Create(string text, Transform customParent = null, int fontSize = 24,
            Color? color = null, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
        {
            Transform targetParent = customParent ?? parent;

            GameObject textObj = new GameObject($"Text_{text.Substring(0, Mathf.Min(10, text.Length))}");
            textObj.transform.SetParent(targetParent, false);

            RectTransform rect = textObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(400, fontSize + 20);

            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.alignment = alignment;
            tmp.color = color ?? Color.white;
            tmp.enableWordWrapping = true;

            return textObj;
        }

        public TextMeshProUGUI GetTextComponent(GameObject textObj)
        {
            return textObj.GetComponent<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Image oluşturma factory'si
    /// </summary>
    public class ImageFactory
    {
        private Transform parent;

        public ImageFactory(Transform parent)
        {
            this.parent = parent;
        }

        public GameObject Create(string name, Sprite sprite = null, Transform customParent = null,
            Vector2? size = null, Color? color = null)
        {
            Transform targetParent = customParent ?? parent;

            GameObject imageObj = new GameObject($"Image_{name}");
            imageObj.transform.SetParent(targetParent, false);

            RectTransform rect = imageObj.AddComponent<RectTransform>();
            rect.sizeDelta = size ?? new Vector2(100, 100);

            Image image = imageObj.AddComponent<Image>();
            image.sprite = sprite;
            image.color = color ?? Color.white;
            image.preserveAspect = true;

            return imageObj;
        }

        public GameObject CreateCircle(string name, Transform customParent = null,
            float radius = 50f, Color? color = null)
        {
            GameObject imageObj = Create(name, null, customParent, new Vector2(radius * 2, radius * 2), color);
            // Not: Gerçek daire için circular sprite gerekir
            return imageObj;
        }
    }

    /// <summary>
    /// Input field oluşturma factory'si
    /// </summary>
    public class InputFactory
    {
        private Transform parent;

        public InputFactory(Transform parent)
        {
            this.parent = parent;
        }

        public GameObject Create(string placeholder, Transform customParent = null,
            Vector2? size = null, Action<string> onValueChanged = null)
        {
            Transform targetParent = customParent ?? parent;

            GameObject inputObj = new GameObject("InputField");
            inputObj.transform.SetParent(targetParent, false);

            RectTransform rect = inputObj.AddComponent<RectTransform>();
            rect.sizeDelta = size ?? new Vector2(300, 50);

            Image image = inputObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            // Text area
            GameObject textArea = new GameObject("TextArea");
            textArea.transform.SetParent(inputObj.transform, false);

            RectTransform textAreaRect = textArea.AddComponent<RectTransform>();
            textAreaRect.anchorMin = Vector2.zero;
            textAreaRect.anchorMax = Vector2.one;
            textAreaRect.offsetMin = new Vector2(10, 5);
            textAreaRect.offsetMax = new Vector2(-10, -5);

            // Placeholder
            GameObject placeholderObj = new GameObject("Placeholder");
            placeholderObj.transform.SetParent(textArea.transform, false);

            RectTransform phRect = placeholderObj.AddComponent<RectTransform>();
            phRect.anchorMin = Vector2.zero;
            phRect.anchorMax = Vector2.one;
            phRect.sizeDelta = Vector2.zero;

            TextMeshProUGUI phText = placeholderObj.AddComponent<TextMeshProUGUI>();
            phText.text = placeholder;
            phText.fontSize = 20;
            phText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            phText.alignment = TextAlignmentOptions.Left;

            // Input text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(textArea.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            TextMeshProUGUI inputText = textObj.AddComponent<TextMeshProUGUI>();
            inputText.fontSize = 20;
            inputText.color = Color.white;
            inputText.alignment = TextAlignmentOptions.Left;

            // TMP Input Field
            TMP_InputField inputField = inputObj.AddComponent<TMP_InputField>();
            inputField.textViewport = textAreaRect;
            inputField.textComponent = inputText;
            inputField.placeholder = phText;

            if (onValueChanged != null)
            {
                inputField.onValueChanged.AddListener((value) => onValueChanged(value));
            }

            return inputObj;
        }
    }

    /// <summary>
    /// ScrollView oluşturma factory'si
    /// </summary>
    public class ScrollViewFactory
    {
        private Transform parent;

        public ScrollViewFactory(Transform parent)
        {
            this.parent = parent;
        }

        public GameObject Create(string name, Transform customParent = null, Vector2? size = null,
            bool vertical = true, bool horizontal = false)
        {
            Transform targetParent = customParent ?? parent;

            // Scroll View
            GameObject scrollObj = new GameObject($"ScrollView_{name}");
            scrollObj.transform.SetParent(targetParent, false);

            RectTransform scrollRect = scrollObj.AddComponent<RectTransform>();
            if (size.HasValue)
            {
                scrollRect.sizeDelta = size.Value;
            }
            else
            {
                scrollRect.anchorMin = Vector2.zero;
                scrollRect.anchorMax = Vector2.one;
                scrollRect.sizeDelta = Vector2.zero;
            }

            ScrollRect scroll = scrollObj.AddComponent<ScrollRect>();
            scroll.horizontal = horizontal;
            scroll.vertical = vertical;

            Image scrollImage = scrollObj.AddComponent<Image>();
            scrollImage.color = new Color(0.1f, 0.1f, 0.1f, 0.5f);

            scrollObj.AddComponent<Mask>();

            // Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollObj.transform, false);

            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.sizeDelta = Vector2.zero;
            viewportRect.pivot = new Vector2(0, 1);

            viewport.AddComponent<Image>().color = Color.clear;
            viewport.AddComponent<Mask>().showMaskGraphic = false;

            scroll.viewport = viewportRect;

            // Content
            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);

            RectTransform contentRect = content.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.sizeDelta = new Vector2(0, 0);

            scroll.content = contentRect;

            // Content layout
            if (vertical)
            {
                VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
                layout.spacing = 10;
                layout.padding = new RectOffset(10, 10, 10, 10);
                layout.childAlignment = TextAnchor.UpperCenter;
                layout.childControlHeight = false;
                layout.childControlWidth = true;
            }

            ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            return scrollObj;
        }

        public Transform GetContent(GameObject scrollView)
        {
            return scrollView.GetComponent<ScrollRect>().content;
        }
    }
}
