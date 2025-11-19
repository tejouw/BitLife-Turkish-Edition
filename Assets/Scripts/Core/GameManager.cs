using UnityEngine;
using BitLifeTR.Systems;
using BitLifeTR.UI;
using BitLifeTR.Data;

namespace BitLifeTR.Core
{
    /// <summary>
    /// Ana oyun yöneticisi - Singleton pattern
    /// Tüm sistemleri koordine eder
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        // Alt sistemler
        public StatSystem StatSystem { get; private set; }
        public EventManager EventManager { get; private set; }
        public UIManager UIManager { get; private set; }
        public SaveManager SaveManager { get; private set; }
        public AgeSystem AgeSystem { get; private set; }
        public RelationshipSystem RelationshipSystem { get; private set; }
        public CareerSystem CareerSystem { get; private set; }
        public HealthSystem HealthSystem { get; private set; }
        public CrimeSystem CrimeSystem { get; private set; }
        public EconomySystem EconomySystem { get; private set; }
        public PetSystem PetSystem { get; private set; }

        // Oyun durumu
        public CharacterData CurrentCharacter { get; private set; }
        public bool IsGameRunning { get; private set; }
        public bool IsPaused { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSystems();
        }

        private void InitializeSystems()
        {
            // Core sistemleri oluştur
            StatSystem = gameObject.AddComponent<StatSystem>();
            EventManager = gameObject.AddComponent<EventManager>();
            UIManager = gameObject.AddComponent<UIManager>();
            SaveManager = gameObject.AddComponent<SaveManager>();
            AgeSystem = gameObject.AddComponent<AgeSystem>();

            // Gelişmiş sistemler
            RelationshipSystem = gameObject.AddComponent<RelationshipSystem>();
            CareerSystem = gameObject.AddComponent<CareerSystem>();
            HealthSystem = gameObject.AddComponent<HealthSystem>();
            CrimeSystem = gameObject.AddComponent<CrimeSystem>();
            EconomySystem = gameObject.AddComponent<EconomySystem>();
            PetSystem = gameObject.AddComponent<PetSystem>();

            Debug.Log("BitLife TR: Tüm sistemler başlatıldı");
        }

        public void StartNewGame(string characterName, Gender gender)
        {
            CurrentCharacter = new CharacterData(characterName, gender);
            IsGameRunning = true;
            IsPaused = false;

            EventBus.Publish(new GameStartedEvent(CurrentCharacter));
            UIManager.ShowScreen(ScreenType.Game);

            Debug.Log($"Yeni oyun başlatıldı: {characterName}");
        }

        public void LoadGame()
        {
            CharacterData loadedData = SaveManager.LoadGame();
            if (loadedData != null)
            {
                CurrentCharacter = loadedData;
                IsGameRunning = true;
                IsPaused = false;

                EventBus.Publish(new GameLoadedEvent(CurrentCharacter));
                UIManager.ShowScreen(ScreenType.Game);

                Debug.Log("Oyun yüklendi");
            }
            else
            {
                Debug.LogWarning("Kayıtlı oyun bulunamadı");
            }
        }

        public void SaveGame()
        {
            if (CurrentCharacter != null)
            {
                SaveManager.SaveGame(CurrentCharacter);
                Debug.Log("Oyun kaydedildi");
            }
        }

        public void PauseGame()
        {
            IsPaused = true;
            EventBus.Publish(new GamePausedEvent());
        }

        public void ResumeGame()
        {
            IsPaused = false;
            EventBus.Publish(new GameResumedEvent());
        }

        public void EndGame(DeathReason reason)
        {
            IsGameRunning = false;
            EventBus.Publish(new GameEndedEvent(CurrentCharacter, reason));
            UIManager.ShowScreen(ScreenType.GameOver);

            Debug.Log($"Oyun sona erdi: {reason}");
        }

        public void AdvanceYear()
        {
            if (!IsGameRunning || IsPaused || CurrentCharacter == null)
                return;

            // Yaşı ilerlet
            CurrentCharacter.Age++;
            AgeSystem.ProcessAgeChange(CurrentCharacter);

            // Yıllık olayları işle
            EventManager.ProcessYearlyEvents(CurrentCharacter);

            // Statları güncelle
            StatSystem.ProcessYearlyStatChanges(CurrentCharacter);

            // Sağlık kontrolü
            HealthSystem.ProcessYearlyHealth(CurrentCharacter);

            // Ekonomi güncelle
            EconomySystem.ProcessYearlyEconomy(CurrentCharacter);

            // Evcil hayvanları güncelle
            PetSystem.ProcessYearlyPets(CurrentCharacter);

            // Ölüm kontrolü
            if (CurrentCharacter.Stats.Health <= 0)
            {
                EndGame(DeathReason.HealthDepleted);
                return;
            }

            // Doğal ölüm şansı (yaşa bağlı)
            if (AgeSystem.CheckNaturalDeath(CurrentCharacter))
            {
                EndGame(DeathReason.NaturalCauses);
                return;
            }

            EventBus.Publish(new YearAdvancedEvent(CurrentCharacter));
            SaveGame(); // Otomatik kaydet
        }

        public void ReturnToMainMenu()
        {
            IsGameRunning = false;
            CurrentCharacter = null;
            UIManager.ShowScreen(ScreenType.MainMenu);
        }

        private void OnApplicationQuit()
        {
            if (IsGameRunning && CurrentCharacter != null)
            {
                SaveGame();
            }
        }
    }

    // Oyun olayları
    public class GameStartedEvent
    {
        public CharacterData Character { get; }
        public GameStartedEvent(CharacterData character) => Character = character;
    }

    public class GameLoadedEvent
    {
        public CharacterData Character { get; }
        public GameLoadedEvent(CharacterData character) => Character = character;
    }

    public class GamePausedEvent { }
    public class GameResumedEvent { }

    public class GameEndedEvent
    {
        public CharacterData Character { get; }
        public DeathReason Reason { get; }
        public GameEndedEvent(CharacterData character, DeathReason reason)
        {
            Character = character;
            Reason = reason;
        }
    }

    public class YearAdvancedEvent
    {
        public CharacterData Character { get; }
        public YearAdvancedEvent(CharacterData character) => Character = character;
    }
}
