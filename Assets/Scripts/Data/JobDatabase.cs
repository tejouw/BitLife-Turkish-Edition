using System.Collections.Generic;

namespace BitLifeTR.Data
{
    /// <summary>
    /// Database of all jobs/careers available in the game.
    /// </summary>
    public static class JobDatabase
    {
        /// <summary>
        /// Get all available jobs.
        /// </summary>
        public static List<JobData> GetAllJobs()
        {
            return new List<JobData>
            {
                // Devlet İşleri
                new JobData("Memur", "Devlet", EducationLevel.Lise, 6000, 15000, "Devlet dairelerinde çalışırsın."),
                new JobData("Öğretmen", "Devlet", EducationLevel.Universite, 8000, 18000, "Okullarda öğretmenlik yaparsın."),
                new JobData("Polis", "Devlet", EducationLevel.Lise, 7000, 16000, "Halkın güvenliğini sağlarsın."),
                new JobData("Asker (Subay)", "Devlet", EducationLevel.Universite, 8000, 20000, "Türk Silahlı Kuvvetleri'nde görev yaparsın."),
                new JobData("Hemşire", "Sağlık", EducationLevel.Universite, 7000, 15000, "Hastanelerde hasta bakımı yaparsın."),
                new JobData("Doktor", "Sağlık", EducationLevel.Universite, 15000, 50000, "Hastaları tedavi edersin.", requiredIntelligence: 70),
                new JobData("Hakim", "Hukuk", EducationLevel.Universite, 20000, 45000, "Mahkemelerde adalet dağıtırsın.", requiredIntelligence: 75),
                new JobData("Savcı", "Hukuk", EducationLevel.Universite, 18000, 40000, "Suçluları yargılarsın.", requiredIntelligence: 70),

                // Özel Sektör
                new JobData("Bankacı", "Finans", EducationLevel.Universite, 8000, 25000, "Bankada çalışırsın."),
                new JobData("Muhasebeci", "Finans", EducationLevel.Universite, 6000, 20000, "Şirketlerin hesaplarını tutarsın."),
                new JobData("Yazılımcı", "Teknoloji", EducationLevel.Universite, 12000, 40000, "Yazılım geliştirirsin.", requiredIntelligence: 60),
                new JobData("Mühendis", "Teknoloji", EducationLevel.Universite, 10000, 35000, "Mühendislik projeleri yaparsın.", requiredIntelligence: 65),
                new JobData("Mimar", "Tasarım", EducationLevel.Universite, 10000, 35000, "Binalar tasarlarsın.", requiredIntelligence: 60),
                new JobData("Avukat", "Hukuk", EducationLevel.Universite, 10000, 50000, "Müvekkillerini savunursun.", requiredIntelligence: 65),
                new JobData("Reklamcı", "Medya", EducationLevel.Universite, 7000, 25000, "Reklamlar üretirsin."),
                new JobData("Gazeteci", "Medya", EducationLevel.Universite, 6000, 20000, "Haberleri takip edersin."),
                new JobData("Pilot", "Ulaşım", EducationLevel.Universite, 20000, 60000, "Uçak uçurursun.", requiredIntelligence: 65),

                // Esnaf/Serbest
                new JobData("Esnaf", "Ticaret", EducationLevel.Yok, 5000, 30000, "Kendi dükkanını işletirsin."),
                new JobData("Berber/Kuaför", "Hizmet", EducationLevel.Ortaokul, 4000, 15000, "Saç kesiyor ve bakım yapıyorsun."),
                new JobData("Şoför", "Ulaşım", EducationLevel.Yok, 4000, 12000, "Araç kullanırsın."),
                new JobData("Garson", "Hizmet", EducationLevel.Yok, 3500, 8000, "Restoranlarda servis yaparsın."),
                new JobData("Aşçı", "Yiyecek", EducationLevel.Lise, 5000, 20000, "Yemek pişirirsin."),
                new JobData("Tamirci", "Teknik", EducationLevel.Ortaokul, 4000, 15000, "Araçları tamir edersin."),
                new JobData("İnşaat İşçisi", "İnşaat", EducationLevel.Yok, 4000, 10000, "İnşaatlarda çalışırsın."),
                new JobData("Temizlik Görevlisi", "Hizmet", EducationLevel.Yok, 3500, 7000, "Temizlik işleri yaparsın."),
                new JobData("Kasap", "Yiyecek", EducationLevel.Yok, 5000, 15000, "Et satarsın."),
                new JobData("Manav", "Yiyecek", EducationLevel.Yok, 4000, 12000, "Meyve sebze satarsın."),

                // Sanat/Eğlence
                new JobData("Şarkıcı", "Sanat", EducationLevel.Yok, 3000, 100000, "Şarkı söylersin.", requiredLooks: 50),
                new JobData("Oyuncu", "Sanat", EducationLevel.Yok, 3000, 150000, "Dizi/film oyuncususun.", requiredLooks: 60),
                new JobData("Model", "Moda", EducationLevel.Yok, 5000, 80000, "Moda şovlarında yürürsün.", requiredLooks: 75),
                new JobData("Futbolcu", "Spor", EducationLevel.Yok, 5000, 500000, "Futbol oynarsın."),
                new JobData("YouTuber", "Medya", EducationLevel.Yok, 0, 200000, "Video içerik üretirsin."),
                new JobData("Influencer", "Medya", EducationLevel.Yok, 0, 100000, "Sosyal medyada içerik üretirsin.", requiredLooks: 50),
                new JobData("DJ", "Sanat", EducationLevel.Yok, 3000, 50000, "Müzik çalarsın."),
                new JobData("Ressam", "Sanat", EducationLevel.Yok, 2000, 30000, "Resim yaparsın."),
                new JobData("Yazar", "Sanat", EducationLevel.Lise, 2000, 40000, "Kitap yazarsın.", requiredIntelligence: 60),

                // Diğer
                new JobData("Emlakçı", "Gayrimenkul", EducationLevel.Lise, 5000, 40000, "Ev/arsa satarsın."),
                new JobData("Sigortacı", "Finans", EducationLevel.Lise, 5000, 20000, "Sigorta satarsın."),
                new JobData("Çağrı Merkezi", "Hizmet", EducationLevel.Lise, 4000, 8000, "Telefonda müşteri hizmeti verirsin."),
                new JobData("Güvenlik Görevlisi", "Hizmet", EducationLevel.Yok, 4000, 8000, "Güvenlik sağlarsın."),
                new JobData("Veteriner", "Sağlık", EducationLevel.Universite, 8000, 25000, "Hayvanları tedavi edersin.", requiredIntelligence: 60),
                new JobData("Eczacı", "Sağlık", EducationLevel.Universite, 10000, 30000, "İlaç satarsın.", requiredIntelligence: 60),
                new JobData("Psikolog", "Sağlık", EducationLevel.Universite, 8000, 25000, "İnsanlara terapi verirsin.", requiredIntelligence: 60),
                new JobData("Diş Hekimi", "Sağlık", EducationLevel.Universite, 12000, 40000, "Diş tedavisi yaparsın.", requiredIntelligence: 65)
            };
        }

