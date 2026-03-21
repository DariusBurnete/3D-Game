using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Library")]
    [SerializeField] 
    private AudioLibrary library;

    [Header("Pooling")]
    [SerializeField] 
    private int pooledSfxSources = 6;

    [Header("Mixer Volumes")]
    [SerializeField, Range(0f, 1f)] 
    private float masterVolume = 1f;
    [SerializeField, Range(0f, 1f)] 
    private float sfxVolume = 1f;
    [SerializeField, Range(0f, 1f)] 
    private float musicVolume = 1f;

    private readonly List<AudioSource> _sfxPool = new();
    private int _nextSfxIndex = 0;

    private AudioSource _musicSource;
    private AudioEntry _currentMusicEntry;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        if (library != null)
            library.BuildLookup();

        CreateMusicSource();
        CreateSfxPool();
    }

    private void CreateMusicSource()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.playOnAwake = false;
        _musicSource.loop = true;
        _musicSource.spatialBlend = 0f;
    }

    private void CreateSfxPool()
    {
        for (int i = 0; i < pooledSfxSources; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f; 
            _sfxPool.Add(source);
        }
    }

    public void PlayMusic(AudioID id)
    {
        if (library == null)
        {
            Debug.LogWarning("AudioManager has no AudioLibrary assigned.", this);
            return;
        }

        AudioEntry entry = library.Get(id);
        if (entry == null || entry.clip == null)
        {
            Debug.LogWarning($"No music entry found for {id}", this);
            return;
        }

        _currentMusicEntry = entry;
        _musicSource.clip = entry.clip;
        _musicSource.loop = true;
        _musicSource.pitch = entry.pitch;
        _musicSource.volume = entry.volume * musicVolume * masterVolume;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
        _currentMusicEntry = null;
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        RefreshMusicVolume();
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        RefreshMusicVolume();
    }

    private void RefreshMusicVolume()
    {
        if (_musicSource == null || _currentMusicEntry == null)
            return;

        _musicSource.volume = _currentMusicEntry.volume * musicVolume * masterVolume;
    }

    public void PlaySfx(AudioID id)
    {
        PlaySfx(id, null, 1f, 1f);
    }

    public void PlaySfx(AudioID id, Vector3 position)
    {
        PlaySfx(id, position, 1f, 1f);
    }

    public void PlaySfx(AudioID id, Vector3? worldPosition, float volumeMultiplier = 1f, float pitchMultiplier = 1f)
    {
        if (library == null)
        {
            Debug.LogWarning("AudioManager has no AudioLibrary assigned.", this);
            return;
        }

        AudioEntry entry = library.Get(id);
        if (entry == null || entry.clip == null)
        {
            Debug.LogWarning($"No SFX entry found for {id}", this);
            return;
        }

        AudioSource source = GetNextSfxSource();

        source.Stop();
        source.clip = entry.clip;
        source.loop = false;
        source.pitch = entry.pitch * pitchMultiplier;
        source.volume = entry.volume * sfxVolume * masterVolume * volumeMultiplier;

        if (worldPosition.HasValue)
        {
            source.transform.position = worldPosition.Value;
            source.spatialBlend = 1f;
        }
        else
        {
            source.spatialBlend = 0f;
        }

        source.Play();
    }

    private AudioSource GetNextSfxSource()
    {
        AudioSource source = _sfxPool[_nextSfxIndex];
        _nextSfxIndex = (_nextSfxIndex + 1) % _sfxPool.Count;
        return source;
    }
}