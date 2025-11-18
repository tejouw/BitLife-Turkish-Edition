using System.Collections.Generic;
using UnityEngine;

namespace BitLifeTR.Utils
{
    /// <summary>
    /// Helper class for random operations.
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// Returns true with given probability (0-1).
        /// </summary>
        public static bool Chance(float probability)
        {
            return Random.value < probability;
        }

        /// <summary>
        /// Returns true with given percentage (0-100).
        /// </summary>
        public static bool ChancePercent(float percent)
        {
            return Random.value * 100f < percent;
        }

        /// <summary>
        /// Get random int in range (inclusive).
        /// </summary>
        public static int Range(int min, int maxInclusive)
        {
            return Random.Range(min, maxInclusive + 1);
        }

        /// <summary>
        /// Get random float in range.
        /// </summary>
        public static float Range(float min, float max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        /// Get random element from array.
        /// </summary>
        public static T Pick<T>(params T[] items)
        {
            if (items == null || items.Length == 0)
                return default;

            return items[Random.Range(0, items.Length)];
        }

        /// <summary>
        /// Get random element based on weights.
        /// </summary>
        public static T WeightedPick<T>(IList<T> items, IList<float> weights)
        {
            if (items == null || items.Count == 0)
                return default;

            if (weights == null || weights.Count != items.Count)
            {
                Debug.LogError("[RandomHelper] Weights must match items count");
                return items[0];
            }

            float totalWeight = 0f;
            foreach (var weight in weights)
            {
                totalWeight += weight;
            }

            float randomValue = Random.value * totalWeight;
            float currentWeight = 0f;

            for (int i = 0; i < items.Count; i++)
            {
                currentWeight += weights[i];
                if (randomValue <= currentWeight)
                {
                    return items[i];
                }
            }

            return items[items.Count - 1];
        }

        /// <summary>
        /// Get random stat value (bell curve distribution).
        /// </summary>
        public static float RandomStat()
        {
            // Use normal distribution for more realistic stats
            float u1 = 1f - Random.value;
            float u2 = 1f - Random.value;
            float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);

            // Convert to 0-100 range with mean 50 and std dev 15
            float value = 50f + 15f * randStdNormal;
            return Mathf.Clamp(value, 0f, 100f);
        }

        /// <summary>
        /// Roll dice (e.g., 2d6 = Roll(2, 6)).
        /// </summary>
        public static int Roll(int diceCount, int sides)
        {
            int total = 0;
            for (int i = 0; i < diceCount; i++)
            {
                total += Random.Range(1, sides + 1);
            }
            return total;
        }

        /// <summary>
        /// Get a random Turkish male name.
        /// </summary>
        public static string RandomMaleName()
        {
            string[] names = {
                "Ahmet", "Mehmet", "Mustafa", "Ali", "Hüseyin", "Hasan", "İbrahim", "İsmail",
                "Yusuf", "Osman", "Murat", "Ömer", "Fatih", "Emre", "Burak", "Serkan",
                "Kemal", "Cem", "Onur", "Tolga", "Berk", "Kaan", "Efe", "Arda",
                "Yiğit", "Ege", "Deniz", "Barış", "Eren", "Can", "Tuna", "Alp"
            };
            return names[Random.Range(0, names.Length)];
        }

        /// <summary>
        /// Get a random Turkish female name.
        /// </summary>
        public static string RandomFemaleName()
        {
            string[] names = {
                "Fatma", "Ayşe", "Emine", "Hatice", "Zeynep", "Elif", "Meryem", "Şerife",
                "Zehra", "Sultan", "Hanife", "Merve", "Büşra", "Esra", "Seda", "Özlem",
                "Gül", "Derya", "Ebru", "Sibel", "Gamze", "Ceren", "Melis", "Buse",
                "İrem", "Defne", "Ela", "Ezgi", "Cansu", "Pelin", "Aslı", "Başak"
            };
            return names[Random.Range(0, names.Length)];
        }

        /// <summary>
        /// Get a random Turkish surname.
        /// </summary>
        public static string RandomSurname()
        {
            string[] surnames = {
                "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Yıldız", "Yıldırım", "Öztürk",
                "Aydın", "Özdemir", "Arslan", "Doğan", "Kılıç", "Aslan", "Çetin", "Kara",
                "Koç", "Kurt", "Özkan", "Şimşek", "Polat", "Korkmaz", "Özcan", "Erdoğan",
                "Güneş", "Ak", "Bulut", "Aktaş", "Karaca", "Taş", "Aksoy", "Güler"
            };
            return surnames[Random.Range(0, surnames.Length)];
        }

        /// <summary>
        /// Get a random Turkish city.
        /// </summary>
        public static string RandomCity()
        {
            string[] cities = {
                "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya", "Adana", "Konya", "Gaziantep",
                "Mersin", "Diyarbakır", "Kayseri", "Eskişehir", "Samsun", "Denizli", "Şanlıurfa",
                "Malatya", "Trabzon", "Erzurum", "Van", "Batman", "Elazığ", "Manisa", "Balıkesir",
                "Kocaeli", "Sakarya", "Tekirdağ", "Aydın", "Muğla", "Hatay", "Kahramanmaraş"
            };
            return cities[Random.Range(0, cities.Length)];
        }
    }
}
