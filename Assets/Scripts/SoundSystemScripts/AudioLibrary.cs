using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [SerializeField] 
    private AudioEntry[] entries;

    private Dictionary<AudioID, AudioEntry> _lookup;

    private void OnEnable()
    {
        BuildLookup();
    }

    public void BuildLookup()
    {
        _lookup = new Dictionary<AudioID, AudioEntry>();

        if (entries == null)
            return;

        foreach (var entry in entries)
        {
            if (entry == null || entry.clip == null)
                continue;

            if (_lookup.ContainsKey(entry.id))
            {
                Debug.LogWarning($"Duplicate AudioId in AudioLibrary: {entry.id}", this);
                continue;
            }

            _lookup.Add(entry.id, entry);
        }
    }

    public AudioEntry Get(AudioID id)
    {
        if (_lookup == null || _lookup.Count == 0)
            BuildLookup();

        _lookup.TryGetValue(id, out var entry);
        return entry;
    }
}