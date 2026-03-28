using System;
using System.Collections.Generic;

namespace Frezerka.Utility
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        public static void Subscribe<T>(Action<T> handler) where T : struct
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();

            _subscribers[type].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            var type = typeof(T);
            if (_subscribers.ContainsKey(type))
                _subscribers[type].Remove(handler);
        }

        public static void Publish<T>(T eventData) where T : struct
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type)) return;

            // Iterate a copy to allow modifications during iteration
            var handlers = _subscribers[type].ToArray();
            foreach (var handler in handlers)
            {
                ((Action<T>)handler)?.Invoke(eventData);
            }
        }

        public static void Clear()
        {
            _subscribers.Clear();
        }
    }

    // --- Event Definitions ---

    public struct InteractionEvent
    {
        public string InteractionId;
        public string InteractionType;
        public UnityEngine.Vector3 PlayerPosition;
        public UnityEngine.Quaternion PlayerRotation;
        public float Timestamp;
        public string Value;
    }

    public struct StepChangedEvent
    {
        public string PreviousStepId;
        public string NewStepId;
        public int StepIndex;
        public int TotalSteps;
    }

    public struct StepCompletedEvent
    {
        public string StepId;
        public int StepIndex;
        public float DurationSeconds;
    }

    public struct MachineStateChangedEvent
    {
        public string MachineId;
        public int OldState;
        public int NewState;
    }

    public struct SafetyViolationEvent
    {
        public string ViolationType;
        public string ActiveStepId;
        public UnityEngine.Vector3 PlayerPosition;
        public string Description;
    }

    public struct ErrorEvent
    {
        public string StepId;
        public string ErrorType;
        public string Details;
        public float StepTimeAtError;
    }

    public struct SessionEvent
    {
        public enum SessionEventType { Started, Ended, Paused, Resumed }
        public SessionEventType EventType;
        public string ParticipantId;
        public string MachineType;
        public string SessionMode;
    }

    public struct EmergencyEvent
    {
        public string EventType;
        public float ReactionTimeSeconds;
        public bool HandledCorrectly;
    }

    public struct GazeEvent
    {
        public string TargetId;
        public UnityEngine.Vector3 HitPoint;
        public float Distance;
        public float Timestamp;
    }
}
