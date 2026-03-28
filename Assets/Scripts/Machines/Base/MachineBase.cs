using System;
using UnityEngine;
using Frezerka.Machines.Interfaces;
using Frezerka.Utility;

namespace Frezerka.Machines.Base
{
    public abstract class MachineBase : MonoBehaviour, IMachine
    {
        [Header("Machine Settings")]
        [SerializeField] protected string machineId;
        [SerializeField] protected AudioSource machineAudio;

        public string MachineId => machineId;
        public abstract MachineType MachineType { get; }
        public MachineState CurrentState { get; protected set; } = MachineState.Off;
        public bool IsPoweredOn => CurrentState != MachineState.Off;

        public event Action<MachineState, MachineState> OnStateChanged;

        protected void SetState(MachineState newState)
        {
            var oldState = CurrentState;
            if (oldState == newState) return;

            CurrentState = newState;
            OnStateChanged?.Invoke(oldState, newState);

            EventBus.Publish(new MachineStateChangedEvent
            {
                MachineId = machineId,
                OldState = (int)oldState,
                NewState = (int)newState
            });

            Debug.Log($"[{GetType().Name}] State: {oldState} → {newState}");
        }

        public virtual void PowerOn()
        {
            if (CurrentState != MachineState.Off) return;
            SetState(MachineState.Idle);
        }

        public virtual void PowerOff()
        {
            SetState(MachineState.Off);
        }

        public virtual void EmergencyStop()
        {
            SetState(MachineState.Emergency);
        }
    }
}
