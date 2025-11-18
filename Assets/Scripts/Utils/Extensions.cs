using UnityEngine;
using System.Collections.Generic;

namespace BitLifeTR.Utils
{
    /// <summary>
    /// Yardımcı extension metodları
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Listeden rastgele bir eleman seç
        /// </summary>
        public static T RandomElement<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Diziden rastgele bir eleman seç
        /// </summary>
        public static T RandomElement<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                return default;

            return array[Random.Range(0, array.Length)];
        }

        /// <summary>
        /// Listeyi karıştır (shuffle)
        /// </summary>
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Float değeri yüzde formatında göster
        /// </summary>
        public static string ToPercentString(this float value)
        {
            return $"%{value:F0}";
        }

        /// <summary>
        /// Float değeri para formatında göster
        /// </summary>
        public static string ToMoneyString(this float value)
        {
            if (value >= 1000000)
                return $"₺{value / 1000000:F1}M";
            if (value >= 1000)
                return $"₺{value / 1000:F1}K";

            return $"₺{value:N0}";
        }

        /// <summary>
        /// RectTransform'u tam ekran yap
        /// </summary>
        public static void StretchToParent(this RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// GameObject'i belirli bir süre sonra yok et
        /// </summary>
        public static void DestroyAfter(this GameObject obj, float seconds)
        {
            Object.Destroy(obj, seconds);
        }

        /// <summary>
        /// String'in boş veya null olup olmadığını kontrol et
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Değeri belirli aralıkta tut (Clamp)
        /// </summary>
        public static float Clamp01(this float value)
        {
            return Mathf.Clamp01(value);
        }

        /// <summary>
        /// İki değer arasında lerp
        /// </summary>
        public static float LerpTo(this float from, float to, float t)
        {
            return Mathf.Lerp(from, to, t);
        }
    }

    /// <summary>
    /// Genel yardımcı metodlar
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Yüzde şansı kontrol et
        /// </summary>
        public static bool Chance(float percentage)
        {
            return Random.value * 100f < percentage;
        }

        /// <summary>
        /// İki değer arasında rastgele int
        /// </summary>
        public static int RandomRange(int min, int max)
        {
            return Random.Range(min, max + 1);
        }

        /// <summary>
        /// Ağırlıklı rastgele seçim
        /// </summary>
        public static int WeightedRandom(float[] weights)
        {
            float total = 0;
            foreach (float w in weights)
                total += w;

            float random = Random.value * total;
            float cumulative = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                cumulative += weights[i];
                if (random <= cumulative)
                    return i;
            }

            return weights.Length - 1;
        }

        /// <summary>
        /// Yaşı yıl/ay formatında göster
        /// </summary>
        public static string FormatAge(int years)
        {
            if (years == 0)
                return "Yeni doğdu";
            if (years == 1)
                return "1 yaşında";

            return $"{years} yaşında";
        }

        /// <summary>
        /// Tarihi Türkçe formatla
        /// </summary>
        public static string FormatDate(System.DateTime date)
        {
            string[] months = {
                "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
                "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
            };

            return $"{date.Day} {months[date.Month - 1]} {date.Year}";
        }
    }
}
