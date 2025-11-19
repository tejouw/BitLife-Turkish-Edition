using System;
using System.Collections.Generic;
using UnityEngine;
using BitLifeTR.Core;

namespace BitLifeTR.Data
{
    /// <summary>
    /// Karakter veri modeli
    /// </summary>
    [Serializable]
    public class CharacterData
    {
        // Temel bilgiler
        public string Id;
        public string Name;
        public string Surname;
        public Gender Gender;
        public int Age;
        public int BirthYear;
        public string BirthCity;

        // Statlar
        public CharacterStats Stats;

        // İlişkiler
        public List<RelationshipData> Relationships;

        // Eğitim ve kariyer
        public EducationData Education;
        public CareerData Career;

        // Sağlık
        public HealthData Health;

        // Varlıklar
        public FinancialData Finance;

        // Hukuki durum
        public LegalData Legal;

        // Geçmiş olaylar
        public List<string> LifeEvents;

        // Askerlik
        public MilitaryData Military;

        // Evcil hayvanlar
        public List<PetData> Pets;

        public CharacterData()
        {
            Id = Guid.NewGuid().ToString();
            Stats = new CharacterStats();
            Relationships = new List<RelationshipData>();
            Education = new EducationData();
            Career = new CareerData();
            Health = new HealthData();
            Finance = new FinancialData();
            Legal = new LegalData();
            LifeEvents = new List<string>();
            Military = new MilitaryData();
            Pets = new List<PetData>();
        }

        public CharacterData(string name, Gender gender) : this()
        {
            Name = name;
            Gender = gender;
            Age = 0;
            BirthYear = DateTime.Now.Year;
            BirthCity = GetRandomTurkishCity();

            // Başlangıç statlarını rastgele belirle
            Stats.Health = UnityEngine.Random.Range(50f, 100f);
            Stats.Happiness = UnityEngine.Random.Range(50f, 100f);
            Stats.Intelligence = UnityEngine.Random.Range(0f, 100f);
            Stats.Appearance = UnityEngine.Random.Range(0f, 100f);
            Stats.Money = 0;
            Stats.Fame = 0;
            Stats.Karma = 50f;

            // Aile oluştur
            GenerateFamily();
        }

        private string GetRandomTurkishCity()
        {
            string[] cities = {
                "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya",
                "Adana", "Konya", "Gaziantep", "Mersin", "Kayseri",
                "Eskişehir", "Trabzon", "Samsun", "Denizli", "Malatya"
            };
            return cities[UnityEngine.Random.Range(0, cities.Length)];
        }

        private void GenerateFamily()
        {
            // Anne
            Relationships.Add(new RelationshipData
            {
                Id = Guid.NewGuid().ToString(),
                Name = GenerateTurkishName(Gender.Female),
                RelationType = RelationType.Parent,
                Age = UnityEngine.Random.Range(20, 40),
                RelationshipLevel = UnityEngine.Random.Range(50f, 100f),
                IsAlive = true
            });

            // Baba
            Relationships.Add(new RelationshipData
            {
                Id = Guid.NewGuid().ToString(),
                Name = GenerateTurkishName(Gender.Male),
                RelationType = RelationType.Parent,
                Age = UnityEngine.Random.Range(22, 45),
                RelationshipLevel = UnityEngine.Random.Range(50f, 100f),
                IsAlive = true
            });

            // Rastgele kardeş (0-3)
            int siblingCount = UnityEngine.Random.Range(0, 4);
            for (int i = 0; i < siblingCount; i++)
            {
                Gender siblingGender = (Gender)UnityEngine.Random.Range(0, 2);
                Relationships.Add(new RelationshipData
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = GenerateTurkishName(siblingGender),
                    RelationType = RelationType.Sibling,
                    Age = UnityEngine.Random.Range(0, 18),
                    RelationshipLevel = UnityEngine.Random.Range(40f, 100f),
                    IsAlive = true
                });
            }
        }

