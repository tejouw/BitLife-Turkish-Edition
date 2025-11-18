using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using BitLifeTR.Core;
using BitLifeTR.Data;

namespace BitLifeTR.Systems
{
    /// <summary>
    /// Handles saving and loading game data.
    /// </summary>
    public class SaveManager : Singleton<SaveManager>
    {
        private string saveDirectory;

        protected override void OnSingletonAwake()
        {
            saveDirectory = Path.Combine(Application.persistentDataPath, Constants.SAVE_FOLDER);

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            Debug.Log($"[SaveManager] Save directory: {saveDirectory}");
        }

        #region Save Operations

        /// <summary>
        /// Save character data to a slot.
        /// </summary>
        public bool SaveGame(CharacterData character, string slot = "autosave")
        {
            try
            {
                var saveData = new SaveData
                {
                    Character = character,
                    SaveTime = DateTime.Now,
                    GameVersion = Constants.GAME_VERSION
                };

                string json = JsonUtility.ToJson(saveData, true);
                string filePath = GetSaveFilePath(slot);

                File.WriteAllText(filePath, json);

                Debug.Log($"[SaveManager] Game saved to slot: {slot}");

                EventBus.Publish(new GameSavedEvent
                {
                    SaveSlot = slot,
                    SaveTime = saveData.SaveTime
                });

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to save game: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Quick save to autosave slot.
        /// </summary>
        public bool QuickSave(CharacterData character)
        {
            return SaveGame(character, Constants.DEFAULT_SAVE_SLOT);
        }

        #endregion

        #region Load Operations

        /// <summary>
        /// Load character data from a slot.
        /// </summary>
        public CharacterData LoadGame(string slot = "autosave")
        {
            try
            {
                string filePath = GetSaveFilePath(slot);

                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"[SaveManager] Save file not found: {slot}");
                    return null;
                }

                string json = File.ReadAllText(filePath);
                var saveData = JsonUtility.FromJson<SaveData>(json);

                Debug.Log($"[SaveManager] Game loaded from slot: {slot}");

                EventBus.Publish(new GameLoadedEvent { SaveSlot = slot });

                return saveData.Character;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to load game: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Quick load from autosave slot.
        /// </summary>
        public CharacterData QuickLoad()
        {
            return LoadGame(Constants.DEFAULT_SAVE_SLOT);
        }

        #endregion

        #region Save Slot Management

        /// <summary>
        /// Check if a save slot exists.
        /// </summary>
        public bool SaveExists(string slot)
        {
            return File.Exists(GetSaveFilePath(slot));
        }

        /// <summary>
        /// Delete a save slot.
        /// </summary>
        public bool DeleteSave(string slot)
        {
            try
            {
                string filePath = GetSaveFilePath(slot);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log($"[SaveManager] Deleted save: {slot}");
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to delete save: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get all available save slots.
        /// </summary>
        public List<SaveSlotInfo> GetAllSaves()
        {
            var saves = new List<SaveSlotInfo>();

            if (!Directory.Exists(saveDirectory))
                return saves;

            string[] files = Directory.GetFiles(saveDirectory, "*" + Constants.SAVE_FILE_EXTENSION);

            foreach (string file in files)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var saveData = JsonUtility.FromJson<SaveData>(json);

                    saves.Add(new SaveSlotInfo
                    {
                        SlotName = Path.GetFileNameWithoutExtension(file),
                        CharacterName = saveData.Character.FullName,
                        Age = saveData.Character.Age,
                        SaveTime = saveData.SaveTime,
                        IsAlive = saveData.Character.IsAlive
                    });
                }
                catch
                {
                    // Skip corrupted saves
                }
            }

            // Sort by save time (newest first)
            saves.Sort((a, b) => b.SaveTime.CompareTo(a.SaveTime));

            return saves;
        }

        /// <summary>
        /// Get info for a specific save slot.
        /// </summary>
        public SaveSlotInfo GetSaveInfo(string slot)
        {
            string filePath = GetSaveFilePath(slot);

            if (!File.Exists(filePath))
                return null;

            try
            {
                string json = File.ReadAllText(filePath);
                var saveData = JsonUtility.FromJson<SaveData>(json);

                return new SaveSlotInfo
                {
                    SlotName = slot,
                    CharacterName = saveData.Character.FullName,
                    Age = saveData.Character.Age,
                    SaveTime = saveData.SaveTime,
                    IsAlive = saveData.Character.IsAlive
                };
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Settings

        /// <summary>
        /// Save game settings.
        /// </summary>
        public void SaveSettings(GameSettings settings)
        {
            string json = JsonUtility.ToJson(settings);
            PlayerPrefs.SetString("GameSettings", json);
            PlayerPrefs.Save();

            Debug.Log("[SaveManager] Settings saved");
        }

        /// <summary>
        /// Load game settings.
        /// </summary>
        public GameSettings LoadSettings()
        {
            if (PlayerPrefs.HasKey("GameSettings"))
            {
                string json = PlayerPrefs.GetString("GameSettings");
                return JsonUtility.FromJson<GameSettings>(json);
            }

            return new GameSettings();
        }

        #endregion

        #region Utility

        private string GetSaveFilePath(string slot)
        {
            return Path.Combine(saveDirectory, slot + Constants.SAVE_FILE_EXTENSION);
        }

        /// <summary>
        /// Delete all saves.
        /// </summary>
        public void DeleteAllSaves()
        {
            if (Directory.Exists(saveDirectory))
            {
                Directory.Delete(saveDirectory, true);
                Directory.CreateDirectory(saveDirectory);
                Debug.Log("[SaveManager] All saves deleted");
            }
        }

        #endregion
    }

    #region Save Data Structures

    /// <summary>
    /// Complete save data structure.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public CharacterData Character;
        public DateTime SaveTime;
        public string GameVersion;
    }

    /// <summary>
    /// Information about a save slot.
    /// </summary>
    [Serializable]
    public class SaveSlotInfo
    {
        public string SlotName;
        public string CharacterName;
        public int Age;
        public DateTime SaveTime;
        public bool IsAlive;

        public string FormattedTime => SaveTime.ToString("dd/MM/yyyy HH:mm");
        public string Summary => IsAlive
            ? $"{CharacterName}, {Age} yaşında"
            : $"{CharacterName}, {Age} yaşında vefat etti";
    }

    /// <summary>
    /// Game settings.
    /// </summary>
    [Serializable]
    public class GameSettings
    {
        public float MusicVolume = 1f;
        public float SfxVolume = 1f;
        public bool Notifications = true;
        public bool AutoSave = true;
        public bool Vibration = true;
        public string Language = "tr";
    }

    #endregion
}
