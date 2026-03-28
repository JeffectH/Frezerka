using UnityEngine;
using Frezerka.Machines.Interfaces;
using Frezerka.Machines.Lathe;

namespace Frezerka.Machines.Shared
{
    public class WorkpieceGenerator : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject cylinderPrefab;
        [SerializeField] private GameObject cubePrefab;

        [Header("Default Settings")]
        [SerializeField] private Material workpieceMaterial;

        private int _workpieceCounter;

        public LatheWorkpiece GenerateLathePiece(WorkpieceParams parameters, Transform parent)
        {
            _workpieceCounter++;
            string id = $"workpiece_lathe_{_workpieceCounter}";

            GameObject obj;
            if (cylinderPrefab != null)
            {
                obj = Instantiate(cylinderPrefab, parent);
            }
            else
            {
                // Fallback: use Unity primitive cylinder
                obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                obj.transform.SetParent(parent);
            }

            obj.name = id;

            if (workpieceMaterial != null)
            {
                var renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                    renderer.material = workpieceMaterial;
            }

            var workpiece = obj.AddComponent<LatheWorkpiece>();
            workpiece.Initialize(id, parameters);

            Debug.Log($"[WorkpieceGenerator] Generated lathe workpiece: {id}");
            return workpiece;
        }

        public GameObject GenerateMillingPiece(WorkpieceParams parameters, Transform parent)
        {
            _workpieceCounter++;
            string id = $"workpiece_milling_{_workpieceCounter}";

            GameObject obj;
            if (cubePrefab != null)
            {
                obj = Instantiate(cubePrefab, parent);
            }
            else
            {
                obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.SetParent(parent);
            }

            obj.name = id;

            // Scale: convert mm to Unity units
            float l = parameters.Length / 1000f;
            float w = parameters.Width / 1000f;
            float h = parameters.Height / 1000f;
            obj.transform.localScale = new Vector3(l, h, w);
            obj.transform.localPosition = Vector3.zero;

            if (workpieceMaterial != null)
            {
                var renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                    renderer.material = workpieceMaterial;
            }

            Debug.Log($"[WorkpieceGenerator] Generated milling workpiece: {id}");
            return obj;
        }
    }
}
