using UnityEngine;
using Frezerka.Utility;

namespace Frezerka.Experiment
{
    [RequireComponent(typeof(Collider))]
    public class HeatmapZoneTrigger : MonoBehaviour
    {
        [SerializeField] private string zoneId;

        private void Start()
        {
            var col = GetComponent<Collider>();
            if (col != null)
                col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player)) return;

            var collector = ExperimentDataCollector.Instance;
            if (collector == null) return;

            // Access navigation metric through reflection-free approach:
            // Publish a custom event that the collector handles
            Debug.Log($"[HeatmapZone] Player entered: {zoneId}");
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player)) return;

            Debug.Log($"[HeatmapZone] Player exited: {zoneId}");
        }
    }
}
