using UnityEngine;

public class PlanetSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Transform landingPoint;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && landingPoint != null)
        {
            Rigidbody shipRB = player.GetComponent<Rigidbody>();
            SpaceShipP planetShip = player.GetComponent<SpaceShipP>();

            Vector3 spawnPosition = landingPoint.position;
            Quaternion spawnRotation = Quaternion.LookRotation(landingPoint.forward, landingPoint.up);

            if (shipRB != null)
            {
                shipRB.linearVelocity = Vector3.zero;
                shipRB.angularVelocity = Vector3.zero;
                shipRB.position = spawnPosition;
                shipRB.rotation = spawnRotation;
            }
            else
            {
                player.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            }

            if (planetShip != null)
            {
                planetShip.ResetInput();
                planetShip.SnapToCurrentHoverHeight();
            }
        }

        PlayerWithBodies playerWithBodies = FindFirstObjectByType<PlayerWithBodies>();
        if (playerWithBodies != null)
        {
            playerWithBodies.ClearCurrentInteractable();
            playerWithBodies.LockInteractionTemporarily();
        }
    }
}