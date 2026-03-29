using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestore : MonoBehaviour
{
    public static SceneRestore Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        RestoreSceneState();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RestoreSceneState();
    }

    private void RestoreSceneState()
    {
        RestoreSkybox();
        RestoreShip();
        RestorePlayerMode();
        LockPlayerInteraction();
    }

    private void RestoreSkybox()
    {
        if (GameStateManager.Instance == null) return;

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == GameStateManager.Instance.SpaceSceneName)
        {
            SkyboxHelper.ApplySkybox(GameStateManager.Instance.SpaceSkybox);
            return;
        }

        PlanetData currentPlanet = GameStateManager.Instance.CurrentPlanet;
        if (currentPlanet != null && currentPlanet.planetSkybox != null)
        {
            SkyboxHelper.ApplySkybox(currentPlanet.planetSkybox);
            return;
        }
    }

    private void RestoreShip()
    {
        if (!GameStateManager.Instance.HasSavedShipTransform) return;
        if (GameStateManager.Instance == null) return;
        SpaceShip ship = FindFirstObjectByType<SpaceShip>();
        Rigidbody rb = ship.GetComponent<Rigidbody>();
        if (ship == null) return;

        Vector3 pos = GameStateManager.Instance.SavedShipPosition;
        Quaternion rot = GameStateManager.Instance.SavedShipRotation;

        if (rot == Quaternion.identity || rot == new Quaternion(0, 0, 0, 0))
            rot = Quaternion.identity;
        else
            rot = Quaternion.Normalize(rot);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = pos;
            rb.rotation = rot;
        }
        else
        {
            ship.transform.SetPositionAndRotation(pos, rot);
        }
    }

    private void RestorePlayerMode()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "MainMenu") return;

        PlayerModeController modeController = FindFirstObjectByType<PlayerModeController>();
        if (modeController == null || GameStateManager.Instance == null) return;

        if (currentSceneName == GameStateManager.Instance.SpaceSceneName)
            modeController.EnterSpaceMode();
        else
            modeController.EnterPlanetMode();
    }

    private void LockPlayerInteraction()
    {
        PlayerWithBodies player = FindFirstObjectByType<PlayerWithBodies>();
        if (player != null)
        {
            player.ClearCurrentInteractable();
            player.LockInteractionTemporarily();
        }
    }
}