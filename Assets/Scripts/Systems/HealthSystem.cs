using UnityEngine;
using BitLifeTR.Core;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Manages health, diseases, and medical treatments.
    /// </summary>
    public static class HealthSystem
    {
        /// <summary>
        /// Check for random illness.
        /// </summary>
        public static (bool sick, string illness, float severity) CheckForIllness(CharacterData character)
        {
            // Base illness chance
            float sickChance = 0.05f;

            // Age factor
            if (character.Age < 10) sickChance += 0.05f;
            if (character.Age > 50) sickChance += 0.05f;
            if (character.Age > 70) sickChance += 0.1f;

            // Health factor
            sickChance += (100 - character.Health) * 0.001f;

            if (!RandomHelper.Chance(sickChance))
                return (false, null, 0);

            // Determine illness
            var (illness, severity) = GetRandomIllness(character.Age);

            return (true, illness, severity);
        }

        private static (string illness, float severity) GetRandomIllness(int age)
        {
            // Different illnesses for different ages
            if (age < 18)
            {
                string[] childIllnesses = { "Grip", "Soğuk Algınlığı", "Su Çiçeği", "Kızamık", "Bademcik İltihabı" };
                return (childIllnesses[Random.Range(0, childIllnesses.Length)], RandomHelper.Range(10f, 30f));
            }
            else if (age < 50)
            {
                string[] adultIllnesses = { "Grip", "Bronşit", "Mide Ülseri", "Migren", "Alerji", "Depresyon" };
                return (adultIllnesses[Random.Range(0, adultIllnesses.Length)], RandomHelper.Range(15f, 40f));
            }
            else
            {
                string[] elderIllnesses = { "Tansiyon", "Diyabet", "Kalp Hastalığı", "Artrit", "Katarakt", "Kanser" };
                string illness = elderIllnesses[Random.Range(0, elderIllnesses.Length)];
                float severity = illness == "Kanser" ? RandomHelper.Range(50f, 90f) : RandomHelper.Range(20f, 50f);
                return (illness, severity);
            }
        }

        /// <summary>
        /// Visit the doctor.
        /// </summary>
        public static (float healthGain, decimal cost, string message) VisitDoctor(CharacterData character)
        {
            decimal cost = 500;

            if (character.Money < cost)
                return (0, 0, "Doktor ücretini ödeyemiyorsun.");

            CharacterManager.Instance.Stats.ModifyMoney(-cost);

            float healthGain = RandomHelper.Range(5f, 15f);
            CharacterManager.Instance.Stats.ModifyHealth(healthGain);

            return (healthGain, cost, $"Doktor kontrolü tamamlandı. Sağlığın {healthGain:F0} arttı.");
        }

        /// <summary>
        /// Get hospital treatment.
        /// </summary>
        public static (float healthGain, decimal cost, string message) GetHospitalTreatment(CharacterData character, string illness, float severity)
        {
            decimal cost = severity * 100;

            if (character.Money < cost)
            {
                // Can't afford treatment
                CharacterManager.Instance.Stats.ModifyHealth(-severity * 0.5f);
                return (0, 0, "Tedavi masraflarını karşılayamıyorsun. Hastalık devam ediyor.");
            }

            CharacterManager.Instance.Stats.ModifyMoney(-cost);

            // Treatment success based on severity
            float successChance = 1f - (severity / 200f);

            if (RandomHelper.Chance(successChance))
            {
                float healthGain = severity * 0.8f;
                CharacterManager.Instance.Stats.ModifyHealth(healthGain);
                return (healthGain, cost, $"{illness} tedavisi başarılı! Sağlığın iyileşti.");
            }
            else
            {
                CharacterManager.Instance.Stats.ModifyHealth(-severity * 0.3f);
                return (0, cost, $"{illness} tedavisi tam başarılı olmadı. Hala biraz hasta hissediyorsun.");
            }
        }

        /// <summary>
        /// Go to therapy (mental health).
        /// </summary>
        public static (float happinessGain, decimal cost, string message) GoToTherapy(CharacterData character)
        {
            decimal cost = 800;

            if (character.Money < cost)
                return (0, 0, "Terapi ücretini ödeyemiyorsun.");

            CharacterManager.Instance.Stats.ModifyMoney(-cost);

            float happinessGain = RandomHelper.Range(10f, 20f);
            CharacterManager.Instance.Stats.ModifyHappiness(happinessGain);

            return (happinessGain, cost, $"Terapi seansı tamamlandı. Mutluluğun {happinessGain:F0} arttı.");
        }

        /// <summary>
        /// Get plastic surgery.
        /// </summary>
        public static (float looksGain, decimal cost, string message) GetPlasticSurgery(CharacterData character)
        {
            decimal cost = 15000;

            if (character.Money < cost)
                return (0, 0, "Estetik operasyon için yeterli paran yok.");

            CharacterManager.Instance.Stats.ModifyMoney(-cost);

            // Risk of complications
            if (RandomHelper.Chance(0.1f))
            {
                CharacterManager.Instance.Stats.ModifyHealth(-20);
                CharacterManager.Instance.Stats.ModifyLooks(-10);
                return (0, cost, "Estetik operasyonda komplikasyon oluştu! Sağlığın ve görünümün kötüleşti.");
            }

            float looksGain = RandomHelper.Range(15f, 30f);
            CharacterManager.Instance.Stats.ModifyLooks(looksGain);
            CharacterManager.Instance.Stats.ModifyHealth(-5); // Surgery risk

            return (looksGain, cost, $"Estetik operasyon başarılı! Görünümün {looksGain:F0} arttı.");
        }

        /// <summary>
        /// Exercise to improve health.
        /// </summary>
        public static void Exercise(CharacterData character)
        {
            float healthGain = RandomHelper.Range(2f, 8f);
            float looksGain = RandomHelper.Range(1f, 3f);

            CharacterManager.Instance.Stats.ModifyHealth(healthGain);
            CharacterManager.Instance.Stats.ModifyLooks(looksGain);
            CharacterManager.Instance.Stats.ModifyHappiness(RandomHelper.Range(2f, 5f));
        }

        /// <summary>
        /// Meditate to improve mental health.
        /// </summary>
        public static void Meditate(CharacterData character)
        {
            float happinessGain = RandomHelper.Range(5f, 12f);
            float healthGain = RandomHelper.Range(1f, 3f);

            CharacterManager.Instance.Stats.ModifyHappiness(happinessGain);
            CharacterManager.Instance.Stats.ModifyHealth(healthGain);
        }

        /// <summary>
        /// Check for accidental death.
        /// </summary>
        public static (bool died, string cause) CheckForAccident(CharacterData character)
        {
            float accidentChance = 0.001f;

            // Increase with age
            if (character.Age > 70) accidentChance += 0.005f;

            if (!RandomHelper.Chance(accidentChance))
                return (false, null);

            string[] accidents = {
                "Trafik kazası",
                "Ev kazası",
                "Doğal afet",
                "Zehirlenme"
            };

            string cause = accidents[Random.Range(0, accidents.Length)];

            return (true, cause);
        }
    }
}
