using UnityEngine;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Yaş ve yaşam döngüsü sistemi
    /// </summary>
    public class AgeSystem : MonoBehaviour
    {
        public void ProcessAgeChange(CharacterData character)
        {
            AgePeriod currentPeriod = character.GetAgePeriod();

            // Yaşam dönemine göre özel işlemler
            switch (currentPeriod)
            {
                case AgePeriod.Infancy:
                    ProcessInfancy(character);
                    break;
                case AgePeriod.Childhood:
                    ProcessChildhood(character);
                    break;
                case AgePeriod.Adolescence:
                    ProcessAdolescence(character);
                    break;
                case AgePeriod.YoungAdult:
                    ProcessYoungAdult(character);
                    break;
                case AgePeriod.Adult:
                    ProcessAdult(character);
                    break;
                case AgePeriod.MiddleAge:
                    ProcessMiddleAge(character);
                    break;
                case AgePeriod.Elder:
                    ProcessElder(character);
                    break;
            }
        }

        private void ProcessInfancy(CharacterData character)
        {
            // Bebeklik: Temel gelişim
            if (Random.value < 0.3f)
            {
                character.Stats.ModifyStat(StatType.Intelligence, Random.Range(1f, 5f));
            }
        }

        private void ProcessChildhood(CharacterData character)
        {
            // Çocukluk: Okul ve sosyal gelişim
            if (character.Education.IsEnrolled)
            {
                character.Stats.ModifyStat(StatType.Intelligence, Random.Range(2f, 5f));
            }

            // Rastgele görünüm değişimi
            if (Random.value < 0.2f)
            {
                character.Stats.ModifyStat(StatType.Appearance, Random.Range(-5f, 5f));
            }
        }

        private void ProcessAdolescence(CharacterData character)
        {
            // Ergenlik: Hormonal değişimler
            character.Stats.ModifyStat(StatType.Happiness, Random.Range(-5f, 5f));

            // Görünüm değişimi (sivilce vs.)
            if (Random.value < 0.3f)
            {
                character.Stats.ModifyStat(StatType.Appearance, Random.Range(-10f, 10f));
            }

            // Zeka gelişimi
            if (character.Education.IsEnrolled)
            {
                character.Stats.ModifyStat(StatType.Intelligence, Random.Range(1f, 4f));
            }
        }

        private void ProcessYoungAdult(CharacterData character)
        {
            // Genç yetişkinlik: Kariyer ve ilişkiler

            // Görünüm en iyi dönemde
            if (character.Age < 25 && Random.value < 0.2f)
            {
                character.Stats.ModifyStat(StatType.Appearance, Random.Range(0f, 3f));
            }

            // Eğer işsizse stres
            if (!character.Career.IsEmployed && !character.Education.IsEnrolled && character.Age > 22)
            {
                character.Stats.ModifyStat(StatType.Happiness, -3f);
            }
        }

        private void ProcessAdult(CharacterData character)
        {
            // Yetişkinlik: Kariyer zirvesi, aile sorumlulukları

            // Kariyer bonusu
            if (character.Career.IsEmployed)
            {
                character.Career.JobPerformance += Random.Range(-2f, 3f);
                character.Career.JobPerformance = Mathf.Clamp(character.Career.JobPerformance, 0f, 100f);
            }

            // Görünüm yavaşça azalır
            character.Stats.ModifyStat(StatType.Appearance, -0.5f);
        }

        private void ProcessMiddleAge(CharacterData character)
        {
            // Orta yaş: Sağlık sorunları başlayabilir

            // Sağlık riski artışı
            if (Random.value < 0.1f)
            {
                character.Stats.ModifyStat(StatType.Health, Random.Range(-5f, -1f));
            }

            // Görünüm azalır
            character.Stats.ModifyStat(StatType.Appearance, -1f);

            // Deneyim ve bilgelik
            character.Stats.ModifyStat(StatType.Intelligence, Random.Range(0f, 1f));
        }

        private void ProcessElder(CharacterData character)
        {
            // Yaşlılık: Sağlık kritik, emeklilik

            // Sağlık daha hızlı azalır
            character.Stats.ModifyStat(StatType.Health, Random.Range(-3f, -1f));

            // Görünüm azalır
            character.Stats.ModifyStat(StatType.Appearance, -1.5f);

            // Eğer emekliyse ve hobisi varsa mutluluk
            if (character.Career.IsRetired)
            {
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(0f, 3f));
            }
        }

        public bool CheckNaturalDeath(CharacterData character)
        {
            // Doğal ölüm şansı hesaplama
            float baseChance = Constants.NATURAL_DEATH_BASE_CHANCE;

            // Yaşa göre artış
            if (character.Age > 60)
            {
                baseChance += (character.Age - 60) * Constants.NATURAL_DEATH_AGE_FACTOR;
            }

            // Sağlık durumu
            float healthFactor = 1f - (character.Stats.Health / 100f);
            baseChance *= (1f + healthFactor);

            // Hastalıklar
            foreach (var disease in character.Health.CurrentDiseases)
            {
                if (disease.IsChronic)
                {
                    baseChance += disease.Severity * 0.001f;
                }
            }

            // Sağlıksız alışkanlıklar
            if (character.Health.IsSmoker)
                baseChance *= 1.5f;
            if (character.Health.UsesDrugs)
                baseChance *= 2f;

            // Fitness bonus
            float fitnessBonus = character.Health.Fitness / 200f; // Max 0.5 azalma
            baseChance *= (1f - fitnessBonus);

            return Random.value < baseChance;
        }

        public string GetAgePeriodName(AgePeriod period)
        {
            return period switch
            {
                AgePeriod.Infancy => "Bebeklik",
                AgePeriod.Childhood => "Çocukluk",
                AgePeriod.Adolescence => "Ergenlik",
                AgePeriod.YoungAdult => "Genç Yetişkinlik",
                AgePeriod.Adult => "Yetişkinlik",
                AgePeriod.MiddleAge => "Orta Yaş",
                AgePeriod.Elder => "Yaşlılık",
                _ => "Bilinmiyor"
            };
        }

        public string GetAgePeriodDescription(AgePeriod period)
        {
            return period switch
            {
                AgePeriod.Infancy => "Hayatın ilk yılları. Her şey yeni ve heyecan verici.",
                AgePeriod.Childhood => "Okul, arkadaşlar ve keşifler dönemi.",
                AgePeriod.Adolescence => "Değişim ve kimlik arayışı zamanı.",
                AgePeriod.YoungAdult => "Kariyer, aşk ve bağımsızlık.",
                AgePeriod.Adult => "Sorumluluklar ve başarılar dönemi.",
                AgePeriod.MiddleAge => "Deneyim ve bilgelik zamanı.",
                AgePeriod.Elder => "Huzur ve anılar dönemi.",
                _ => ""
            };
        }
    }
}
