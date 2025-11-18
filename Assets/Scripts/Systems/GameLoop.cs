using UnityEngine;
using System.Collections.Generic;
using BitLifeTR.Core;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Controls the main game loop and year progression.
    /// </summary>
    public class GameLoop : Singleton<GameLoop>
    {
        private bool isProcessingYear = false;
        private List<GameEvent> currentYearEvents = new List<GameEvent>();
        private int currentEventIndex = 0;

        public bool IsProcessingYear => isProcessingYear;
        public List<GameEvent> CurrentYearEvents => currentYearEvents;

        protected override void OnSingletonAwake()
        {
            EventBus.Subscribe<GameStateChangedEvent>(OnGameStateChanged);
            Debug.Log("[GameLoop] Initialized");
        }

        protected override void OnDestroy()
        {
            EventBus.Unsubscribe<GameStateChangedEvent>(OnGameStateChanged);
            base.OnDestroy();
        }

        private void OnGameStateChanged(GameStateChangedEvent e)
        {
            if (e.NewState == GameState.Playing && e.PreviousState == GameState.CharacterCreation)
            {
                // New game started, generate first year events
                StartYear();
            }
        }

        #region Year Management

        /// <summary>
        /// Start a new year.
        /// </summary>
        public void StartYear()
        {
            if (!CharacterManager.Instance.HasCharacter)
                return;

            var character = CharacterManager.Instance.CurrentCharacter;
            if (!character.IsAlive)
                return;

            isProcessingYear = true;
            currentEventIndex = 0;

            // Generate events for this year
            currentYearEvents = GenerateYearEvents();

            // Add mandatory age-specific events
            AddAgeSpecificEvents(character);

            Debug.Log($"[GameLoop] Started year {character.CurrentYear} (Age {character.Age}), {currentYearEvents.Count} events");
        }

        /// <summary>
        /// Progress to the next year.
        /// </summary>
        public void AdvanceYear()
        {
            if (!CharacterManager.Instance.HasCharacter)
                return;

            var character = CharacterManager.Instance.CurrentCharacter;
            if (!character.IsAlive)
            {
                GameManager.Instance.EndGame(character.DeathCause, character.DeathAge);
                return;
            }

            // Age up the character
            CharacterManager.Instance.AgeUp();

            // Check if character died during age up
            if (!character.IsAlive)
            {
                GameManager.Instance.EndGame(character.DeathCause, character.DeathAge);
                return;
            }

            // Auto-save
            CharacterManager.Instance.SaveCurrentCharacter();

            // Start next year
            StartYear();
        }

        /// <summary>
        /// Get the current event to display.
        /// </summary>
        public GameEvent GetCurrentEvent()
        {
            if (currentEventIndex < currentYearEvents.Count)
            {
                return currentYearEvents[currentEventIndex];
            }
            return null;
        }

        /// <summary>
        /// Move to the next event.
        /// </summary>
        public void NextEvent()
        {
            currentEventIndex++;

            if (currentEventIndex >= currentYearEvents.Count)
            {
                EndYear();
            }
        }

        /// <summary>
        /// End the current year.
        /// </summary>
        private void EndYear()
        {
            isProcessingYear = false;
            currentYearEvents.Clear();
            currentEventIndex = 0;

            Debug.Log("[GameLoop] Year ended");
        }

        /// <summary>
        /// Check if there are more events this year.
        /// </summary>
        public bool HasMoreEvents()
        {
            return currentEventIndex < currentYearEvents.Count;
        }

        #endregion

        #region Event Generation

        private List<GameEvent> GenerateYearEvents()
        {
            var events = new List<GameEvent>();
            var character = CharacterManager.Instance.CurrentCharacter;

            // Number of events based on life stage
            int eventCount = GetEventCountForAge(character.Age);

            // Get random events
            var randomEvents = EventManager.Instance.GetYearlyEvents(eventCount);
            events.AddRange(randomEvents);

            return events;
        }

        private int GetEventCountForAge(int age)
        {
            // Babies have fewer events
            if (age < 5) return RandomHelper.Range(1, 2);

            // Children have moderate events
            if (age < 12) return RandomHelper.Range(2, 3);

            // Teens and adults have more events
            if (age < 65) return RandomHelper.Range(2, 4);

            // Elderly have fewer events
            return RandomHelper.Range(1, 3);
        }

        private void AddAgeSpecificEvents(CharacterData character)
        {
            var age = character.Age;

            // Starting school at age 6
            if (age == 6)
            {
                var schoolEvent = CreateSchoolStartEvent();
                currentYearEvents.Insert(0, schoolEvent);
            }

            // Middle school at age 10
            if (age == 10)
            {
                var middleSchoolEvent = CreateMiddleSchoolEvent();
                currentYearEvents.Insert(0, middleSchoolEvent);
            }

            // High school at age 14
            if (age == 14)
            {
                var highSchoolEvent = CreateHighSchoolEvent();
                currentYearEvents.Insert(0, highSchoolEvent);
            }

            // University exam at age 18
            if (age == 18 && character.EducationLevel >= EducationLevel.Lise)
            {
                var examEvent = CreateUniversityExamEvent();
                currentYearEvents.Insert(0, examEvent);
            }

            // Military service for males at age 20
            if (age == 20 && character.Gender == Gender.Erkek && !character.CompletedMilitaryService)
            {
                var militaryEvent = CreateMilitaryServiceEvent();
                currentYearEvents.Insert(0, militaryEvent);
            }

            // Retirement at appropriate age
            if ((age == Constants.EMEKLILIK_AGE_ERKEK && character.Gender == Gender.Erkek) ||
                (age == Constants.EMEKLILIK_AGE_KADIN && character.Gender == Gender.Kadin))
            {
                if (character.IsEmployed)
                {
                    var retirementEvent = CreateRetirementEvent();
                    currentYearEvents.Insert(0, retirementEvent);
                }
            }
        }

        #endregion

        #region Milestone Events

        private GameEvent CreateSchoolStartEvent()
        {
            return new GameEvent
            {
                Id = "school_start",
                Title = "Okula Başlama Zamanı",
                Description = "Bugün ilkokula başlıyorsun! Yeni arkadaşlar edinecek ve çok şey öğreneceksin.",
                Category = EventCategory.Egitim,
                IsMandatory = true,
                Choices = new List<EventChoice>
                {
                    EventChoice.Create("Heyecanlıyım!", EventOutcome.StatChange(
                        "İlkokula başladın ve yeni arkadaşlar edindin.",
                        happiness: 10, intelligence: 5))
                }
            };
        }

        private GameEvent CreateMiddleSchoolEvent()
        {
            return new GameEvent
            {
                Id = "middle_school",
                Title = "Ortaokula Geçiş",
                Description = "İlkokulu bitirdin ve artık ortaokula başlıyorsun. Dersler daha zorlaşacak.",
                Category = EventCategory.Egitim,
                IsMandatory = true,
                Choices = new List<EventChoice>
                {
                    EventChoice.Create("Hazırım!", EventOutcome.StatChange(
                        "Ortaokula başladın.",
                        intelligence: 3))
                }
            };
        }

        private GameEvent CreateHighSchoolEvent()
        {
            return new GameEvent
            {
                Id = "high_school",
                Title = "Liseye Geçiş",
                Description = "Ortaokulu bitirdin ve liseye başlıyorsun. Üniversite için hazırlanma zamanı!",
                Category = EventCategory.Egitim,
                IsMandatory = true,
                Choices = new List<EventChoice>
                {
                    EventChoice.Create("Devam et", EventOutcome.StatChange(
                        "Liseye başladın.",
                        intelligence: 5))
                }
            };
        }

        private GameEvent CreateUniversityExamEvent()
        {
            return new GameEvent
            {
                Id = "university_exam",
                Title = "Üniversite Sınavı (YKS)",
                Description = "Hayatının en önemli sınavlarından birine girme zamanı geldi. YKS'ye hazır mısın?",
                Category = EventCategory.Egitim,
                IsMandatory = true,
                Choices = new List<EventChoice>
                {
                    EventChoice.CreateWithChance("Sınava gir",
                        (EventOutcome.StatChange("Sınavda başarılı oldun ve üniversiteyi kazandın!", happiness: 20, intelligence: 10), 0.6f),
                        (EventOutcome.StatChange("Sınavda başarısız oldun. Belki gelecek yıl tekrar denersin.", happiness: -15), 0.4f)
                    ),
                    EventChoice.Create("Sınava girme", EventOutcome.StatChange(
                        "Üniversite sınavına girmedin.",
                        happiness: -5))
                }
            };
        }

        private GameEvent CreateMilitaryServiceEvent()
        {
            return new GameEvent
            {
                Id = "military_service",
                Title = "Askerlik Zamanı",
                Description = "Vatan borcu zamanı geldi. Askerlik hizmetini yapmak zorundasın.",
                Category = EventCategory.Ozel,
                IsMandatory = true,
                Choices = new List<EventChoice>
                {
                    EventChoice.Create("Askere git", new EventOutcome
                    {
                        Description = "Askerlik hizmetini tamamladın.",
                        StatChanges = new StatModifierData { Health = -5, Happiness = -10 },
                        Effects = new OutcomeEffects { }
                    }),
                    EventChoice.Create("Bedelli askerlik (paralı)", new EventOutcome
                    {
                        Description = "Bedelli askerlik yaptın.",
                        MoneyChange = -50000,
                        StatChanges = new StatModifierData { Happiness = 5 }
                    })
                }
            };
        }

        private GameEvent CreateRetirementEvent()
        {
            return new GameEvent
            {
                Id = "retirement",
                Title = "Emeklilik Zamanı",
                Description = $"Uzun yıllar çalıştıktan sonra emekli olma zamanı geldi. {CharacterManager.Instance.CurrentCharacter.CurrentJob} olarak çalıştığın yıllar sona eriyor.",
                Category = EventCategory.Kariyer,
                IsMandatory = true,
                Choices = new List<EventChoice>
                {
                    EventChoice.Create("Emekli ol", new EventOutcome
                    {
                        Description = "Emekli oldun. Artık dinlenme zamanı!",
                        StatChanges = new StatModifierData { Happiness = 15 },
                        Effects = new OutcomeEffects { LoseJob = true }
                    }),
                    EventChoice.Create("Çalışmaya devam et", EventOutcome.StatChange(
                        "Emekli olmayı reddedip çalışmaya devam ettin.",
                        health: -5))
                }
            };
        }

        #endregion

        #region Quick Actions

        /// <summary>
        /// Perform a quick action (activities menu).
        /// </summary>
        public void PerformActivity(string activityId)
        {
            var character = CharacterManager.Instance.CurrentCharacter;
            var stats = CharacterManager.Instance.Stats;

            switch (activityId)
            {
                case "gym":
                    stats.ModifyHealth(RandomHelper.Range(1f, 5f), "Spor salonu");
                    stats.ModifyHappiness(RandomHelper.Range(1f, 3f));
                    stats.ModifyLooks(RandomHelper.Range(0.5f, 2f));
                    CharacterManager.Instance.AddLifeEvent("Spor salonuna gittin.", "Aktivite");
                    break;

                case "library":
                    stats.ModifyIntelligence(RandomHelper.Range(1f, 5f), "Kütüphane");
                    stats.ModifyHappiness(RandomHelper.Range(-1f, 2f));
                    CharacterManager.Instance.AddLifeEvent("Kütüphaneye gittin.", "Aktivite");
                    break;

                case "meditation":
                    stats.ModifyHappiness(RandomHelper.Range(3f, 8f), "Meditasyon");
                    stats.ModifyHealth(RandomHelper.Range(1f, 3f));
                    CharacterManager.Instance.AddLifeEvent("Meditasyon yaptın.", "Aktivite");
                    break;

                case "doctor":
                    stats.ModifyHealth(RandomHelper.Range(5f, 15f), "Doktor");
                    stats.ModifyMoney(-500, "Doktor ücreti");
                    CharacterManager.Instance.AddLifeEvent("Doktora gittin.", "Sağlık");
                    break;

                case "plastic_surgery":
                    if (character.Money >= 10000)
                    {
                        stats.ModifyLooks(RandomHelper.Range(10f, 25f), "Estetik");
                        stats.ModifyMoney(-10000, "Estetik operasyon");
                        stats.ModifyHealth(-5, "Operasyon riski");
                        CharacterManager.Instance.AddLifeEvent("Estetik operasyon yaptırdın.", "Sağlık");
                    }
                    break;

                case "vacation":
                    if (character.Money >= 5000)
                    {
                        stats.ModifyHappiness(RandomHelper.Range(10f, 20f), "Tatil");
                        stats.ModifyMoney(-5000, "Tatil masrafları");
                        CharacterManager.Instance.AddLifeEvent("Tatile gittin.", "Eğlence");
                    }
                    break;

                case "crime":
                    // Risky activity
                    if (RandomHelper.Chance(0.3f))
                    {
                        stats.ModifyMoney(RandomHelper.Range(1000, 5000), "Hırsızlık");
                        stats.ModifyKarma(-10);
                        CharacterManager.Instance.AddLifeEvent("Hırsızlık yaptın ve yakalanmadın.", "Suç");
                    }
                    else
                    {
                        character.HasCriminalRecord = true;
                        stats.ModifyHappiness(-20);
                        CharacterManager.Instance.AddLifeEvent("Hırsızlık yaparken yakalandın!", "Suç");
                    }
                    break;
            }
        }

        #endregion
    }
}
