using System.Collections.Generic;
using UnityEngine;
using global::Shapes;
using System.Collections;
using System;

namespace CupkekGames.Shapes
{
    public class ShapeCooldown
    {
        private ShapesDrawer _drawer;
        private ShapeSequence _sequence;
        // State
        private Coroutine coroutine;
        public ShapeCooldown(ShapesDrawer drawer, ShapeSequence sequence)
        {
            this._drawer = drawer;
            this._sequence = sequence;
        }
        public void StartCooldown(float duration)
        {
            if (coroutine != null)
            {
                _drawer.StopCoroutine(coroutine);
            }

            coroutine = _drawer.StartCoroutine(Cooldown(duration));
        }

        private IEnumerator Cooldown(float duration)
        {
            yield return new WaitForSeconds(duration);

            _drawer.Unregister(_sequence);
        }

        public void Destroy()
        {
            if (coroutine != null)
            {
                _drawer.StopCoroutine(coroutine);
            }

            _drawer.Unregister(_sequence);
        }
    }
}