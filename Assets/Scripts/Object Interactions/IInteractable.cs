public enum CelestialBodyType
{
    Moon,
    Planet,
    Station
}

public interface IInteractable
{
    void Interact();
    string GetPromptText();
    CelestialBodyType GetCelestialBodyType();
}
