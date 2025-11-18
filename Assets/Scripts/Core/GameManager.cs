using UnityEngine;

namespace BitLifeTR.Core
{
    /// <summary>
    /// Main game manager that controls game flow and state.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        private GameState _currentState = GameState.Initializing;

        public GameState CurrentState => _currentState;
        public bool IsPlaying => _currentState == GameState.Playing;
        public bool IsPaused => _currentState == GameState.Paused;

        protected override void OnSingletonAwake()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Debug.Log("[GameManager] Initialized");
        }

        /// <summary>
        /// Change the current game state.
        /// </summary>
        public void SetState(GameState newState)
        {
            if (_currentState == newState) return;

            var previousState = _currentState;
            _currentState = newState;

            Debug.Log($"[GameManager] State changed: {previousState} -> {newState}");

            EventBus.Publish(new GameStateChangedEvent
            {
                PreviousState = previousState,
                NewState = newState
            });

            OnStateChanged(previousState, newState);
        }

        private void OnStateChanged(GameState previousState, GameState newState)
        {
            switch (newState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    break;

                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;

                case GameState.GameOver:
                    Time.timeScale = 1f;
                    break;
            }
        }

        /// <summary>
        /// Start a new game.
        /// </summary>
        public void StartNewGame()
        {
            Debug.Log("[GameManager] Starting new game...");
            SetState(GameState.CharacterCreation);
        }

        /// <summary>
        /// Continue playing after character creation.
        /// </summary>
        public void BeginLife()
        {
            Debug.Log("[GameManager] Beginning life simulation...");
            SetState(GameState.Playing);
        }

        /// <summary>
        /// Load a saved game.
        /// </summary>
        public void LoadGame(string saveSlot = "default")
        {
            Debug.Log($"[GameManager] Loading game from slot: {saveSlot}");
            SetState(GameState.Loading);

            // SaveManager will handle the actual loading
            EventBus.Publish(new GameLoadedEvent { SaveSlot = saveSlot });
        }

        /// <summary>
        /// Save the current game.
        /// </summary>
        public void SaveGame(string saveSlot = "default")
        {
            if (_currentState != GameState.Playing)
            {
                Debug.LogWarning("[GameManager] Can only save while playing");
                return;
            }

            Debug.Log($"[GameManager] Saving game to slot: {saveSlot}");

            var previousState = _currentState;
            SetState(GameState.Saving);

            // SaveManager will handle the actual saving
            EventBus.Publish(new GameSavedEvent
            {
                SaveSlot = saveSlot,
                SaveTime = System.DateTime.Now
            });

            SetState(previousState);
        }

        /// <summary>
        /// Pause the game.
        /// </summary>
        public void PauseGame()
        {
            if (_currentState == GameState.Playing)
            {
                SetState(GameState.Paused);
            }
        }

        /// <summary>
        /// Resume the game from pause.
        /// </summary>
        public void ResumeGame()
        {
            if (_currentState == GameState.Paused)
            {
                SetState(GameState.Playing);
            }
        }

        /// <summary>
        /// End the current game (character died).
        /// </summary>
        public void EndGame(string causeOfDeath, int age)
        {
            Debug.Log($"[GameManager] Game over - Died at age {age}: {causeOfDeath}");

            EventBus.Publish(new CharacterDiedEvent
            {
                Age = age,
                CauseOfDeath = causeOfDeath
            });

            SetState(GameState.GameOver);
        }

        /// <summary>
        /// Return to main menu.
        /// </summary>
        public void ReturnToMainMenu()
        {
            Debug.Log("[GameManager] Returning to main menu");
            SetState(GameState.MainMenu);
        }

        /// <summary>
        /// Quit the application.
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("[GameManager] Quitting game...");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
