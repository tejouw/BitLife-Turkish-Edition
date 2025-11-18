using System;
using BitLifeTR.Core;

namespace BitLifeTR.Data
{
    /// <summary>
    /// A condition that must be met for an event to trigger.
    /// </summary>
    [Serializable]
    public class EventCondition
    {
        public ConditionType Type;
        public string Parameter;
        public ComparisonOperator Operator;
        public float NumericValue;
        public string StringValue;
        public bool BoolValue;

        /// <summary>
        /// Create an age condition.
        /// </summary>
        public static EventCondition Age(ComparisonOperator op, int age)
        {
            return new EventCondition
            {
                Type = ConditionType.Age,
                Operator = op,
                NumericValue = age
            };
        }

        /// <summary>
        /// Create a stat condition.
        /// </summary>
        public static EventCondition Stat(string statName, ComparisonOperator op, float value)
        {
            return new EventCondition
            {
                Type = ConditionType.Stat,
                Parameter = statName,
                Operator = op,
                NumericValue = value
            };
        }

        /// <summary>
        /// Create a money condition.
        /// </summary>
        public static EventCondition Money(ComparisonOperator op, decimal amount)
        {
            return new EventCondition
            {
                Type = ConditionType.Money,
                Operator = op,
                NumericValue = (float)amount
            };
        }

        /// <summary>
        /// Create an education condition.
        /// </summary>
        public static EventCondition Education(ComparisonOperator op, EducationLevel level)
        {
            return new EventCondition
            {
                Type = ConditionType.Education,
                Operator = op,
                NumericValue = (int)level
            };
        }

        /// <summary>
        /// Create a job condition.
        /// </summary>
        public static EventCondition HasJob(bool hasJob)
        {
            return new EventCondition
            {
                Type = ConditionType.HasJob,
                BoolValue = hasJob
            };
        }

        /// <summary>
        /// Create a marriage condition.
        /// </summary>
        public static EventCondition IsMarried(bool isMarried)
        {
            return new EventCondition
            {
                Type = ConditionType.IsMarried,
                BoolValue = isMarried
            };
        }

        /// <summary>
        /// Create a gender condition.
        /// </summary>
        public static EventCondition Gender(Core.Gender gender)
        {
            return new EventCondition
            {
                Type = ConditionType.Gender,
                NumericValue = (int)gender
            };
        }

        /// <summary>
        /// Create a life stage condition.
        /// </summary>
        public static EventCondition LifeStage(Core.LifeStage stage)
        {
            return new EventCondition
            {
                Type = ConditionType.LifeStage,
                NumericValue = (int)stage
            };
        }

        /// <summary>
        /// Create a has children condition.
        /// </summary>
        public static EventCondition HasChildren(bool hasChildren)
        {
            return new EventCondition
            {
                Type = ConditionType.HasChildren,
                BoolValue = hasChildren
            };
        }

        /// <summary>
        /// Create a criminal record condition.
        /// </summary>
        public static EventCondition HasCriminalRecord(bool has)
        {
            return new EventCondition
            {
                Type = ConditionType.HasCriminalRecord,
                BoolValue = has
            };
        }

        /// <summary>
        /// Create a military service condition.
        /// </summary>
        public static EventCondition CompletedMilitary(bool completed)
        {
            return new EventCondition
            {
                Type = ConditionType.CompletedMilitary,
                BoolValue = completed
            };
        }

        /// <summary>
        /// Create a relationship exists condition.
        /// </summary>
        public static EventCondition HasRelationship(RelationType type)
        {
            return new EventCondition
            {
                Type = ConditionType.HasRelationship,
                NumericValue = (int)type
            };
        }

        /// <summary>
        /// Create an asset ownership condition.
        /// </summary>
        public static EventCondition OwnsAsset(AssetType type)
        {
            return new EventCondition
            {
                Type = ConditionType.OwnsAsset,
                NumericValue = (int)type
            };
        }
    }

    /// <summary>
    /// Types of conditions.
    /// </summary>
    public enum ConditionType
    {
        Age,
        Stat,
        Money,
        Education,
        HasJob,
        JobType,
        IsMarried,
        Gender,
        LifeStage,
        HasChildren,
        HasCriminalRecord,
        CompletedMilitary,
        HasRelationship,
        RelationshipLevel,
        OwnsAsset,
        City,
        Fame,
        Custom
    }

    /// <summary>
    /// Comparison operators.
    /// </summary>
    public enum ComparisonOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterOrEqual,
        LessOrEqual
    }
}
