using System;
using System.Collections.Generic;
using BitLifeTR.Core;

namespace BitLifeTR.Data
{
    /// <summary>
    /// Contains all data for a character/player.
    /// This is the main save data structure.
    /// </summary>
    [Serializable]
    public class CharacterData
    {
        #region Identity

        public string Id;
        public string FirstName;
        public string LastName;
        public Gender Gender;
        public string BirthCity;
        public int BirthYear;

        #endregion

        #region Stats

        public float Health;
        public float Happiness;
        public float Intelligence;
        public float Looks;
        public decimal Money;
        public float Fame;
        public float Karma;

        #endregion

        #region Life State

        public int Age;
        public LifeStage LifeStage;
        public bool IsAlive;
        public string DeathCause;
        public int DeathAge;

        #endregion

        #region Education

        public EducationLevel EducationLevel;
        public string CurrentSchool;
        public float SchoolPerformance;
        public bool HasDiploma;
        public bool HasUniversityDegree;
        public string UniversityMajor;

        #endregion

        #region Career

        public string CurrentJob;
        public string CurrentCompany;
        public float JobPerformance;
        public int YearsAtJob;
        public decimal Salary;
        public bool IsEmployed;
        public bool IsRetired;

        #endregion

        #region Relationships

        public List<RelationshipData> Relationships;
        public string SpouseId;
        public bool IsMarried;
        public int ChildrenCount;

        #endregion

        #region Assets

        public List<AssetData> Assets;
        public decimal BankBalance;
        public decimal Debt;

        #endregion

        #region History

        public List<LifeEventRecord> LifeHistory;
        public List<string> Achievements;
        public int CrimeCount;
        public int JailTime;
        public bool HasCriminalRecord;

        #endregion

        #region Military

        public bool CompletedMilitaryService;
        public int MilitaryServiceYear;

        #endregion

        /// <summary>
        /// Create a new character with default values.
        /// </summary>
        public CharacterData()
        {
            Id = Guid.NewGuid().ToString();
            Relationships = new List<RelationshipData>();
            Assets = new List<AssetData>();
            LifeHistory = new List<LifeEventRecord>();
            Achievements = new List<string>();
            IsAlive = true;
            Age = 0;
            LifeStage = LifeStage.Bebek;
            BirthYear = DateTime.Now.Year;
        }

        /// <summary>
        /// Get full name.
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Get current year in game.
        /// </summary>
        public int CurrentYear => BirthYear + Age;

        /// <summary>
        /// Get net worth.
        /// </summary>
        public decimal NetWorth => BankBalance + GetAssetsValue() - Debt;

        /// <summary>
        /// Calculate total value of assets.
        /// </summary>
        public decimal GetAssetsValue()
        {
            decimal total = 0;
            foreach (var asset in Assets)
            {
                total += asset.CurrentValue;
            }
            return total;
        }

        /// <summary>
        /// Update life stage based on age.
        /// </summary>
        public void UpdateLifeStage()
        {
            LifeStage = Age switch
            {
                <= Constants.AGE_BABY_MAX => LifeStage.Bebek,
                <= Constants.AGE_CHILD_MAX => LifeStage.Cocuk,
                <= Constants.AGE_TEEN_MAX => LifeStage.Ergen,
                <= Constants.AGE_YOUNG_ADULT_MAX => LifeStage.GencYetiskin,
                <= Constants.AGE_ADULT_MAX => LifeStage.Yetiskin,
                <= Constants.AGE_MIDDLE_AGE_MAX => LifeStage.OrtaYas,
                _ => LifeStage.Yasli
            };
        }

        /// <summary>
        /// Clone this character data.
        /// </summary>
        public CharacterData Clone()
        {
            var json = UnityEngine.JsonUtility.ToJson(this);
            return UnityEngine.JsonUtility.FromJson<CharacterData>(json);
        }
    }

    #region Supporting Data Structures

    /// <summary>
    /// Education levels.
    /// </summary>
    public enum EducationLevel
    {
        Yok,
        Ilkokul,
        Ortaokul,
        Lise,
        Universite,
        YuksekLisans,
        Doktora
    }

    /// <summary>
    /// Relationship data.
    /// </summary>
    [Serializable]
    public class RelationshipData
    {
        public string Id;
        public string Name;
        public RelationType Type;
        public int RelationshipLevel; // 0-100
        public int Age;
        public Gender Gender;
        public bool IsAlive;
        public int YearsKnown;

        public RelationshipData()
        {
            Id = Guid.NewGuid().ToString();
            RelationshipLevel = 50;
            IsAlive = true;
        }
    }

    /// <summary>
    /// Types of relationships.
    /// </summary>
    public enum RelationType
    {
        Anne,
        Baba,
        Kardes,
        EsKardes,
        Cocuk,
        Es,
        Sevgili,
        Arkadas,
        Dusman,
        IsBasi,
        Komsu
    }

    /// <summary>
    /// Asset/property data.
    /// </summary>
    [Serializable]
    public class AssetData
    {
        public string Id;
        public string Name;
        public AssetType Type;
        public decimal PurchasePrice;
        public decimal CurrentValue;
        public int PurchaseYear;
        public int Condition; // 0-100

        public AssetData()
        {
            Id = Guid.NewGuid().ToString();
            Condition = 100;
        }
    }

    /// <summary>
    /// Types of assets.
    /// </summary>
    public enum AssetType
    {
        Ev,
        Araba,
        Motosiklet,
        Tekne,
        Ucak,
        Gayrimenkul,
        Diger
    }

    /// <summary>
    /// Record of a life event.
    /// </summary>
    [Serializable]
    public class LifeEventRecord
    {
        public int Age;
        public int Year;
        public string EventId;
        public string Description;
        public string Category;

        public LifeEventRecord() { }

        public LifeEventRecord(int age, int year, string description, string category = "Genel")
        {
            Age = age;
            Year = year;
            Description = description;
            Category = category;
            EventId = Guid.NewGuid().ToString();
        }
    }

    #endregion
}
