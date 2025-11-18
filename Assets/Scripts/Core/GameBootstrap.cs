using UnityEngine;
using BitLifeTR.Localization;

namespace BitLifeTR.Core
{
    /// <summary>
    /// Oyun başlatıcı - Sahneye eklenmesi gereken ilk script
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Debug.Log("BitLife TR başlatılıyor...");

            // Localization'ı başlat
            LocalizationManager.Initialize();

            // GameManager'ı oluştur
            GameManager.Instance.ToString(); // Lazy initialization tetikle

            Debug.Log("BitLife TR hazır!");
        }

        private void Start()
        {
            // Ana menüyü göster
            if (GameManager.Instance.UIManager != null)
            {
                GameManager.Instance.UIManager.ShowScreen(ScreenType.MainMenu);
            }
        }
    }
}
