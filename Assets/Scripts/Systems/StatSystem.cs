using UnityEngine;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Stat yönetim sistemi
    /// </summary>
    public class StatSystem : MonoBehaviour
    {
        public void ModifyStat(CharacterData character, StatType statType, float delta)
        {
            float oldValue = character.Stats.GetStat(statType);
            character.Stats.ModifyStat(statType, delta);
            float newValue = character.Stats.GetStat(statType);

            EventBus.Publish(new StatChangedEvent(statType, oldValue, newValue));
        }

        public void SetStat(CharacterData character, StatType statType, float value)
        {
            float oldValue = character.Stats.GetStat(statType);
            character.Stats.SetStat(statType, value);

            EventBus.Publish(new StatChangedEvent(statType, oldValue, value));
        }

        public void ProcessYearlyStatChanges(CharacterData character)
        {
            // Yaşa göre stat değişimleri
            AgePeriod agePeriod = character.GetAgePeriod();

            // Sağlık yaşla azalır
            float healthDecay = CalculateHealthDecay(character);
            ModifyStat(character, StatType.Health, -healthDecay);

            // Mutluluk rastgele değişir
            float happinessChange = Random.Range(-5f, 5f);
            ModifyStat(character, StatType.Happiness, happinessChange);

            // Görünüm yaşla azalır (30 yaşından sonra)
            if (character.Age > 30)
            {
                float appearanceDecay = (character.Age - 30) * 0.1f;
                ModifyStat(character, StatType.Appearance, -appearanceDecay);
            }

            // Karma aktivitelere göre değişir (başka yerlerde işlenir)

            // Eğitim/iş durumuna göre zeka
            if (character.Education.IsEnrolled || character.Career.IsEmployed)
            {
                float intelligenceGain = Random.Range(0f, 2f);
                ModifyStat(character, StatType.Intelligence, intelligenceGain);
            }
            else if (character.Age > 18)
            {
                // İşsiz ve okul dışındaysa zeka azalabilir
                float intelligenceDecay = Random.Range(0f, 1f);
                ModifyStat(character, StatType.Intelligence, -intelligenceDecay);
            }

            // Şöhret zamanla azalır (aktif tutulmazsa)
            if (character.Stats.Fame > 0)
            {
                float fameDecay = character.Stats.Fame * 0.05f;
                ModifyStat(character, StatType.Fame, -fameDecay);
            }
        }

        private float CalculateHealthDecay(CharacterData character)
        {
            float baseDecay = Constants.HEALTH_DECAY_RATE;

            // Yaşa göre artış
            if (character.Age > 50)
            {
                baseDecay += (character.Age - 50) * 0.1f;
            }

            // Sağlıksız alışkanlıklar
            if (character.Health.IsSmoker)
                baseDecay += 1f;
            if (character.Health.IsDrinker)
                baseDecay += 0.5f;
            if (character.Health.UsesDrugs)
                baseDecay += 2f;

            // Fitness bonus
            float fitnessBonus = (character.Health.Fitness / 100f) * 0.5f;
            baseDecay -= fitnessBonus;

            // Hastalıklar
            foreach (var disease in character.Health.CurrentDiseases)
            {
                baseDecay += disease.Severity * 0.1f;
            }

            return Mathf.Max(0, baseDecay);
        }

        public void ApplyActivity(CharacterData character, ActivityType activity)
        {
            switch (activity)
            {
                case ActivityType.Exercise:
                    ModifyStat(character, StatType.Health, Random.Range(1f, 5f));
                    ModifyStat(character, StatType.Happiness, Random.Range(1f, 3f));
                    character.Health.Fitness = Mathf.Min(100, character.Health.Fitness + 2);
                    break;

                case ActivityType.ReadBook:
                    ModifyStat(character, StatType.Intelligence, Random.Range(1f, 3f));
                    ModifyStat(character, StatType.Happiness, Random.Range(0f, 2f));
                    break;

                case ActivityType.Meditation:
                    ModifyStat(character, StatType.Happiness, Random.Range(2f, 5f));
                    ModifyStat(character, StatType.Health, Random.Range(0f, 2f));
                    break;

                case ActivityType.SocialMedia:
                    ModifyStat(character, StatType.Happiness, Random.Range(-2f, 3f));
                    ModifyStat(character, StatType.Fame, Random.Range(0f, 1f));
                    break;

                case ActivityType.Party:
                    ModifyStat(character, StatType.Happiness, Random.Range(3f, 8f));
                    ModifyStat(character, StatType.Health, Random.Range(-2f, 0f));
                    break;

                case ActivityType.Gambling:
                    float outcome = Random.Range(-1f, 1f);
                    if (outcome > 0)
                    {
                        ModifyStat(character, StatType.Happiness, 10f);
                        ModifyStat(character, StatType.Money, Random.Range(100f, 1000f));
                    }
                    else
                    {
                        ModifyStat(character, StatType.Happiness, -5f);
                        ModifyStat(character, StatType.Money, -Random.Range(100f, 500f));
                    }
                    ModifyStat(character, StatType.Karma, -2f);
                    break;

                case ActivityType.Volunteer:
                    ModifyStat(character, StatType.Happiness, Random.Range(3f, 6f));
                    ModifyStat(character, StatType.Karma, Random.Range(3f, 8f));
                    break;

                case ActivityType.Travel:
                    ModifyStat(character, StatType.Happiness, Random.Range(5f, 10f));
                    ModifyStat(character, StatType.Money, -Random.Range(500f, 5000f));
                    break;

                case ActivityType.Shopping:
                    float cost = Random.Range(50f, 500f);
                    ModifyStat(character, StatType.Happiness, Random.Range(2f, 5f));
                    ModifyStat(character, StatType.Money, -cost);
                    break;

                case ActivityType.Hobby:
                    ModifyStat(character, StatType.Happiness, Random.Range(2f, 6f));
                    ModifyStat(character, StatType.Intelligence, Random.Range(0f, 2f));
                    break;

                case ActivityType.WatchTV:
                    ModifyStat(character, StatType.Happiness, Random.Range(1f, 3f));
                    ModifyStat(character, StatType.Intelligence, Random.Range(-1f, 0f));
                    break;
            }
        }
    }
}