        /// <summary>
        /// Get jobs by category.
        /// </summary>
        public static List<JobData> GetJobsByCategory(string category)
        {
            return GetAllJobs().FindAll(j => j.Category == category);
        }

        /// <summary>
        /// Get entry-level jobs (no education required).
        /// </summary>
        public static List<JobData> GetEntryLevelJobs()
        {
            return GetAllJobs().FindAll(j => j.RequiredEducation == EducationLevel.Yok);
        }

        /// <summary>
        /// Get jobs requiring university degree.
        /// </summary>
        public static List<JobData> GetProfessionalJobs()
        {
            return GetAllJobs().FindAll(j => j.RequiredEducation == EducationLevel.Universite);
        }
    }

    /// <summary>
    /// Data for a single job/career.
    /// </summary>
    [System.Serializable]
    public class JobData
    {
        public string Name;
        public string Category;
        public EducationLevel RequiredEducation;
        public decimal MinSalary;
        public decimal MaxSalary;
        public string Description;
        public float RequiredIntelligence;
        public float RequiredLooks;

        public JobData(string name, string category, EducationLevel education,
            decimal minSalary, decimal maxSalary, string description,
            float requiredIntelligence = 0, float requiredLooks = 0)
        {
            Name = name;
            Category = category;
            RequiredEducation = education;
            MinSalary = minSalary;
            MaxSalary = maxSalary;
            Description = description;
            RequiredIntelligence = requiredIntelligence;
            RequiredLooks = requiredLooks;
        }

        /// <summary>
        /// Get a random salary within range.
        /// </summary>
        public decimal GetRandomSalary()
        {
            return (decimal)UnityEngine.Random.Range((float)MinSalary, (float)MaxSalary);
        }
    }
}
