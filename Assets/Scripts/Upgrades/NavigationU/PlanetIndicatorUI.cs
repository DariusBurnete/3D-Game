using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class PlanetIndicatorUI : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text nameText;
    [SerializeField] 
    private TMP_Text distanceText;
    [SerializeField] 
    private Transform circleTransform;

    private Transform player;
    private PlanetMarker planet;

    private float disableDistance;

    public void Init(PlanetMarker planet, Transform player)
    {
        this.planet = planet;
        this.player = player;

        nameText.text = planet.planetName;

        float planetScale = planet.transform.localScale.x;
        disableDistance = planetScale * 0.9f;//tweak

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (planet == null || player == null) return;

        float distance = Vector3.Distance(player.position, planet.transform.position);
        bool visible = distance > disableDistance;

        circleTransform.gameObject.SetActive(visible);
        nameText.gameObject.SetActive(visible);
        distanceText.gameObject.SetActive(visible);

        transform.position = planet.transform.position + Vector3.up * 2f;
        distanceText.text = $"{distance:0} m";

        float planetScale = planet.transform.localScale.x;
        float baseScale = planetScale / 200f;
        float distanceScale = Mathf.Log(distance + 1f, 10f); //tweak
        float finalScale = baseScale * distanceScale;
        transform.localScale = Vector3.one * finalScale;

        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180f, 0);
    }
}