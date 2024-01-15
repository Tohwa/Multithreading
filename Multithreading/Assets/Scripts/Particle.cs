using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using PimDeWitte.UnityMainThreadDispatcher;

public class Particle : MonoBehaviour
{
    public float moveSpeed;

    public void Update()
    {
        // Access Unity-specific operations within this block
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Vector3 newPosition = transform.position + Random.onUnitSphere * 3f * moveSpeed * Time.deltaTime;
            transform.position = newPosition;
        });
    }
}
