using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Oyun olaylarını yöneten sistem
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        private List<GameEvent> allEvents;
        private Queue<GameEvent> eventQueue;

        private void Awake()
        {
            allEvents = new List<GameEvent>();
            eventQueue = new Queue<GameEvent>();
            LoadEvents();
        }

        private void LoadEvents()
        {
            // Varsayılan olayları yükle
            allEvents.AddRange(CreateDefaultEvents());

            // JSON'dan ek olaylar yüklenebilir
            // TextAsset eventFile = Resources.Load<TextAsset>("Events/events");
            // if (eventFile != null) { ... }

            Debug.Log($"Toplam {allEvents.Count} olay yüklendi");
        }

        public void ProcessYearlyEvents(CharacterData character)
        {
            // Yaşa uygun olayları filtrele
            var eligibleEvents = allEvents.Where(e => IsEventEligible(e, character)).ToList();

            // Rastgele 1-3 olay seç
            int eventCount = Random.Range(1, 4);
            var selectedEvents = eligibleEvents
                .OrderBy(x => Random.value)
                .Take(eventCount)
                .ToList();

            foreach (var evt in selectedEvents)
            {
                eventQueue.Enqueue(evt);
            }

            // Özel yaş olayları
            ProcessAgeSpecificEvents(character);

            // Kuyruktaki ilk olayı işle
            ProcessNextEvent(character);
        }

        private bool IsEventEligible(GameEvent evt, CharacterData character)
        {
            // Yaş kontrolü
            if (character.Age < evt.MinAge || character.Age > evt.MaxAge)
                return false;

            // Cinsiyet kontrolü
            if (evt.RequiredGender != null && evt.RequiredGender != character.Gender)
                return false;

            // Eğitim kontrolü
            if (evt.RequiredEducation != null &&
                character.Education.HighestLevel < evt.RequiredEducation)
                return false;

            // İş kontrolü
            if (evt.RequiresJob && !character.Career.IsEmployed)
                return false;

            // Olasılık kontrolü
            if (Random.value > evt.Probability)
                return false;

            return true;
        }

        private void ProcessAgeSpecificEvents(CharacterData character)
        {
            // Okul başlangıcı
            if (character.Age == Constants.SCHOOL_START_AGE)
            {
                eventQueue.Enqueue(CreateSchoolStartEvent());
            }

            // Ortaokul
            if (character.Age == Constants.MIDDLE_SCHOOL_AGE)
            {
                eventQueue.Enqueue(CreateMiddleSchoolEvent());
            }

            // Lise
            if (character.Age == Constants.HIGH_SCHOOL_AGE)
            {
                eventQueue.Enqueue(CreateHighSchoolEvent());
            }

            // YKS sınavı
            if (character.Age == 18 && character.Education.CurrentLevel == EducationLevel.HighSchool)
            {
                eventQueue.Enqueue(CreateYKSEvent());
            }

            // Askerlik (erkekler için)
            if (character.Age == Constants.MILITARY_AGE &&
                character.Gender == Gender.Male &&
                !character.Military.HasServed)
            {
                eventQueue.Enqueue(CreateMilitaryEvent());
            }

            // Emeklilik
            if (character.Age == Constants.RETIREMENT_AGE && character.Career.IsEmployed)
            {
                eventQueue.Enqueue(CreateRetirementEvent());
            }
        }

        public void ProcessNextEvent(CharacterData character)
        {
            if (eventQueue.Count == 0)
                return;

            GameEvent evt = eventQueue.Dequeue();
            EventBus.Publish(new GameEventTriggeredEvent(evt));

            // UI'da olayı göster
            GameManager.Instance.UIManager.ShowEventPopup(evt, (choiceIndex) =>
            {
                ProcessEventChoice(character, evt, choiceIndex);

                // Sonraki olay
                if (eventQueue.Count > 0)
                {
                    ProcessNextEvent(character);
                }
            });
        }

        private void ProcessEventChoice(CharacterData character, GameEvent evt, int choiceIndex)
        {
            if (choiceIndex < 0 || choiceIndex >= evt.Choices.Count)
                return;

            EventChoice choice = evt.Choices[choiceIndex];

            // Sonuçları uygula
            foreach (var outcome in choice.Outcomes)
            {
                ApplyOutcome(character, outcome);
            }

            // Hayat olayına ekle
            string eventText = $"{evt.Title}: {choice.Text}";
            character.AddLifeEvent(eventText);

            EventBus.Publish(new DecisionMadeEvent(evt.Id, choiceIndex));
        }

        private void ApplyOutcome(CharacterData character, EventOutcome outcome)
        {
            // Stat değişimleri
            if (outcome.StatChanges != null)
            {
                foreach (var change in outcome.StatChanges)
                {
                    GameManager.Instance.StatSystem.ModifyStat(character, change.Key, change.Value);
                }
            }

            // Özel sonuçlar
            if (!string.IsNullOrEmpty(outcome.SpecialEffect))
            {
                ProcessSpecialEffect(character, outcome.SpecialEffect);
            }
        }

        private void ProcessSpecialEffect(CharacterData character, string effect)
        {
            switch (effect)
            {
                case "start_school":
                    character.Education.IsEnrolled = true;
                    character.Education.CurrentLevel = EducationLevel.PrimarySchool;
                    break;

                case "graduate":
                    character.Education.IsEnrolled = false;
                    character.Education.HighestLevel = character.Education.CurrentLevel;
                    break;

                case "get_job":
                    character.Career.IsEmployed = true;
                    break;

                case "lose_job":
                    character.Career.IsEmployed = false;
                    break;

                case "get_married":
                    // İlişki sisteminde işlenir
                    break;

                case "have_child":
                    // İlişki sisteminde işlenir
                    break;

                case "go_to_prison":
                    character.Legal.IsInPrison = true;
                    break;

                case "military_service":
                    character.Military.IsServing = true;
                    character.Military.ServiceStartYear = character.Age + character.BirthYear;
                    break;
            }
        }

        #region Event Creation Methods
        private List<GameEvent> CreateDefaultEvents()
        {
            return new List<GameEvent>
            {
                // Çocukluk olayları
                new GameEvent
                {
                    Id = "childhood_friend",
                    Title = "Yeni Arkadaş",
                    Description = "Okulda yeni bir arkadaş edindin!",
                    Category = EventCategory.Social,
                    MinAge = 5,
                    MaxAge = 12,
                    Probability = 0.3f,
                    Choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            Text = "Onunla arkadaş ol",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Happiness, 5 }
                                    }
                                }
                            }
                        },
                        new EventChoice
                        {
                            Text = "Uzak dur",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Happiness, -2 }
                                    }
                                }
                            }
                        }
                    }
                },

                // Ergenlik olayları
                new GameEvent
                {
                    Id = "teen_rebellion",
                    Title = "Ergenlik Krizi",
                    Description = "Ailene karşı isyan etmek istiyorsun.",
                    Category = EventCategory.Social,
                    MinAge = 13,
                    MaxAge = 17,
                    Probability = 0.2f,
                    Choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            Text = "İsyan et",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Happiness, 3 },
                                        { StatType.Karma, -5 }
                                    }
                                }
                            }
                        },
                        new EventChoice
                        {
                            Text = "Sakin kal",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Karma, 3 }
                                    }
                                }
                            }
                        }
                    }
                },

                // Yetişkin olayları
                new GameEvent
                {
                    Id = "job_offer",
                    Title = "İş Teklifi",
                    Description = "Yeni bir iş teklifi aldın!",
                    Category = EventCategory.Career,
                    MinAge = 18,
                    MaxAge = 65,
                    Probability = 0.15f,
                    Choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            Text = "Kabul et",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Happiness, 10 },
                                        { StatType.Money, 1000 }
                                    },
                                    SpecialEffect = "get_job"
                                }
                            }
                        },
                        new EventChoice
                        {
                            Text = "Reddet",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Happiness, -2 }
                                    }
                                }
                            }
                        }
                    }
                },

                // Sağlık olayları
                new GameEvent
                {
                    Id = "minor_illness",
                    Title = "Hastalık",
                    Description = "Grip oldun ve kendinizi kötü hissediyorsun.",
                    Category = EventCategory.Health,
                    MinAge = 0,
                    MaxAge = 120,
                    Probability = 0.2f,
                    Choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            Text = "Doktora git",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Health, 5 },
                                        { StatType.Money, -100 }
                                    }
                                }
                            }
                        },
                        new EventChoice
                        {
                            Text = "Evde dinlen",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Health, -5 }
                                    }
                                }
                            }
                        }
                    }
                },

                // Türkiye'ye özgü olaylar
                new GameEvent
                {
                    Id = "bayram_visit",
                    Title = "Bayram Ziyareti",
                    Description = "Bayram geldi! Akrabaları ziyaret edecek misin?",
                    Category = EventCategory.Holiday,
                    MinAge = 5,
                    MaxAge = 120,
                    Probability = 0.4f,
                    Choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            Text = "Akrabaları ziyaret et",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Happiness, 8 },
                                        { StatType.Karma, 5 },
                                        { StatType.Money, 50 } // Harçlık
                                    }
                                }
                            }
                        },
                        new EventChoice
                        {
                            Text = "Evde kal",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Happiness, -3 },
                                        { StatType.Karma, -2 }
                                    }
                                }
                            }
                        }
                    }
                },

                // Komşu ilişkileri
                new GameEvent
                {
                    Id = "neighbor_conflict",
                    Title = "Komşu Sorunu",
                    Description = "Komşun çok gürültü yapıyor.",
                    Category = EventCategory.Social,
                    MinAge = 18,
                    MaxAge = 120,
                    Probability = 0.15f,
                    Choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            Text = "Kibarca uyar",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Karma, 3 }
                                    }
                                }
                            }
                        },
                        new EventChoice
                        {
                            Text = "Kapıcıya şikayet et",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Happiness, 2 },
                                        { StatType.Karma, -2 }
                                    }
                                }
                            }
                        },
                        new EventChoice
                        {
                            Text = "Sen de gürültü yap",
                            Outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    StatChanges = new Dictionary<StatType, float>
                                    {
                                        { StatType.Happiness, -5 },
                                        { StatType.Karma, -5 }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private GameEvent CreateSchoolStartEvent()
        {
            return new GameEvent
            {
                Id = "school_start",
                Title = "İlkokula Başlama",
                Description = "İlkokula başlama zamanı geldi!",
                Category = EventCategory.Education,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "Heyecanla başla",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Happiness, 5 },
                                    { StatType.Intelligence, 3 }
                                },
                                SpecialEffect = "start_school"
                            }
                        }
                    }
                }
            };
        }

        private GameEvent CreateMiddleSchoolEvent()
        {
            return new GameEvent
            {
                Id = "middle_school",
                Title = "Ortaokul",
                Description = "Ortaokula geçtin!",
                Category = EventCategory.Education,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "Devam et",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Intelligence, 5 }
                                }
                            }
                        }
                    }
                }
            };
        }

        private GameEvent CreateHighSchoolEvent()
        {
            return new GameEvent
            {
                Id = "high_school",
                Title = "Lise",
                Description = "Liseye geçtin! Hangi bölümü seçeceksin?",
                Category = EventCategory.Education,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "Sayısal",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Intelligence, 5 }
                                }
                            }
                        }
                    },
                    new EventChoice
                    {
                        Text = "Sözel",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Intelligence, 3 },
                                    { StatType.Happiness, 2 }
                                }
                            }
                        }
                    },
                    new EventChoice
                    {
                        Text = "Eşit Ağırlık",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Intelligence, 4 }
                                }
                            }
                        }
                    }
                }
            };
        }

        private GameEvent CreateYKSEvent()
        {
            return new GameEvent
            {
                Id = "yks_exam",
                Title = "YKS Sınavı",
                Description = "Üniversite sınavı zamanı! Nasıl hazırlandın?",
                Category = EventCategory.Education,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "Çok çalıştım",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Intelligence, 10 },
                                    { StatType.Happiness, -5 }
                                }
                            }
                        }
                    },
                    new EventChoice
                    {
                        Text = "Normal çalıştım",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Intelligence, 5 }
                                }
                            }
                        }
                    },
                    new EventChoice
                    {
                        Text = "Hiç çalışmadım",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Happiness, 3 },
                                    { StatType.Intelligence, -2 }
                                }
                            }
                        }
                    }
                }
            };
        }

        private GameEvent CreateMilitaryEvent()
        {
            return new GameEvent
            {
                Id = "military_service",
                Title = "Askerlik",
                Description = "Askerlik zamanı geldi! Ne yapacaksın?",
                Category = EventCategory.Military,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "Normal askerlik yap",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Health, 10 },
                                    { StatType.Happiness, -10 }
                                },
                                SpecialEffect = "military_service"
                            }
                        }
                    },
                    new EventChoice
                    {
                        Text = "Bedelli askerlik yap",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Money, -50000 },
                                    { StatType.Happiness, 5 }
                                }
                            }
                        }
                    },
                    new EventChoice
                    {
                        Text = "Ertele (eğer öğrenciysen)",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Happiness, 2 }
                                }
                            }
                        }
                    }
                }
            };
        }

        private GameEvent CreateRetirementEvent()
        {
            return new GameEvent
            {
                Id = "retirement",
                Title = "Emeklilik",
                Description = "Emeklilik yaşına geldin!",
                Category = EventCategory.Career,
                Choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        Text = "Emekli ol",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Happiness, 15 }
                                }
                            }
                        }
                    },
                    new EventChoice
                    {
                        Text = "Çalışmaya devam et",
                        Outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                StatChanges = new Dictionary<StatType, float>
                                {
                                    { StatType.Money, 5000 },
                                    { StatType.Health, -5 }
                                }
                            }
                        }
                    }
                }
            };
        }
        #endregion
    }

    /// <summary>
    /// Oyun olayı modeli
    /// </summary>
    [System.Serializable]
    public class GameEvent
    {
        public string Id;
        public string Title;
        public string Description;
        public EventCategory Category;
        public int MinAge;
        public int MaxAge = 120;
        public float Probability = 1f;
        public Gender? RequiredGender;
        public EducationLevel? RequiredEducation;
        public bool RequiresJob;
        public List<EventChoice> Choices;

        public GameEvent()
        {
            Choices = new List<EventChoice>();
        }
    }

    /// <summary>
    /// Olay seçeneği
    /// </summary>
    [System.Serializable]
    public class EventChoice
    {
        public string Text;
        public List<EventOutcome> Outcomes;

        public EventChoice()
        {
            Outcomes = new List<EventOutcome>();
        }
    }

    /// <summary>
    /// Olay sonucu
    /// </summary>
    [System.Serializable]
    public class EventOutcome
    {
        public Dictionary<StatType, float> StatChanges;
        public string SpecialEffect;
        public string Message;

        public EventOutcome()
        {
            StatChanges = new Dictionary<StatType, float>();
        }
    }
}
