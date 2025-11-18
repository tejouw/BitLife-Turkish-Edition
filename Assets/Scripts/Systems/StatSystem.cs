using UnityEngine;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Manages character stats and their modifications.
    /// </summary>
    public class StatSystem
    {
        private CharacterData character;

        public StatSystem(CharacterData characterData)
        {
            character = characterData;
        }

        #region Stat Getters

        public float Health => character.Health;
        public float Happiness => character.Happiness;
        public float Intelligence => character.Intelligence;
        public float Looks => character.Looks;
        public decimal Money => character.Money;
        public float Fame => character.Fame;
        public float Karma => character.Karma;

        #endregion

        #region Stat Modifiers

        /// <summary>
        /// Modify health stat.
        /// </summary>
        public void ModifyHealth(float delta, string reason = "")
        {
            float oldValue = character.Health;
            character.Health = Mathf.Clamp(character.Health + delta, Constants.STAT_MIN, Constants.STAT_MAX);

            PublishStatChange("Sağlık", oldValue, character.Health, delta);

            if (character.Health <= 0 && character.IsAlive)
            {
                // Character dies from health
                character.IsAlive = false;
                character.DeathCause = string.IsNullOrEmpty(reason) ? "Sağlık sorunları" : reason;
                character.DeathAge = character.Age;
            }
        }

        /// <summary>
        /// Modify happiness stat.
        /// </summary>
        public void ModifyHappiness(float delta, string reason = "")
        {
            float oldValue = character.Happiness;
            character.Happiness = Mathf.Clamp(character.Happiness + delta, Constants.STAT_MIN, Constants.STAT_MAX);

            PublishStatChange("Mutluluk", oldValue, character.Happiness, delta);
        }

        /// <summary>
        /// Modify intelligence stat.
        /// </summary>
        public void ModifyIntelligence(float delta, string reason = "")
        {
            float oldValue = character.Intelligence;
            character.Intelligence = Mathf.Clamp(character.Intelligence + delta, Constants.STAT_MIN, Constants.STAT_MAX);

            PublishStatChange("Zeka", oldValue, character.Intelligence, delta);
        }

        /// <summary>
        /// Modify looks stat.
        /// </summary>
        public void ModifyLooks(float delta, string reason = "")
        {
            float oldValue = character.Looks;
            character.Looks = Mathf.Clamp(character.Looks + delta, Constants.STAT_MIN, Constants.STAT_MAX);

            PublishStatChange("Görünüm", oldValue, character.Looks, delta);
        }

        /// <summary>
        /// Modify money.
        /// </summary>
        public void ModifyMoney(decimal delta, string reason = "")
        {
            decimal oldValue = character.Money;
            character.Money += delta;

            // Also update bank balance
            character.BankBalance += delta;

            EventBus.Publish(new MoneyChangedEvent
            {
                OldAmount = oldValue,
                NewAmount = character.Money,
                Delta = delta,
                Reason = reason
            });
        }

        /// <summary>
        /// Modify fame stat.
        /// </summary>
        public void ModifyFame(float delta, string reason = "")
        {
            float oldValue = character.Fame;
            character.Fame = Mathf.Clamp(character.Fame + delta, Constants.STAT_MIN, Constants.STAT_MAX);

            PublishStatChange("Şöhret", oldValue, character.Fame, delta);
        }

        /// <summary>
        /// Modify karma stat.
        /// </summary>
        public void ModifyKarma(float delta, string reason = "")
        {
            float oldValue = character.Karma;
            character.Karma = Mathf.Clamp(character.Karma + delta, Constants.STAT_MIN, Constants.STAT_MAX);

            PublishStatChange("Karma", oldValue, character.Karma, delta);
        }

        /// <summary>
        /// Apply multiple stat changes at once.
        /// </summary>
        public void ApplyStatModifier(StatModifier modifier)
        {
            if (modifier.Health != 0) ModifyHealth(modifier.Health, modifier.Reason);
            if (modifier.Happiness != 0) ModifyHappiness(modifier.Happiness, modifier.Reason);
            if (modifier.Intelligence != 0) ModifyIntelligence(modifier.Intelligence, modifier.Reason);
            if (modifier.Looks != 0) ModifyLooks(modifier.Looks, modifier.Reason);
            if (modifier.Money != 0) ModifyMoney(modifier.Money, modifier.Reason);
            if (modifier.Fame != 0) ModifyFame(modifier.Fame, modifier.Reason);
            if (modifier.Karma != 0) ModifyKarma(modifier.Karma, modifier.Reason);
        }

        #endregion

        #region Stat Setters (for initialization)

        /// <summary>
        /// Set all base stats at once (for character creation).
        /// </summary>
        public void SetBaseStats(float health, float happiness, float intelligence, float looks, float karma)
        {
            character.Health = Mathf.Clamp(health, Constants.STAT_MIN, Constants.STAT_MAX);
            character.Happiness = Mathf.Clamp(happiness, Constants.STAT_MIN, Constants.STAT_MAX);
            character.Intelligence = Mathf.Clamp(intelligence, Constants.STAT_MIN, Constants.STAT_MAX);
            character.Looks = Mathf.Clamp(looks, Constants.STAT_MIN, Constants.STAT_MAX);
            character.Karma = Mathf.Clamp(karma, Constants.STAT_MIN, Constants.STAT_MAX);
            character.Fame = 0;
            character.Money = Constants.STARTING_MONEY;
        }

        /// <summary>
        /// Randomize stats for a new character.
        /// </summary>
        public void RandomizeStats()
        {
            character.Health = Utils.RandomHelper.RandomStat();
            character.Happiness = Utils.RandomHelper.RandomStat();
            character.Intelligence = Utils.RandomHelper.RandomStat();
            character.Looks = Utils.RandomHelper.RandomStat();
            character.Karma = 50f; // Start neutral
            character.Fame = 0f;
            character.Money = Constants.STARTING_MONEY;
        }

        #endregion

        #region Yearly Updates

        /// <summary>
        /// Apply yearly stat decay/changes.
        /// </summary>
        public void ApplyYearlyChanges()
        {
            // Health decays faster with age
            float healthDecay = Constants.YEARLY_STAT_DECAY;
            if (character.Age > 50) healthDecay *= 1.5f;
            if (character.Age > 70) healthDecay *= 2f;

            ModifyHealth(-healthDecay, "Yaşlanma");

            // Happiness has small random fluctuation
            float happinessDelta = Random.Range(-3f, 3f);
            ModifyHappiness(happinessDelta);

            // Looks decay with age
            if (character.Age > 30)
            {
                float looksDecay = (character.Age - 30) * 0.1f;
                ModifyLooks(-looksDecay, "Yaşlanma");
            }

            // Fame decays if not maintained
            if (character.Fame > 0)
            {
                ModifyFame(-1f, "Unutulma");
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Get stat value by name.
        /// </summary>
        public float GetStat(string statName)
        {
            return statName.ToLower() switch
            {
                "health" or "sağlık" or "saglik" => character.Health,
                "happiness" or "mutluluk" => character.Happiness,
                "intelligence" or "zeka" => character.Intelligence,
                "looks" or "görünüm" or "gorunum" => character.Looks,
                "fame" or "şöhret" or "sohret" => character.Fame,
                "karma" => character.Karma,
                _ => 0f
            };
        }

        /// <summary>
        /// Check if character meets stat requirements.
        /// </summary>
        public bool MeetsRequirement(string statName, float minValue)
        {
            return GetStat(statName) >= minValue;
        }

        /// <summary>
        /// Get overall life satisfaction score.
        /// </summary>
        public float GetLifeSatisfaction()
        {
            return (character.Health + character.Happiness + character.Looks) / 3f;
        }

        private void PublishStatChange(string statName, float oldValue, float newValue, float delta)
        {
            EventBus.Publish(new StatChangedEvent
            {
                StatName = statName,
                OldValue = oldValue,
                NewValue = newValue,
                Delta = delta
            });
        }

        #endregion
    }

    /// <summary>
    /// Represents a set of stat modifications.
    /// </summary>
    [System.Serializable]
    public class StatModifier
    {
        public float Health;
        public float Happiness;
        public float Intelligence;
        public float Looks;
        public decimal Money;
        public float Fame;
        public float Karma;
        public string Reason;

        public StatModifier() { }

        public StatModifier(float health = 0, float happiness = 0, float intelligence = 0,
            float looks = 0, decimal money = 0, float fame = 0, float karma = 0, string reason = "")
        {
            Health = health;
            Happiness = happiness;
            Intelligence = intelligence;
            Looks = looks;
            Money = money;
            Fame = fame;
            Karma = karma;
            Reason = reason;
        }

        /// <summary>
        /// Create a simple stat modifier.
        /// </summary>
        public static StatModifier Simple(string stat, float value, string reason = "")
        {
            var modifier = new StatModifier { Reason = reason };

            switch (stat.ToLower())
            {
                case "health":
                case "sağlık":
                    modifier.Health = value;
                    break;
                case "happiness":
                case "mutluluk":
                    modifier.Happiness = value;
                    break;
                case "intelligence":
                case "zeka":
                    modifier.Intelligence = value;
                    break;
                case "looks":
                case "görünüm":
                    modifier.Looks = value;
                    break;
                case "fame":
                case "şöhret":
                    modifier.Fame = value;
                    break;
                case "karma":
                    modifier.Karma = value;
                    break;
            }

            return modifier;
        }
    }
}
