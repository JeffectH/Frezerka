using System;
using System.Collections.Generic;
using System.Linq;

namespace Frezerka.Experiment.Metrics
{
    public class ErrorTrackingMetric
    {
        private List<StepError> _allErrors = new List<StepError>();
        private Dictionary<string, List<StepError>> _errorsByStep = new Dictionary<string, List<StepError>>();

        public void Reset()
        {
            _allErrors.Clear();
            _errorsByStep.Clear();
        }

        public void RecordError(string stepId, string errorType, string details, float stepTimeAtError)
        {
            var error = new StepError
            {
                timestamp = DateTime.UtcNow.ToString("o"),
                errorType = errorType,
                details = details,
                stepTimeAtErrorSeconds = stepTimeAtError
            };

            _allErrors.Add(error);

            if (!_errorsByStep.ContainsKey(stepId))
                _errorsByStep[stepId] = new List<StepError>();
            _errorsByStep[stepId].Add(error);
        }

        public List<StepError> GetStepErrors(string stepId)
        {
            return _errorsByStep.GetValueOrDefault(stepId, new List<StepError>());
        }

        public int TotalErrors => _allErrors.Count;

        public Dictionary<string, int> GetErrorCountsByType()
        {
            return _allErrors
                .GroupBy(e => e.errorType)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
