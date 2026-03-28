using UnityEngine;

namespace Frezerka.Core
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Frezerka/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Audio")]
        [Range(0f, 1f)]
        public float masterVolume = 1f;

        [Header("Interaction")]
        public float interactionDistance = 5f;
        public float grabDistance = 2f;

        [Header("Data Collection")]
        public float positionSampleInterval = 1f;
        public float gazeSampleInterval = 0.5f;

        [Header("Visuals")]
        public Color highlightColor = Color.yellow;
        public Color trainingHighlightColor = Color.green;
        public Color dangerHighlightColor = Color.red;
    }
}
