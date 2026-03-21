using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeRestore : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PlayerData.Instance != null)
            PlayerData.Instance.ReapplyPurchasedUpgrades();
    }
}