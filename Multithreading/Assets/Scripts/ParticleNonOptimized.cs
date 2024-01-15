using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleNonOptimized : MonoBehaviour
{
    public float moveSpeed;

    public void Update()
    {
        Vector3 newPosition = transform.position + Random.onUnitSphere * 3f * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }
}
