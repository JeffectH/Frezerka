using System.Collections.Generic;

namespace Frezerka.Experiment.Metrics
{
    public class HesitationMetric
    {
        private Dictionary<string, float> _stepEnterTimes = new Dictionary<string, float>();
        private Dictionary<string, float> _firstInteractionTimes = new Dictionary<string, float>();
        private Dictionary<string, string> _gazeBeforeAction = new Dictionary<string, string>();
        private Dictionary<string, float> _gazeStartTimes = new Dictionary<string, float>();
        private Dictionary<string, int> _lookAwayCounts = new Dictionary<string, int>();

        private string _currentStepId;
        private string _lastGazeTarget;

        public void Reset()
        {
            _stepEnterTimes.Clear();
            _firstInteractionTimes.Clear();
            _gazeBeforeAction.Clear();
            _gazeStartTimes.Clear();
            _lookAwayCounts.Clear();
            _currentStepId = null;
            _lastGazeTarget = null;
        }

        public void OnStepEntered(string stepId, float time)
        {
            _currentStepId = stepId;
            _stepEnterTimes[stepId] = time;
            _lookAwayCounts[stepId] = 0;
            _lastGazeTarget = null;
        }

        public void RecordInteraction(string interactionId, float time)
        {
            if (_currentStepId == null) return;

            if (!_firstInteractionTimes.ContainsKey(_currentStepId))
            {
                _firstInteractionTimes[_currentStepId] = time;
            }
        }

        public void RecordGaze(string targetId, float time)
        {
            if (_currentStepId == null) return;

            // Track gaze target before first interaction
            if (!_firstInteractionTimes.ContainsKey(_currentStepId))
            {
                if (!_gazeBeforeAction.ContainsKey(_currentStepId))
                {
                    _gazeBeforeAction[_currentStepId] = targetId;
                    _gazeStartTimes[_currentStepId] = time;
                }

                // Count look-aways
                if (_lastGazeTarget != null && targetId != _lastGazeTarget)
                {
                    _lookAwayCounts[_currentStepId]++;
                }
            }

            _lastGazeTarget = targetId;
        }

        public HesitationData GetHesitationData(string stepId)
        {
            var data = new HesitationData();

            if (_stepEnterTimes.TryGetValue(stepId, out float enterTime))
            {
                if (_firstInteractionTimes.TryGetValue(stepId, out float firstTime))
                {
                    data.timeBeforeFirstInteractionSeconds = firstTime - enterTime;
                }
            }

            if (_gazeBeforeAction.TryGetValue(stepId, out string gazeTarget))
            {
                data.gazeTargetBeforeAction = gazeTarget;
            }

            if (_gazeStartTimes.TryGetValue(stepId, out float gazeStart) &&
                _firstInteractionTimes.TryGetValue(stepId, out float interactionTime))
            {
                data.gazeDurationBeforeActionSeconds = interactionTime - gazeStart;
            }

            data.lookAwayCount = _lookAwayCounts.GetValueOrDefault(stepId, 0);

            return data;
        }
    }
}
