using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI instance;

    [Header("UI Elements")]

    [SerializeField] 
    private GameObject cButton_Image;
    [SerializeField] 
    private TMP_Text cButton_Text;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Hide();
    }

    public void Show(string message)
    {
        cButton_Image.SetActive(true);
        cButton_Text.text = message;
    }

    public void Hide()
    {
        cButton_Image.SetActive(false);
        cButton_Text.text = "";
    }
}
