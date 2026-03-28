using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Frezerka.Utility;

namespace Frezerka.UI
{
    public class WorkpieceParameterUI : MonoBehaviour
    {
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField diameterInput;
        [SerializeField] private TMP_InputField lengthInput;
        [SerializeField] private TMP_InputField widthInput;
        [SerializeField] private TMP_InputField heightInput;
        [SerializeField] private TMP_InputField spindleDepthInput;

        [Header("Buttons")]
        [SerializeField] private Button generateButton;

        [Header("Labels (for lathe/milling switching)")]
        [SerializeField] private GameObject latheParamsGroup;
        [SerializeField] private GameObject millingParamsGroup;

        private bool _isLathe = true;

        private void Start()
        {
            if (generateButton != null)
                generateButton.onClick.AddListener(OnGenerateClicked);

            // Hook up value changed events for experiment tracking
            if (diameterInput != null)
                diameterInput.onEndEdit.AddListener(val => PublishParamInput("workpiece_param_panel.diameter", val));
            if (lengthInput != null)
                lengthInput.onEndEdit.AddListener(val => PublishParamInput("workpiece_param_panel.length", val));
            if (widthInput != null)
                widthInput.onEndEdit.AddListener(val => PublishParamInput("mill_param_panel.width", val));
            if (heightInput != null)
                heightInput.onEndEdit.AddListener(val => PublishParamInput("mill_param_panel.height", val));
            if (spindleDepthInput != null)
                spindleDepthInput.onEndEdit.AddListener(val => PublishParamInput("workpiece_param_panel.depth", val));
        }

        public void SetMode(bool isLathe)
        {
            _isLathe = isLathe;
            if (latheParamsGroup != null) latheParamsGroup.SetActive(isLathe);
            if (millingParamsGroup != null) millingParamsGroup.SetActive(!isLathe);
        }

        private void OnGenerateClicked()
        {
            string btnId = _isLathe ? "workpiece_param_panel.generate_btn" : "mill_param_panel.generate_btn";
            EventBus.Publish(new InteractionEvent
            {
                InteractionId = btnId,
                InteractionType = "Click",
                Timestamp = Time.time
            });

            Debug.Log("[WorkpieceParameterUI] Generate clicked");
        }

        private void PublishParamInput(string paramId, string value)
        {
            EventBus.Publish(new InteractionEvent
            {
                InteractionId = paramId,
                InteractionType = "NumpadInput",
                Value = value,
                Timestamp = Time.time
            });
        }
    }
}
