using UnityEngine;

namespace Frezerka.Machines.Milling
{
    public class MillingCoolantSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ParticleSystem coolantParticles;
        [SerializeField] private AudioSource coolantAudio;

        private bool _isActive;

        public bool IsActive => _isActive;

        public void StartCoolant()
        {
            _isActive = true;

            if (coolantParticles != null)
                coolantParticles.Play();

            if (coolantAudio != null)
                coolantAudio.Play();

            Debug.Log("[MillingCoolant] Started");
        }

        public void StopCoolant()
        {
            _isActive = false;

            if (coolantParticles != null)
                coolantParticles.Stop();

            if (coolantAudio != null)
                coolantAudio.Stop();

            Debug.Log("[MillingCoolant] Stopped");
        }

        public void Toggle()
        {
            if (_isActive)
                StopCoolant();
            else
                StartCoolant();
        }
    }
}
