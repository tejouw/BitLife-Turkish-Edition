using UnityEngine;

namespace BitLifeTR.Data
{
    /// <summary>
    /// Game balance configuration values.
    /// Tweak these to adjust game difficulty and progression.
    /// </summary>
    public static class BalanceConfig
    {
        #region Stat Changes

        // Yearly stat decay
        public const float HealthDecayBase = 1f;
        public const float HealthDecayAgeMultiplier = 0.05f;
        public const float LooksDecayAgeStart = 30;
        public const float LooksDecayRate = 0.5f;
        public const float HappinessFluctuationMin = -3f;
        public const float HappinessFluctuationMax = 3f;
        public const float FameDecayRate = 1f;
        public const float RelationshipDecayRate = 2f;

        // Activity effects
        public const float GymHealthMin = 1f;
        public const float GymHealthMax = 5f;
        public const float GymLooksMin = 0.5f;
        public const float GymLooksMax = 2f;
        public const float LibraryIntelligenceMin = 1f;
        public const float LibraryIntelligenceMax = 5f;
        public const float MeditationHappinessMin = 3f;
        public const float MeditationHappinessMax = 8f;

        #endregion

        #region Death Chances

        public const float DeathChanceBase = 0.001f;
        public const float DeathChanceAgeStart = 60;
        public const float DeathChanceAgeMultiplier = 0.0001f;
        public const float DeathChanceLowHealth = 0.05f;
        public const float DeathChanceVeryLowHealth = 0.15f;
        public const float DeathChanceOld100 = 0.3f;
        public const float DeathChanceOld110 = 0.5f;

        #endregion

        #region Event Probabilities

        public const float EventCatchChanceBase = 0.4f;
        public const float EventSuccessChanceBase = 0.5f;
        public const float PromotionChanceBase = 0.15f;
        public const float RaiseChanceBase = 0.2f;
        public const float FireChanceBase = 0.3f;
        public const float LotteryWinChanceBig = 0.001f;
        public const float LotteryWinChanceSmall = 0.05f;

        #endregion

        #region Money Values

        public const decimal StartingMoney = 0;
        public const decimal DoctorCost = 500;
        public const decimal TherapyCost = 800;
        public const decimal PlasticSurgeryCost = 15000;
        public const decimal VacationCost = 5000;
        public const decimal LawyerCost = 10000;
        public const decimal DrivingLicenseCost = 1500;
        public const decimal PromBalCost = 500;

        #endregion

        #region Education

        public const float StudyIntelligenceMin = 1f;
        public const float StudyIntelligenceMax = 3f;
        public const float ExamScoreIntelligenceMultiplier = 3f;
        public const float ExamScoreRandomMax = 200f;
        public const float ExamPassThreshold = 300f;
        public const float ExamExcellentThreshold = 450f;
        public const float ExamGoodThreshold = 400f;

        #endregion

        #region Relationships

        public const int InitialParentRelationMin = 60;
        public const int InitialParentRelationMax = 100;
        public const int InitialSiblingRelationMin = 50;
        public const int InitialSiblingRelationMax = 90;
        public const float RelationshipDeathChanceStart = 85;
        public const float RelationshipDeathChanceMultiplier = 0.1f;

        #endregion

        #region Career

        public const float JobApplicationBaseChance = 0.3f;
        public const float JobIntelligenceBonus = 0.3f;
        public const float JobLooksBonus = 0.2f;
        public const float JobEducationBonus = 0.1f;
        public const float JobCriminalRecordPenalty = 0.3f;
        public const float PromotionSalaryMultiplier = 1.2f;
        public const float RaiseSalaryMultiplier = 1.05f;

        #endregion

        #region Crime

        public const float PrisonEscapeBaseChance = 0.1f;
        public const float PrisonEscapeIntelligenceBonus = 0.005f;
        public const int PrisonEscapeFailPenalty = 5;
        public const float LawyerSuccessChance = 0.5f;

        #endregion

        #region Health

        public const float IllnessBaseChance = 0.05f;
        public const float IllnessChildBonus = 0.05f;
        public const float IllnessElderlyBonus = 0.05f;
        public const float IllnessVeryElderlyBonus = 0.1f;
        public const float TreatmentSuccessBaseChance = 1f;
        public const float PlasticSurgeryComplicationChance = 0.1f;
        public const float AccidentBaseChance = 0.001f;
        public const float AccidentElderlyBonus = 0.005f;

        #endregion

        #region Events Per Year

        public static int GetEventsPerYear(int age)
        {
            if (age < 5) return Random.Range(1, 3);
            if (age < 12) return Random.Range(2, 4);
            if (age < 65) return Random.Range(2, 5);
            return Random.Range(1, 4);
        }

        #endregion
    }
}
