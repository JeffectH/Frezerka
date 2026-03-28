using System;
using System.Collections.Generic;

namespace Frezerka.Experiment
{
    [Serializable]
    public class ExperimentSessionData
    {
        public string sessionId;
        public string participantId;
        public string machineType;
        public string sessionMode;
        public string startTimestampUTC;
        public string endTimestampUTC;
        public float totalDurationSeconds;
        public bool scenarioCompleted;
        public string applicationVersion;

        public SessionSummary summary = new SessionSummary();
        public List<StepData> steps = new List<StepData>();
        public List<SafetyViolationData> safetyViolations = new List<SafetyViolationData>();
        public NavigationData navigationData = new NavigationData();
        public GazeData gazeData = new GazeData();
        public List<EmergencyEventData> emergencyEvents = new List<EmergencyEventData>();
    }

    [Serializable]
    public class SessionSummary
    {
        public int totalSteps;
        public int completedSteps;
        public int failedSteps;
        public int totalErrors;
        public int totalSafetyViolations;
        public float totalDistanceWalkedMeters;
        public float averageStepTimeSeconds;
        public float medianStepTimeSeconds;
    }

    [Serializable]
    public class StepData
    {
        public string stepId;
        public int stepIndex;
        public string stepNameRU;
        public string stepNameEN;
        public string startTimestamp;
        public string endTimestamp;
        public float durationSeconds;
        public string result; // Completed, Failed, Skipped
        public int attemptCount;

        public HesitationData hesitation = new HesitationData();
        public List<StepError> errors = new List<StepError>();
        public List<InteractionRecord> interactions = new List<InteractionRecord>();
    }

    [Serializable]
    public class HesitationData
    {
        public float timeBeforeFirstInteractionSeconds;
        public string gazeTargetBeforeAction;
        public float gazeDurationBeforeActionSeconds;
        public int lookAwayCount;
    }

    [Serializable]
    public class StepError
    {
        public string timestamp;
        public string errorType;
        public string details;
        public float stepTimeAtErrorSeconds;
    }

    [Serializable]
    public class InteractionRecord
    {
        public string timestamp;
        public string interactionId;
        public string interactionType;
        public string value;
        public bool wasCorrect;
        public Vec3Data playerPosition = new Vec3Data();
        public RotationData playerRotation = new RotationData();
    }

    [Serializable]
    public class SafetyViolationData
    {
        public string timestamp;
        public string violationType;
        public string activeStepId;
        public Vec3Data playerPosition = new Vec3Data();
        public string description;
    }

    [Serializable]
    public class NavigationData
    {
        public float totalDistanceMeters;
        public List<PositionSample> positionSamples = new List<PositionSample>();
        public float sampleIntervalSeconds;
        public Dictionary<string, ZoneData> heatmapZones = new Dictionary<string, ZoneData>();
    }

    [Serializable]
    public class PositionSample
    {
        public float t;
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class ZoneData
    {
        public float timeSeconds;
        public int visits;
    }

    [Serializable]
    public class GazeData
    {
        public float sampleIntervalSeconds;
        public Dictionary<string, float> targetDwellTimes = new Dictionary<string, float>();
    }

    [Serializable]
    public class EmergencyEventData
    {
        public string timestamp;
        public string eventType;
        public float reactionTimeSeconds;
        public bool handledCorrectly;
    }

    [Serializable]
    public class Vec3Data
    {
        public float x;
        public float y;
        public float z;

        public static Vec3Data From(UnityEngine.Vector3 v)
        {
            return new Vec3Data { x = v.x, y = v.y, z = v.z };
        }
    }

    [Serializable]
    public class RotationData
    {
        public float yaw;
        public float pitch;

        public static RotationData From(UnityEngine.Quaternion q)
        {
            var euler = q.eulerAngles;
            return new RotationData { yaw = euler.y, pitch = euler.x };
        }
    }
}
