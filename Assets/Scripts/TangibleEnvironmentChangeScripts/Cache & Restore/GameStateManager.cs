using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private Material spaceSkybox;
    public Material SpaceSkybox => spaceSkybox;

    public static GameStateManager Instance { get; private set; }

    public Vector3 SavedShipPosition { get; private set; }
    public Quaternion SavedShipRotation { get; private set; }
    public bool HasSavedShipTransform { get; private set; }

    public PlanetData CurrentPlanet { get; private set; }
    public string SpaceSceneName = "Space";
    private readonly Dictionary<string, CelestialRuntimeState> savedCelestialStates = new();
    public bool HasSavedCelestialStates => savedCelestialStates.Count > 0;

    public bool IsInSpaceScene => SceneManager.GetActiveScene().name == SpaceSceneName;
    public bool IsInPlanetScene => !IsInSpaceScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentPlanet(PlanetData planet)
    {
        CurrentPlanet = planet;
    }

    public void SaveShipTransform(Vector3 position, Quaternion rotation)
    {
        SavedShipPosition = position;
        SavedShipRotation = rotation;
        HasSavedShipTransform = true;
    }

    public void ClearSavedShipTransform()
    {
        HasSavedShipTransform = false;
    }

    public void SaveCelestialState(string uniqueId, Rigidbody rb)
    {
        if (string.IsNullOrEmpty(uniqueId) || rb == null) return;

        savedCelestialStates[uniqueId] = new CelestialRuntimeState(
            rb.position,
            rb.rotation,
            rb.linearVelocity,
            rb.angularVelocity
        );
    }

    public void SaveCelestialState(string uniqueId, Vector3 position, Quaternion rotation, Vector3 linearVelocity, Vector3 angularVelocity)
    {
        if (string.IsNullOrEmpty(uniqueId)) return;

        savedCelestialStates[uniqueId] = new CelestialRuntimeState(
            position,
            rotation,
            linearVelocity,
            angularVelocity
        );
    }

    public bool TryGetCelestialState(string uniqueId, out CelestialRuntimeState state)
    {
        return savedCelestialStates.TryGetValue(uniqueId, out state);
    }

    public void ClearCelestialStates()
    {
        savedCelestialStates.Clear();
    }

    public List<CelestialStateSave> ExportCelestialStates()
    {
        List<CelestialStateSave> result = new();

        foreach (var pair in savedCelestialStates)
        {
            CelestialRuntimeState state = pair.Value;

            result.Add(new CelestialStateSave
            {
                uniqueId = pair.Key,
                position = new float[] { state.position.x, state.position.y, state.position.z },
                rotation = new float[] { state.rotation.x, state.rotation.y, state.rotation.z, state.rotation.w },
                linearVelocity = new float[] { state.linearVelocity.x, state.linearVelocity.y, state.linearVelocity.z },
                angularVelocity = new float[] { state.angularVelocity.x, state.angularVelocity.y, state.angularVelocity.z }
            });
        }

        return result;
    }

    public void ImportCelestialStates(List<CelestialStateSave> states)
    {
        savedCelestialStates.Clear();

        if (states == null) return;

        foreach (var state in states)
        {
            if (state == null || string.IsNullOrWhiteSpace(state.uniqueId)) continue;
            if (state.position == null || state.position.Length < 3) continue;
            if (state.rotation == null || state.rotation.Length < 4) continue;
            if (state.linearVelocity == null || state.linearVelocity.Length < 3) continue;
            if (state.angularVelocity == null || state.angularVelocity.Length < 3) continue;

            Vector3 position = new(state.position[0], state.position[1], state.position[2]);
            Quaternion rotation = new(state.rotation[0], state.rotation[1], state.rotation[2], state.rotation[3]);
            Vector3 linearVelocity = new(state.linearVelocity[0], state.linearVelocity[1], state.linearVelocity[2]);
            Vector3 angularVelocity = new(state.angularVelocity[0], state.angularVelocity[1], state.angularVelocity[2]);

            if (rotation == new Quaternion(0f, 0f, 0f, 0f))
                rotation = Quaternion.identity;
            else
                rotation = Quaternion.Normalize(rotation);

            SaveCelestialState(state.uniqueId, position, rotation, linearVelocity, angularVelocity);
        }
    }
}