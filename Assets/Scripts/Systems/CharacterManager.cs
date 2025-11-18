using UnityEngine;
using BitLifeTR.Core;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Manages the current character and their state.
    /// </summary>
    public class CharacterManager : Singleton<CharacterManager>
    {
        private CharacterData currentCharacter;
        private StatSystem statSystem;

        public CharacterData CurrentCharacter => currentCharacter;
        public StatSystem Stats => statSystem;
        public bool HasCharacter => currentCharacter != null;

        protected override void OnSingletonAwake()
        {
            Debug.Log("[CharacterManager] Initialized");
        }

        #region Character Creation

        /// <summary>
        /// Create a new random character.
        /// </summary>
        public CharacterData CreateNewCharacter()
        {
            currentCharacter = new CharacterData();

            // Random gender
            currentCharacter.Gender = RandomHelper.Chance(0.5f) ? Gender.Erkek : Gender.Kadin;

            // Random name based on gender
            currentCharacter.FirstName = currentCharacter.Gender == Gender.Erkek
                ? RandomHelper.RandomMaleName()
                : RandomHelper.RandomFemaleName();
            currentCharacter.LastName = RandomHelper.RandomSurname();

            // Random birth city
            currentCharacter.BirthCity = RandomHelper.RandomCity();

            // Initialize stat system and randomize stats
            statSystem = new StatSystem(currentCharacter);
            statSystem.RandomizeStats();

            // Create initial family relationships
            CreateInitialFamily();

            // Add birth event to history
            AddLifeEvent($"{currentCharacter.BirthCity}'da doğdun.", "Doğum");

            Debug.Log($"[CharacterManager] Created new character: {currentCharacter.FullName}");

            return currentCharacter;
        }

        /// <summary>
        /// Create a character with specific parameters.
        /// </summary>
        public CharacterData CreateCharacter(string firstName, string lastName, Gender gender, string city)
        {
            currentCharacter = new CharacterData
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                BirthCity = city
            };

            statSystem = new StatSystem(currentCharacter);
            statSystem.RandomizeStats();

            CreateInitialFamily();
            AddLifeEvent($"{city}'da doğdun.", "Doğum");

            Debug.Log($"[CharacterManager] Created character: {currentCharacter.FullName}");

            return currentCharacter;
        }

        /// <summary>
        /// Load an existing character.
        /// </summary>
        public void LoadCharacter(CharacterData character)
        {
            currentCharacter = character;
            statSystem = new StatSystem(currentCharacter);

            Debug.Log($"[CharacterManager] Loaded character: {currentCharacter.FullName}");
        }

        private void CreateInitialFamily()
        {
            // Create mother
            var mother = new RelationshipData
            {
                Name = RandomHelper.RandomFemaleName() + " " + currentCharacter.LastName,
                Type = RelationType.Anne,
                Gender = Gender.Kadin,
                Age = RandomHelper.Range(22, 40),
                RelationshipLevel = RandomHelper.Range(60, 100)
            };
            currentCharacter.Relationships.Add(mother);

            // Create father
            var father = new RelationshipData
            {
                Name = RandomHelper.RandomMaleName() + " " + currentCharacter.LastName,
                Type = RelationType.Baba,
                Gender = Gender.Erkek,
                Age = RandomHelper.Range(24, 45),
                RelationshipLevel = RandomHelper.Range(60, 100)
            };
            currentCharacter.Relationships.Add(father);

            // Maybe add siblings
            if (RandomHelper.Chance(0.4f))
            {
                int siblingCount = RandomHelper.Range(1, 3);
                for (int i = 0; i < siblingCount; i++)
                {
                    var siblingGender = RandomHelper.Chance(0.5f) ? Gender.Erkek : Gender.Kadin;
                    var sibling = new RelationshipData
                    {
                        Name = (siblingGender == Gender.Erkek
                            ? RandomHelper.RandomMaleName()
                            : RandomHelper.RandomFemaleName()) + " " + currentCharacter.LastName,
                        Type = RelationType.Kardes,
                        Gender = siblingGender,
                        Age = RandomHelper.Range(0, 10),
                        RelationshipLevel = RandomHelper.Range(50, 90)
                    };
                    currentCharacter.Relationships.Add(sibling);
                }
            }
        }

        #endregion

        #region Life Progression

        /// <summary>
        /// Age the character by one year.
        /// </summary>
        public void AgeUp()
        {
            if (currentCharacter == null || !currentCharacter.IsAlive)
                return;

            currentCharacter.Age++;
            currentCharacter.UpdateLifeStage();

            // Apply yearly stat changes
            statSystem.ApplyYearlyChanges();

            // Age all relationships
            foreach (var rel in currentCharacter.Relationships)
            {
                if (rel.IsAlive)
                {
                    rel.Age++;
                    rel.YearsKnown++;

                    // Check for relationship deaths (old age)
                    if (rel.Age > 85 && RandomHelper.Chance(0.1f * (rel.Age - 85)))
                    {
                        rel.IsAlive = false;
                        AddLifeEvent($"{rel.Name} vefat etti.", "Ölüm");
                        statSystem.ModifyHappiness(-15f, "Kayıp");
                    }
                }

                // Relationship decay
                if (rel.RelationshipLevel > 0)
                {
                    rel.RelationshipLevel = Mathf.Max(0,
                        rel.RelationshipLevel - (int)Constants.RELATIONSHIP_DECAY_RATE);
                }
            }

            // Check for death
            if (ShouldDie())
            {
                Die(GetDeathCause());
                return;
            }

            // Publish age event
            EventBus.Publish(new YearAdvancedEvent
            {
                NewAge = currentCharacter.Age,
                LifeStage = currentCharacter.LifeStage
            });

            Debug.Log($"[CharacterManager] {currentCharacter.FirstName} is now {currentCharacter.Age}");
        }

        private bool ShouldDie()
        {
            // Base death chance increases with age
            float deathChance = Constants.DEATH_CHANCE_BASE;

            if (currentCharacter.Age > 60)
            {
                deathChance += (currentCharacter.Age - 60) * Constants.DEATH_CHANCE_AGE_MULTIPLIER;
            }

            // Low health increases death chance
            if (currentCharacter.Health < 20)
            {
                deathChance *= 3f;
            }
            else if (currentCharacter.Health < 50)
            {
                deathChance *= 1.5f;
            }

            // Very old age
            if (currentCharacter.Age > 100)
            {
                deathChance = 0.3f;
            }

            if (currentCharacter.Age > 110)
            {
                deathChance = 0.5f;
            }

            return RandomHelper.Chance(deathChance);
        }

        private string GetDeathCause()
        {
            if (currentCharacter.Health <= 0)
                return "Hastalık";

            if (currentCharacter.Age > 90)
            {
                string[] causes = { "Yaşlılık", "Doğal nedenler", "Uykusunda", "Kalp yetmezliği" };
                return causes[Random.Range(0, causes.Length)];
            }

            if (currentCharacter.Age > 60)
            {
                string[] causes = { "Kalp krizi", "Kanser", "Felç", "Organ yetmezliği" };
                return causes[Random.Range(0, causes.Length)];
            }

            string[] youngCauses = { "Kaza", "Hastalık", "Beklenmedik komplikasyon" };
            return youngCauses[Random.Range(0, youngCauses.Length)];
        }

        /// <summary>
        /// Kill the character.
        /// </summary>
        public void Die(string cause)
        {
            if (currentCharacter == null)
                return;

            currentCharacter.IsAlive = false;
            currentCharacter.DeathCause = cause;
            currentCharacter.DeathAge = currentCharacter.Age;

            AddLifeEvent($"{cause} nedeniyle hayatını kaybettin.", "Ölüm");

            EventBus.Publish(new CharacterDiedEvent
            {
                Age = currentCharacter.Age,
                CauseOfDeath = cause
            });

            Debug.Log($"[CharacterManager] {currentCharacter.FirstName} died at age {currentCharacter.Age}: {cause}");
        }

        #endregion

        #region Life Events

        /// <summary>
        /// Add an event to life history.
        /// </summary>
        public void AddLifeEvent(string description, string category = "Genel")
        {
            if (currentCharacter == null)
                return;

            var record = new LifeEventRecord(
                currentCharacter.Age,
                currentCharacter.CurrentYear,
                description,
                category
            );

            currentCharacter.LifeHistory.Add(record);
        }

        #endregion

        #region Relationships

        /// <summary>
        /// Get relationship by type.
        /// </summary>
        public RelationshipData GetRelationship(RelationType type)
        {
            return currentCharacter?.Relationships.Find(r => r.Type == type);
        }

        /// <summary>
        /// Get all relationships of a type.
        /// </summary>
        public System.Collections.Generic.List<RelationshipData> GetRelationships(RelationType type)
        {
            return currentCharacter?.Relationships.FindAll(r => r.Type == type)
                ?? new System.Collections.Generic.List<RelationshipData>();
        }

        /// <summary>
        /// Modify a relationship level.
        /// </summary>
        public void ModifyRelationship(string personId, int delta)
        {
            var rel = currentCharacter?.Relationships.Find(r => r.Id == personId);
            if (rel != null)
            {
                int oldValue = rel.RelationshipLevel;
                rel.RelationshipLevel = Mathf.Clamp(rel.RelationshipLevel + delta, 0, 100);

                EventBus.Publish(new RelationshipChangedEvent
                {
                    PersonId = personId,
                    PersonName = rel.Name,
                    OldValue = oldValue,
                    NewValue = rel.RelationshipLevel
                });
            }
        }

        /// <summary>
        /// Add a new relationship.
        /// </summary>
        public RelationshipData AddRelationship(string name, RelationType type, Gender gender, int age)
        {
            var rel = new RelationshipData
            {
                Name = name,
                Type = type,
                Gender = gender,
                Age = age,
                RelationshipLevel = 50
            };

            currentCharacter?.Relationships.Add(rel);
            return rel;
        }

        #endregion

        #region Save/Load

        /// <summary>
        /// Save current character.
        /// </summary>
        public bool SaveCurrentCharacter(string slot = "autosave")
        {
            if (currentCharacter == null)
                return false;

            return SaveManager.Instance.SaveGame(currentCharacter, slot);
        }

        /// <summary>
        /// Load character from save.
        /// </summary>
        public bool LoadSavedCharacter(string slot = "autosave")
        {
            var character = SaveManager.Instance.LoadGame(slot);

            if (character != null)
            {
                LoadCharacter(character);
                return true;
            }

            return false;
        }

        #endregion
    }
}
