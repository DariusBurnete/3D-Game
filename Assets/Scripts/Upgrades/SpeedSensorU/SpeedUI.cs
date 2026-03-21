using UnityEngine;
using TMPro;

public class SpeedUI : MonoBehaviour
{
    public static SpeedUI Instance { get; private set; }

    [SerializeField] 
    private TMP_Text speedText;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetSpeed(float speed)
    {
        if (speedText == null) return;

        speedText.text = $"Speed: {speed:0}";
    }
}