        private string GenerateTurkishName(Gender gender)
        {
            string[] maleNames = {
                "Ahmet", "Mehmet", "Ali", "Mustafa", "Hasan",
                "Hüseyin", "İbrahim", "Osman", "Yusuf", "Emre",
                "Burak", "Murat", "Serkan", "Tolga", "Onur"
            };

            string[] femaleNames = {
                "Fatma", "Ayşe", "Emine", "Hatice", "Zeynep",
                "Elif", "Merve", "Büşra", "Esra", "Selin",
                "Deniz", "Ceren", "Gizem", "Yasemin", "Melis"
            };

            string[] surnames = {
                "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin",
                "Öztürk", "Aydın", "Özdemir", "Arslan", "Doğan",
                "Kılıç", "Aslan", "Çetin", "Koç", "Kurt"
            };

            string firstName = gender == Gender.Male
                ? maleNames[UnityEngine.Random.Range(0, maleNames.Length)]
                : femaleNames[UnityEngine.Random.Range(0, femaleNames.Length)];

            return firstName;
        }

        public AgePeriod GetAgePeriod()
        {
            if (Age <= 4) return AgePeriod.Infancy;
            if (Age <= 11) return AgePeriod.Childhood;
            if (Age <= 17) return AgePeriod.Adolescence;
            if (Age <= 29) return AgePeriod.YoungAdult;
            if (Age <= 49) return AgePeriod.Adult;
            if (Age <= 64) return AgePeriod.MiddleAge;
            return AgePeriod.Elder;
        }

