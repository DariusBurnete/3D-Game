using UnityEngine;

public class OpenNPCButton : MonoBehaviour
{
    [SerializeField] 
    private NPCData merchant;
    [SerializeField]
    private NPCData mechanic;
    [SerializeField]
    private MechanicUIScreen mechanicScreen;

    public void OpenMerchant()
    {
        ScreensInteraction.Instance.OpenMerchant(PlayerData.Instance, merchant);
    }

    public void OpenMechanic()
    {
        ScreensInteraction.Instance.OpenMechanic(PlayerData.Instance, mechanic);
    }

}

