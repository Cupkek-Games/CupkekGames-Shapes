using System.Collections.Generic;
using UnityEngine;
using CupkekGames.Singletons;
using global::Shapes;
using System.Collections;
using System;

namespace CupkekGames.Shapes
{
    public class ShapesDrawer : Singleton<ShapesDrawer>
    {
        [Tooltip("When enabled, shapes will only draw in cameras that can see the layer of this GameObject")]
        public bool useCullingMasks = false;
        private List<ShapeSequence> _shapeDrawers = new();
        private List<ShapeCooldown> _cooldowns = new();
        protected virtual void OnEnable()
        {
            UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering += DrawShapesSRP;
        }
        protected virtual void OnDisable()
        {
            UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering -= DrawShapesSRP;

            foreach (ShapeCooldown cooldown in _cooldowns)
            {
                cooldown.Destroy();
            }
        }

        private void DrawShapesSRP(UnityEngine.Rendering.ScriptableRenderContext ctx, Camera cam) => OnCameraPreRender(cam);
        private void OnCameraPreRender(Camera cam)
        {
            switch (cam.cameraType)
            {
                case CameraType.Preview:
                case CameraType.Reflection:
                    return; // Don't render in preview windows or in reflection probes in case we run this script in the editor
            }
            if (useCullingMasks && (cam.cullingMask & (1 << gameObject.layer)) == 0)
                return; // scene & game view cameras should respect culling layer settings if you tell them to

            DrawShapes(cam);
        }
        private void DrawShapes(Camera cam)
        {
            using (Draw.Command(cam))
            {
                foreach (ShapeSequence sequence in _shapeDrawers)
                {
                    sequence.Draw();
                }
            }
        }

        public void Register(ShapeSequence sequence)
        {
            _shapeDrawers.Add(sequence);
        }

        public void Unregister(ShapeSequence sequence)
        {
            _shapeDrawers.Remove(sequence);
        }
    }
}