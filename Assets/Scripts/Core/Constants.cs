namespace BitLifeTR.Core
{
    /// <summary>
    /// Global constants used throughout the game.
    /// </summary>
    public static class Constants
    {
        #region Game Settings

        public const string GAME_NAME = "BitLife Türkiye";
        public const string GAME_VERSION = "0.1.0";
        public const int TARGET_FRAME_RATE = 60;

        #endregion

        #region Stat Limits

        public const float STAT_MIN = 0f;
        public const float STAT_MAX = 100f;
        public const float STAT_DEFAULT = 50f;

        #endregion

        #region Age Ranges

        public const int AGE_BABY_MIN = 0;
        public const int AGE_BABY_MAX = 4;
        public const int AGE_CHILD_MIN = 5;
        public const int AGE_CHILD_MAX = 11;
        public const int AGE_TEEN_MIN = 12;
        public const int AGE_TEEN_MAX = 17;
        public const int AGE_YOUNG_ADULT_MIN = 18;
        public const int AGE_YOUNG_ADULT_MAX = 29;
        public const int AGE_ADULT_MIN = 30;
        public const int AGE_ADULT_MAX = 49;
        public const int AGE_MIDDLE_AGE_MIN = 50;
        public const int AGE_MIDDLE_AGE_MAX = 64;
        public const int AGE_ELDERLY_MIN = 65;
        public const int AGE_MAX = 122; // Oldest recorded human

        #endregion

        #region Save System

        public const string SAVE_FILE_EXTENSION = ".json";
        public const string SAVE_FOLDER = "Saves";
        public const string DEFAULT_SAVE_SLOT = "autosave";
        public const int MAX_SAVE_SLOTS = 5;

        #endregion

        #region UI Settings

        public const float UI_ANIMATION_DURATION = 0.3f;
        public const float NOTIFICATION_DURATION = 3f;
        public const int UI_SORTING_ORDER_BASE = 100;

        #endregion

        #region Turkish Specific

        public const int ASKERLIK_AGE = 20;
        public const int EMEKLILIK_AGE_ERKEK = 65;
        public const int EMEKLILIK_AGE_KADIN = 60;
        public const int UNIVERSITE_SINAV_AGE = 18;
        public const int EHLIYET_AGE = 18;
        public const int EVLILIK_MIN_AGE = 18;

        #endregion

        #region Gameplay Balance

        public const float YEARLY_STAT_DECAY = 1f;
        public const float DEATH_CHANCE_BASE = 0.001f;
        public const float DEATH_CHANCE_AGE_MULTIPLIER = 0.0001f;
        public const decimal STARTING_MONEY = 0m;
        public const float RELATIONSHIP_DECAY_RATE = 2f;

        #endregion
    }
}
