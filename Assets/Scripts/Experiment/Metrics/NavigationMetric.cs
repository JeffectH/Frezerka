using System.Collections.Generic;
using UnityEngine;

namespace Frezerka.Experiment.Metrics
{
    public class NavigationMetric
    {
        private List<PositionSample> _samples = new List<PositionSample>();
        private float _totalDistance;
        private Vector3 _lastPosition;
        private bool _hasLastPosition;

        // Heatmap zones: tracked via trigger colliders calling RecordZoneEnter/Exit
        private Dictionary<string, ZoneData> _zones = new Dictionary<string, ZoneData>();
        private Dictionary<string, float> _zoneEnterTimes = new Dictionary<string, float>();

        public void Reset()
        {
            _samples.Clear();
            _totalDistance = 0f;
            _hasLastPosition = false;
            _zones.Clear();
            _zoneEnterTimes.Clear();
        }

        public void SamplePosition(Vector3 position, float time)
        {
            _samples.Add(new PositionSample
            {
                t = time,
                x = position.x,
                y = position.y,
                z = position.z
            });

            if (_hasLastPosition)
            {
                float dist = Vector3.Distance(position, _lastPosition);
                _totalDistance += dist;
            }

            _lastPosition = position;
            _hasLastPosition = true;
        }

        public void RecordZoneEnter(string zoneId, float time)
        {
            _zoneEnterTimes[zoneId] = time;

            if (!_zones.ContainsKey(zoneId))
                _zones[zoneId] = new ZoneData { timeSeconds = 0f, visits = 0 };
            _zones[zoneId].visits++;
        }

        public void RecordZoneExit(string zoneId, float time)
        {
            if (_zoneEnterTimes.TryGetValue(zoneId, out float enterTime))
            {
                float duration = time - enterTime;
                if (!_zones.ContainsKey(zoneId))
                    _zones[zoneId] = new ZoneData();
                _zones[zoneId].timeSeconds += duration;
                _zoneEnterTimes.Remove(zoneId);
            }
        }

        public NavigationData GetNavigationData()
        {
            return new NavigationData
            {
                totalDistanceMeters = _totalDistance,
                positionSamples = new List<PositionSample>(_samples),
                sampleIntervalSeconds = _samples.Count > 1
                    ? _samples[1].t - _samples[0].t
                    : 1f,
                heatmapZones = new Dictionary<string, ZoneData>(_zones)
            };
        }
    }
}
