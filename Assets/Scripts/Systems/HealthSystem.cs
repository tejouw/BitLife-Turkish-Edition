using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Sağlık ve hastalık sistemi
    /// </summary>
    public class HealthSystem : MonoBehaviour
    {
        private List<DiseaseDefinition> diseases;

        private void Awake()
        {
            InitializeDiseases();
        }

        private void InitializeDiseases()
        {
            diseases = new List<DiseaseDefinition>
            {
                // Hafif hastalıklar
                new DiseaseDefinition
                {
                    Name = "Grip",
                    Type = DiseaseType.Flu,
                    Severity = 10,
                    IsChronic = false,
                    TreatmentCost = 100,
                    MinAge = 0,
                    BaseChance = 0.15f
                },
                new DiseaseDefinition
                {
                    Name = "Soğuk Algınlığı",
                    Type = DiseaseType.Cold,
                    Severity = 5,
                    IsChronic = false,
                    TreatmentCost = 50,
                    MinAge = 0,
                    BaseChance = 0.2f
                },

                // Orta şiddetli
                new DiseaseDefinition
                {
                    Name = "Depresyon",
                    Type = DiseaseType.Depression,
                    Severity = 30,
                    IsChronic = true,
                    TreatmentCost = 500,
                    MinAge = 12,
                    BaseChance = 0.05f
                },
                new DiseaseDefinition
                {
                    Name = "Anksiyete",
                    Type = DiseaseType.Anxiety,
                    Severity = 25,
                    IsChronic = true,
                    TreatmentCost = 400,
                    MinAge = 10,
                    BaseChance = 0.06f
                },
                new DiseaseDefinition
                {
                    Name = "Obezite",
                    Type = DiseaseType.Obesity,
                    Severity = 20,
                    IsChronic = true,
                    TreatmentCost = 1000,
                    MinAge = 5,
                    BaseChance = 0.08f
                },

                // Ciddi hastalıklar
                new DiseaseDefinition
                {
                    Name = "Yüksek Tansiyon",
                    Type = DiseaseType.Hypertension,
                    Severity = 40,
                    IsChronic = true,
                    TreatmentCost = 800,
                    MinAge = 30,
                    BaseChance = 0.04f
                },
                new DiseaseDefinition
                {
                    Name = "Şeker Hastalığı",
                    Type = DiseaseType.Diabetes,
                    Severity = 50,
                    IsChronic = true,
                    TreatmentCost = 1500,
                    MinAge = 20,
                    BaseChance = 0.03f
                },
                new DiseaseDefinition
                {
                    Name = "Kalp Hastalığı",
                    Type = DiseaseType.HeartDisease,
                    Severity = 70,
                    IsChronic = true,
                    TreatmentCost = 10000,
                    MinAge = 40,
                    BaseChance = 0.02f
                },
                new DiseaseDefinition
                {
                    Name = "Kanser",
                    Type = DiseaseType.Cancer,
                    Severity = 90,
                    IsChronic = true,
                    TreatmentCost = 50000,
                    MinAge = 20,
                    BaseChance = 0.01f
                },

                // Bağımlılık
                new DiseaseDefinition
                {
                    Name = "Alkol Bağımlılığı",
                    Type = DiseaseType.Addiction,
                    Severity = 45,
                    IsChronic = true,
                    TreatmentCost = 5000,
                    MinAge = 16,
                    BaseChance = 0.03f
                },
                new DiseaseDefinition
                {
                    Name = "Sigara Bağımlılığı",
                    Type = DiseaseType.Addiction,
                    Severity = 35,
                    IsChronic = true,
                    TreatmentCost = 2000,
                    MinAge = 14,
                    BaseChance = 0.05f
                }
            };
        }

        public void ProcessYearlyHealth(CharacterData character)
        {
            // Rastgele hastalık şansı
            CheckForNewDiseases(character);

            // Mevcut hastalıkları işle
            ProcessCurrentDiseases(character);

            // Alışkanlıkların etkisi
            ProcessHabits(character);
        }

        private void CheckForNewDiseases(CharacterData character)
        {
            foreach (var disease in diseases)
            {
                // Yaş kontrolü
                if (character.Age < disease.MinAge)
                    continue;

                // Zaten bu hastalığa sahip mi?
                if (character.Health.CurrentDiseases.Any(d => d.Type == disease.Type))
                    continue;

                // Hastalık şansı hesapla
                float chance = CalculateDiseaseChance(character, disease);

                if (Random.value < chance)
                {
                    ContractDisease(character, disease);
                }
            }
        }

        private float CalculateDiseaseChance(CharacterData character, DiseaseDefinition disease)
        {
            float chance = disease.BaseChance;

            // Yaşa göre artış
            if (character.Age > 50)
            {
                chance *= 1f + ((character.Age - 50) * 0.02f);
            }

            // Düşük sağlık
            float healthFactor = 1f - (character.Stats.Health / 100f);
            chance *= (1f + healthFactor);

            // Düşük fitness
            float fitnessFactor = 1f - (character.Health.Fitness / 100f);
            chance *= (1f + fitnessFactor * 0.5f);

            // Sigara etkisi
            if (character.Health.IsSmoker)
            {
                if (disease.Type == DiseaseType.Cancer || disease.Type == DiseaseType.HeartDisease)
                {
                    chance *= 3f;
                }
                else
                {
                    chance *= 1.5f;
                }
            }

            // Alkol etkisi
            if (character.Health.IsDrinker)
            {
                chance *= 1.3f;
            }

            // Uyuşturucu etkisi
            if (character.Health.UsesDrugs)
            {
                chance *= 2f;
            }

            return chance;
        }

        private void ContractDisease(CharacterData character, DiseaseDefinition disease)
        {
            DiseaseData newDisease = new DiseaseData
            {
                Name = disease.Name,
                Type = disease.Type,
                Severity = disease.Severity,
                IsChronic = disease.IsChronic,
                YearDiagnosed = character.Age + character.BirthYear
            };

            character.Health.CurrentDiseases.Add(newDisease);
            character.AddLifeEvent($"{disease.Name} teşhisi konuldu.");

            // Sağlık ve mutluluk düşüşü
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Health, -disease.Severity * 0.5f);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -disease.Severity * 0.3f);
        }

        private void ProcessCurrentDiseases(CharacterData character)
        {
            foreach (var disease in character.Health.CurrentDiseases.ToList())
            {
                // Kronik hastalıklar kalıcı etki
                if (disease.IsChronic)
                {
                    GameManager.Instance.StatSystem.ModifyStat(character, StatType.Health, -disease.Severity * 0.1f);
                }
                else
                {
                    // Geçici hastalıklar iyileşebilir
                    if (Random.value < 0.7f)
                    {
                        character.Health.CurrentDiseases.Remove(disease);
                        character.Health.PastDiseases.Add(disease.Name);
                        character.AddLifeEvent($"{disease.Name}'den iyileştin.");
                    }
                }
            }
        }

        private void ProcessHabits(CharacterData character)
        {
            // Sigara
            if (character.Health.IsSmoker)
            {
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Health, -2f);
                character.Health.Fitness = Mathf.Max(0, character.Health.Fitness - 1f);
            }

            // Alkol
            if (character.Health.IsDrinker)
            {
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Health, -1f);
            }

            // Uyuşturucu
            if (character.Health.UsesDrugs)
            {
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Health, -5f);
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Intelligence, -2f);
            }
        }

        public bool TreatDisease(CharacterData character, DiseaseData disease)
        {
            var definition = diseases.Find(d => d.Type == disease.Type);
            if (definition == null)
                return false;

            // Para kontrolü
            float cost = definition.TreatmentCost;
            if (character.Health.HasInsurance)
            {
                cost *= 0.2f; // Sigorta %80 karşılıyor
            }

            if (character.Stats.Money < cost)
            {
                character.AddLifeEvent("Tedavi için yeterli paran yok!");
                return false;
            }

            // Para harca
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -cost);

            // Tedavi başarı şansı
            float successChance = disease.IsChronic ? 0.3f : 0.8f;

            if (Random.value < successChance)
            {
                character.Health.CurrentDiseases.Remove(disease);
                character.Health.PastDiseases.Add(disease.Name);
                character.AddLifeEvent($"{disease.Name} tedavisi başarılı oldu!");

                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Health, disease.Severity * 0.5f);
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 10f);

                return true;
            }
            else
            {
                character.AddLifeEvent($"{disease.Name} tedavisi başarısız oldu.");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -5f);
                return false;
            }
        }

        public void GoToDoctor(CharacterData character)
        {
            float checkupCost = 200f;
            if (character.Health.HasInsurance)
            {
                checkupCost *= 0.2f;
            }

            if (character.Stats.Money < checkupCost)
            {
                character.AddLifeEvent("Doktor kontrolü için yeterli paran yok!");
                return;
            }

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -checkupCost);

            // Sağlık iyileştirmesi
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Health, Random.Range(2f, 8f));
            character.AddLifeEvent("Doktor kontrolüne gittin.");
        }

        public void GoToGym(CharacterData character)
        {
            float gymCost = 100f;
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -gymCost);

            // Fitness artışı
            character.Health.Fitness = Mathf.Min(100, character.Health.Fitness + 5f);

            // Sağlık ve mutluluk
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Health, Random.Range(1f, 3f));
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, Random.Range(2f, 5f));

            character.AddLifeEvent("Spor salonuna gittin.");
        }

        public void StartSmoking(CharacterData character)
        {
            character.Health.IsSmoker = true;
            character.AddLifeEvent("Sigara içmeye başladın.");
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, -5f);
        }

        public void QuitSmoking(CharacterData character)
        {
            if (!character.Health.IsSmoker)
                return;

            // Bırakma başarı şansı
            if (Random.value < 0.3f)
            {
                character.Health.IsSmoker = false;
                character.AddLifeEvent("Sigarayı bıraktın!");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 10f);
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Karma, 5f);
            }
            else
            {
                character.AddLifeEvent("Sigarayı bırakamadın.");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -5f);
            }
        }

        public void BuyHealthInsurance(CharacterData character)
        {
            float insuranceCost = 5000f;

            if (character.Stats.Money < insuranceCost)
            {
                character.AddLifeEvent("Sağlık sigortası için yeterli paran yok!");
                return;
            }

            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, -insuranceCost);
            character.Health.HasInsurance = true;
            character.AddLifeEvent("Sağlık sigortası satın aldın.");
        }
    }

    [System.Serializable]
    public class DiseaseDefinition
    {
        public string Name;
        public DiseaseType Type;
        public float Severity;
        public bool IsChronic;
        public float TreatmentCost;
        public int MinAge;
        public float BaseChance;
    }
}
