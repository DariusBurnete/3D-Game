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

    public void SaveShipTransform(Transform shipTransform)
    {
        SavedShipPosition = shipTransform.position;
        SavedShipRotation = shipTransform.rotation;
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

    public bool TryGetCelestialState(string uniqueId, out CelestialRuntimeState state)
    {
        return savedCelestialStates.TryGetValue(uniqueId, out state);
    }

    public void ClearCelestialStates()
    {
        savedCelestialStates.Clear();
    }
}