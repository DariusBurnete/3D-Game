using UnityEngine;

public class StopCollision : MonoBehaviour
{
    Rigidbody shipRB;
    BoxCollider shipBC;

    [HideInInspector]
    public Vector3 surfaceNormal;
    [HideInInspector]
    public bool inContact;

    void Start()
    {
        shipRB = GetComponent<Rigidbody>();
        shipBC = GetComponent<BoxCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!(other.CompareTag("Planet") || other.CompareTag("Moon") || other.CompareTag("Sun"))) return;
        
        inContact = true;
        shipRB.angularVelocity = Vector3.zero;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!(other.CompareTag("Planet") || other.CompareTag("Moon") || other.CompareTag("Sun"))) return;

        Vector3 distToShip = transform.position - other.transform.position;
        float currentDistance = distToShip.magnitude;
        surfaceNormal = distToShip / currentDistance;

        Vector3 inward = Vector3.Project(shipRB.linearVelocity, -surfaceNormal);
        if(Vector3.Dot(inward, -surfaceNormal) > 0f)
            shipRB.linearVelocity -= inward;

        float otherRadius = other.bounds.extents.magnitude;
        float shipRadius = shipBC.bounds.extents.magnitude;
        float neededDistance = otherRadius + shipRadius + 0.5f;

        float penDepth = neededDistance - currentDistance;

        if (penDepth > 0f)
        {
            float pushStrength = 12f;
            Vector3 pushForce = penDepth * pushStrength * surfaceNormal;
            shipRB.AddForce(pushForce, ForceMode.Acceleration);
        }

        shipRB.linearVelocity *= 0.98f;
    }

    public void OnTriggerExit(Collider other)
    {
        if (!(other.CompareTag("Planet") || other.CompareTag("Moon") || other.CompareTag("Sun"))) return;
        inContact = false;
    }

    //to add another collider for interaction (not done) and a smaller one for limits (done, just make it smaller)
}
