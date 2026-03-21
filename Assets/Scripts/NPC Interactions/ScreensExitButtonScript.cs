using UnityEngine;

public class ScreensExitButtonScript : MonoBehaviour
{
    public void CloseCurrentScreen()
    {
        if (ScreensInteraction.Instance != null)
            ScreensInteraction.Instance.CloseCurrent();
    }
}