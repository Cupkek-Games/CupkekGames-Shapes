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
        // State
        private Coroutine _updateCoroutine;
        private WaitForSeconds _internal = new WaitForSeconds(0.02f);
        private float _minDistance = 0.1f;
        private Vector3 _lastEndPosition;
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
            _internal = new WaitForSeconds(internalTimeSeconds);
        }
        public void Start()
        {
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
                Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

                Ray mouseRay = _camera.ScreenPointToRay(mouseScreenPosition);

                if (Physics.Raycast(mouseRay, out RaycastHit hit, 1000f, _layerMask))
                {
                    Vector3 end = hit.point;
                    end.y = _endFixedHeight;

                    // distance between end and last end
                    float distance = Vector3.Distance(end, _lastEndPosition);
                    if (distance > _minDistance)
                    {
                        _lastEndPosition = end;

                        Rect rect = _visualElement.worldBound;
                        Vector3 screenPosition = new Vector3(rect.center.x, Screen.height - rect.center.y, _depth);
                        Vector3 start = _camera.ScreenToWorldPoint(screenPosition);

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
    }
}
