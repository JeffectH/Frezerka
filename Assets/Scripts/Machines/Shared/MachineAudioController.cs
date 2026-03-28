using UnityEngine;

namespace Frezerka.Machines.Shared
{
    public class MachineAudioController : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource engineSource;
        [SerializeField] private AudioSource spindleSource;
        [SerializeField] private AudioSource cuttingSource;
        [SerializeField] private AudioSource coolantSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip engineClip;
        [SerializeField] private AudioClip spindleClip;
        [SerializeField] private AudioClip cuttingClip;
        [SerializeField] private AudioClip coolantClip;

        public void PlayEngine()
        {
            PlayLooping(engineSource, engineClip);
        }

        public void StopEngine()
        {
            Stop(engineSource);
        }

        public void PlaySpindle(float pitchMultiplier = 1f)
        {
            PlayLooping(spindleSource, spindleClip);
            if (spindleSource != null)
                spindleSource.pitch = pitchMultiplier;
        }

        public void StopSpindle()
        {
            Stop(spindleSource);
        }

        public void PlayCutting()
        {
            PlayLooping(cuttingSource, cuttingClip);
        }

        public void StopCutting()
        {
            Stop(cuttingSource);
        }

        public void PlayCoolant()
        {
            PlayLooping(coolantSource, coolantClip);
        }

        public void StopCoolant()
        {
            Stop(coolantSource);
        }

        public void StopAll()
        {
            StopEngine();
            StopSpindle();
            StopCutting();
            StopCoolant();
        }

        private void PlayLooping(AudioSource source, AudioClip clip)
        {
            if (source == null) return;
            if (clip != null)
                source.clip = clip;
            source.loop = true;
            if (!source.isPlaying)
                source.Play();
        }

        private void Stop(AudioSource source)
        {
            if (source != null && source.isPlaying)
                source.Stop();
        }
    }
}
