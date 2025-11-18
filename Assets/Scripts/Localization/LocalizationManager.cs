using UnityEngine;
using System.Collections.Generic;

namespace BitLifeTR.Localization
{
    /// <summary>
    /// Türkçe yerelleştirme yöneticisi
    /// </summary>
    public static class LocalizationManager
    {
        private static Dictionary<string, string> localizedTexts;
        private static bool isInitialized = false;

        public static void Initialize()
        {
            if (isInitialized) return;

            localizedTexts = new Dictionary<string, string>
            {
                // Genel
                { "app_name", "BitLife Türkiye" },
                { "new_game", "Yeni Hayat" },
                { "continue", "Devam Et" },
                { "settings", "Ayarlar" },
                { "exit", "Çıkış" },
                { "back", "Geri" },
                { "confirm", "Tamam" },
                { "cancel", "İptal" },
                { "save", "Kaydet" },
                { "load", "Yükle" },

                // Statlar
                { "stat_health", "Sağlık" },
                { "stat_happiness", "Mutluluk" },
                { "stat_intelligence", "Zeka" },
                { "stat_appearance", "Görünüm" },
                { "stat_money", "Para" },
                { "stat_fame", "Şöhret" },
                { "stat_karma", "Karma" },

                // Yaş dönemleri
                { "age_infancy", "Bebeklik" },
                { "age_childhood", "Çocukluk" },
                { "age_adolescence", "Ergenlik" },
                { "age_young_adult", "Genç Yetişkinlik" },
                { "age_adult", "Yetişkinlik" },
                { "age_middle_age", "Orta Yaş" },
                { "age_elder", "Yaşlılık" },

                // Ekranlar
                { "screen_main_menu", "Ana Menü" },
                { "screen_game", "Oyun" },
                { "screen_stats", "İstatistikler" },
                { "screen_relationships", "İlişkiler" },
                { "screen_career", "Kariyer" },
                { "screen_education", "Eğitim" },
                { "screen_activities", "Aktiviteler" },
                { "screen_assets", "Varlıklar" },

                // İlişki tipleri
                { "rel_parent", "Ebeveyn" },
                { "rel_sibling", "Kardeş" },
                { "rel_child", "Çocuk" },
                { "rel_spouse", "Eş" },
                { "rel_friend", "Arkadaş" },
                { "rel_romantic", "Sevgili" },
                { "rel_colleague", "İş Arkadaşı" },
                { "rel_neighbor", "Komşu" },

                // Eğitim seviyeleri
                { "edu_none", "Eğitimsiz" },
                { "edu_primary", "İlkokul" },
                { "edu_middle", "Ortaokul" },
                { "edu_high", "Lise" },
                { "edu_university", "Üniversite" },
                { "edu_masters", "Yüksek Lisans" },
                { "edu_doctorate", "Doktora" },

                // Aktiviteler
                { "act_exercise", "Egzersiz" },
                { "act_read", "Kitap Oku" },
                { "act_meditate", "Meditasyon" },
                { "act_party", "Parti" },
                { "act_travel", "Seyahat" },
                { "act_shopping", "Alışveriş" },
                { "act_gamble", "Kumar" },
                { "act_volunteer", "Gönüllülük" },

                // Mesajlar
                { "msg_game_saved", "Oyun kaydedildi!" },
                { "msg_game_loaded", "Oyun yüklendi!" },
                { "msg_no_save", "Kayıtlı oyun bulunamadı" },
                { "msg_not_enough_money", "Yeterli paran yok!" },
                { "msg_enter_name", "Lütfen bir isim girin" },

                // Türkiye'ye özgü
                { "military_service", "Askerlik" },
                { "military_paid", "Bedelli Askerlik" },
                { "yks_exam", "YKS Sınavı" },
                { "bayram", "Bayram" },
                { "wedding", "Düğün" },
                { "engagement", "Nişan" },

                // Para birimi
                { "currency", "₺" },
                { "currency_name", "Türk Lirası" },

                // Şehirler
                { "city_istanbul", "İstanbul" },
                { "city_ankara", "Ankara" },
                { "city_izmir", "İzmir" },
                { "city_bursa", "Bursa" },
                { "city_antalya", "Antalya" },

                // Ölüm nedenleri
                { "death_natural", "Doğal nedenler" },
                { "death_health", "Sağlık sorunları" },
                { "death_accident", "Kaza" },
                { "death_disease", "Hastalık" },
                { "death_murder", "Cinayet" }
            };

            isInitialized = true;
            Debug.Log("Localization sistemi başlatıldı");
        }

        public static string Get(string key)
        {
            if (!isInitialized)
                Initialize();

            if (localizedTexts.TryGetValue(key, out string value))
                return value;

            Debug.LogWarning($"Localization key bulunamadı: {key}");
            return key;
        }

        public static string Get(string key, params object[] args)
        {
            string text = Get(key);
            return string.Format(text, args);
        }

        public static bool HasKey(string key)
        {
            if (!isInitialized)
                Initialize();

            return localizedTexts.ContainsKey(key);
        }
    }
}
