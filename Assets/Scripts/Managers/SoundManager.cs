using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        // List of audio sources
        [SerializeField] private List<AudioSource> audioSource; // List of audio sources.

        // Play duck sound
        public void PlayDuckSound() => audioSource[0].Play();
        
        // Play smoke sound
        public void PlaySmokeSound() => audioSource[1].Play();
        
        // Play victory sound
        public void PlayVictorySound() => audioSource[2].Play();
        
        // Play stars sound
        public void PlayStarsSound() => audioSource[3].Play();
        
        // Stop stars sound
        public void DisableStarsSound() => audioSource[3].Stop();
        
        // Play water splash sound
        public void PlayWaterSplashSound() => audioSource[4].Play();
        
        // Play wrong sound
        public void PlayWrongSound() =>  audioSource[5].Play();

        // Play correct snap sounds
        public void PlayCorrectSnapSounds()
        {
            PlayWaterSplashSound();
            PlayDuckSound();
            PlayVictorySound();
        }

        // Disappear smoke sound with delay
        public void DisappearSmokeSound() => StartCoroutine(SmokeSoundWithDelay());

        // Coroutine for smoke sound with delay
        private IEnumerator SmokeSoundWithDelay()
        {
            yield return new WaitForSecondsRealtime(0.7f);
            audioSource[1].Play();
        }
    }
}