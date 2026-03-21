using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class OnMouseFeedback : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{ 
    [SerializeField] 
    private float hoverScale = 1.05f;
    [SerializeField] 
    private float animationSpeed = 15f;

    private Vector3 targetScale;
    private RectTransform visual;

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
            Time.deltaTime * animationSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = Vector3.one * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = Vector3.one;
    }
}
