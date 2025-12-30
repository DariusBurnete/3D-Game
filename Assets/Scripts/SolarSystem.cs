using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    readonly float G = 100f;

    GameObject sun;
    GameObject[] planets;
    GameObject[] moons;
    Rigidbody[] celestials;

    void Start()
    {
        sun = GameObject.FindGameObjectWithTag("Sun");
        planets = GameObject.FindGameObjectsWithTag("Planet");
        moons = GameObject.FindGameObjectsWithTag("Moon");

        InitRigidBodies();
        InitPlanetVelocities();
        InitMoonVelocities();
    }

    void FixedUpdate()
    {
        Gravity();
    }

    void InitRigidBodies()
    {
        int totalCelestials = 1 + planets.Length + moons.Length;
        celestials = new Rigidbody[totalCelestials];
        int index = 0;

        celestials[index++] = sun.GetComponent<Rigidbody>();
        foreach (GameObject planet in planets)
            celestials[index++] = planet.GetComponent<Rigidbody>();
        foreach (GameObject moon in moons)
            celestials[index++] = moon.GetComponent<Rigidbody>();
    }

    void Gravity()
    {
        for (int i = 0; i < celestials.Length; i++)
        {
            for (int j = 0; j < celestials.Length; j++)
            {
                if (i == j) continue;

                Rigidbody celestialA = celestials[i];
                Rigidbody celestialB = celestials[j];
                float m1 = celestialA.mass;
                float m2 = celestialB.mass;
                Vector3 direction = celestialB.position - celestialA.position;
                float distance = direction.magnitude;

                celestialA.AddForce(direction.normalized * G * (m1 * m2) / (distance * distance));
            }
        }
    }

    void InitPlanetVelocities()
    {
        Rigidbody sunRB = sun.GetComponent<Rigidbody>();

        foreach(GameObject planet in planets)
        {
            Rigidbody planetRB = planet.GetComponent<Rigidbody>();

            Vector3 direction = planetRB.position - sunRB.position;
            float distance = direction.magnitude;

            Vector3 orbitalDirection = Vector3.Cross(Vector3.up, direction.normalized);
            float speed = Mathf.Sqrt(G * sunRB.mass / distance);
            planetRB.linearVelocity = orbitalDirection * speed;
        }
    }

    void InitMoonVelocities()
    {
        foreach(GameObject moon in moons)
        {
            Rigidbody moonRB = moon.GetComponent<Rigidbody>();

            Rigidbody planetRB = FindParentPlanet(moonRB.position);

            Vector3 direction = moonRB.position - planetRB.position;
            float distance = direction.magnitude;

            Vector3 orbitalDirection = Vector3.Cross(Vector3.up, direction.normalized);
            float speed = Mathf.Sqrt(G * planetRB.mass / distance);
            moonRB.linearVelocity = planetRB.linearVelocity + orbitalDirection * speed;
        }
    }

    Rigidbody FindParentPlanet(Vector3 moonPosition)
    {
        Rigidbody parent = null;
        float minDistance = float.MaxValue;

        foreach(GameObject planet in planets)
        {
            Rigidbody planetRB = planet.GetComponent<Rigidbody>();
            Vector3 direction = moonPosition - planetRB.position;
            float distance = direction.magnitude;

            if(distance < minDistance)
            {
                minDistance = distance;
                parent = planetRB;
            }
        }

        return parent;
    }
}
