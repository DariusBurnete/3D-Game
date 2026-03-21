using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ExitToSpace : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (GameStateManager.Instance == null) return;

            SceneManager.LoadScene(GameStateManager.Instance.SpaceSceneName);
        }
    }
}