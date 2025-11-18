using UnityEngine;
using System.Collections.Generic;
using BitLifeTR.Core;
using BitLifeTR.Data;
using BitLifeTR.Utils;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Manages careers, jobs, and employment.
    /// </summary>
    public static class CareerSystem
    {
        /// <summary>
        /// Get available jobs for the character.
        /// </summary>
        public static List<JobData> GetAvailableJobs(CharacterData character)
        {
            var allJobs = JobDatabase.GetAllJobs();
            var available = new List<JobData>();

            foreach (var job in allJobs)
            {
                if (MeetsRequirements(character, job))
                {
                    available.Add(job);
                }
            }

            return available;
        }

        /// <summary>
        /// Check if character meets job requirements.
        /// </summary>
        public static bool MeetsRequirements(CharacterData character, JobData job)
        {
            // Education requirement
            if (character.EducationLevel < job.RequiredEducation)
                return false;

            // Intelligence requirement
            if (character.Intelligence < job.RequiredIntelligence)
                return false;

            // Looks requirement
            if (character.Looks < job.RequiredLooks)
                return false;

            // Age requirement (must be at least 16 for most jobs)
            if (character.Age < 16)
                return false;

            return true;
        }

        /// <summary>
        /// Apply for a job.
        /// </summary>
        public static (bool success, string message) ApplyForJob(CharacterData character, JobData job)
        {
            if (!MeetsRequirements(character, job))
            {
                return (false, "Bu iş için gerekli niteliklere sahip değilsin.");
            }

            // Calculate success chance based on stats
            float successChance = 0.3f; // Base 30%

            // Intelligence bonus
            successChance += (character.Intelligence / 100f) * 0.3f;

            // Looks bonus for some jobs
            if (job.RequiredLooks > 0)
            {
                successChance += (character.Looks / 100f) * 0.2f;
            }

            // Education bonus
            int educationDiff = (int)character.EducationLevel - (int)job.RequiredEducation;
            successChance += educationDiff * 0.1f;

            // Criminal record penalty
            if (character.HasCriminalRecord)
            {
                successChance -= 0.3f;
            }

            successChance = Mathf.Clamp(successChance, 0.1f, 0.9f);

            if (RandomHelper.Chance(successChance))
            {
                // Got the job!
                character.IsEmployed = true;
                character.CurrentJob = job.Name;
                character.CurrentCompany = GenerateCompanyName(job.Category);
                character.Salary = job.GetRandomSalary();
                character.YearsAtJob = 0;
                character.JobPerformance = 50f;

                return (true, $"{character.CurrentCompany}'de {job.Name} olarak işe başladın! Maaşın: {character.Salary:N0} TL");
            }
            else
            {
                return (false, "İş başvurun reddedildi.");
            }
        }

        /// <summary>
        /// Quit current job.
        /// </summary>
        public static void QuitJob(CharacterData character)
        {
            character.IsEmployed = false;
            character.CurrentJob = null;
            character.CurrentCompany = null;
            character.Salary = 0;
            character.YearsAtJob = 0;
        }

        /// <summary>
        /// Process yearly work.
        /// </summary>
        public static (decimal salary, string message) ProcessYearlyWork(CharacterData character)
        {
            if (!character.IsEmployed)
                return (0, null);

            character.YearsAtJob++;

            // Add yearly salary
            decimal yearlySalary = character.Salary * 12;

            // Random work events
            string message = null;

            // Chance of promotion
            if (character.YearsAtJob >= 2 && RandomHelper.Chance(0.15f * character.JobPerformance / 50f))
            {
                character.Salary *= 1.2m;
                message = $"Terfi aldın! Yeni maaşın: {character.Salary:N0} TL";
            }
            // Chance of raise
            else if (RandomHelper.Chance(0.2f))
            {
                character.Salary *= 1.05m;
                message = $"Zam aldın! Yeni maaşın: {character.Salary:N0} TL";
            }
            // Chance of getting fired
            else if (character.JobPerformance < 30 && RandomHelper.Chance(0.3f))
            {
                QuitJob(character);
                message = "Düşük performans nedeniyle işten çıkarıldın!";
                yearlySalary *= 0.5m; // Half year salary
            }

            return (yearlySalary, message);
        }

        /// <summary>
        /// Work harder to improve performance.
        /// </summary>
        public static void WorkHarder(CharacterData character)
        {
            character.JobPerformance = Mathf.Min(100, character.JobPerformance + RandomHelper.Range(5f, 15f));
            CharacterManager.Instance.Stats.ModifyHappiness(-5);
            CharacterManager.Instance.Stats.ModifyHealth(-2);
        }

        /// <summary>
        /// Slack off at work.
        /// </summary>
        public static void SlackOff(CharacterData character)
        {
            character.JobPerformance = Mathf.Max(0, character.JobPerformance - RandomHelper.Range(5f, 10f));
            CharacterManager.Instance.Stats.ModifyHappiness(3);
        }

        private static string GenerateCompanyName(string category)
        {
            string[] prefixes = { "Türk", "Anadolu", "İstanbul", "Ankara", "Global", "Mega", "Super", "Pro" };
            string[] suffixes = { "A.Ş.", "Ltd.", "Holding", "Grup", "& Co." };

            string prefix = prefixes[Random.Range(0, prefixes.Length)];
            string suffix = suffixes[Random.Range(0, suffixes.Length)];

            return $"{prefix} {category} {suffix}";
        }
    }
}
