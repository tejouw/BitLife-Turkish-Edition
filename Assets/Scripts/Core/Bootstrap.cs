using UnityEngine;
using BitLifeTR.UI;
using BitLifeTR.Systems;
using BitLifeTR.Data;

namespace BitLifeTR.Core
{
    /// <summary>
    /// Bootstrap class that initializes all game systems.
    /// This is the entry point of the game.
    /// </summary>
    public static class Bootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Debug.Log("[Bootstrap] Initializing BitLife Turkish Edition...");

            // Create persistent game object for all managers
            var gameRoot = new GameObject("[GameRoot]");
            Object.DontDestroyOnLoad(gameRoot);

            // Initialize core managers
            InitializeManagers(gameRoot);

            Debug.Log("[Bootstrap] Initialization complete");
        }

        private static void InitializeManagers(GameObject root)
        {
            // Core managers - order matters!

            // 1. Game Manager - main game control
            root.AddComponent<GameManager>();

            // 2. UI Manager - handles all UI
            root.AddComponent<UIManager>();

            // 3. Save Manager - handles save/load
            root.AddComponent<SaveManager>();

            // 4. Character Manager - handles character state
            root.AddComponent<CharacterManager>();

            // 5. Event Manager - handles game events
            root.AddComponent<EventManager>();

            // 6. Game Loop - handles year progression
            root.AddComponent<GameLoop>();

            // All core managers initialized

            Debug.Log("[Bootstrap] Core managers initialized");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void PostInitialize()
        {
            Debug.Log("[Bootstrap] Post-initialization...");

            // Load all game events
            LoadGameContent();

            // Start the game at main menu
            if (GameManager.HasInstance)
            {
                GameManager.Instance.SetState(GameState.MainMenu);
            }

            Debug.Log("[Bootstrap] Game ready");
        }

        private static void LoadGameContent()
        {
            // Register all events from database
            var events = EventDatabase.GetAllEvents();
            EventManager.Instance.RegisterEvents(events);

            Debug.Log($"[Bootstrap] Loaded {events.Count} events");
        }
    }
}
