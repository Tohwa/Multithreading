using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManagerNonOptimized : MonoBehaviour
{
    public int particleCount = 10000;
    private List<ParticleNonOptimized> particles = new List<ParticleNonOptimized>();

    void Start()
    {
        InitializeParticles();
    }

    void Update()
    {
        UpdateParticles();
    }

    void InitializeParticles()
    {
        GameObject particleParent = new GameObject("ParticleParent");
        for (int i = 0; i < particleCount; i++)
        {
            // Instantiate a GameObject with the Particle component attached
            GameObject particleObject = new GameObject("Particle");
            particleObject.transform.parent = particleParent.transform;
            ParticleNonOptimized particleComponent = particleObject.AddComponent<ParticleNonOptimized>();
            particleComponent.moveSpeed = Random.Range(1f, 5f);
            particles.Add(particleComponent);
        }
    }

    void UpdateParticles()
    {
        foreach (ParticleNonOptimized particle in particles)
        {
            Vector3 newPosition = particle.transform.position + Random.onUnitSphere * 3f * particle.moveSpeed * Time.deltaTime;
            particle.transform.position = newPosition;
        }
    }
}
