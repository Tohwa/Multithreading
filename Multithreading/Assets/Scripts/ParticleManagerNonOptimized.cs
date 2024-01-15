using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManagerNonOptimized : MonoBehaviour
{
    public int particleCount = 10000;
    private List<ParticleNonOptimized> particles = new List<ParticleNonOptimized>();
    public GameObject fireParticle;

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
            particleObject = fireParticle;
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
            particle.Update();
        }
    }
}
