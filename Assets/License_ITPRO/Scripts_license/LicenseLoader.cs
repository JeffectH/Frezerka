using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ITPRO.License
{
    public class LicenseLoader : MonoBehaviour
    {
        [SerializeField]private GameObject UI;
        [SerializeField]private GameObject load;
        [SerializeField]private Image loadProgress;

        private void Start()
        {
            StartLoad();
        }

        public void StartLoad()
        {
            if (UI != null)
                UI.SetActive(false);
            if(load != null)
                load.SetActive(true);
            StartCoroutine(AsyncLoadScene());
        }

        IEnumerator AsyncLoadScene()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);
            while (!operation.isDone)
            {
                float progress = operation.progress/0.9f;
                print(progress);
                loadProgress.fillAmount = progress;
                yield return null;
            }
        }
    }
}