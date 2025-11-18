using UnityEngine;

namespace BitLifeTR.Utils
{
    /// <summary>
    /// Ses yönetim sistemi
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private AudioSource musicSource;
        private AudioSource sfxSource;

        private float musicVolume = 1f;
        private float sfxVolume = 1f;
        private bool isMuted = false;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            // Ses kaynaklarını oluştur
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;

            LoadSettings();
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;

            musicSource.clip = clip;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null || isMuted) return;

            sfxSource.PlayOneShot(clip, sfxVolume);
        }

        public void PlayButtonClick()
        {
            // Varsayılan buton sesi
            // AudioClip buttonClip = Resources.Load<AudioClip>("Audio/button_click");
            // PlaySFX(buttonClip);
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            musicSource.volume = musicVolume;
            SaveSettings();
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            SaveSettings();
        }

        public void ToggleMute()
        {
            isMuted = !isMuted;
            musicSource.mute = isMuted;
            SaveSettings();
        }

        public float GetMusicVolume() => musicVolume;
        public float GetSFXVolume() => sfxVolume;
        public bool IsMuted() => isMuted;

        private void SaveSettings()
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadSettings()
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

            musicSource.volume = musicVolume;
            musicSource.mute = isMuted;
        }
    }
}
