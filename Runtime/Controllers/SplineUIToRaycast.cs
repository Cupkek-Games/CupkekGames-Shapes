using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace CupkekGames.Shapes
{
    public class SplineUIToRaycast
    {
        // References
        private VisualElement _visualElement;
        private PolylineSpline _polylineSpline;
        public PolylineSpline PolylineSpline => _polylineSpline;
        private Camera _camera;
        // Settings
        private LayerMask _layerMask;
        private float _depth = 2f;
        private Vector3 _offsetStartTangent = Vector3.zero;
        private Vector3 _offsetEndTangent = new Vector3(0, 2, 0);
        private float _endTangentLerp = 0.5f;
        private float _endFixedHeight = 0f;
        // Where the arc ends. The default raycasts the mouse on _layerMask;
        // a consumer that already raycasts the pointer each frame supplies
        // its answer here instead (null = no ground under the pointer).
        private Func<Vector3?> _endPointProvider;
        // Where the arc starts. The default projects the element's centre
        // _depth units into the scene; a consumer can place it instead.
        private Func<Vector3?> _startPointProvider;
        // State
        private Coroutine _updateCoroutine;
        // Unscaled: the arc follows the pointer while the game is paused.
        private WaitForSecondsRealtime _internal = new WaitForSecondsRealtime(0.02f);
        private float _minDistance = 0.1f;
        private Vector3 _lastEndPosition;
        private Vector3 _lastStartPosition;
        // False until the arc is drawn after a Start: a new aim draws at once, wherever it ends.
        private bool _drawn;
        public event Action OnUpdate;
        public SplineUIToRaycast(PolylineSpline polylineSpline, Camera mainCamera)
        {
            _polylineSpline = polylineSpline;
            _camera = mainCamera;

            _layerMask = Physics.DefaultRaycastLayers;
        }
        public void SetVisualElement(VisualElement visualElement)
        {
            _visualElement = visualElement;
        }
        public void SetLayerMask(LayerMask layerMask)
        {
            _layerMask = layerMask;
        }
        public void SetEndPointProvider(Func<Vector3?> provider)
        {
            _endPointProvider = provider;
        }
        public void SetStartPointProvider(Func<Vector3?> provider)
        {
            _startPointProvider = provider;
        }
        public void SetDepth(float depth)
        {
            _depth = depth;
        }
        public void SetOffsetStartTangent(Vector3 offset)
        {
            _offsetStartTangent = offset;
        }
        public void SetOffsetEndTangent(Vector3 offset)
        {
            _offsetEndTangent = offset;
        }
        public void SetEndTangentLerp(float lerp)
        {
            _endTangentLerp = Mathf.Clamp01(lerp);
        }
        public void SetEndFixedHeight(float height)
        {
            _endFixedHeight = height;
        }
        public void SetGradient(Gradient gradient)
        {
            _polylineSpline.ColorGradient = gradient;
        }
        public void SetMinDistance(float minDistance)
        {
            _minDistance = minDistance;
        }
        public void SetInternal(float internalTimeSeconds)
        {
            _internal = new WaitForSecondsRealtime(internalTimeSeconds);
        }
        public void Start()
        {
            _drawn = false;
            _updateCoroutine = _polylineSpline.StartCoroutine(UpdateCoroutine());
        }
        public void Stop()
        {
            if (_updateCoroutine != null)
            {
                _polylineSpline.StopCoroutine(_updateCoroutine);
                _updateCoroutine = null;
            }
        }
        public void Clear()
        {
            _polylineSpline.Clear();
        }

        private IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                Vector3? endPoint = _endPointProvider != null ? _endPointProvider() : RaycastEndPoint();

                Vector3? startPoint = endPoint.HasValue ? (_startPointProvider != null ? _startPointProvider() : ProjectedStartPoint()) : null;
                if (endPoint.HasValue && startPoint.HasValue)
                {
                    Vector3 end = endPoint.Value;
                    end.y = _endFixedHeight;
                    Vector3 start = startPoint.Value;

                    // Redrawn when either end moved: the pointer, or the hand it is thrown from.
                    if (!_drawn || Vector3.Distance(end, _lastEndPosition) > _minDistance || Vector3.Distance(start, _lastStartPosition) > _minDistance)
                    {
                        _drawn = true;
                        _lastEndPosition = end;
                        _lastStartPosition = start;

                        Vector3 interpolatedPosition = Vector3.Lerp(start, end, _endTangentLerp);

                        _polylineSpline.StartPosition = start;
                        _polylineSpline.StartTangent = start + _offsetStartTangent;
                        _polylineSpline.EndPosition = end;
                        _polylineSpline.EndTangent = interpolatedPosition + _offsetEndTangent;
                        _polylineSpline.Calculate();

                        OnUpdate?.Invoke();
                    }
                }

                yield return _internal;
            }
        }

        private Vector3? RaycastEndPoint()
        {
            Camera camera = ActiveCamera;
            if (camera == null) return null;
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Ray mouseRay = camera.ScreenPointToRay(mouseScreenPosition);
            return Physics.Raycast(mouseRay, out RaycastHit hit, 1000f, _layerMask) ? hit.point : null;
        }

        private Vector3? ProjectedStartPoint()
        {
            Camera camera = ActiveCamera;
            if (camera == null) return null;
            Rect rect = _visualElement.worldBound;
            return camera.ScreenToWorldPoint(new Vector3(rect.center.x, Screen.height - rect.center.y, _depth));
        }

        // The camera handed in may die with a scene boot; fall back to the live main camera.
        private Camera ActiveCamera => _camera != null ? _camera : Camera.main;
    }
}
