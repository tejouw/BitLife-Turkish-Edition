using System;
using System.Collections.Generic;
using BitLifeTR.Core;

namespace BitLifeTR.Data
{
    /// <summary>
    /// Represents a game event that can occur during gameplay.
    /// </summary>
    [Serializable]
    public class GameEvent
    {
        /// <summary>Unique identifier for this event</summary>
        public string Id;

        /// <summary>Display title of the event</summary>
        public string Title;

        /// <summary>Main description/narrative text</summary>
        public string Description;

        /// <summary>Category of the event</summary>
        public EventCategory Category;

        /// <summary>Conditions that must be met for this event to trigger</summary>
        public List<EventCondition> Conditions;

        /// <summary>Available choices/decisions</summary>
        public List<EventChoice> Choices;

        /// <summary>Weight for random selection (higher = more likely)</summary>
        public float Weight;

        /// <summary>Whether this event can only happen once per lifetime</summary>
        public bool OneTimeOnly;

        /// <summary>Minimum age for this event</summary>
        public int MinAge;

        /// <summary>Maximum age for this event</summary>
        public int MaxAge;

        /// <summary>Specific life stages when this event can occur</summary>
        public List<LifeStage> ValidLifeStages;

        /// <summary>Whether this is a mandatory/story event</summary>
        public bool IsMandatory;

        /// <summary>Priority for mandatory events (higher = first)</summary>
        public int Priority;

        public GameEvent()
        {
            Id = Guid.NewGuid().ToString();
            Conditions = new List<EventCondition>();
            Choices = new List<EventChoice>();
            ValidLifeStages = new List<LifeStage>();
            Weight = 1f;
            MinAge = 0;
            MaxAge = 122;
        }

        /// <summary>
        /// Check if this event has choices (is a decision event).
        /// </summary>
        public bool HasChoices => Choices != null && Choices.Count > 0;

        /// <summary>
        /// Create a simple event with automatic outcome.
        /// </summary>
        public static GameEvent CreateSimple(string id, string title, string description,
            EventCategory category, EventOutcome outcome)
        {
            return new GameEvent
            {
                Id = id,
                Title = title,
                Description = description,
                Category = category,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "Tamam",
                        Outcomes = new List<EventOutcome> { outcome }
                    }
                }
            };
        }

        /// <summary>
        /// Create an event with multiple choices.
        /// </summary>
        public static GameEvent CreateDecision(string id, string title, string description,
            EventCategory category, params EventChoice[] choices)
        {
            return new GameEvent
            {
                Id = id,
                Title = title,
                Description = description,
                Category = category,
                Choices = new List<EventChoice>(choices)
            };
        }
    }

    /// <summary>
    /// Categories of events.
    /// </summary>
    public enum EventCategory
    {
        Genel,
        Aile,
        Egitim,
        Kariyer,
        Iliski,
        Saglik,
        Finans,
        Suc,
        Eglence,
        Random,
        Ozel // Special/milestone events
    }

    /// <summary>
    /// A choice/decision option in an event.
    /// </summary>
    [Serializable]
    public class EventChoice
    {
        /// <summary>Text displayed on the choice button</summary>
        public string Text;

        /// <summary>Possible outcomes of this choice</summary>
        public List<EventOutcome> Outcomes;

        /// <summary>Conditions required to see this choice</summary>
        public List<EventCondition> Conditions;

        /// <summary>Whether this choice is available</summary>
        public bool IsAvailable;

        /// <summary>Reason why choice is unavailable</summary>
        public string UnavailableReason;

        public EventChoice()
        {
            Outcomes = new List<EventOutcome>();
            Conditions = new List<EventCondition>();
            IsAvailable = true;
        }

        /// <summary>
        /// Create a simple choice.
        /// </summary>
        public static EventChoice Create(string text, EventOutcome outcome)
        {
            return new EventChoice
            {
                Text = text,
                Outcomes = new List<EventOutcome> { outcome }
            };
        }

        /// <summary>
        /// Create a choice with multiple possible outcomes.
        /// </summary>
        public static EventChoice CreateWithChance(string text, params (EventOutcome outcome, float chance)[] outcomes)
        {
            var choice = new EventChoice { Text = text };

            foreach (var (outcome, chance) in outcomes)
            {
                outcome.Chance = chance;
                choice.Outcomes.Add(outcome);
            }

            return choice;
        }
    }

    /// <summary>
    /// The outcome/result of an event choice.
    /// </summary>
    [Serializable]
    public class EventOutcome
    {
        /// <summary>Description of what happened</summary>
        public string Description;

        /// <summary>Stat changes</summary>
        public StatModifierData StatChanges;

        /// <summary>Money change</summary>
        public decimal MoneyChange;

        /// <summary>Chance of this outcome (0-1)</summary>
        public float Chance;

        /// <summary>Follow-up event ID</summary>
        public string FollowUpEventId;

        /// <summary>Achievement unlocked</summary>
        public string AchievementId;

        /// <summary>Relationship changes</summary>
        public List<RelationshipChange> RelationshipChanges;

        /// <summary>Asset changes (add/remove)</summary>
        public List<AssetChange> AssetChanges;

        /// <summary>Special effects</summary>
        public OutcomeEffects Effects;

        public EventOutcome()
        {
            Chance = 1f;
            StatChanges = new StatModifierData();
            RelationshipChanges = new List<RelationshipChange>();
            AssetChanges = new List<AssetChange>();
            Effects = new OutcomeEffects();
        }

        /// <summary>
        /// Create a simple stat change outcome.
        /// </summary>
        public static EventOutcome StatChange(string description, float health = 0, float happiness = 0,
            float intelligence = 0, float looks = 0, float fame = 0, float karma = 0)
        {
            return new EventOutcome
            {
                Description = description,
                StatChanges = new StatModifierData
                {
                    Health = health,
                    Happiness = happiness,
                    Intelligence = intelligence,
                    Looks = looks,
                    Fame = fame,
                    Karma = karma
                }
            };
        }

        /// <summary>
        /// Create a money outcome.
        /// </summary>
        public static EventOutcome Money(string description, decimal amount)
        {
            return new EventOutcome
            {
                Description = description,
                MoneyChange = amount
            };
        }
    }

    /// <summary>
    /// Stat modifier data for serialization.
    /// </summary>
    [Serializable]
    public class StatModifierData
    {
        public float Health;
        public float Happiness;
        public float Intelligence;
        public float Looks;
        public float Fame;
        public float Karma;
    }

    /// <summary>
    /// Change to a relationship.
    /// </summary>
    [Serializable]
    public class RelationshipChange
    {
        public RelationType TargetType;
        public string TargetId;
        public int Change;
    }

    /// <summary>
    /// Change to assets.
    /// </summary>
    [Serializable]
    public class AssetChange
    {
        public bool IsAdding;
        public AssetType Type;
        public string Name;
        public decimal Value;
    }

    /// <summary>
    /// Special effects from an outcome.
    /// </summary>
    [Serializable]
    public class OutcomeEffects
    {
        public bool StartSchool;
        public string SchoolName;
        public bool GraduateSchool;
        public bool GetJob;
        public string JobName;
        public bool LoseJob;
        public bool GetMarried;
        public bool GetDivorced;
        public bool HaveChild;
        public bool GoToJail;
        public int JailYears;
        public bool Die;
        public string DeathCause;
    }
}
