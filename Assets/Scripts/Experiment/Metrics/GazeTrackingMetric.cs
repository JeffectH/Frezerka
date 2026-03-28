using System.Collections.Generic;

namespace Frezerka.Experiment.Metrics
{
    public class GazeTrackingMetric
    {
        private Dictionary<string, float> _dwellTimes = new Dictionary<string, float>();
        private string _currentTarget;
        private float _lastGazeTime;

        public void Reset()
        {
            _dwellTimes.Clear();
            _currentTarget = null;
            _lastGazeTime = 0f;
        }

        public void RecordGaze(string targetId, float timestamp)
        {
            if (string.IsNullOrEmpty(targetId))
            {
                // Looking at nothing
                if (_currentTarget != null && _lastGazeTime > 0f)
                {
                    float duration = timestamp - _lastGazeTime;
                    AddDwellTime(_currentTarget, duration);
                }
                _currentTarget = null;
                _lastGazeTime = timestamp;
                return;
            }

            if (targetId != _currentTarget)
            {
                // Target changed
                if (_currentTarget != null && _lastGazeTime > 0f)
                {
                    float duration = timestamp - _lastGazeTime;
                    AddDwellTime(_currentTarget, duration);
                }

                _currentTarget = targetId;
                _lastGazeTime = timestamp;
            }
        }

        private void AddDwellTime(string targetId, float duration)
        {
            if (!_dwellTimes.ContainsKey(targetId))
                _dwellTimes[targetId] = 0f;
            _dwellTimes[targetId] += duration;
        }

        public GazeData GetGazeData(float sampleInterval)
        {
            // Flush current target
            if (_currentTarget != null && _lastGazeTime > 0f)
            {
                AddDwellTime(_currentTarget, UnityEngine.Time.time - _lastGazeTime);
            }

            return new GazeData
            {
                sampleIntervalSeconds = sampleInterval,
                targetDwellTimes = new Dictionary<string, float>(_dwellTimes)
            };
        }
    }
}
