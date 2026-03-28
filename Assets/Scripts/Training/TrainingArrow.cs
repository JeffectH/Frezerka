using UnityEngine;

namespace Frezerka.Training
{
    public class TrainingArrow : MonoBehaviour
    {
        [Header("Arrow Settings")]
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private float heightOffset = 1.5f;
        [SerializeField] private float bobSpeed = 2f;
        [SerializeField] private float bobAmount = 0.2f;

        private GameObject _arrowInstance;
        private Transform _target;
        private float _bobTime;

        public void PointAt(Transform target)
        {
            _target = target;

            if (_arrowInstance == null)
            {
                if (arrowPrefab != null)
                    _arrowInstance = Instantiate(arrowPrefab);
                else
                {
                    // Fallback: create a simple arrow from primitive
                    _arrowInstance = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    _arrowInstance.name = "TrainingArrow";
                    _arrowInstance.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);

                    var renderer = _arrowInstance.GetComponent<Renderer>();
                    if (renderer != null)
                        renderer.material.color = Color.green;

                    // Remove collider so it doesn't interfere
                    var col = _arrowInstance.GetComponent<Collider>();
                    if (col != null) Destroy(col);
                }
            }

            _arrowInstance.SetActive(true);
            _bobTime = 0f;
        }

        public void Hide()
        {
            if (_arrowInstance != null)
                _arrowInstance.SetActive(false);
            _target = null;
        }

        private void Update()
        {
            if (_target == null || _arrowInstance == null || !_arrowInstance.activeSelf) return;

            _bobTime += Time.deltaTime * bobSpeed;
            float bob = Mathf.Sin(_bobTime) * bobAmount;

            _arrowInstance.transform.position = _target.position + Vector3.up * (heightOffset + bob);

            // Billboard toward camera
            var cam = Camera.main;
            if (cam != null)
            {
                _arrowInstance.transform.LookAt(cam.transform);
                _arrowInstance.transform.Rotate(0, 180, 0);
            }
        }

        private void OnDestroy()
        {
            if (_arrowInstance != null)
                Destroy(_arrowInstance);
        }
    }
}
