using System;
using System.Collections.Generic;
using UnityEngine;

namespace BitLifeTR.Core
{
    /// <summary>
    /// Event-driven mimari için merkezi event bus
    /// Sistemler arası iletişimi sağlar
    /// </summary>
    public static class EventBus
    {
        private static Dictionary<Type, List<Delegate>> eventHandlers = new Dictionary<Type, List<Delegate>>();

        /// <summary>
        /// Bir event tipine abone ol
        /// </summary>
        public static void Subscribe<T>(Action<T> handler)
        {
            Type eventType = typeof(T);

            if (!eventHandlers.ContainsKey(eventType))
            {
                eventHandlers[eventType] = new List<Delegate>();
            }

            eventHandlers[eventType].Add(handler);
        }

        /// <summary>
        /// Bir event tipinden aboneliği kaldır
        /// </summary>
        public static void Unsubscribe<T>(Action<T> handler)
        {
            Type eventType = typeof(T);

            if (eventHandlers.ContainsKey(eventType))
            {
                eventHandlers[eventType].Remove(handler);
            }
        }

        /// <summary>
        /// Event yayınla - tüm abonelere bildirim gönder
        /// </summary>
        public static void Publish<T>(T eventData)
        {
            Type eventType = typeof(T);

            if (eventHandlers.ContainsKey(eventType))
            {
                foreach (var handler in eventHandlers[eventType].ToArray())
                {
                    try
                    {
                        ((Action<T>)handler)?.Invoke(eventData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"EventBus hata: {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Belirli bir event tipinin tüm abonelerini temizle
        /// </summary>
        public static void Clear<T>()
        {
            Type eventType = typeof(T);

            if (eventHandlers.ContainsKey(eventType))
            {
                eventHandlers[eventType].Clear();
            }
        }

        /// <summary>
        /// Tüm eventleri temizle
        /// </summary>
        public static void ClearAll()
        {
            eventHandlers.Clear();
        }
    }

    // UI Eventleri
    public class ScreenChangedEvent
    {
        public ScreenType NewScreen { get; }
        public ScreenChangedEvent(ScreenType newScreen) => NewScreen = newScreen;
    }

    public class ButtonClickedEvent
    {
        public string ButtonId { get; }
        public ButtonClickedEvent(string buttonId) => ButtonId = buttonId;
    }

    // Stat Eventleri
    public class StatChangedEvent
    {
        public StatType StatType { get; }
        public float OldValue { get; }
        public float NewValue { get; }
        public StatChangedEvent(StatType statType, float oldValue, float newValue)
        {
            StatType = statType;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    // İlişki Eventleri
    public class RelationshipChangedEvent
    {
        public string PersonId { get; }
        public float OldValue { get; }
        public float NewValue { get; }
        public RelationshipChangedEvent(string personId, float oldValue, float newValue)
        {
            PersonId = personId;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    // Kariyer Eventleri
    public class JobChangedEvent
    {
        public string OldJob { get; }
        public string NewJob { get; }
        public JobChangedEvent(string oldJob, string newJob)
        {
            OldJob = oldJob;
            NewJob = newJob;
        }
    }

    public class EducationCompletedEvent
    {
        public EducationLevel Level { get; }
        public EducationCompletedEvent(EducationLevel level) => Level = level;
    }

    // Olay Eventleri
    public class GameEventTriggeredEvent
    {
        public GameEvent Event { get; }
        public GameEventTriggeredEvent(GameEvent evt) => Event = evt;
    }

    public class DecisionMadeEvent
    {
        public string EventId { get; }
        public int ChoiceIndex { get; }
        public DecisionMadeEvent(string eventId, int choiceIndex)
        {
            EventId = eventId;
            ChoiceIndex = choiceIndex;
        }
    }
}
