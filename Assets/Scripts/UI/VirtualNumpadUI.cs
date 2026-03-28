using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Frezerka.UI
{
    public class VirtualNumpadUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private Button[] digitButtons; // 0-9
        [SerializeField] private Button dotButton;
        [SerializeField] private Button backspaceButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private string _currentInput = "";
        private Action<string> _onConfirm;
        private Action _onCancel;

        private void Start()
        {
            for (int i = 0; i < digitButtons.Length && i < 10; i++)
            {
                int digit = i;
                if (digitButtons[i] != null)
                    digitButtons[i].onClick.AddListener(() => AppendDigit(digit.ToString()));
            }

            if (dotButton != null)
                dotButton.onClick.AddListener(() => AppendDigit("."));
            if (backspaceButton != null)
                backspaceButton.onClick.AddListener(Backspace);
            if (confirmButton != null)
                confirmButton.onClick.AddListener(Confirm);
            if (cancelButton != null)
                cancelButton.onClick.AddListener(Cancel);

            gameObject.SetActive(false);
        }

        public void Show(Action<string> onConfirm, Action onCancel = null, string initialValue = "")
        {
            _onConfirm = onConfirm;
            _onCancel = onCancel;
            _currentInput = initialValue;
            UpdateDisplay();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _onConfirm = null;
            _onCancel = null;
        }

        private void AppendDigit(string digit)
        {
            if (digit == "." && _currentInput.Contains(".")) return;
            if (_currentInput.Length >= 10) return;

            _currentInput += digit;
            UpdateDisplay();
        }

        private void Backspace()
        {
            if (_currentInput.Length > 0)
                _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            UpdateDisplay();
        }

        private void Confirm()
        {
            _onConfirm?.Invoke(_currentInput);
            Hide();
        }

        private void Cancel()
        {
            _onCancel?.Invoke();
            Hide();
        }

        private void UpdateDisplay()
        {
            if (displayText != null)
                displayText.text = _currentInput.Length > 0 ? _currentInput : "0";
        }
    }
}
