using UnityEngine;

public static class SkyboxHelper
{
    public static void ApplySkybox(Material skyboxMaterial)
    {
        if (skyboxMaterial == null) return;

        Camera mainCamera = Camera.main;
        Skybox skybox = mainCamera.GetComponent<Skybox>();
        if (mainCamera != null)
            skybox.material = skyboxMaterial;
    }
}