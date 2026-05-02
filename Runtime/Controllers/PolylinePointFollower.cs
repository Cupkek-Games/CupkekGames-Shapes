using UnityEngine;
using System.Collections;
using global::Shapes;
using System.Collections.Generic;
using System;
using CupkekGames.TimeSystem;

namespace CupkekGames.Shapes
{
    public class PolylinePointFollower
    {
        private MonoBehaviour _runner;
        private Transform _objectToMove;
        private List<PolylinePoint> _waypoints;
        private float _moveSpeed;
        private float _speedIncreaseRate;
        private float _stopThreshold;
        private TimeContext _timeContext;

        private Coroutine _coroutine;
        public event Action<Transform> OnEnd;

        public PolylinePointFollower(MonoBehaviour runner, Transform objectToMove, List<PolylinePoint> waypoints,
            float moveSpeed = 5f, float speedIncreaseRate = 0.1f, float stopThreshold = 0.1f,
            TimeContext timeContext = null)
        {
            _runner = runner;
            _objectToMove = objectToMove;
            _waypoints = waypoints;
            _moveSpeed = moveSpeed;
            _speedIncreaseRate = speedIncreaseRate;
            _stopThreshold = stopThreshold;
            _timeContext = timeContext;
        }

        public void StartFollowing()
        {
            _coroutine = _runner.StartCoroutine(FollowWaypoints());
        }

        public void StopFollowing()
        {
            if (_coroutine != null)
            {
                _runner.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator FollowWaypoints()
        {
            if (_waypoints.Count == 0)
            {
                StopFollowing();
                yield break;
            }

            float speedMultiplier = 1f;

            for (int i = 0; i < _waypoints.Count; i++)
            {
                PolylinePoint targetWaypoint = _waypoints[i];

                while (Vector3.Distance(_objectToMove.position, targetWaypoint.point) > _stopThreshold)
                {
                    float deltaTime = _timeContext?.DeltaTime ?? Time.deltaTime;
                    float currentSpeed = _moveSpeed * speedMultiplier;
                    _objectToMove.position = Vector3.MoveTowards(_objectToMove.position, targetWaypoint.point,
                        currentSpeed * deltaTime);

                    // Increase speed multiplier over time
                    speedMultiplier += _speedIncreaseRate * deltaTime;

                    yield return null;
                }
            }

            StopFollowing();
            OnEnd?.Invoke(_objectToMove);
        }
    }
}
