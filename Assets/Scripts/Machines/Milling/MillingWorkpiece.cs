using UnityEngine;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Machines.Milling
{
    public class MillingWorkpiece : MonoBehaviour, IWorkpiece
    {
        [Header("Workpiece")]
        [SerializeField] private string workpieceId;
        [SerializeField] private WorkpieceParams parameters;
        [SerializeField] private bool isFinished;

        public string WorkpieceId => workpieceId;
        public WorkpieceParams Parameters => parameters;
        public bool IsFinished => isFinished;
        public Transform Transform => transform;

        public void Initialize(string id, WorkpieceParams p)
        {
            workpieceId = id;
            parameters = p;
            isFinished = false;

            // Stub: set scale (mm to Unity units)
            float l = p.Length / 1000f;
            float w = p.Width / 1000f;
            float h = p.Height / 1000f;
            transform.localScale = new Vector3(l, h, w);

            Debug.Log($"[MillingWorkpiece] Initialized: L={p.Length}mm, W={p.Width}mm, H={p.Height}mm");
        }

        public void ApplyCut(CutOperation cut)
        {
            // Stub: reduce height to simulate material removal
            var scale = transform.localScale;
            if (cut.Depth > 0)
            {
                float depthUnits = cut.Depth / 1000f;
                scale.y -= depthUnits;
                scale.y = Mathf.Max(0.001f, scale.y);
                transform.localScale = scale;
            }

            Debug.Log($"[MillingWorkpiece] Cut applied: pass={cut.PassNumber}");
        }

        public void MarkFinished()
        {
            isFinished = true;
        }
    }
}
