using UnityEngine;

namespace BitLifeTR.UI
{
    /// <summary>
    /// Tüm ekranlar için temel sınıf
    /// </summary>
    public abstract class BaseScreen
    {
        protected UIManager uiManager;
        protected GameObject rootObject;
        protected bool isInitialized;

        public BaseScreen(UIManager manager)
        {
            uiManager = manager;
        }

        /// <summary>
        /// Ekranı oluştur - ilk kez çağrılır
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Ekranı göster
        /// </summary>
        public virtual void Show()
        {
            if (!isInitialized)
            {
                Initialize();
                isInitialized = true;
            }

            if (rootObject != null)
            {
                rootObject.SetActive(true);
            }
        }

        /// <summary>
        /// Ekranı gizle
        /// </summary>
        public virtual void Hide()
        {
            if (rootObject != null)
            {
                rootObject.SetActive(false);
            }
        }

        /// <summary>
        /// Ekranı güncelle/yenile
        /// </summary>
        public virtual void Refresh()
        {
            // Override edilebilir
        }

        /// <summary>
        /// Ekranı temizle
        /// </summary>
        public virtual void Destroy()
        {
            if (rootObject != null)
            {
                GameObject.Destroy(rootObject);
                rootObject = null;
                isInitialized = false;
            }
        }
    }
}
