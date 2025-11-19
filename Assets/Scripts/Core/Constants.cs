namespace BitLifeTR.Core
{
    /// <summary>
    /// Oyun sabitleri ve enum tanımları
    /// </summary>
    public static class Constants
    {
        // Stat sınırları
        public const float MIN_STAT_VALUE = 0f;
        public const float MAX_STAT_VALUE = 100f;

        // Yaş sınırları
        public const int MAX_AGE = 120;
        public const int SCHOOL_START_AGE = 6;
        public const int MIDDLE_SCHOOL_AGE = 11;
        public const int HIGH_SCHOOL_AGE = 14;
        public const int UNIVERSITY_AGE = 18;
        public const int MILITARY_AGE = 20;
        public const int RETIREMENT_AGE = 65;

        // Dosya yolları
        public const string SAVE_FILE_NAME = "bitlife_save.json";
        public const string EVENTS_FILE_PATH = "Events/";
        public const string LOCALIZATION_FILE_PATH = "Localization/";

        // UI sabitleri
        public const float DEFAULT_BUTTON_WIDTH = 200f;
        public const float DEFAULT_BUTTON_HEIGHT = 50f;
        public const float DEFAULT_PADDING = 10f;
        public const int DEFAULT_FONT_SIZE = 24;

        // Oyun dengeleme
        public const float YEARLY_STAT_DECAY = 1f;
        public const float HAPPINESS_DECAY_RATE = 2f;
        public const float HEALTH_DECAY_RATE = 0.5f;
        public const float NATURAL_DEATH_BASE_CHANCE = 0.001f;
        public const float NATURAL_DEATH_AGE_FACTOR = 0.01f;
    }

    /// <summary>
    /// Stat tipleri
    /// </summary>
    public enum StatType
    {
        Health,      // Sağlık
        Happiness,   // Mutluluk
        Intelligence,// Zeka
        Appearance,  // Görünüm
        Money,       // Para
        Fame,        // Şöhret
        Karma        // Karma
    }

    /// <summary>
    /// Cinsiyet
    /// </summary>
    public enum Gender
    {
        Male,    // Erkek
        Female   // Kadın
    }

    /// <summary>
    /// Yaş dönemleri
    /// </summary>
    public enum AgePeriod
    {
        Infancy,      // Bebeklik (0-4)
        Childhood,    // Çocukluk (5-11)
        Adolescence,  // Ergenlik (12-17)
        YoungAdult,   // Genç Yetişkinlik (18-29)
        Adult,        // Yetişkinlik (30-49)
        MiddleAge,    // Orta Yaş (50-64)
        Elder         // Yaşlılık (65+)
    }

    /// <summary>
    /// Ekran tipleri
    /// </summary>
    public enum ScreenType
    {
        MainMenu,
        NewGame,
        Game,
        Stats,
        Relationships,
        Career,
        Education,
        Activities,
        Assets,
        Settings,
        GameOver,
        Event,
        Pets,
        Legacy,
        Will
    }

    /// <summary>
    /// Eğitim seviyeleri
    /// </summary>
    public enum EducationLevel
    {
        None,           // Eğitimsiz
        PrimarySchool,  // İlkokul
        MiddleSchool,   // Ortaokul
        HighSchool,     // Lise
        University,     // Üniversite
        Masters,        // Yüksek Lisans
        Doctorate       // Doktora
    }

    /// <summary>
    /// İlişki tipleri
    /// </summary>
    public enum RelationType
    {
        Parent,     // Ebeveyn
        Sibling,    // Kardeş
        Child,      // Çocuk
        Spouse,     // Eş
        Friend,     // Arkadaş
        Colleague,  // İş arkadaşı
        Neighbor,   // Komşu
        Relative,   // Akraba
        Romantic    // Romantik ilişki
    }

    /// <summary>
    /// Meslek kategorileri
    /// </summary>
    public enum JobCategory
    {
        Unemployed,     // İşsiz
        Student,        // Öğrenci
        Government,     // Devlet memuru
        Healthcare,     // Sağlık
        Law,            // Hukuk
        Engineering,    // Mühendislik
        Education,      // Eğitim
        Business,       // İş dünyası
        Arts,           // Sanat
        Sports,         // Spor
        Media,          // Medya
        Service,        // Hizmet
        Military,       // Askerlik
        Criminal        // Suç
    }

    /// <summary>
    /// Ölüm nedenleri
    /// </summary>
    public enum DeathReason
    {
        NaturalCauses,  // Doğal nedenler
        HealthDepleted, // Sağlık tükendi
        Accident,       // Kaza
        Disease,        // Hastalık
        Murder,         // Cinayet
        Suicide,        // İntihar
        DrugOverdose,   // Aşırı doz
        Execution,      // İdam
        War             // Savaş
    }

    /// <summary>
    /// Olay kategorileri
    /// </summary>
    public enum EventCategory
    {
        Random,         // Rastgele
        Age,            // Yaşa bağlı
        Career,         // Kariyer
        Relationship,   // İlişki
        Health,         // Sağlık
        Education,      // Eğitim
        Legal,          // Hukuki
        Financial,      // Finansal
        Social,         // Sosyal
        Military,       // Askerlik
        Holiday         // Bayram/tatil
    }

    /// <summary>
    /// Suç tipleri
    /// </summary>
    public enum CrimeType
    {
        Theft,          // Hırsızlık
        Robbery,        // Soygun
        Assault,        // Saldırı
        Murder,         // Cinayet
        Fraud,          // Dolandırıcılık
        DrugDealing,    // Uyuşturucu ticareti
        DrugPossession, // Uyuşturucu bulundurma
        Vandalism,      // Vandallık
        TaxEvasion,     // Vergi kaçırma
        Bribery         // Rüşvet
    }

    /// <summary>
    /// Hastalık tipleri
    /// </summary>
    public enum DiseaseType
    {
        Cold,           // Soğuk algınlığı
        Flu,            // Grip
        Depression,     // Depresyon
        Anxiety,        // Anksiyete
        HeartDisease,   // Kalp hastalığı
        Cancer,         // Kanser
        Diabetes,       // Şeker hastalığı
        Hypertension,   // Yüksek tansiyon
        Obesity,        // Obezite
        Addiction       // Bağımlılık
    }

    /// <summary>
    /// Aktivite tipleri
    /// </summary>
    public enum ActivityType
    {
        Exercise,       // Egzersiz
        ReadBook,       // Kitap okuma
        WatchTV,        // TV izleme
        SocialMedia,    // Sosyal medya
        Meditation,     // Meditasyon
        Hobby,          // Hobi
        Travel,         // Seyahat
        Party,          // Parti
        Shopping,       // Alışveriş
        Gambling,       // Kumar
        Volunteer       // Gönüllülük
    }

    /// <summary>
    /// Evcil hayvan tipleri
    /// </summary>
    public enum PetType
    {
        Dog,            // Köpek
        Cat,            // Kedi
        Bird,           // Kuş
        Fish,           // Balık
        Hamster,        // Hamster
        Rabbit,         // Tavşan
        Turtle,         // Kaplumbağa
        Parrot          // Papağan
    }

    /// <summary>
    /// Evcil hayvan kategorileri
    /// </summary>
    public enum PetCategory
    {
        Common,         // Yaygın
        Exotic,         // Egzotik
        Aquatic         // Su hayvanı
    }

    /// <summary>
    /// Vasiyetname dağıtım tipleri
    /// </summary>
    public enum WillDistribution
    {
        Equal,          // Eşit dağıtım
        Custom,         // Özel dağıtım
        FirstChild,     // İlk çocuğa
        Spouse,         // Eşe
        Charity         // Hayır kurumuna
    }
}
