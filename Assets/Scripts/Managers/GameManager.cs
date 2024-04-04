using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        // Reference to DuckManager
        [SerializeField] private DuckManager duckManager; // Reference to the DuckManager.
        
        // Reference to BucketManager
        [SerializeField] private BucketManager bucketManager; // Reference to the BucketManager.
        
        // Reference to ColorManager
        [SerializeField] private ColorManager colorManager; // Reference to the ColorManager.
        
        // Reference to SoundManager
        [SerializeField] private SoundManager soundManager; // Reference to the SoundManager.
        
        // Reference to ParticleManager
        [SerializeField] private ParticleManager particleManager; // Reference to the ParticleManager.
        
        // Reference to UiManager
        [SerializeField] private UiManager uiManager; // Reference to the UiManager.
        
        // Reference to HintManager
        [SerializeField] private HintManager hintManager; // Reference to the HintManager.
        
        // Reference to LevelManager
        [SerializeField] private LevelManager levelManager; // Reference to the LevelManager.

        [SerializeField] private DuckMoveData data;
        // Subscribe to events when the object is enabled
        private void OnEnable()
        {
            // Subscribe to particle start event to play smoke sound
            particleManager.OnParticleStart += soundManager.PlaySmokeSound;
            
            // Subscribe to particle finished event to spawn buckets
            particleManager.OnParticleFinished += bucketManager.BucketSpawner.SpawnBuckets;
            
            // Subscribe to bucket spawn event to set active ducks
            bucketManager.BucketSpawner.OnBucketsSpawn += duckManager.DuckSpawner.SetActiveDucks;
            
            // Subscribe to duck spawn event to disable star particles
            duckManager.DuckSpawner.OnDucksSpawn += particleManager.DisableStarParticles;
            
            // Subscribe to duck align event
            duckManager.DuckMovement.OnDuckAlign += DuckAlignEvent;
            
            // Subscribe to events for grabbed ducks
            foreach (var dragged in duckManager.SnappedDucks) dragged.OnObjGrab += GrabEvent;
            
            // Subscribe to events for dropped ducks
            foreach (var dropped in duckManager.SnappedDucks) dropped.OnObjDrop += DropEvent;
            
            // Subscribe to events for snapped ducks' transform change
            foreach (var snapped in duckManager.SnappedDucks) snapped.OnTransformChange += particleManager.EnableSnappedParticle;
            
            // Subscribe to events for correct snap
            foreach (var snapped in duckManager.SnappedDucks) snapped.OnCorrectSnap += CorrectEvent;
            
            // Subscribe to snap index increase event
            levelManager.OnSnapIndexIncrease += FinishLevel;
            
            // Subscribe to level change event
            levelManager.OnLevelChange += GoToNextLevel;
        }

        // Unsubscribe from events when the object is disabled
        private void OnDisable()
        {
            // Unsubscribe from particle start event to play smoke sound
            particleManager.OnParticleStart -= soundManager.PlaySmokeSound;
            
            // Unsubscribe from particle finished event to spawn buckets
            particleManager.OnParticleFinished -= bucketManager.BucketSpawner.SpawnBuckets;
            
            // Unsubscribe from bucket spawn event to set active ducks
            bucketManager.BucketSpawner.OnBucketsSpawn -= duckManager.DuckSpawner.SetActiveDucks;
            
            // Unsubscribe from duck spawn event to disable star particles
            duckManager.DuckSpawner.OnDucksSpawn -= particleManager.DisableStarParticles;
            
            // Unsubscribe from duck align event
            duckManager.DuckMovement.OnDuckAlign -= DuckAlignEvent;
            
            // Unsubscribe from events for grabbed ducks
            foreach (var dragged in duckManager.SnappedDucks) dragged.OnObjGrab -= GrabEvent;
            
            // Unsubscribe from events for dropped ducks
            foreach (var dropped in duckManager.SnappedDucks) dropped.OnObjDrop -= DropEvent;
            
            // Unsubscribe from events for snapped ducks' transform change
            foreach (var snapped in duckManager.SnappedDucks) snapped.OnTransformChange -= particleManager.EnableSnappedParticle;
            
            // Unsubscribe from events for correct snap
            foreach (var snapped in duckManager.SnappedDucks) snapped.OnCorrectSnap -= CorrectEvent;
            
            // Unsubscribe from snap index increase event
            levelManager.OnSnapIndexIncrease -= FinishLevel;
            
            // Unsubscribe from level change event
            levelManager.OnLevelChange -= GoToNextLevel;
        }

        // Duck align event handler
        private void DuckAlignEvent()
        {
            soundManager.PlayDuckSound();
            hintManager.ActivateTimer();
        }

        // Grab event handler
        private void GrabEvent(int index)
        {
            particleManager.GrabObjectParticle(index);
            soundManager.PlayStarsSound();
        }

        // Drop event handler
        private void DropEvent(int index)
        {
            particleManager.DisableDuckStarParticle(index);
            soundManager.DisableStarsSound();
            soundManager.PlayWrongSound();
        }

        // Correct event handler
        private void CorrectEvent()
        {
            levelManager.IncrementSnapIndex();
            soundManager.PlayCorrectSnapSounds();
            soundManager.PlayWaterSplashSound();
            hintManager.HintIndexHandler();
        }

        // Finish level event handler
        private void FinishLevel()
        {
            soundManager.PlayVictorySound();
            particleManager.PlayVictoryParticle();
            hintManager.StartTimer = false;
            data.IsStopped = false;
            bucketManager.BucketSpawner.DisableAllBuckets();
            duckManager.DuckSpawner.DisableDucks();
            particleManager.DisableSmokeAnimation();
            soundManager.DisappearSmokeSound();
            uiManager.FillProgressBar(levelManager.LevelIndex);
            StartCoroutine(KillTween());
        }

        // Coroutine to kill tween
        private IEnumerator KillTween()
        {
            yield return new WaitForSecondsRealtime(0.9f);
            foreach (var dragObj in duckManager.SnappedDucks) dragObj.transform.DOKill();
        }

        // Go to next level event handler
        private void GoToNextLevel()
        {
            StartCoroutine(SetNewLevel());
        }

        // Coroutine to set new level
        private IEnumerator SetNewLevel()
        {
            yield return new WaitForSecondsRealtime(1f);
            particleManager.SpawnSmokeParticle();
            duckManager.DuckSpawner.SpawnDucksToStartPoint();
            colorManager.SetColorToObjects();
        }
    }
}
