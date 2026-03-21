using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [SerializeField] 
    private UpgradeScreenUI upgradeScreen;
    [SerializeField] 
    private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OpenScreen);
    }

    private void OpenScreen()
    {
        ScreensInteraction.Instance.OpenUpgrade();
    }
}
