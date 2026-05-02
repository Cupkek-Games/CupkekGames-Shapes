using System;
using global::Shapes;
using UnityEngine;

namespace CupkekGames.Shapes
{
    public abstract class DrawCommand : MonoBehaviour
    {
        [SerializeField] protected ShapesDrawer _drawer;
        protected ShapeSequence _sequence;
        protected virtual void Awake()
        {
            if (_drawer == null)
            {
                _drawer = ShapesDrawer.Instance;
            }
        }
        protected virtual void OnEnable()
        {
            _sequence = CreateSequence();
            _drawer.Register(_sequence);
        }
        protected virtual void OnDisable()
        {
            _drawer.Unregister(_sequence);
        }
        public abstract ShapeSequence CreateSequence();
    }
}