using UnityEngine;
using System.Collections.Generic;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Suç ve hukuk sistemi
    /// </summary>
    public class CrimeSystem : MonoBehaviour
    {
        public CrimeResult CommitCrime(CharacterData character, CrimeType crimeType)
        {
            var result = new CrimeResult();

            // Suçun detayları
            var crimeData = GetCrimeData(crimeType);

            // Yakalanma şansı
            float catchChance = crimeData.BaseCatchChance;

            // Zeka düşükse yakalanma şansı artar
            catchChance += (100 - character.Stats.Intelligence) * 0.002f;

            // Suç geçmişi varsa daha dikkatli izlenirsin
            if (character.Legal.HasCriminalRecord)
            {
                catchChance += 0.2f;
            }

            result.WasCaught = Random.value < catchChance;

            if (result.WasCaught)
            {
                // Yakalandın
                result.Success = false;
                ProcessArrest(character, crimeType, crimeData);
            }
            else
            {
                // Başarılı suç
                result.Success = true;

                // Para kazanç
                float earnings = Random.Range(crimeData.MinEarnings, crimeData.MaxEarnings);
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, earnings);
                result.Earnings = earnings;

                // Karma düşüşü
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, -crimeData.KarmaLoss);

                character.AddLifeEvent($"{crimeData.Name} yaptın ve {earnings:N0} TL kazandın.");
            }

            return result;
        }

        private void ProcessArrest(CharacterData character, CrimeType crimeType, CrimeData crimeData)
        {
            // Suç kaydı oluştur
            var crimeRecord = new CrimeRecord
            {
                CrimeType = crimeType,
                Year = character.Age + character.BirthYear,
                WasCaught = true
            };

            // Ceza hesapla
            int prisonYears = Random.Range(crimeData.MinPrisonYears, crimeData.MaxPrisonYears + 1);
            float fine = Random.Range(crimeData.MinFine, crimeData.MaxFine);

            // Avukat tutma şansı
            if (character.Stats.Money > fine * 2)
            {
                // Avukat tut
                float lawyerCost = fine;
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -lawyerCost);

                // Cezayı azaltma şansı
                if (Random.value < 0.5f)
                {
                    prisonYears = Mathf.Max(0, prisonYears - Random.Range(1, prisonYears));
                    fine *= 0.5f;
                    character.AddLifeEvent("Avukatın cezanı azalttı.");
                }
            }

            crimeRecord.PrisonSentence = prisonYears;
            crimeRecord.Fine = fine;

            character.Legal.CrimeHistory.Add(crimeRecord);
            character.Legal.HasCriminalRecord = true;

            // Para cezası
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -fine);

            // Hapis cezası
            if (prisonYears > 0)
            {
                character.Legal.IsInPrison = true;
                character.Legal.PrisonYearsRemaining = prisonYears;

                // İşten kovul
                if (character.Career.IsEmployed)
                {
                    GameManager.Instance.CareerSystem.QuitJob(character, true);
                }

                character.AddLifeEvent($"{crimeData.Name} suçundan {prisonYears} yıl hapis cezası aldın. (Ceza: {fine:N0} TL)");
            }
            else
            {
                character.AddLifeEvent($"{crimeData.Name} suçundan {fine:N0} TL para cezası aldın.");
            }

            // Mutluluk ve karma düşüşü
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -30f);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, -crimeData.KarmaLoss);
        }

        public void ProcessPrisonYear(CharacterData character)
        {
            if (!character.Legal.IsInPrison)
                return;

            character.Legal.PrisonYearsRemaining--;

            // Hapiste zaman geçirme
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -10f);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Health, -5f);

            // Hapiste öğrenme (olumsuz)
            if (Random.value < 0.3f)
            {
                character.AddLifeEvent("Hapiste başka mahkumlardan yeni 'şeyler' öğrendin.");
            }

            // Tahliye
            if (character.Legal.PrisonYearsRemaining <= 0)
            {
                character.Legal.IsInPrison = false;
                character.Legal.IsOnProbation = true;
                character.Legal.ProbationYearsRemaining = 2;

                character.AddLifeEvent("Hapisten çıktın! 2 yıl denetimli serbestlik.");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 20f);
            }
        }

        public bool TryEscape(CharacterData character)
        {
            if (!character.Legal.IsInPrison)
                return false;

            // Kaçış şansı çok düşük
            float escapeChance = 0.05f + (character.Stats.Intelligence / 500f);

            if (Random.value < escapeChance)
            {
                character.Legal.IsInPrison = false;
                character.AddLifeEvent("Hapishaneden kaçtın!");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 30f);

                // Ama aranıyorsun...
                return true;
            }
            else
            {
                // Yakalandın, ceza artışı
                character.Legal.PrisonYearsRemaining += 3;
                character.AddLifeEvent("Kaçış girişimi başarısız! Cezana 3 yıl eklendi.");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -20f);
                return false;
            }
        }

        private CrimeData GetCrimeData(CrimeType type)
        {
            return type switch
            {
                CrimeType.Theft => new CrimeData
                {
                    Name = "Hırsızlık",
                    BaseCatchChance = 0.3f,
                    MinEarnings = 100,
                    MaxEarnings = 1000,
                    MinPrisonYears = 0,
                    MaxPrisonYears = 2,
                    MinFine = 500,
                    MaxFine = 5000,
                    KarmaLoss = 10
                },
                CrimeType.Robbery => new CrimeData
                {
                    Name = "Soygun",
                    BaseCatchChance = 0.4f,
                    MinEarnings = 1000,
                    MaxEarnings = 50000,
                    MinPrisonYears = 2,
                    MaxPrisonYears = 10,
                    MinFine = 5000,
                    MaxFine = 50000,
                    KarmaLoss = 25
                },
                CrimeType.Assault => new CrimeData
                {
                    Name = "Saldırı",
                    BaseCatchChance = 0.5f,
                    MinEarnings = 0,
                    MaxEarnings = 0,
                    MinPrisonYears = 1,
                    MaxPrisonYears = 5,
                    MinFine = 2000,
                    MaxFine = 20000,
                    KarmaLoss = 20
                },
                CrimeType.Murder => new CrimeData
                {
                    Name = "Cinayet",
                    BaseCatchChance = 0.7f,
                    MinEarnings = 0,
                    MaxEarnings = 0,
                    MinPrisonYears = 15,
                    MaxPrisonYears = 100, // Ömür boyu
                    MinFine = 0,
                    MaxFine = 0,
                    KarmaLoss = 100
                },
                CrimeType.Fraud => new CrimeData
                {
                    Name = "Dolandırıcılık",
                    BaseCatchChance = 0.35f,
                    MinEarnings = 5000,
                    MaxEarnings = 100000,
                    MinPrisonYears = 1,
                    MaxPrisonYears = 8,
                    MinFine = 10000,
                    MaxFine = 100000,
                    KarmaLoss = 30
                },
                CrimeType.DrugDealing => new CrimeData
                {
                    Name = "Uyuşturucu Ticareti",
                    BaseCatchChance = 0.45f,
                    MinEarnings = 2000,
                    MaxEarnings = 50000,
                    MinPrisonYears = 3,
                    MaxPrisonYears = 15,
                    MinFine = 10000,
                    MaxFine = 200000,
                    KarmaLoss = 40
                },
                CrimeType.DrugPossession => new CrimeData
                {
                    Name = "Uyuşturucu Bulundurma",
                    BaseCatchChance = 0.3f,
                    MinEarnings = 0,
                    MaxEarnings = 0,
                    MinPrisonYears = 0,
                    MaxPrisonYears = 3,
                    MinFine = 1000,
                    MaxFine = 10000,
                    KarmaLoss = 15
                },
                CrimeType.Vandalism => new CrimeData
                {
                    Name = "Vandalizm",
                    BaseCatchChance = 0.25f,
                    MinEarnings = 0,
                    MaxEarnings = 0,
                    MinPrisonYears = 0,
                    MaxPrisonYears = 1,
                    MinFine = 500,
                    MaxFine = 5000,
                    KarmaLoss = 8
                },
                CrimeType.TaxEvasion => new CrimeData
                {
                    Name = "Vergi Kaçırma",
                    BaseCatchChance = 0.2f,
                    MinEarnings = 10000,
                    MaxEarnings = 500000,
                    MinPrisonYears = 1,
                    MaxPrisonYears = 5,
                    MinFine = 50000,
                    MaxFine = 500000,
                    KarmaLoss = 20
                },
                CrimeType.Bribery => new CrimeData
                {
                    Name = "Rüşvet",
                    BaseCatchChance = 0.15f,
                    MinEarnings = 5000,
                    MaxEarnings = 100000,
                    MinPrisonYears = 1,
                    MaxPrisonYears = 6,
                    MinFine = 20000,
                    MaxFine = 200000,
                    KarmaLoss = 25
                },
                _ => new CrimeData
                {
                    Name = "Bilinmeyen Suç",
                    BaseCatchChance = 0.5f,
                    MinEarnings = 0,
                    MaxEarnings = 0,
                    MinPrisonYears = 1,
                    MaxPrisonYears = 5,
                    MinFine = 1000,
                    MaxFine = 10000,
                    KarmaLoss = 15
                }
            };
        }
    }

    public class CrimeData
    {
        public string Name;
        public float BaseCatchChance;
        public float MinEarnings;
        public float MaxEarnings;
        public int MinPrisonYears;
        public int MaxPrisonYears;
        public float MinFine;
        public float MaxFine;
        public float KarmaLoss;
    }

    public class CrimeResult
    {
        public bool Success;
        public bool WasCaught;
        public float Earnings;
    }
}
