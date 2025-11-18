using System;
using System.Collections.Generic;
using UnityEngine;

namespace BitLifeTR.Utils
{
    /// <summary>
    /// Extension methods for various types.
    /// </summary>
    public static class Extensions
    {
        #region Float Extensions

        /// <summary>
        /// Clamp a float between 0 and 100 (stat range).
        /// </summary>
        public static float ClampStat(this float value)
        {
            return Mathf.Clamp(value, 0f, 100f);
        }

        /// <summary>
        /// Clamp a float between min and max.
        /// </summary>
        public static float Clamp(this float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }

        /// <summary>
        /// Check if float is approximately equal to another.
        /// </summary>
        public static bool Approximately(this float value, float other)
        {
            return Mathf.Approximately(value, other);
        }

        #endregion

        #region Int Extensions

        /// <summary>
        /// Clamp an int between min and max.
        /// </summary>
        public static int Clamp(this int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }

        #endregion

        #region String Extensions

        /// <summary>
        /// Check if string is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Check if string is null or whitespace.
        /// </summary>
        public static bool IsNullOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Format money as Turkish Lira.
        /// </summary>
        public static string ToTurkishLira(this decimal value)
        {
            return $"{value:N0} TL";
        }

        /// <summary>
        /// Format money as Turkish Lira.
        /// </summary>
        public static string ToTurkishLira(this float value)
        {
            return $"{value:N0} TL";
        }

        #endregion

        #region List Extensions

        /// <summary>
        /// Get a random element from a list.
        /// </summary>
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Shuffle a list in place.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Check if list is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        #endregion

        #region Dictionary Extensions

        /// <summary>
        /// Get value or default from dictionary.
        /// </summary>
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default)
        {
            return dict.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        #endregion

        #region Transform Extensions

        /// <summary>
        /// Destroy all children of a transform.
        /// </summary>
        public static void DestroyAllChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Set active state of all children.
        /// </summary>
        public static void SetChildrenActive(this Transform transform, bool active)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(active);
            }
        }

        #endregion

        #region Color Extensions

        /// <summary>
        /// Return color with modified alpha.
        /// </summary>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        #endregion

        #region Enum Extensions

        /// <summary>
        /// Get Turkish display name for LifeStage.
        /// </summary>
        public static string ToTurkish(this BitLifeTR.Core.LifeStage stage)
        {
            return stage switch
            {
                BitLifeTR.Core.LifeStage.Bebek => "Bebek",
                BitLifeTR.Core.LifeStage.Cocuk => "Çocuk",
                BitLifeTR.Core.LifeStage.Ergen => "Ergen",
                BitLifeTR.Core.LifeStage.GencYetiskin => "Genç Yetişkin",
                BitLifeTR.Core.LifeStage.Yetiskin => "Yetişkin",
                BitLifeTR.Core.LifeStage.OrtaYas => "Orta Yaş",
                BitLifeTR.Core.LifeStage.Yasli => "Yaşlı",
                _ => stage.ToString()
            };
        }

        /// <summary>
        /// Get Turkish display name for Gender.
        /// </summary>
        public static string ToTurkish(this BitLifeTR.Core.Gender gender)
        {
            return gender switch
            {
                BitLifeTR.Core.Gender.Erkek => "Erkek",
                BitLifeTR.Core.Gender.Kadin => "Kadın",
                _ => gender.ToString()
            };
        }

        #endregion
    }
}
