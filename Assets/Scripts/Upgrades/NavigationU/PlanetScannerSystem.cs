using System.Collections.Generic;
using UnityEngine;

public class PlanetScannerSystem : ShipUpgrade
{
    private GameObject planetIndicatorPrefab;
    private List<PlanetIndicatorUI> indicators = new();

    private Transform player;

    protected override void Awake()
    {
        base.Awake();

        planetIndicatorPrefab = Resources.Load<GameObject>("UpgradesUI/PlanetIndicatorUI");
        player = transform;

        CreateIndicators();
    }

    private void CreateIndicators()
    {
        if (planetIndicatorPrefab == null) return;

        ClearMissingIndicators();

        if (indicators.Count > 0) return;

        foreach (var planet in PlanetMarker.AllPlanets)
        {
            if (planet == null) continue;

            GameObject planetIndicator = Instantiate(
                planetIndicatorPrefab,
                planet.transform.position + Vector3.up * 2f,
                Quaternion.identity
            );

            if (!planetIndicator.TryGetComponent<PlanetIndicatorUI>(out var indicatorUI))
            {
                Destroy(planetIndicator);
                continue;
            }

            indicatorUI.Init(planet, player);
            indicators.Add(indicatorUI);
        }
    }

    private void ClearMissingIndicators()
    {
        for (int i = indicators.Count - 1; i >= 0; i--)
        {
            if (indicators[i] == null)
                indicators.RemoveAt(i);
        }
    }

    public override void SetUpgradeActive(bool active)
    {
        if (active) CreateIndicators();
        base.SetUpgradeActive(active);
        ClearMissingIndicators();

        foreach (var indicator in indicators)
        {
            if (indicator != null)
                indicator.gameObject.SetActive(active);
        }
    }
}