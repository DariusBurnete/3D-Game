using UnityEngine;

[ExecuteAlways]
public class CelestialStateId : MonoBehaviour
{
    [SerializeField]
    private string uniqueId;

    public string UniqueId => uniqueId;

    private void Awake()
    {
        EnsureId();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EnsureId();
    }
#endif

    private void EnsureId()
    {
        if (!string.IsNullOrWhiteSpace(uniqueId)) return;

        string tagPart = gameObject.tag.ToLowerInvariant();
        string namePart = Sanitize(gameObject.name);

        string baseId = $"{tagPart}_{namePart}";
        string finalId = baseId;

        int duplicateIndex = CountEarlierMatchingIds(baseId);
        if (duplicateIndex > 0)
            finalId = $"{baseId}_{duplicateIndex}";

        uniqueId = finalId;
    }

    private string Sanitize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "unnamed";

        value = value.ToLowerInvariant();
        value = value.Replace(" ", "_");
        value = value.Replace("-", "_");
        value = value.Replace(".", "_");

        return value;
    }

    private int CountEarlierMatchingIds(string baseId)
    {
        CelestialStateId[] allIds = FindObjectsByType<CelestialStateId>(FindObjectsSortMode.None);
        int count = 0;

        foreach (CelestialStateId other in allIds)
        {
            if (other == null || other == this)
                continue;

            string otherTag = other.gameObject.tag.ToLowerInvariant();
            string otherName = Sanitize(other.gameObject.name);
            string otherBaseId = $"{otherTag}_{otherName}";

            if (otherBaseId == baseId) count++;
        }

        return count;
    }

    [ContextMenu("Regenerate ID")]
    private void RegenerateId()
    {
        uniqueId = string.Empty;
        EnsureId();
    }
}