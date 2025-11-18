using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Helper class for UI animations.
    /// </summary>
    public static class UIAnimations
    {
        #region Fade Animations

        /// <summary>
        /// Fade a CanvasGroup.
        /// </summary>
        public static IEnumerator Fade(CanvasGroup group, float from, float to, float duration)
        {
            float elapsed = 0f;
            group.alpha = from;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                group.alpha = Mathf.Lerp(from, to, elapsed / duration);
                yield return null;
            }

            group.alpha = to;
        }

        /// <summary>
        /// Fade in a CanvasGroup.
        /// </summary>
        public static IEnumerator FadeIn(CanvasGroup group, float duration = 0.3f)
        {
            return Fade(group, 0f, 1f, duration);
        }

        /// <summary>
        /// Fade out a CanvasGroup.
        /// </summary>
        public static IEnumerator FadeOut(CanvasGroup group, float duration = 0.3f)
        {
            return Fade(group, 1f, 0f, duration);
        }

        /// <summary>
        /// Fade a Graphic (Image, Text, etc).
        /// </summary>
        public static IEnumerator FadeGraphic(Graphic graphic, float from, float to, float duration)
        {
            float elapsed = 0f;
            Color color = graphic.color;
            color.a = from;
            graphic.color = color;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                color.a = Mathf.Lerp(from, to, elapsed / duration);
                graphic.color = color;
                yield return null;
            }

            color.a = to;
            graphic.color = color;
        }

        #endregion

        #region Scale Animations

        /// <summary>
        /// Scale a transform.
        /// </summary>
        public static IEnumerator Scale(Transform transform, Vector3 from, Vector3 to, float duration)
        {
            float elapsed = 0f;
            transform.localScale = from;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                // Ease out cubic
                t = 1f - Mathf.Pow(1f - t, 3f);
                transform.localScale = Vector3.Lerp(from, to, t);
                yield return null;
            }

            transform.localScale = to;
        }

        /// <summary>
        /// Pop in effect (scale from small to normal).
        /// </summary>
        public static IEnumerator PopIn(Transform transform, float duration = 0.3f)
        {
            return Scale(transform, Vector3.zero, Vector3.one, duration);
        }

        /// <summary>
        /// Pop out effect (scale from normal to small).
        /// </summary>
        public static IEnumerator PopOut(Transform transform, float duration = 0.3f)
        {
            return Scale(transform, Vector3.one, Vector3.zero, duration);
        }

        /// <summary>
        /// Bounce scale effect.
        /// </summary>
        public static IEnumerator Bounce(Transform transform, float scale = 1.1f, float duration = 0.2f)
        {
            Vector3 original = transform.localScale;
            Vector3 scaled = original * scale;

            // Scale up
            float elapsed = 0f;
            float halfDuration = duration * 0.5f;

            while (elapsed < halfDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                transform.localScale = Vector3.Lerp(original, scaled, elapsed / halfDuration);
                yield return null;
            }

            // Scale back
            elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                transform.localScale = Vector3.Lerp(scaled, original, elapsed / halfDuration);
                yield return null;
            }

            transform.localScale = original;
        }

        #endregion

        #region Movement Animations

        /// <summary>
        /// Move a RectTransform.
        /// </summary>
        public static IEnumerator Move(RectTransform rect, Vector2 from, Vector2 to, float duration)
        {
            float elapsed = 0f;
            rect.anchoredPosition = from;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                // Ease out cubic
                t = 1f - Mathf.Pow(1f - t, 3f);
                rect.anchoredPosition = Vector2.Lerp(from, to, t);
                yield return null;
            }

            rect.anchoredPosition = to;
        }

        /// <summary>
        /// Slide in from direction.
        /// </summary>
        public static IEnumerator SlideIn(RectTransform rect, Direction direction, float duration = 0.3f)
        {
            Vector2 to = rect.anchoredPosition;
            Vector2 from = to;

            float offset = direction == Direction.Left || direction == Direction.Right
                ? Screen.width
                : Screen.height;

            switch (direction)
            {
                case Direction.Left: from.x = -offset; break;
                case Direction.Right: from.x = offset; break;
                case Direction.Up: from.y = offset; break;
                case Direction.Down: from.y = -offset; break;
            }

            return Move(rect, from, to, duration);
        }

        /// <summary>
        /// Slide out to direction.
        /// </summary>
        public static IEnumerator SlideOut(RectTransform rect, Direction direction, float duration = 0.3f)
        {
            Vector2 from = rect.anchoredPosition;
            Vector2 to = from;

            float offset = direction == Direction.Left || direction == Direction.Right
                ? Screen.width
                : Screen.height;

            switch (direction)
            {
                case Direction.Left: to.x = -offset; break;
                case Direction.Right: to.x = offset; break;
                case Direction.Up: to.y = offset; break;
                case Direction.Down: to.y = -offset; break;
            }

            return Move(rect, from, to, duration);
        }

        /// <summary>
        /// Shake effect.
        /// </summary>
        public static IEnumerator Shake(RectTransform rect, float intensity = 10f, float duration = 0.3f)
        {
            Vector2 original = rect.anchoredPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float strength = intensity * (1f - elapsed / duration);
                rect.anchoredPosition = original + Random.insideUnitCircle * strength;
                yield return null;
            }

            rect.anchoredPosition = original;
        }

        #endregion

        #region Progress Bar Animations

        /// <summary>
        /// Animate a progress bar fill.
        /// </summary>
        public static IEnumerator AnimateProgress(Image fillImage, float fromValue, float toValue, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                float value = Mathf.Lerp(fromValue, toValue, t);

                // Update anchors for fill
                var rect = fillImage.rectTransform;
                rect.anchorMax = new Vector2(value / 100f, 1f);

                yield return null;
            }

            fillImage.rectTransform.anchorMax = new Vector2(toValue / 100f, 1f);
        }

        /// <summary>
        /// Animate a TextMeshPro number counter.
        /// </summary>
        public static IEnumerator AnimateNumber(TextMeshProUGUI text, float from, float to, float duration, string format = "0")
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                float value = Mathf.Lerp(from, to, t);
                text.text = value.ToString(format);
                yield return null;
            }

            text.text = to.ToString(format);
        }

        #endregion

        #region Color Animations

        /// <summary>
        /// Animate color of a Graphic.
        /// </summary>
        public static IEnumerator AnimateColor(Graphic graphic, Color from, Color to, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                graphic.color = Color.Lerp(from, to, elapsed / duration);
                yield return null;
            }

            graphic.color = to;
        }

        /// <summary>
        /// Pulse color effect.
        /// </summary>
        public static IEnumerator PulseColor(Graphic graphic, Color pulseColor, float duration = 0.5f)
        {
            Color original = graphic.color;

            yield return AnimateColor(graphic, original, pulseColor, duration * 0.5f);
            yield return AnimateColor(graphic, pulseColor, original, duration * 0.5f);
        }

        #endregion

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }
    }
}
