using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using PimDeWitte.UnityMainThreadDispatcher;

public class ParticleControllerThreaded : MonoBehaviour
{
    public int particleCount = 1000;
    public GameObject particlePrefab;
    private List<GameObject> particles;

    private Thread particleThread;
    private bool isThreadRunning = false;
    private object threadLock = new object();

    void Start()
    {
        InitializeParticles();
        StartParticleThread();
    }

    void Update()
    {
        // Hier könnte ggf. andere Logik für die Haupt-Update-Schleife stehen
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

    void StartParticleThread()
    {
        particleThread = new Thread(MoveParticles);
        particleThread.Start();
        isThreadRunning = true;
    }

    void MoveParticles()
    {
        while (isThreadRunning)
        {
            List<Vector3> newPositions = new List<Vector3>();

            lock (threadLock)
            {
                foreach (var particle in particles)
                {
                    // Hier erfolgt die optimierte Berechnung der Partikelbewegung (im separaten Thread)
                    // Zum Beispiel: Berechnungen ohne direkte Unity-Operationen
                    Vector3 currentPosition = GetParticlePosition(particle);
                    Vector3 newPosition = currentPosition + Random.onUnitSphere * Time.deltaTime;
                    newPositions.Add(newPosition);
                }
            }

            UnityMainThreadDispatcher.Instance().Enqueue(() => UpdateParticlePositions(newPositions));

            Thread.Sleep(16); // Verhindert, dass der Thread zu viele Ressourcen verbraucht (ca. 60 FPS)
        }
    }

    Vector3 GetParticlePosition(GameObject particle)
    {
        lock (threadLock)
        {
            return particle.transform.position;
        }
    }

    void UpdateParticlePositions(List<Vector3> newPositions)
    {
        lock (threadLock)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].transform.position = newPositions[i];
            }
        }
    }

    void OnApplicationQuit()
    {
        // Beende den Thread, wenn die Anwendung geschlossen wird
        if (particleThread != null && particleThread.IsAlive)
        {
            isThreadRunning = false;
            particleThread.Join();
        }
    }
}
