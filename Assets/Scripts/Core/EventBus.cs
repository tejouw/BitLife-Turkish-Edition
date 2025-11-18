using System;
using System.Collections.Generic;
using UnityEngine;

namespace BitLifeTR.Core
{
    /// <summary>
    /// Central event bus for communication between game systems.
    /// Allows decoupled communication using publish/subscribe pattern.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _eventHandlers = new Dictionary<Type, List<Delegate>>();

        /// <summary>
        /// Subscribe to an event type.
        /// </summary>
        public static void Subscribe<T>(Action<T> handler) where T : struct
        {
            var type = typeof(T);

            if (!_eventHandlers.ContainsKey(type))
            {
                _eventHandlers[type] = new List<Delegate>();
            }

            _eventHandlers[type].Add(handler);
        }

        /// <summary>
        /// Unsubscribe from an event type.
        /// </summary>
        public static void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            var type = typeof(T);

            if (_eventHandlers.ContainsKey(type))
            {
                _eventHandlers[type].Remove(handler);
            }
        }

        /// <summary>
        /// Publish an event to all subscribers.
        /// </summary>
        public static void Publish<T>(T eventData) where T : struct
        {
            var type = typeof(T);

            if (_eventHandlers.ContainsKey(type))
            {
                var handlers = _eventHandlers[type].ToArray();

                foreach (var handler in handlers)
                {
                    try
                    {
                        ((Action<T>)handler)?.Invoke(eventData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[EventBus] Error invoking handler for {type.Name}: {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Clear all event subscriptions.
        /// </summary>
        public static void Clear()
        {
            _eventHandlers.Clear();
        }

        /// <summary>
        /// Clear subscriptions for a specific event type.
        /// </summary>
        public static void Clear<T>() where T : struct
        {
            var type = typeof(T);

            if (_eventHandlers.ContainsKey(type))
            {
                _eventHandlers[type].Clear();
            }
        }
    }

    #region Game Events

    /// <summary>
    /// Fired when game state changes.
    /// </summary>
    public struct GameStateChangedEvent
    {
        public GameState PreviousState;
        public GameState NewState;
    }

    /// <summary>
    /// Fired when a new year begins.
    /// </summary>
    public struct YearAdvancedEvent
    {
        public int NewAge;
        public LifeStage LifeStage;
    }

    /// <summary>
    /// Fired when a stat value changes.
    /// </summary>
    public struct StatChangedEvent
    {
        public string StatName;
        public float OldValue;
        public float NewValue;
        public float Delta;
    }

    /// <summary>
    /// Fired when character dies.
    /// </summary>
    public struct CharacterDiedEvent
    {
        public int Age;
        public string CauseOfDeath;
    }

    /// <summary>
    /// Fired when a game event occurs.
    /// </summary>
    public struct GameEventOccurredEvent
    {
        public string EventId;
        public string EventTitle;
        public string EventDescription;
    }

    /// <summary>
    /// Fired when player makes a decision.
    /// </summary>
    public struct DecisionMadeEvent
    {
        public string EventId;
        public int ChoiceIndex;
        public string ChoiceText;
    }

    /// <summary>
    /// Fired when money changes.
    /// </summary>
    public struct MoneyChangedEvent
    {
        public decimal OldAmount;
        public decimal NewAmount;
        public decimal Delta;
        public string Reason;
    }

    /// <summary>
    /// Fired when a relationship changes.
    /// </summary>
    public struct RelationshipChangedEvent
    {
        public string PersonId;
        public string PersonName;
        public int OldValue;
        public int NewValue;
    }

    /// <summary>
    /// Fired when UI needs to show a notification.
    /// </summary>
    public struct ShowNotificationEvent
    {
        public string Title;
        public string Message;
        public float Duration;
    }

    /// <summary>
    /// Fired when game is saved.
    /// </summary>
    public struct GameSavedEvent
    {
        public string SaveSlot;
        public DateTime SaveTime;
    }

    /// <summary>
    /// Fired when game is loaded.
    /// </summary>
    public struct GameLoadedEvent
    {
        public string SaveSlot;
    }

    #endregion
}
