using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleController : MonoBehaviour
{
    public int particleCount = 1000;
    public float moveSpeed = 1.0f;

    public GameObject particlePrefab;
    private List<GameObject> particles;

    void Start()
    {
        InitializeParticles();
    }

    void Update()
    {
        MoveParticles();
    }

    void InitializeParticles()
    {
        particles = new List<GameObject>();
        for (int i = 0; i < particleCount; i++)
        {
            GameObject particle = Instantiate(particlePrefab, Random.insideUnitSphere * 10f, Quaternion.identity);
            particles.Add(particle);
        }
    }

    void MoveParticles()
    {
        foreach (var particle in particles)
        {
            // Hier erfolgt die Berechnung der Partikelbewegung (vor der Optimierung)
            Vector3 newPosition = particle.transform.position + Random.onUnitSphere * 3f * moveSpeed * Time.deltaTime;
            particle.transform.position = newPosition;
        }
    }
}
