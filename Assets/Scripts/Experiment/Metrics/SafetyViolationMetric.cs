using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frezerka.Experiment.Metrics
{
    public class SafetyViolationMetric
    {
        private List<SafetyViolationData> _violations = new List<SafetyViolationData>();

        public void Reset()
        {
            _violations.Clear();
        }

        public void RecordViolation(string violationType, string activeStepId,
            Vector3 playerPosition, string description)
        {
            _violations.Add(new SafetyViolationData
            {
                timestamp = DateTime.UtcNow.ToString("o"),
                violationType = violationType,
                activeStepId = activeStepId,
                playerPosition = Vec3Data.From(playerPosition),
                description = description
            });
        }

        public List<SafetyViolationData> GetViolations()
        {
            return new List<SafetyViolationData>(_violations);
        }

        public int TotalViolations => _violations.Count;
    }
}
