using UnityEngine;
using BitLifeTR.Core;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Manages education, schools, and exams.
    /// </summary>
    public static class EducationSystem
    {
        /// <summary>
        /// Get school name for education level.
        /// </summary>
        public static string GetSchoolName(EducationLevel level, string city)
        {
            return level switch
            {
                EducationLevel.Ilkokul => $"{city} İlkokulu",
                EducationLevel.Ortaokul => $"{city} Ortaokulu",
                EducationLevel.Lise => GetRandomHighSchool(city),
                EducationLevel.Universite => GetRandomUniversity(),
                EducationLevel.YuksekLisans => GetRandomUniversity() + " Yüksek Lisans",
                EducationLevel.Doktora => GetRandomUniversity() + " Doktora",
                _ => null
            };
        }

        private static string GetRandomHighSchool(string city)
        {
            string[] types = { "Anadolu Lisesi", "Fen Lisesi", "İmam Hatip Lisesi", "Meslek Lisesi" };
            return $"{city} {types[Random.Range(0, types.Length)]}";
        }

        private static string GetRandomUniversity()
        {
            string[] universities = {
                "İstanbul Üniversitesi",
                "Ankara Üniversitesi",
                "Boğaziçi Üniversitesi",
                "ODTÜ",
                "İTÜ",
                "Hacettepe Üniversitesi",
                "Ege Üniversitesi",
                "Dokuz Eylül Üniversitesi",
                "Marmara Üniversitesi",
                "Gazi Üniversitesi",
                "Koç Üniversitesi",
                "Sabancı Üniversitesi",
                "Bilkent Üniversitesi"
            };
            return universities[Random.Range(0, universities.Length)];
        }

        /// <summary>
        /// Start education at a level.
        /// </summary>
        public static void StartEducation(CharacterData character, EducationLevel level)
        {
            character.CurrentSchool = GetSchoolName(level, character.BirthCity);
            character.SchoolPerformance = 50f;
        }

        /// <summary>
        /// Process yearly school performance.
        /// </summary>
        public static (bool passed, string message) ProcessSchoolYear(CharacterData character)
        {
            if (string.IsNullOrEmpty(character.CurrentSchool))
                return (true, null);

            // Calculate grade based on intelligence and effort
            float grade = character.Intelligence * 0.7f + character.SchoolPerformance * 0.3f;
            grade += RandomHelper.Range(-10f, 10f);

            bool passed = grade >= 50f;

            if (passed)
            {
                // Improve intelligence
                CharacterManager.Instance.Stats.ModifyIntelligence(RandomHelper.Range(1f, 3f));

                return (true, $"Okul yılını {grade:F0} ortalamayla geçtin.");
            }
            else
            {
                return (false, $"Okul yılını {grade:F0} ortalamayla geçemedin. Sınıfta kaldın!");
            }
        }

        /// <summary>
        /// Graduate from current school.
        /// </summary>
        public static void Graduate(CharacterData character)
        {
            // Determine what level they're graduating from
            switch (character.EducationLevel)
            {
                case EducationLevel.Yok:
                    character.EducationLevel = EducationLevel.Ilkokul;
                    break;
                case EducationLevel.Ilkokul:
                    character.EducationLevel = EducationLevel.Ortaokul;
                    break;
                case EducationLevel.Ortaokul:
                    character.EducationLevel = EducationLevel.Lise;
                    character.HasDiploma = true;
                    break;
                case EducationLevel.Lise:
                    character.EducationLevel = EducationLevel.Universite;
                    character.HasUniversityDegree = true;
                    break;
                case EducationLevel.Universite:
                    character.EducationLevel = EducationLevel.YuksekLisans;
                    break;
                case EducationLevel.YuksekLisans:
                    character.EducationLevel = EducationLevel.Doktora;
                    break;
            }

            character.CurrentSchool = null;
        }

        /// <summary>
        /// Take university entrance exam (YKS).
        /// </summary>
        public static (bool passed, float score, string message) TakeUniversityExam(CharacterData character)
        {
            // Score based on intelligence and random factor
            float score = character.Intelligence * 3f + RandomHelper.Range(0f, 200f);
            score = Mathf.Clamp(score, 0f, 500f);

            // Need at least 300 to pass
            bool passed = score >= 300f;

            string message;
            if (score >= 450f)
            {
                message = $"Harika bir sınav! {score:F0} puanla tıp fakültesi bile kazanabilirsin!";
            }
            else if (score >= 400f)
            {
                message = $"Çok iyi bir sınav! {score:F0} puanla popüler bölümleri kazanabilirsin.";
            }
            else if (passed)
            {
                message = $"{score:F0} puanla üniversiteyi kazandın, ama bölüm seçeneklerin sınırlı.";
            }
            else
            {
                message = $"{score:F0} puanla üniversiteyi kazanamadın. Belki gelecek yıl tekrar denersin.";
            }

            return (passed, score, message);
        }

        /// <summary>
        /// Study harder.
        /// </summary>
        public static void StudyHarder(CharacterData character)
        {
            character.SchoolPerformance = Mathf.Min(100, character.SchoolPerformance + RandomHelper.Range(5f, 15f));
            CharacterManager.Instance.Stats.ModifyIntelligence(RandomHelper.Range(1f, 3f));
            CharacterManager.Instance.Stats.ModifyHappiness(-5);
        }

        /// <summary>
        /// Skip school.
        /// </summary>
        public static void SkipSchool(CharacterData character)
        {
            character.SchoolPerformance = Mathf.Max(0, character.SchoolPerformance - RandomHelper.Range(5f, 10f));
            CharacterManager.Instance.Stats.ModifyHappiness(5);
        }

        /// <summary>
        /// Drop out of school.
        /// </summary>
        public static void DropOut(CharacterData character)
        {
            character.CurrentSchool = null;
            character.SchoolPerformance = 0;
        }

        /// <summary>
        /// Get available university majors.
        /// </summary>
        public static string[] GetUniversityMajors()
        {
            return new string[]
            {
                "Tıp", "Hukuk", "Mühendislik", "İşletme", "Ekonomi",
                "Bilgisayar Mühendisliği", "Elektrik-Elektronik", "Makine Mühendisliği",
                "Mimarlık", "Psikoloji", "Sosyoloji", "Tarih", "Edebiyat",
                "Matematik", "Fizik", "Kimya", "Biyoloji",
                "İletişim", "Siyaset Bilimi", "Uluslararası İlişkiler"
            };
        }
    }
}
