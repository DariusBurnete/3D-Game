using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class OnKeyFeedback : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] 
    private float pressScale = 0.95f;
    [SerializeField] 
    private float animationSpeed = 15f;

    private RectTransform visual;
    private Vector3 targetScale;

    void Awake()
    {
        visual = GetComponent<RectTransform>();
        targetScale = visual.localScale;
    }

    void Update()
    {
        visual.localScale = Vector3.Lerp(
            visual.localScale,
            targetScale,
            Time.unscaledDeltaTime * animationSpeed
        );
    }

    public void Press()
    {
        targetScale = Vector3.one * pressScale;
    }

    public void Release()
    {
        targetScale = Vector3.one;
    }
}

