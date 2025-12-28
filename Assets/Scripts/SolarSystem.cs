using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    readonly float G = 100f;
    GameObject[] celestials;

    void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestial");

        InitialVelocity();
    }

    void FixedUpdate()
    {
        Gravity();
    }

    void Gravity()
    {
        foreach(GameObject celestialBody in celestials)
        {
            foreach(GameObject otherCelestialBody in celestials)
            {
                if(!celestialBody.Equals(otherCelestialBody))
                {
                    float m1 = celestialBody.GetComponent<Rigidbody>().mass;
                    float m2 = otherCelestialBody.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(celestialBody.transform.position, otherCelestialBody.transform.position);

                    celestialBody
                        .GetComponent<Rigidbody>()
                        .AddForce((otherCelestialBody.transform.position - celestialBody.transform.position).normalized * (G * (m1 * m2) / (r * r)));
                }
            }
        }
    }

    void InitialVelocity()
    {
        foreach (GameObject celestialBody in celestials)
        {
            foreach (GameObject otherCelestialBody in celestials)
            {
                if (!celestialBody.Equals(otherCelestialBody))
                {
                    float m2 = otherCelestialBody.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(celestialBody.transform.position, otherCelestialBody.transform.position);
                    celestialBody.transform.LookAt(otherCelestialBody.transform);

                    celestialBody
                        .GetComponent<Rigidbody>()
                        .linearVelocity += celestialBody.transform.right * Mathf.Sqrt((G * m2) / r);
                }
            }
        }
    }

}
