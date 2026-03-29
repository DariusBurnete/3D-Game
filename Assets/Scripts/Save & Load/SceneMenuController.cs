using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneMenuController : MonoBehaviour
{
    public static SceneMenuController Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (SceneMenuUI.Instance == null || SceneManager.GetActiveScene().name == "MainMenu" || ScreensInteraction.Instance.Mode != ScreenMode.None) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (SceneMenuUI.Instance.gameObject.activeSelf)
            {
                SceneMenuUI.Instance.gameObject.SetActive(false);
                ScreensInteraction.Instance.Mode = ScreenMode.None;
                Time.timeScale = 1f;
            }
            else
            {
                SceneMenuUI.Instance.gameObject.SetActive(true);
                ScreensInteraction.Instance.Mode = ScreenMode.SceneMenu;
                Time.timeScale = 0f;
            }
        }
    }

    public void SaveAndExit()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("SaveManager instance not found.");
            return;
        }

        SaveManager.Instance.SaveGame();
        ExitGame();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit game.");
    }

    public void ExitToMainMenu() 
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("SaveManager instance not found.");
            return;
        }

        SaveManager.Instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

}