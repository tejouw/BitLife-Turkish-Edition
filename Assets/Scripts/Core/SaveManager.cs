using UnityEngine;
using System.IO;
using BitLifeTR.Data;

namespace BitLifeTR.Core
{
    /// <summary>
    /// Oyun kaydetme ve yükleme yöneticisi
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        private string SavePath => Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_NAME);

        public void SaveGame(CharacterData character)
        {
            try
            {
                SaveData saveData = new SaveData
                {
                    CharacterJson = JsonUtility.ToJson(character),
                    SaveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    GameVersion = Application.version
                };

                string json = JsonUtility.ToJson(saveData, true);
                File.WriteAllText(SavePath, json);

                Debug.Log($"Oyun kaydedildi: {SavePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Kaydetme hatası: {e.Message}");
            }
        }

        public CharacterData LoadGame()
        {
            try
            {
                if (!File.Exists(SavePath))
                {
                    Debug.LogWarning("Kayıt dosyası bulunamadı");
                    return null;
                }

                string json = File.ReadAllText(SavePath);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);
                CharacterData character = JsonUtility.FromJson<CharacterData>(saveData.CharacterJson);

                Debug.Log($"Oyun yüklendi: {saveData.SaveTime}");
                return character;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Yükleme hatası: {e.Message}");
                return null;
            }
        }

        public bool HasSaveFile()
        {
            return File.Exists(SavePath);
        }

        public void DeleteSave()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    File.Delete(SavePath);
                    Debug.Log("Kayıt dosyası silindi");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Silme hatası: {e.Message}");
            }
        }

        public SaveInfo GetSaveInfo()
        {
            if (!HasSaveFile())
                return null;

            try
            {
                string json = File.ReadAllText(SavePath);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);
                CharacterData character = JsonUtility.FromJson<CharacterData>(saveData.CharacterJson);

                return new SaveInfo
                {
                    CharacterName = character.Name,
                    Age = character.Age,
                    SaveTime = saveData.SaveTime
                };
            }
            catch
            {
                return null;
            }
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public string CharacterJson;
        public string SaveTime;
        public string GameVersion;
    }

    public class SaveInfo
    {
        public string CharacterName;
        public int Age;
        public string SaveTime;
    }
}
