using System.Collections.Generic;
using UnityEngine;

public class PlanetMarker : MonoBehaviour
{
    public static List<PlanetMarker> AllPlanets = new();

    public string planetName;

    private void OnEnable()
    {
        AllPlanets.Add(this);
    }

    private void OnDisable()
    {
        AllPlanets.Remove(this);
    }
}
