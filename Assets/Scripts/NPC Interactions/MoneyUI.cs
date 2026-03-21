using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text moneyText;

    public void SetAmount(float amount)
    {
        moneyText.text = $"Credits: {Mathf.Round(amount * 100f) / 100f}Åò";
    }
}

