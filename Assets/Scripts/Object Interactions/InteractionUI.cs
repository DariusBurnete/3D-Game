using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIElement
{
    public GameObject rootObject;
    public TMP_Text text; 
    public Image image;
    public Sprite defaultSprite;  
}

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI instance;
    private OnKeyFeedback activeKeyFeedback;

    [Header("UI Elements")]
    public UIElement landUI;
    public UIElement takeOffUI;
    public UIElement merchantUI;
    public UIElement mechanicUI;

    [Header("landUI Sprites")]
    public Sprite moonSprite;
    public Sprite planetSprite;
    public Sprite stationSprite;

    [Header("Key Feedback")]
    [SerializeField] 
    private OnKeyFeedback landKeyFeedback;
    [SerializeField] 
    private OnKeyFeedback takeOffKeyFeedback;

    [Header("Screens")]
    [SerializeField] 
    private GameObject merchantScreen;
    [SerializeField]
    private GameObject mechanicScreen;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        HideAll();
    }

    private void SetActive(UIElement element, bool active)
    {
        if (element == null || element.rootObject == null) return;
        element.rootObject.SetActive(active);
    }

    public void HideAll()
    {
        SetActive(landUI, false);
        SetActive(takeOffUI, false);
        SetActive(merchantUI, false);
        SetActive(mechanicUI, false);
        ClearKeyFeedback();
    }

    public void ShowBodyType(string message, Sprite sprite = null)
    {
        if (landUI == null) return;

        SetActive(landUI, true);
        if (landUI.text != null) landUI.text.text = message;
        if (landUI.image != null) landUI.image.sprite = sprite != null ? sprite : landUI.defaultSprite;

        SetActive(takeOffUI, false);
        SetActive(merchantUI, false);
        SetActive(mechanicUI, false);

        SetActiveKeyFeedback(landKeyFeedback);
    }

    public void ShowTakeOff(string message = "Press 'C' to take-off")
    {
        if (takeOffUI == null) return;

        SetActive(takeOffUI, true);
        if (takeOffUI.text != null) takeOffUI.text.text = message;

        SetActive(landUI, false);

        SetActiveKeyFeedback(takeOffKeyFeedback);
    }

    public void ShowMerchantUI()
    {
        if (merchantUI == null) return;
        SetActive(merchantUI, true);
        if (merchantUI.text != null) merchantUI.text.text = "Merchant";
    }
    public bool IsMerchantOpen => merchantScreen != null && merchantScreen.activeSelf;

    public void ShowMechanicUI()
    {
        if (mechanicUI == null) return;
        SetActive(mechanicUI, true);
        if (mechanicUI.text != null) mechanicUI.text.text = "Mechanic";
    }
    public bool IsMechanicOpen => mechanicScreen != null && mechanicScreen.activeSelf;

    #region Key Feedback Methods
    public void SetActiveKeyFeedback(OnKeyFeedback feedback)
    {
        activeKeyFeedback = feedback;
    }

    public void ClearKeyFeedback()
    {
        activeKeyFeedback = null;
    }

    public void PressKeyFeedback()
    {
        activeKeyFeedback?.Press();
    }

    public void ReleaseKeyFeedback()
    {
        activeKeyFeedback?.Release();
    }
    #endregion

}
