using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ITPRO.License
{
    public enum Error : int
    {
        Key = 0,
        Server = 1,
        Max = 2,
        Time = 3,
        App = 4,
        License = 5,
        Unknow = 6,
        Json = 7,
        Connect = 8,
        Input = 9,
    }

    public class ErrorMessages : MonoBehaviour
    {
        public List<GameObject> errors;

        [SerializeField] private List<TextMeshProUGUI> textSpecial;

        private Error errorLast;

        public void ActivateError(Error error)
        {
            errors[(int)errorLast].SetActive(false);
            errors[(int)error].SetActive(true);
            errorLast = error;
            Invoke(nameof(DeactivateError), 4);
        }

        public void ActivateSpecialError(Error error, string text)
        {
            errors[(int)errorLast].SetActive(false);
            errors[(int)error].SetActive(true);
            textSpecial[(int)error].text = text;
            errorLast = error;
            Invoke(nameof(DeactivateError), 4);
        }

        public void DeactivateError()
        {
            errors[(int)errorLast].SetActive(false);
        }
    }
}