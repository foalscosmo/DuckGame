using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> smokeParticle = new(); // List of smoke particle game objects
    [SerializeField] private List<Transform> spawnPoints = new(); // List of spawn points for smoke particles
    [SerializeField] private ParticleSystem victoryParticle; // Victory particle system
    [SerializeField] private List<ParticleSystem> startParticle; // List of particle systems for object grab animation
    [SerializeField] private ParticleSystem snappedParticle; // Particle system for snapped object indication
    public event Action<int> OnParticleFinished; // Event triggered when particle animation finishes
    public event Action OnParticleStart; // Event triggered when particle animation starts

    private void Awake()
    {
        DisableVictoryParticle(); // Disable victory particle system on awake
        DisableSmokeParticles(); // Disable smoke particles on awake
        DisableStarParticles(); // Disable object grab particles on awake
        DisableSnappedParticle(); // Disable snapped particle system on awake
    }

    private void Start()
    {
        SpawnSmokeParticle(); // Start spawning smoke particles
    }

    // Method to spawn smoke particles with a delay
    public void SpawnSmokeParticle()
    {
        StartCoroutine(SpawnBucketsWithDelay());
    }
    
    private IEnumerator SpawnBucketsWithDelay()
    {
        yield return new WaitForSecondsRealtime(2f); // Wait for a delay before spawning
        for (var i = 0; i < smokeParticle.Count; i++)
        {
            smokeParticle[i].transform.position = spawnPoints[i].position; // Set smoke particle position
            smokeParticle[i].gameObject.SetActive(true); // Activate smoke particle
            OnParticleStart?.Invoke(); // Trigger particle start event
            OnParticleFinished?.Invoke(i); // Trigger particle finish event
            yield return new WaitForSecondsRealtime(0.3f); // Wait for a short delay
            smokeParticle[i].gameObject.SetActive(false); // Deactivate smoke particle
        }
    }

    // Method to disable smoke animation with a delay
    public void DisableSmokeAnimation()
    {
        StartCoroutine(DisableSmokeAnimationWithDelay());
    }

    private IEnumerator DisableSmokeAnimationWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.7f); // Wait for a delay before disabling smoke animation
        foreach (var smoke in smokeParticle)
        {
            smoke.SetActive(true); // Activate smoke particle
        }
        yield return new WaitForSecondsRealtime(0.3f); // Wait for a short delay
        foreach (var smoke in smokeParticle)
        {
            smoke.SetActive(false); // Deactivate smoke particle
        }
    }

    // Method to disable victory particle system
    private void DisableVictoryParticle()
    {
        victoryParticle.Stop(); // Stop victory particle system
    }

    // Method to disable smoke particles
    private void DisableSmokeParticles()
    {
        foreach (var particle in smokeParticle) particle.SetActive(false); // Deactivate all smoke particles
    }

    // Method to play victory particle system
    public void PlayVictoryParticle()
    {
        victoryParticle.Play(); // Play victory particle system
    }

    // Method to play object grab particle animation for a specific object
    public void GrabObjectParticle(int index)
    {
        startParticle[index].Play(); // Play object grab particle animation
    }

    // Method to disable object grab particle animations
    public void DisableStarParticles()
    {
        foreach (var particle in startParticle)  particle.Stop(); // Stop all object grab particle animations
    }

    // Method to disable object grab particle animation for a specific object
    public void DisableDuckStarParticle(int index)
    {
        startParticle[index].Stop(); // Stop object grab particle animation for a specific object
    }

    // Method to enable snapped particle system at a specific position
    public void EnableSnappedParticle(Transform targetTransform)
    {
        snappedParticle.transform.position = targetTransform.position; // Set snapped particle position
        snappedParticle.Play(); // Play snapped particle system
    }

    // Method to disable snapped particle system
    private void DisableSnappedParticle()
    {
        snappedParticle.Stop(); // Stop snapped particle system
    }
}
