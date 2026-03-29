using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string SceneToLoad = "Space";

    [Header("Optional UI")]
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        RefreshUI();
    }

    public void NewGame()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.HasSave())
        {
            SaveManager.Instance.DeleteSave();
        }

        SceneManager.LoadScene(SceneToLoad);
    }

    public void LoadGame()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("SaveManager instance not found.");
            return;
        }

        if (!SaveManager.Instance.HasSave())
        {
            Debug.Log("No save file exists.");
            return;
        }

        SaveManager.Instance.LoadGameAndChangeScene();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit game.");
    }

    public void RefreshUI()
    {
        if (loadGameButton != null)
        {
            bool canLoad = SaveManager.Instance != null && SaveManager.Instance.HasSave();
            loadGameButton.interactable = canLoad;
        }
    }
}