using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BitLifeTR.Core;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Manages game events, selection, and execution.
    /// </summary>
    public class EventManager : Singleton<EventManager>
    {
        private List<GameEvent> allEvents = new List<GameEvent>();
        private HashSet<string> triggeredOneTimeEvents = new HashSet<string>();
        private GameEvent currentEvent;
        private Queue<GameEvent> eventQueue = new Queue<GameEvent>();

        public GameEvent CurrentEvent => currentEvent;
        public bool HasPendingEvents => eventQueue.Count > 0;

        protected override void OnSingletonAwake()
        {
            Debug.Log("[EventManager] Initialized");
        }

        #region Event Registration

        /// <summary>
        /// Register a new event.
        /// </summary>
        public void RegisterEvent(GameEvent gameEvent)
        {
            if (gameEvent == null) return;

            allEvents.Add(gameEvent);
        }

        /// <summary>
        /// Register multiple events.
        /// </summary>
        public void RegisterEvents(IEnumerable<GameEvent> events)
        {
            foreach (var e in events)
            {
                RegisterEvent(e);
            }
        }

        /// <summary>
        /// Clear all registered events.
        /// </summary>
        public void ClearEvents()
        {
            allEvents.Clear();
            triggeredOneTimeEvents.Clear();
        }

        /// <summary>
        /// Get event by ID.
        /// </summary>
        public GameEvent GetEvent(string eventId)
        {
            return allEvents.Find(e => e.Id == eventId);
        }

        #endregion

        #region Event Selection

        /// <summary>
        /// Get a random event for the current character state.
        /// </summary>
        public GameEvent GetRandomEvent()
        {
            var character = CharacterManager.Instance.CurrentCharacter;
            if (character == null) return null;

            // Get all valid events
            var validEvents = allEvents.Where(e => IsEventValid(e, character)).ToList();

            if (validEvents.Count == 0)
                return null;

            // Separate mandatory and random events
            var mandatoryEvents = validEvents.Where(e => e.IsMandatory).OrderByDescending(e => e.Priority).ToList();

            if (mandatoryEvents.Count > 0)
            {
                return mandatoryEvents[0];
            }

            // Weighted random selection
            return WeightedRandomSelect(validEvents);
        }

        /// <summary>
        /// Get events for a specific category.
        /// </summary>
        public List<GameEvent> GetEventsByCategory(EventCategory category)
        {
            var character = CharacterManager.Instance.CurrentCharacter;
            if (character == null) return new List<GameEvent>();

            return allEvents
                .Where(e => e.Category == category && IsEventValid(e, character))
                .ToList();
        }

        /// <summary>
        /// Get multiple random events for the year.
        /// </summary>
        public List<GameEvent> GetYearlyEvents(int count = 3)
        {
            var events = new List<GameEvent>();
            var usedIds = new HashSet<string>();

            for (int i = 0; i < count; i++)
            {
                var validEvents = allEvents
                    .Where(e => IsEventValid(e, CharacterManager.Instance.CurrentCharacter) && !usedIds.Contains(e.Id))
                    .ToList();

                if (validEvents.Count == 0)
                    break;

                var selected = WeightedRandomSelect(validEvents);
                if (selected != null)
                {
                    events.Add(selected);
                    usedIds.Add(selected.Id);
                }
            }

            return events;
        }

        private bool IsEventValid(GameEvent gameEvent, CharacterData character)
        {
            // Check one-time events
            if (gameEvent.OneTimeOnly && triggeredOneTimeEvents.Contains(gameEvent.Id))
                return false;

            // Check age
            if (character.Age < gameEvent.MinAge || character.Age > gameEvent.MaxAge)
                return false;

            // Check life stage
            if (gameEvent.ValidLifeStages.Count > 0 && !gameEvent.ValidLifeStages.Contains(character.LifeStage))
                return false;

            // Check all conditions
            foreach (var condition in gameEvent.Conditions)
            {
                if (!EvaluateCondition(condition, character))
                    return false;
            }

            return true;
        }

        private GameEvent WeightedRandomSelect(List<GameEvent> events)
        {
            if (events.Count == 0) return null;
            if (events.Count == 1) return events[0];

            float totalWeight = events.Sum(e => e.Weight);
            float random = Random.value * totalWeight;
            float current = 0;

            foreach (var e in events)
            {
                current += e.Weight;
                if (random <= current)
                    return e;
            }

            return events[events.Count - 1];
        }

        #endregion

        #region Condition Evaluation

        /// <summary>
        /// Evaluate if a condition is met.
        /// </summary>
        public bool EvaluateCondition(EventCondition condition, CharacterData character)
        {
            switch (condition.Type)
            {
                case ConditionType.Age:
                    return CompareNumbers(character.Age, condition.NumericValue, condition.Operator);

                case ConditionType.Stat:
                    var statValue = CharacterManager.Instance.Stats.GetStat(condition.Parameter);
                    return CompareNumbers(statValue, condition.NumericValue, condition.Operator);

                case ConditionType.Money:
                    return CompareNumbers((float)character.Money, condition.NumericValue, condition.Operator);

                case ConditionType.Education:
                    return CompareNumbers((int)character.EducationLevel, condition.NumericValue, condition.Operator);

                case ConditionType.HasJob:
                    return character.IsEmployed == condition.BoolValue;

                case ConditionType.IsMarried:
                    return character.IsMarried == condition.BoolValue;

                case ConditionType.Gender:
                    return (int)character.Gender == (int)condition.NumericValue;

                case ConditionType.LifeStage:
                    return (int)character.LifeStage == (int)condition.NumericValue;

                case ConditionType.HasChildren:
                    return (character.ChildrenCount > 0) == condition.BoolValue;

                case ConditionType.HasCriminalRecord:
                    return character.HasCriminalRecord == condition.BoolValue;

                case ConditionType.CompletedMilitary:
                    return character.CompletedMilitaryService == condition.BoolValue;

                case ConditionType.HasRelationship:
                    var relType = (RelationType)(int)condition.NumericValue;
                    return character.Relationships.Any(r => r.Type == relType && r.IsAlive);

                case ConditionType.OwnsAsset:
                    var assetType = (AssetType)(int)condition.NumericValue;
                    return character.Assets.Any(a => a.Type == assetType);

                default:
                    return true;
            }
        }

        private bool CompareNumbers(float value1, float value2, ComparisonOperator op)
        {
            return op switch
            {
                ComparisonOperator.Equal => Mathf.Approximately(value1, value2),
                ComparisonOperator.NotEqual => !Mathf.Approximately(value1, value2),
                ComparisonOperator.GreaterThan => value1 > value2,
                ComparisonOperator.LessThan => value1 < value2,
                ComparisonOperator.GreaterOrEqual => value1 >= value2,
                ComparisonOperator.LessOrEqual => value1 <= value2,
                _ => true
            };
        }

        #endregion

        #region Event Execution

        /// <summary>
        /// Start processing an event.
        /// </summary>
        public void TriggerEvent(GameEvent gameEvent)
        {
            if (gameEvent == null) return;

            currentEvent = gameEvent;

            if (gameEvent.OneTimeOnly)
            {
                triggeredOneTimeEvents.Add(gameEvent.Id);
            }

            // Publish event occurred
            EventBus.Publish(new GameEventOccurredEvent
            {
                EventId = gameEvent.Id,
                EventTitle = gameEvent.Title,
                EventDescription = gameEvent.Description
            });

            Debug.Log($"[EventManager] Triggered event: {gameEvent.Title}");
        }

        /// <summary>
        /// Queue an event to be processed later.
        /// </summary>
        public void QueueEvent(GameEvent gameEvent)
        {
            if (gameEvent != null)
            {
                eventQueue.Enqueue(gameEvent);
            }
        }

        /// <summary>
        /// Process next queued event.
        /// </summary>
        public GameEvent ProcessNextEvent()
        {
            if (eventQueue.Count > 0)
            {
                var nextEvent = eventQueue.Dequeue();
                TriggerEvent(nextEvent);
                return nextEvent;
            }
            return null;
        }

        /// <summary>
        /// Execute a choice from the current event.
        /// </summary>
        public EventOutcome ExecuteChoice(int choiceIndex)
        {
            if (currentEvent == null || choiceIndex < 0 || choiceIndex >= currentEvent.Choices.Count)
                return null;

            var choice = currentEvent.Choices[choiceIndex];

            // Publish decision made
            EventBus.Publish(new DecisionMadeEvent
            {
                EventId = currentEvent.Id,
                ChoiceIndex = choiceIndex,
                ChoiceText = choice.Text
            });

            // Select outcome based on chances
            var outcome = SelectOutcome(choice.Outcomes);

            if (outcome != null)
            {
                ApplyOutcome(outcome);
            }

            // Check for follow-up event
            if (!string.IsNullOrEmpty(outcome?.FollowUpEventId))
            {
                var followUp = GetEvent(outcome.FollowUpEventId);
                if (followUp != null)
                {
                    QueueEvent(followUp);
                }
            }

            currentEvent = null;
            return outcome;
        }

        private EventOutcome SelectOutcome(List<EventOutcome> outcomes)
        {
            if (outcomes == null || outcomes.Count == 0)
                return null;

            if (outcomes.Count == 1)
                return outcomes[0];

            // Select based on chance
            float totalChance = outcomes.Sum(o => o.Chance);
            float random = Random.value * totalChance;
            float current = 0;

            foreach (var outcome in outcomes)
            {
                current += outcome.Chance;
                if (random <= current)
                    return outcome;
            }

            return outcomes[outcomes.Count - 1];
        }

        /// <summary>
        /// Apply the effects of an outcome.
        /// </summary>
        public void ApplyOutcome(EventOutcome outcome)
        {
            var character = CharacterManager.Instance.CurrentCharacter;
            var stats = CharacterManager.Instance.Stats;

            // Apply stat changes
            if (outcome.StatChanges != null)
            {
                if (outcome.StatChanges.Health != 0)
                    stats.ModifyHealth(outcome.StatChanges.Health);
                if (outcome.StatChanges.Happiness != 0)
                    stats.ModifyHappiness(outcome.StatChanges.Happiness);
                if (outcome.StatChanges.Intelligence != 0)
                    stats.ModifyIntelligence(outcome.StatChanges.Intelligence);
                if (outcome.StatChanges.Looks != 0)
                    stats.ModifyLooks(outcome.StatChanges.Looks);
                if (outcome.StatChanges.Fame != 0)
                    stats.ModifyFame(outcome.StatChanges.Fame);
                if (outcome.StatChanges.Karma != 0)
                    stats.ModifyKarma(outcome.StatChanges.Karma);
            }

            // Apply money change
            if (outcome.MoneyChange != 0)
            {
                stats.ModifyMoney(outcome.MoneyChange, outcome.Description);
            }

            // Apply special effects
            if (outcome.Effects != null)
            {
                ApplyEffects(outcome.Effects);
            }

            // Add to life history
            if (!string.IsNullOrEmpty(outcome.Description))
            {
                CharacterManager.Instance.AddLifeEvent(outcome.Description, currentEvent?.Category.ToString() ?? "Genel");
            }

            // Unlock achievement
            if (!string.IsNullOrEmpty(outcome.AchievementId))
            {
                if (!character.Achievements.Contains(outcome.AchievementId))
                {
                    character.Achievements.Add(outcome.AchievementId);
                }
            }

            Debug.Log($"[EventManager] Applied outcome: {outcome.Description}");
        }

        private void ApplyEffects(OutcomeEffects effects)
        {
            var character = CharacterManager.Instance.CurrentCharacter;

            if (effects.StartSchool)
            {
                character.CurrentSchool = effects.SchoolName;
            }

            if (effects.GraduateSchool)
            {
                character.HasDiploma = true;
                character.CurrentSchool = null;
            }

            if (effects.GetJob)
            {
                character.IsEmployed = true;
                character.CurrentJob = effects.JobName;
                character.YearsAtJob = 0;
            }

            if (effects.LoseJob)
            {
                character.IsEmployed = false;
                character.CurrentJob = null;
            }

            if (effects.GetMarried)
            {
                character.IsMarried = true;
            }

            if (effects.GetDivorced)
            {
                character.IsMarried = false;
                character.SpouseId = null;
            }

            if (effects.HaveChild)
            {
                character.ChildrenCount++;
            }

            if (effects.GoToJail)
            {
                character.HasCriminalRecord = true;
                character.JailTime += effects.JailYears;
            }

            if (effects.Die)
            {
                CharacterManager.Instance.Die(effects.DeathCause);
            }
        }

        #endregion

        #region Event History

        /// <summary>
        /// Mark an event as triggered (for one-time events).
        /// </summary>
        public void MarkEventTriggered(string eventId)
        {
            triggeredOneTimeEvents.Add(eventId);
        }

        /// <summary>
        /// Check if an event has been triggered.
        /// </summary>
        public bool HasEventBeenTriggered(string eventId)
        {
            return triggeredOneTimeEvents.Contains(eventId);
        }

        /// <summary>
        /// Reset triggered events (for new game).
        /// </summary>
        public void ResetTriggeredEvents()
        {
            triggeredOneTimeEvents.Clear();
        }

        #endregion
    }
}
