using UnityEngine;
using System.Collections.Generic;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Kariyer ve eğitim sistemi
    /// </summary>
    public class CareerSystem : MonoBehaviour
    {
        private List<JobDefinition> availableJobs;

        private void Awake()
        {
            InitializeJobs();
        }

        private void InitializeJobs()
        {
            availableJobs = new List<JobDefinition>
            {
                // Devlet İşleri
                new JobDefinition
                {
                    Title = "Devlet Memuru",
                    Category = JobCategory.Government,
                    BaseSalary = 8000,
                    RequiredEducation = EducationLevel.HighSchool,
                    RequiredIntelligence = 40,
                    Description = "Devlet dairesinde çalışma"
                },
                new JobDefinition
                {
                    Title = "Polis",
                    Category = JobCategory.Government,
                    BaseSalary = 9000,
                    RequiredEducation = EducationLevel.HighSchool,
                    RequiredIntelligence = 35,
                    Description = "Emniyet teşkilatında görev"
                },
                new JobDefinition
                {
                    Title = "Öğretmen",
                    Category = JobCategory.Education,
                    BaseSalary = 10000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 60,
                    Description = "Okullarda eğitim verme"
                },

                // Sağlık
                new JobDefinition
                {
                    Title = "Doktor",
                    Category = JobCategory.Healthcare,
                    BaseSalary = 25000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 80,
                    Description = "Tıp doktorluğu"
                },
                new JobDefinition
                {
                    Title = "Hemşire",
                    Category = JobCategory.Healthcare,
                    BaseSalary = 9000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 50,
                    Description = "Sağlık hizmetleri"
                },
                new JobDefinition
                {
                    Title = "Eczacı",
                    Category = JobCategory.Healthcare,
                    BaseSalary = 15000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 70,
                    Description = "Eczane işletmeciliği"
                },

                // Hukuk
                new JobDefinition
                {
                    Title = "Avukat",
                    Category = JobCategory.Law,
                    BaseSalary = 20000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 75,
                    Description = "Hukuki danışmanlık"
                },
                new JobDefinition
                {
                    Title = "Hakim",
                    Category = JobCategory.Law,
                    BaseSalary = 30000,
                    RequiredEducation = EducationLevel.Masters,
                    RequiredIntelligence = 85,
                    Description = "Yargıçlık"
                },

                // Mühendislik
                new JobDefinition
                {
                    Title = "Yazılım Mühendisi",
                    Category = JobCategory.Engineering,
                    BaseSalary = 35000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 70,
                    Description = "Yazılım geliştirme"
                },
                new JobDefinition
                {
                    Title = "İnşaat Mühendisi",
                    Category = JobCategory.Engineering,
                    BaseSalary = 18000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 65,
                    Description = "İnşaat projeleri"
                },
                new JobDefinition
                {
                    Title = "Elektrik Mühendisi",
                    Category = JobCategory.Engineering,
                    BaseSalary = 16000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 65,
                    Description = "Elektrik sistemleri"
                },

                // İş Dünyası
                new JobDefinition
                {
                    Title = "Bankacı",
                    Category = JobCategory.Business,
                    BaseSalary = 12000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 55,
                    Description = "Bankacılık hizmetleri"
                },
                new JobDefinition
                {
                    Title = "Muhasebeci",
                    Category = JobCategory.Business,
                    BaseSalary = 10000,
                    RequiredEducation = EducationLevel.University,
                    RequiredIntelligence = 50,
                    Description = "Mali işler"
                },
                new JobDefinition
                {
                    Title = "Esnaf",
                    Category = JobCategory.Business,
                    BaseSalary = 8000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 30,
                    Description = "Kendi işini yürütme"
                },

                // Sanat & Medya
                new JobDefinition
                {
                    Title = "Oyuncu",
                    Category = JobCategory.Arts,
                    BaseSalary = 15000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 40,
                    RequiredAppearance = 60,
                    Description = "Oyunculuk"
                },
                new JobDefinition
                {
                    Title = "Şarkıcı",
                    Category = JobCategory.Arts,
                    BaseSalary = 20000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 30,
                    Description = "Müzik kariyeri"
                },
                new JobDefinition
                {
                    Title = "YouTuber",
                    Category = JobCategory.Media,
                    BaseSalary = 5000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 40,
                    Description = "İçerik üreticiliği"
                },
                new JobDefinition
                {
                    Title = "Influencer",
                    Category = JobCategory.Media,
                    BaseSalary = 8000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 35,
                    RequiredAppearance = 70,
                    Description = "Sosyal medya fenomeni"
                },

                // Spor
                new JobDefinition
                {
                    Title = "Futbolcu",
                    Category = JobCategory.Sports,
                    BaseSalary = 50000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 20,
                    RequiredHealth = 80,
                    Description = "Profesyonel futbol"
                },
                new JobDefinition
                {
                    Title = "Basketbolcu",
                    Category = JobCategory.Sports,
                    BaseSalary = 30000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 25,
                    RequiredHealth = 80,
                    Description = "Profesyonel basketbol"
                },

                // Hizmet
                new JobDefinition
                {
                    Title = "Şoför",
                    Category = JobCategory.Service,
                    BaseSalary = 6000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 20,
                    Description = "Taşımacılık"
                },
                new JobDefinition
                {
                    Title = "Kuaför",
                    Category = JobCategory.Service,
                    BaseSalary = 5000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 25,
                    Description = "Güzellik hizmetleri"
                },
                new JobDefinition
                {
                    Title = "Aşçı",
                    Category = JobCategory.Service,
                    BaseSalary = 7000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 30,
                    Description = "Mutfak sanatı"
                },
                new JobDefinition
                {
                    Title = "Garson",
                    Category = JobCategory.Service,
                    BaseSalary = 4000,
                    RequiredEducation = EducationLevel.None,
                    RequiredIntelligence = 20,
                    Description = "Restoran hizmetleri"
                }
            };
        }

        public List<JobDefinition> GetAvailableJobs(CharacterData character)
        {
            List<JobDefinition> eligible = new List<JobDefinition>();

            foreach (var job in availableJobs)
            {
                if (IsJobEligible(character, job))
                {
                    eligible.Add(job);
                }
            }

            return eligible;
        }

        private bool IsJobEligible(CharacterData character, JobDefinition job)
        {
            // Eğitim kontrolü
            if (character.Education.HighestLevel < job.RequiredEducation)
                return false;

            // Zeka kontrolü
            if (character.Stats.Intelligence < job.RequiredIntelligence)
                return false;

            // Görünüm kontrolü
            if (job.RequiredAppearance > 0 && character.Stats.Appearance < job.RequiredAppearance)
                return false;

            // Sağlık kontrolü
            if (job.RequiredHealth > 0 && character.Stats.Health < job.RequiredHealth)
                return false;

            // Yaş kontrolü
            if (character.Age < 18)
                return false;

            return true;
        }

        public bool ApplyForJob(CharacterData character, string jobTitle)
        {
            var job = availableJobs.Find(j => j.Title == jobTitle);
            if (job == null || !IsJobEligible(character, job))
                return false;

            // Kabul şansı
            float acceptChance = CalculateAcceptChance(character, job);

            if (Random.value < acceptChance)
            {
                // İşe alındı
                if (character.Career.IsEmployed)
                {
                    character.Career.PreviousJobs.Add(character.Career.CurrentJob);
                }

                character.Career.IsEmployed = true;
                character.Career.CurrentJob = job.Title;
                character.Career.JobCategory = job.Category;
                character.Career.Salary = job.BaseSalary;
                character.Career.YearsAtJob = 0;
                character.Career.JobPerformance = 50f;

                character.AddLifeEvent($"{job.Title} olarak işe başladın. (Maaş: {job.BaseSalary:N0} TL)");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 15f);

                EventBus.Publish(new JobChangedEvent(character.Career.PreviousJobs.Count > 0 ?
                    character.Career.PreviousJobs[^1] : "", job.Title));

                return true;
            }
            else
            {
                character.AddLifeEvent($"{job.Title} başvurun reddedildi.");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -5f);
                return false;
            }
        }

        private float CalculateAcceptChance(CharacterData character, JobDefinition job)
        {
            float chance = 0.5f;

            // Eğitim bonusu
            int educationDiff = (int)character.Education.HighestLevel - (int)job.RequiredEducation;
            chance += educationDiff * 0.1f;

            // Zeka bonusu
            float intelligenceDiff = character.Stats.Intelligence - job.RequiredIntelligence;
            chance += intelligenceDiff * 0.005f;

            // Deneyim bonusu
            chance += character.Career.TotalWorkYears * 0.02f;

            return Mathf.Clamp(chance, 0.1f, 0.95f);
        }

        public void ProcessYearlyCareer(CharacterData character)
        {
            if (!character.Career.IsEmployed)
                return;

            character.Career.YearsAtJob++;
            character.Career.TotalWorkYears++;

            // Maaş artışı (yıllık)
            float raise = character.Career.Salary * Random.Range(0.02f, 0.08f);
            character.Career.Salary += raise;

            // Para kazan
            float yearlyIncome = character.Career.Salary * 12;
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Money, yearlyIncome);

            // Performans değişimi
            float performanceChange = Random.Range(-5f, 10f);
            character.Career.JobPerformance = Mathf.Clamp(character.Career.JobPerformance + performanceChange, 0, 100);

            // Terfi şansı
            if (character.Career.YearsAtJob >= 3 && character.Career.JobPerformance > 70 && Random.value < 0.2f)
            {
                float promotionRaise = character.Career.Salary * 0.2f;
                character.Career.Salary += promotionRaise;
                character.AddLifeEvent($"Terfi aldın! Yeni maaşın: {character.Career.Salary:N0} TL");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 20f);
            }

            // Kovulma riski (düşük performans)
            if (character.Career.JobPerformance < 20 && Random.value < 0.3f)
            {
                QuitJob(character, true);
            }
        }

        public void QuitJob(CharacterData character, bool fired = false)
        {
            if (!character.Career.IsEmployed)
                return;

            string jobTitle = character.Career.CurrentJob;
            character.Career.PreviousJobs.Add(jobTitle);
            character.Career.IsEmployed = false;
            character.Career.CurrentJob = "";
            character.Career.JobCategory = JobCategory.Unemployed;
            character.Career.Salary = 0;

            if (fired)
            {
                character.AddLifeEvent($"{jobTitle} işinden kovuldun!");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -20f);
            }
            else
            {
                character.AddLifeEvent($"{jobTitle} işinden ayrıldın.");
                GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, -5f);
            }

            EventBus.Publish(new JobChangedEvent(jobTitle, ""));
        }

        public void Retire(CharacterData character)
        {
            if (!character.Career.IsEmployed)
                return;

            character.Career.IsRetired = true;
            character.Career.PreviousJobs.Add(character.Career.CurrentJob);
            character.Career.IsEmployed = false;

            // Emekli maaşı hesapla
            float pension = character.Career.Salary * 0.5f;
            character.Finance.MonthlyIncome = pension;

            character.AddLifeEvent($"Emekli oldun! Emekli maaşın: {pension:N0} TL");
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 20f);
        }

        public void StartEducation(CharacterData character, EducationLevel level)
        {
            character.Education.IsEnrolled = true;
            character.Education.CurrentLevel = level;
            character.Education.YearsInSchool = 0;

            string schoolName = level switch
            {
                EducationLevel.PrimarySchool => "İlkokul",
                EducationLevel.MiddleSchool => "Ortaokul",
                EducationLevel.HighSchool => "Lise",
                EducationLevel.University => "Üniversite",
                EducationLevel.Masters => "Yüksek Lisans",
                EducationLevel.Doctorate => "Doktora",
                _ => "Okul"
            };

            character.AddLifeEvent($"{schoolName}'a başladın.");
        }

        public void GraduateEducation(CharacterData character)
        {
            character.Education.IsEnrolled = false;
            character.Education.HighestLevel = character.Education.CurrentLevel;
            character.Education.Degrees.Add(character.Education.CurrentLevel.ToString());

            string degreeName = character.Education.CurrentLevel switch
            {
                EducationLevel.PrimarySchool => "İlkokul diploması",
                EducationLevel.MiddleSchool => "Ortaokul diploması",
                EducationLevel.HighSchool => "Lise diploması",
                EducationLevel.University => "Lisans diploması",
                EducationLevel.Masters => "Yüksek Lisans diploması",
                EducationLevel.Doctorate => "Doktora derecesi",
                _ => "Diploma"
            };

            character.AddLifeEvent($"{degreeName} aldın!");
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Happiness, 15f);
            GameManager.Instance.StatSystem.ModifyStat(character, StatType.Intelligence, 10f);

            EventBus.Publish(new EducationCompletedEvent(character.Education.CurrentLevel));
        }
    }

    [System.Serializable]
    public class JobDefinition
    {
        public string Title;
        public JobCategory Category;
        public float BaseSalary;
        public EducationLevel RequiredEducation;
        public float RequiredIntelligence;
        public float RequiredAppearance;
        public float RequiredHealth;
        public string Description;
    }
}