        public void AddLifeEvent(string eventDescription)
        {
            string formattedEvent = $"[Yaş {Age}] {eventDescription}";
            LifeEvents.Add(formattedEvent);
        }
    }

    /// <summary>
    /// Karakter statları
    /// </summary>
    [Serializable]
    public class CharacterStats
    {
        public float Health;        // Sağlık
        public float Happiness;     // Mutluluk
        public float Intelligence;  // Zeka
        public float Appearance;    // Görünüm
        public float Money;         // Para
        public float Fame;          // Şöhret
        public float Karma;         // Karma

        public float GetStat(StatType type)
        {
            return type switch
            {
                StatType.Health => Health,
                StatType.Happiness => Happiness,
                StatType.Intelligence => Intelligence,
                StatType.Appearance => Appearance,
                StatType.Money => Money,
                StatType.Fame => Fame,
                StatType.Karma => Karma,
                _ => 0
            };
        }

        public void SetStat(StatType type, float value)
        {
            // Para için sınır yok, diğerleri 0-100
            if (type != StatType.Money)
            {
                value = Mathf.Clamp(value, Constants.MIN_STAT_VALUE, Constants.MAX_STAT_VALUE);
            }

            switch (type)
            {
                case StatType.Health: Health = value; break;
                case StatType.Happiness: Happiness = value; break;
                case StatType.Intelligence: Intelligence = value; break;
                case StatType.Appearance: Appearance = value; break;
                case StatType.Money: Money = value; break;
                case StatType.Fame: Fame = value; break;
                case StatType.Karma: Karma = value; break;
            }
        }

        public void ModifyStat(StatType type, float delta)
        {
            float currentValue = GetStat(type);
            SetStat(type, currentValue + delta);
        }
    }

    /// <summary>
    /// İlişki verisi
    /// </summary>
    [Serializable]
    public class RelationshipData
    {
        public string Id;
        public string Name;
        public RelationType RelationType;
        public int Age;
        public float RelationshipLevel;
        public bool IsAlive;
        public string Occupation;
        public Gender Gender;

        // Romantik ilişkiler için
        public bool IsMarried;
        public bool IsEngaged;
        public int YearsTogether;
    }

    /// <summary>
    /// Eğitim verisi
    /// </summary>
    [Serializable]
    public class EducationData
    {
        public EducationLevel CurrentLevel;
        public EducationLevel HighestLevel;
        public string CurrentSchool;
        public float GPA;
        public bool IsEnrolled;
        public int YearsInSchool;
        public List<string> Degrees;

        // YKS için
        public bool TookYKS;
        public float YKSScore;

        public EducationData()
        {
            Degrees = new List<string>();
            CurrentLevel = EducationLevel.None;
            HighestLevel = EducationLevel.None;
        }
    }

    /// <summary>
    /// Kariyer verisi
    /// </summary>
    [Serializable]
    public class CareerData
    {
        public bool IsEmployed;
        public string CurrentJob;
        public JobCategory JobCategory;
        public float Salary;
        public int YearsAtJob;
        public int TotalWorkYears;
        public float JobPerformance;
        public List<string> PreviousJobs;
        public bool IsRetired;

        public CareerData()
        {
            PreviousJobs = new List<string>();
            JobCategory = JobCategory.Unemployed;
        }
    }

    /// <summary>
    /// Sağlık verisi
    /// </summary>
    [Serializable]
    public class HealthData
    {
        public List<DiseaseData> CurrentDiseases;
        public List<string> PastDiseases;
        public bool HasInsurance;
        public float Fitness;
        public bool IsSmoker;
        public bool IsDrinker;
        public bool UsesDrugs;

        public HealthData()
        {
            CurrentDiseases = new List<DiseaseData>();
            PastDiseases = new List<string>();
            Fitness = 50f;
        }
    }

    /// <summary>
    /// Hastalık verisi
    /// </summary>
    [Serializable]
    public class DiseaseData
    {
        public string Name;
        public DiseaseType Type;
        public float Severity;
        public bool IsChronic;
        public int YearDiagnosed;
    }

    /// <summary>
    /// Finansal veri
    /// </summary>
    [Serializable]
    public class FinancialData
    {
        public float BankBalance;
        public float Debt;
        public List<AssetData> Assets;
        public float MonthlyExpenses;
        public float MonthlyIncome;

        public FinancialData()
        {
            Assets = new List<AssetData>();
        }
    }

    /// <summary>
    /// Varlık verisi
    /// </summary>
    [Serializable]
    public class AssetData
    {
        public string Name;
        public string Type; // Ev, Araba, etc.
        public float Value;
        public float MonthlyPayment;
        public int YearPurchased;
    }

    /// <summary>
    /// Hukuki durum
    /// </summary>
    [Serializable]
    public class LegalData
    {
        public bool HasCriminalRecord;
        public List<CrimeRecord> CrimeHistory;
        public bool IsInPrison;
        public int PrisonYearsRemaining;
        public bool IsOnProbation;
        public int ProbationYearsRemaining;

        public LegalData()
        {
            CrimeHistory = new List<CrimeRecord>();
        }
    }

    /// <summary>
    /// Suç kaydı
    /// </summary>
    [Serializable]
    public class CrimeRecord
    {
        public CrimeType CrimeType;
        public int Year;
        public bool WasCaught;
        public int PrisonSentence;
        public float Fine;
    }

    /// <summary>
    /// Askerlik verisi
    /// </summary>
    [Serializable]
    public class MilitaryData
    {
        public bool HasServed;
        public bool IsServing;
        public int ServiceStartYear;
        public int MonthsRemaining;
        public string Rank;
        public bool IsPaidService; // Bedelli
    }

    /// <summary>
    /// Evcil hayvan verisi
    /// </summary>
    [Serializable]
    public class PetData
    {
        public string Id;
        public string Name;
        public PetType Type;
        public PetCategory Category;
        public int Age;
        public float Health;
        public float Happiness;
        public float RelationshipLevel;
        public bool IsAlive;
        public int YearAdopted;

        public PetData()
        {
            Id = Guid.NewGuid().ToString();
            Health = 100f;
            Happiness = 100f;
            RelationshipLevel = 50f;
            IsAlive = true;
        }

        public PetData(string name, PetType type) : this()
        {
            Name = name;
            Type = type;
            Age = 0;
            Category = GetCategoryForType(type);
        }

        private PetCategory GetCategoryForType(PetType type)
        {
            return type switch
            {
                PetType.Fish => PetCategory.Aquatic,
                PetType.Turtle => PetCategory.Aquatic,
                PetType.Parrot => PetCategory.Exotic,
                PetType.Rabbit => PetCategory.Exotic,
                _ => PetCategory.Common
            };
        }

        public int GetMaxAge()
        {
            return Type switch
            {
                PetType.Dog => 15,
                PetType.Cat => 20,
                PetType.Bird => 10,
                PetType.Fish => 5,
                PetType.Hamster => 3,
                PetType.Rabbit => 12,
                PetType.Turtle => 50,
                PetType.Parrot => 30,
                _ => 10
            };
        }

        public string GetTypeName()
        {
            return Type switch
            {
                PetType.Dog => "Köpek",
                PetType.Cat => "Kedi",
                PetType.Bird => "Kuş",
                PetType.Fish => "Balık",
                PetType.Hamster => "Hamster",
                PetType.Rabbit => "Tavşan",
                PetType.Turtle => "Kaplumbağa",
                PetType.Parrot => "Papağan",
                _ => "Hayvan"
            };
        }
    }
}
