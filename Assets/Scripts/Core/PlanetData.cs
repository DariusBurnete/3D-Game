using UnityEngine;

[CreateAssetMenu(menuName = "Planets/New Planet Data")]
public class PlanetData : ScriptableObject
{
    public string planetId;
    public string planetName;
    public string surfaceSceneName;

    public Material planetSkybox;
}