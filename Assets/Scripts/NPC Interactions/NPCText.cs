using TMPro;
using UnityEngine;

public class NPCText : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text monologueText;

    public void SetText(string text)
    {
        monologueText.text = text;
    }

}

