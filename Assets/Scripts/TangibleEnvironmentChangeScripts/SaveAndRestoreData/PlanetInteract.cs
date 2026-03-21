using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetInteract : MonoBehaviour, IInteractable
{
    [SerializeField]
    private PlanetData planetData;
    public PlanetData PlanetData => planetData;

    public void Interact()
    {
        SpaceShip ship = FindFirstObjectByType<SpaceShip>();
        if (planetData == null || ship == null || GameStateManager.Instance == null) return;

        GameStateManager.Instance.SaveShipTransform(ship.transform);
        SaveAllCelestialBodies();
        GameStateManager.Instance.SetCurrentPlanet(planetData);
        SceneManager.LoadScene(planetData.surfaceSceneName);
    }

    private void SaveAllCelestialBodies()
    {
        CelestialStateId[] stateIds = FindObjectsByType<CelestialStateId>(FindObjectsSortMode.None);

        foreach (CelestialStateId stateId in stateIds)
        {
            Rigidbody rb = stateId.GetComponent<Rigidbody>();
            if (string.IsNullOrEmpty(stateId.UniqueId)) continue;
            if (stateId == null) continue;
            if (rb == null) continue;

            GameStateManager.Instance.SaveCelestialState(stateId.UniqueId, rb);
        }
    }

    public string GetPromptText() => "C";
    public CelestialBodyType GetCelestialBodyType() => CelestialBodyType.Planet;
}