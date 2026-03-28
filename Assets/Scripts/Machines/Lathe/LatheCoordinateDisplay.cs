using UnityEngine;
using TMPro;

namespace Frezerka.Machines.Lathe
{
    public class LatheCoordinateDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LatheCarriage carriage;
        [SerializeField] private TextMeshProUGUI xCoordText;
        [SerializeField] private TextMeshProUGUI zCoordText;

        [Header("Settings")]
        [SerializeField] private float displayScale = 1000f; // Convert to mm
        [SerializeField] private int decimalPlaces = 2;

        private void OnEnable()
        {
            if (carriage != null)
                carriage.OnPositionChanged += UpdateDisplay;
        }

        private void OnDisable()
        {
            if (carriage != null)
                carriage.OnPositionChanged -= UpdateDisplay;
        }

        private void UpdateDisplay(UnityEngine.Vector3 position)
        {
            float x = position.x * displayScale;
            float z = position.z * displayScale;

            if (xCoordText != null)
                xCoordText.text = $"X: {x.ToString($"F{decimalPlaces}")}";
            if (zCoordText != null)
                zCoordText.text = $"Z: {z.ToString($"F{decimalPlaces}")}";
        }

        public void ResetCoordinates()
        {
            carriage?.ResetPosition();
        }
    }
}
