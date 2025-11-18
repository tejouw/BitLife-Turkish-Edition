using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using BitLifeTR.Core;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Manages in-game notifications and toasts.
    /// </summary>
    public class NotificationSystem : Singleton<NotificationSystem>
    {
        private Queue<NotificationData> notificationQueue = new Queue<NotificationData>();
        private RectTransform notificationContainer;
        private bool isShowingNotification = false;

        protected override void OnSingletonAwake()
        {
            EventBus.Subscribe<ShowNotificationEvent>(OnShowNotification);
            CreateNotificationContainer();
            Debug.Log("[NotificationSystem] Initialized");
        }

        protected override void OnDestroy()
        {
            EventBus.Unsubscribe<ShowNotificationEvent>(OnShowNotification);
            base.OnDestroy();
        }

        private void CreateNotificationContainer()
        {
            // Create notification container at top of screen
            if (UIManager.Instance?.MainCanvas == null) return;

            notificationContainer = UIFactory.CreateContainer("NotificationContainer", UIManager.Instance.MainCanvas.transform);
            notificationContainer.anchorMin = new Vector2(0.1f, 0.85f);
            notificationContainer.anchorMax = new Vector2(0.9f, 0.95f);
            notificationContainer.offsetMin = Vector2.zero;
            notificationContainer.offsetMax = Vector2.zero;
        }

        private void OnShowNotification(ShowNotificationEvent e)
        {
            ShowNotification(e.Title, e.Message, e.Duration);
        }

        /// <summary>
        /// Show a notification.
        /// </summary>
        public void ShowNotification(string title, string message, float duration = 3f)
        {
            notificationQueue.Enqueue(new NotificationData
            {
                Title = title,
                Message = message,
                Duration = duration
            });

            if (!isShowingNotification)
            {
                StartCoroutine(ProcessNotificationQueue());
            }
        }

        private IEnumerator ProcessNotificationQueue()
        {
            isShowingNotification = true;

            while (notificationQueue.Count > 0)
            {
                var notification = notificationQueue.Dequeue();
                yield return ShowNotificationCoroutine(notification);
            }

            isShowingNotification = false;
        }

        private IEnumerator ShowNotificationCoroutine(NotificationData data)
        {
            if (notificationContainer == null)
            {
                CreateNotificationContainer();
                if (notificationContainer == null) yield break;
            }

            // Create notification panel
            var panel = UIFactory.CreatePanel("Notification", notificationContainer, UITheme.BackgroundPanel);
            UIFactory.SetFullStretch(panel);
            UIFactory.AddVerticalLayout(panel, UITheme.SpacingTiny, new RectOffset(15, 15, 10, 10), TextAnchor.MiddleCenter);

            // Add canvas group for fading
            var canvasGroup = panel.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;

            // Title
            if (!string.IsNullOrEmpty(data.Title))
            {
                var title = UIFactory.CreateText("Title", panel, data.Title, UITheme.FontSizeSmall,
                    UITheme.TextPrimary, TextAlignmentOptions.Center, FontStyles.Bold);
                UIFactory.AddLayoutElement(title.rectTransform, -1, 20);
            }

            // Message
            var message = UIFactory.CreateText("Message", panel, data.Message, UITheme.FontSizeSmall,
                UITheme.TextSecondary, TextAlignmentOptions.Center);
            UIFactory.AddLayoutElement(message.rectTransform, -1, 20);

            // Fade in
            yield return UIAnimations.FadeIn(canvasGroup, 0.3f);

            // Wait
            yield return new WaitForSeconds(data.Duration);

            // Fade out
            yield return UIAnimations.FadeOut(canvasGroup, 0.3f);

            // Destroy
            Destroy(panel.gameObject);
        }

        private struct NotificationData
        {
            public string Title;
            public string Message;
            public float Duration;
        }
    }
}
