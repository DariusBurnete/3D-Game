using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollisionImmunity : MonoBehaviour
{
    [Range(0f, 1f)]
    public float smoothFactor = 0.95f;

    private Rigidbody celestialRB;
    private Vector3 previousPosition;
    private Vector3 previousVelocity;

    void Awake()
    {
        celestialRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        previousPosition = celestialRB.position;
        previousVelocity = celestialRB.linearVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        CancelCollision();
    }

    void OnCollisionStay(Collision collision)
    {
        CancelCollision();
    }

    private void CancelCollision()
    {
        celestialRB.position = Vector3.Lerp(celestialRB.position, previousPosition, smoothFactor);
        celestialRB.linearVelocity = previousVelocity;
    }
}
