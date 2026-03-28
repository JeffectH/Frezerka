using System;
using UnityEngine;

namespace Frezerka.Machines.Interfaces
{
    [Serializable]
    public struct WorkpieceParams
    {
        public float Diameter;
        public float Length;
        public float Width;
        public float Height;
        public float SpindleDepth;
    }

    [Serializable]
    public struct CutOperation
    {
        public int PassNumber;
        public float TargetDiameter;
        public float TargetLength;
        public float Depth;
        public float StartPosition;
        public float EndPosition;
        public string ToolUsed;
    }

    public interface IWorkpiece
    {
        string WorkpieceId { get; }
        WorkpieceParams Parameters { get; }
        void ApplyCut(CutOperation cut);
        bool IsFinished { get; }
        Transform Transform { get; }
    }
}
