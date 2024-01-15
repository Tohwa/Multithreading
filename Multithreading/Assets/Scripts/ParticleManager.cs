using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;

public class ParticleManager : MonoBehaviour
{
    public int particleCount = 10000;
    private List<Task> particleUpdateTasks = new List<Task>();
    private List<Particle> particles = new List<Particle>();

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
            particles.Add(particleComponent);
        }
    }

    void UpdateParticles()
    {
        // Calculate the batch size based on the number of processors
        int batchSize = particleCount / System.Environment.ProcessorCount;

        // Loop to distribute particle update tasks across multiple threads
        for (int i = 0; i < System.Environment.ProcessorCount; i++)
        {
            int start = i * batchSize;
            int end = (i == System.Environment.ProcessorCount - 1) ? particleCount : (i + 1) * batchSize;

            // Add a task for updating particles in the specified range to the task list
            particleUpdateTasks.Add(Task.Run(() => UpdateParticlesInRange(start, end)));
        }

        // Wait for all tasks to complete before moving on
        Task.WaitAll(particleUpdateTasks.ToArray());

        // Clear the task list for the next frame
        particleUpdateTasks.Clear();
    }

    void UpdateParticlesInRange(int start, int end)
    {
        // Access Unity-specific operations within this block
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            // Loop through the specified range and update each particle
            for (int i = start; i < end; i++)
            {
                Vector3 newPosition = particles[i].transform.position + Random.onUnitSphere * 3f * particles[i].moveSpeed * Time.deltaTime;
                particles[i].transform.position = newPosition;
            }
        });
    }
}
