using UnityEngine;
using BitLifeTR.Core;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Manages crime, police, and prison.
    /// </summary>
    public static class CrimeSystem
    {
        /// <summary>
        /// Commit a crime.
        /// </summary>
        public static (bool success, bool caught, decimal reward, string message) CommitCrime(CharacterData character, CrimeType crimeType)
        {
            var (catchChance, reward, jailTime) = GetCrimeStats(crimeType);

            // Intelligence reduces catch chance
            catchChance -= character.Intelligence * 0.002f;

            // Previous crimes increase catch chance
            catchChance += character.CrimeCount * 0.05f;

            catchChance = Mathf.Clamp(catchChance, 0.05f, 0.95f);

            bool caught = RandomHelper.Chance(catchChance);

            if (caught)
            {
                character.HasCriminalRecord = true;
                character.CrimeCount++;

                // Go to jail
                character.JailTime += jailTime;

                // Lose job
                if (character.IsEmployed)
                {
                    character.IsEmployed = false;
                    character.CurrentJob = null;
                }

                CharacterManager.Instance.Stats.ModifyHappiness(-20);
                CharacterManager.Instance.Stats.ModifyKarma(-15);

                return (false, true, 0, $"Yakalandın! {jailTime} yıl hapis cezası aldın.");
            }
            else
            {
                character.CrimeCount++;
                CharacterManager.Instance.Stats.ModifyKarma(-10);

                return (true, false, reward, $"Başarılı! {reward:N0} TL kazandın.");
            }
        }

        /// <summary>
        /// Get crime statistics.
        /// </summary>
        private static (float catchChance, decimal reward, int jailTime) GetCrimeStats(CrimeType crimeType)
        {
            return crimeType switch
            {
                CrimeType.Hirsizlik => (0.4f, RandomHelper.Range(100, 1000), 1),
                CrimeType.Soygun => (0.5f, RandomHelper.Range(5000, 20000), 5),
                CrimeType.Dolandiricilik => (0.35f, RandomHelper.Range(10000, 50000), 3),
                CrimeType.UyusturucuSatisi => (0.45f, RandomHelper.Range(20000, 100000), 8),
                CrimeType.ArabaCalmak => (0.4f, RandomHelper.Range(10000, 50000), 4),
                CrimeType.BankasoYgunu => (0.6f, RandomHelper.Range(100000, 500000), 15),
                _ => (0.5f, 1000, 1)
            };
        }

        /// <summary>
        /// Process prison time.
        /// </summary>
        public static (bool released, string message) ProcessPrisonYear(CharacterData character)
        {
            if (character.JailTime <= 0)
                return (true, null);

            character.JailTime--;

            // Prison effects
            CharacterManager.Instance.Stats.ModifyHappiness(-10);
            CharacterManager.Instance.Stats.ModifyHealth(-5);

            // Chance of early release for good behavior
            if (character.JailTime > 0 && RandomHelper.Chance(0.1f))
            {
                character.JailTime = 0;
                return (true, "İyi hal indirimi ile serbest bırakıldın!");
            }

            if (character.JailTime <= 0)
            {
                return (true, "Cezanı çektin ve serbest bırakıldın.");
            }

            return (false, $"Hapiste {character.JailTime} yılın kaldı.");
        }

        /// <summary>
        /// Escape from prison.
        /// </summary>
        public static (bool success, string message) AttemptEscape(CharacterData character)
        {
            float successChance = 0.1f + (character.Intelligence / 200f);

            if (RandomHelper.Chance(successChance))
            {
                character.JailTime = 0;
                return (true, "Hapishaneden kaçtın!");
            }
            else
            {
                character.JailTime += 5;
                CharacterManager.Instance.Stats.ModifyHealth(-10);
                return (false, "Kaçış girişimin başarısız oldu! 5 yıl ek ceza aldın.");
            }
        }

        /// <summary>
        /// Get lawyer to reduce sentence.
        /// </summary>
        public static (bool success, string message) HireLawyer(CharacterData character, decimal fee)
        {
            if (character.Money < fee)
                return (false, "Avukat ücretini ödeyemiyorsun.");

            CharacterManager.Instance.Stats.ModifyMoney(-fee);

            float successChance = 0.5f;

            if (RandomHelper.Chance(successChance))
            {
                int reduction = Mathf.Max(1, character.JailTime / 2);
                character.JailTime -= reduction;

                return (true, $"Avukatın {reduction} yıl indirim sağladı!");
            }
            else
            {
                return (false, "Avukatın bir indirim sağlayamadı.");
            }
        }
    }

    /// <summary>
    /// Types of crimes.
    /// </summary>
    public enum CrimeType
    {
        Hirsizlik,
        Soygun,
        Dolandiricilik,
        UyusturucuSatisi,
        ArabaCalmak,
        BankaSoygunu
    }
}
