using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;

public class ParticleManager : MonoBehaviour
{
    public int particleCount = 10000;
    private List<Task> particleUpdateTasks = new List<Task>();

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
            Particle particleComponent = particleObject.AddComponent<Particle>();
            particleComponent.moveSpeed = Random.Range(1f, 5f);
        }
    }

    void UpdateParticles()
    {
        int batchSize = particleCount / System.Environment.ProcessorCount;

        for (int i = 0; i < System.Environment.ProcessorCount; i++)
        {
            int start = i * batchSize;
            int end = (i == System.Environment.ProcessorCount - 1) ? particleCount : (i + 1) * batchSize;

            particleUpdateTasks.Add(Task.Run(() => UpdateParticlesInRange(start, end)));
        }

        Task.WaitAll(particleUpdateTasks.ToArray());
        particleUpdateTasks.Clear();
    }

    void UpdateParticlesInRange(int start, int end)
    {
        // Access Unity-specific operations within this block
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            // Find all GameObjects with the Particle component and update them
            Particle[] particles = GameObject.FindObjectsOfType<Particle>();
            for (int i = start; i < end; i++)
            {
                particles[i].Update();
            }
        });
    }
}
