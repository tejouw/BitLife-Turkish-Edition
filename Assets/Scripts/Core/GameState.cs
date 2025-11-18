namespace BitLifeTR.Core
{
    /// <summary>
    /// Represents the current state of the game.
    /// </summary>
    public enum GameState
    {
        /// <summary>Initial state when game launches</summary>
        Initializing,

        /// <summary>Main menu is displayed</summary>
        MainMenu,

        /// <summary>Character creation screen</summary>
        CharacterCreation,

        /// <summary>Game is actively being played</summary>
        Playing,

        /// <summary>Game is paused</summary>
        Paused,

        /// <summary>Character has died, showing summary</summary>
        GameOver,

        /// <summary>Loading a saved game</summary>
        Loading,

        /// <summary>Saving the game</summary>
        Saving
    }

    /// <summary>
    /// Life stages based on age.
    /// </summary>
    public enum LifeStage
    {
        /// <summary>0-4 years old</summary>
        Bebek,

        /// <summary>5-11 years old</summary>
        Cocuk,

        /// <summary>12-17 years old</summary>
        Ergen,

        /// <summary>18-29 years old</summary>
        GencYetiskin,

        /// <summary>30-49 years old</summary>
        Yetiskin,

        /// <summary>50-64 years old</summary>
        OrtaYas,

        /// <summary>65+ years old</summary>
        Yasli
    }

    /// <summary>
    /// Gender options for characters.
    /// </summary>
    public enum Gender
    {
        Erkek,
        Kadin
    }
}
