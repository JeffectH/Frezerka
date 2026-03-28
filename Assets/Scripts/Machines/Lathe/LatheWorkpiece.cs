using UnityEngine;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Machines.Lathe
{
    public class LatheWorkpiece : MonoBehaviour, IWorkpiece
    {
        [Header("Workpiece")]
        [SerializeField] private string workpieceId;
        [SerializeField] private WorkpieceParams parameters;
        [SerializeField] private bool isFinished;

        private int _cutCount;

        public string WorkpieceId => workpieceId;
        public WorkpieceParams Parameters => parameters;
        public bool IsFinished => isFinished;
        public Transform Transform => transform;

        public void Initialize(string id, WorkpieceParams p)
        {
            workpieceId = id;
            parameters = p;
            isFinished = false;
            _cutCount = 0;

            // Stub: set scale based on diameter and length (convert mm to Unity units)
            float diameterUnits = p.Diameter / 1000f;
            float lengthUnits = p.Length / 1000f;
            transform.localScale = new Vector3(diameterUnits, diameterUnits, lengthUnits);

            Debug.Log($"[LatheWorkpiece] Initialized: D={p.Diameter}mm, L={p.Length}mm");
        }

        public void ApplyCut(CutOperation cut)
        {
            _cutCount++;

            // Stub: visually reduce scale to simulate cutting
            if (cut.TargetDiameter > 0)
            {
                float newDiameter = cut.TargetDiameter / 1000f;
                var scale = transform.localScale;
                scale.x = newDiameter;
                scale.y = newDiameter;
                transform.localScale = scale;
            }

            if (cut.TargetLength > 0)
            {
                float newLength = cut.TargetLength / 1000f;
                var scale = transform.localScale;
                scale.z = newLength;
                transform.localScale = scale;
            }

            Debug.Log($"[LatheWorkpiece] Cut #{_cutCount} applied: pass={cut.PassNumber}, tool={cut.ToolUsed}");
        }

        public void MarkFinished()
        {
            isFinished = true;
            Debug.Log("[LatheWorkpiece] Marked as finished");
        }
    }
}
