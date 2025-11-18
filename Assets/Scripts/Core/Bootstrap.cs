using UnityEngine;

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
            var gameManager = root.AddComponent<GameManager>();

            // Note: Other managers will be added in subsequent phases
            // - UIManager (Phase 2)
            // - SaveManager (Phase 3)
            // - EventManager (Phase 4)
            // - etc.

            Debug.Log("[Bootstrap] Core managers initialized");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void PostInitialize()
        {
            Debug.Log("[Bootstrap] Post-initialization...");

            // Start the game at main menu
            if (GameManager.HasInstance)
            {
                GameManager.Instance.SetState(GameState.MainMenu);
            }

            Debug.Log("[Bootstrap] Game ready");
        }
    }
}
