using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Singleton")]
    public static AudioManager Instance;

    [Header("Audio Sources Settings")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("BGM Settings")]
    [SerializeField] private AudioClip _mainMenuMusic;
    [SerializeField] private AudioClip _gameplayMusic;

    [Header("SFX Settings")]
    [SerializeField] private AudioClip[] _runningSounds;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _slideSound;
    [SerializeField] private AudioClip _coinCollectSound;
    [SerializeField] private AudioClip _powerUpCollectSound;
    [SerializeField] private AudioClip _obstacleHitSound;

    [Header("Footsteps Settings")]
    [SerializeField] private bool _randomizeFootsteps = true; // <- se randomizzare i suoni o meno
    [SerializeField] private bool _preventConsecutiveRepeats = true; // <- per evitare di ripetere lo stesso suono consecutivamente

    [Header("Audio Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 0.8f;

    [Header("Fade Settings")]
    [SerializeField] private float _fadeDuration = 1f;

    private bool _isFading = false;
    private int _lastFootstepIndex = -1; // <- per evitare ripetizioni consecutive

    void Awake() // <- gestisce l'implementazione del Singleton
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayMainMenuMusic(); // <- inizia con la musica del main menu
    }

    #region Initialization
    private void InitializeAudioSources() // <- se non sono assegnate nell'inspector, crea le AudioSource automaticamente
    {
        if (_musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            _musicSource = musicObj.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
        }

        if (_sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            _sfxSource = sfxObj.AddComponent<AudioSource>();
            _sfxSource.loop = false;
            _sfxSource.playOnAwake = false;
        }

        UpdateVolumes();
    }
    #endregion

    #region BGM Methods
    public void PlayMainMenuMusic()
    {
        if (_mainMenuMusic != null && !_isFading)
        {
            if (_musicSource.clip == _mainMenuMusic && _musicSource.isPlaying) return;

            FadeToNewMusic(_mainMenuMusic);
        }
    }

    public void PlayGameplayMusic()
    {
        if (_gameplayMusic != null && !_isFading)
        {
            if (_musicSource.clip == _gameplayMusic && _musicSource.isPlaying) return;

            FadeToNewMusic(_gameplayMusic);
        }
    }

    public void StopMusic()
    {
        if (_musicSource.isPlaying) StartCoroutine(FadeOut(_musicSource, _fadeDuration));
    }

    private void FadeToNewMusic(AudioClip newClip)
    {
        if (_musicSource.isPlaying) StartCoroutine(FadeToNewClip(newClip));

        else
        {
            _musicSource.clip = newClip;
            _musicSource.Play();
            StartCoroutine(FadeIn(_musicSource, _fadeDuration));
        }
    }
    #endregion

    #region SFX Methods
    public void PlayRunningSound()
    {
        if (_runningSounds != null && _runningSounds.Length > 0)
        {
            AudioClip clipToPlay = GetRandomFootstepClip();

            if (clipToPlay != null) _sfxSource.PlayOneShot(clipToPlay);
        }
    }

    private AudioClip GetRandomFootstepClip()
    {
        if (_runningSounds.Length == 0) return null;

        if (_runningSounds.Length == 1)
        {
            return _runningSounds[0];
        }

        int randomIndex;

        if (_randomizeFootsteps)
        {
            if (_preventConsecutiveRepeats && _runningSounds.Length > 1)
            {
                do
                {
                    randomIndex = Random.Range(0, _runningSounds.Length);
                } 
                while (randomIndex == _lastFootstepIndex);
            }
            else
            {
                randomIndex = Random.Range(0, _runningSounds.Length);
            }
        }
        else
        {
            randomIndex = (_lastFootstepIndex + 1) % _runningSounds.Length;
        }

        _lastFootstepIndex = randomIndex;
        return _runningSounds[randomIndex];
    }

    // Metodo alternativo per scegliere un suono specifico
    public void PlayRunningSound(int index)
    {
        if (_runningSounds != null && index >= 0 && index < _runningSounds.Length)
        {
            if (_runningSounds[index] != null)
            {
                _sfxSource.PlayOneShot(_runningSounds[index]);
                _lastFootstepIndex = index;
            }
        }
    }

    public void PlayRandomRunningSound()
    {
        PlayRunningSound();
    }

    public void PlayJumpSound()
    {
        if (_jumpSound != null) _sfxSource.PlayOneShot(_jumpSound);
    }

    public void PlaySlideSound()
    {
        if (_slideSound != null) _sfxSource.PlayOneShot(_slideSound);
    }

    public void PlayCoinCollectSound()
    {
        if (_coinCollectSound != null) _sfxSource.PlayOneShot(_coinCollectSound);
    }

    public void PlayPowerUpCollectSound()
    {
        if (_powerUpCollectSound != null) _sfxSource.PlayOneShot(_powerUpCollectSound);
    }

    public void PlayObstacleHitSound()
    {
        if (_obstacleHitSound != null) _sfxSource.PlayOneShot(_obstacleHitSound);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) _sfxSource.PlayOneShot(clip);
    }
    #endregion

    #region Footsteps Configuration Methods
    public void SetRandomizeFootsteps(bool randomize)
    {
        _randomizeFootsteps = randomize;
    }

    public void SetPreventConsecutiveRepeats(bool prevent)
    {
        _preventConsecutiveRepeats = prevent;
    }

    public bool IsRandomizeFootstepsEnabled()
    {
        return _randomizeFootsteps;
    }

    public bool IsPreventConsecutiveRepeatsEnabled()
    {
        return _preventConsecutiveRepeats;
    }

    public int GetFootstepsCount()
    {
        return _runningSounds != null ? _runningSounds.Length : 0;
    }
    #endregion

    #region Volume Methods
    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);

        if (_musicSource != null) _musicSource.volume = _musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);

        if (_sfxSource != null) _sfxSource.volume = _sfxVolume;
    }

    public float GetMusicVolume()
    {
        return _musicVolume;
    }

    public float GetSFXVolume()
    {
        return _sfxVolume;
    }

    private void UpdateVolumes()
    {
        if (_musicSource != null) _musicSource.volume = _musicVolume;

        if (_sfxSource != null) _sfxSource.volume = _sfxVolume;
    }

    public void SetMusic()
    {
        if (_musicSource.volume > 0) SetMusicVolume(0);

        else SetMusicVolume(_musicVolume);
    }

    public void SetSFX()
    {
        if (_sfxSource.volume > 0) SetSFXVolume(0);

        else SetSFXVolume(_sfxVolume);
    }
    #endregion

    #region Fade Coroutines
    private System.Collections.IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        _isFading = true;
        float startVolume = 0f;
        audioSource.volume = startVolume;

        while (audioSource.volume < _musicVolume)
        {
            audioSource.volume += _musicVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = _musicVolume;
        _isFading = false;
    }

    private System.Collections.IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        _isFading = true;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        _isFading = false;
    }

    private System.Collections.IEnumerator FadeToNewClip(AudioClip newClip)
    {
        _isFading = true;

        yield return StartCoroutine(FadeOut(_musicSource, _fadeDuration * 0.5f));

        _musicSource.clip = newClip;
        _musicSource.Play();

        yield return StartCoroutine(FadeIn(_musicSource, _fadeDuration * 0.5f));

        _isFading = false;
    }
    #endregion

    #region Scene Management
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) // <- gestisce automaticamente il cambio di musica in base alla scena
    {
        switch (scene.name.ToLower())
        {
            case "mainmenu":
            case "menu":
                {
                    PlayMainMenuMusic();
                    break;
                }
            case "game":
            case "gameplay":
            case "level":
                {
                    PlayGameplayMusic();
                    break;
                }
        }
    }
    #endregion

    #region Public Methods
    public bool IsMusicPlaying()
    {
        return _musicSource != null && _musicSource.isPlaying;
    }

    public void PauseMusic()
    {
        if (_musicSource != null && _musicSource.isPlaying) _musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (_musicSource != null && !_musicSource.isPlaying) _musicSource.UnPause();
    }
    #endregion
}