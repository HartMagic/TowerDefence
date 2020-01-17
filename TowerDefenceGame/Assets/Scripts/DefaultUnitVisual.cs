using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

public class DefaultUnitVisual : UnitVisual
    {
        public virtual void UpdateVisualTransform(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public override void ResetVisual()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        public sealed class Factory : IFactory<DefaultUnitVisual, DefaultUnitVisualFactoryData>
        {
            private readonly IList<DefaultUnitVisual>
                _visualsOnScene = new List<DefaultUnitVisual>(); // pool of unit visuals (default)

            private readonly DefaultUnitVisual _prefab;

            public Factory(DefaultUnitVisual prefab)
            {
                _prefab = prefab;
            }

            public DefaultUnitVisual Create(DefaultUnitVisualFactoryData data)
            {
                if (data == null)
                    return null;

                DefaultUnitVisual unitVisual;

                if (_visualsOnScene.All(x => x.gameObject.activeSelf))
                {
                    unitVisual = Instantiate(_prefab, data.Position, data.Rotation, data.Parent);

                    _visualsOnScene.Add(unitVisual);
                }
                else
                {
                    unitVisual = _visualsOnScene.First(x => !x.gameObject.activeSelf);
                    unitVisual.UpdateVisualTransform(data.Position, data.Rotation);
                }

                return unitVisual;
            }
        }

        public sealed class DefaultUnitVisualFactoryData
        {
            public Vector3 Position
            {
                get { return _position; }
            }

            public Quaternion Rotation
            {
                get { return _rotation; }
            }

            public Transform Parent
            {
                get { return _parent; }
            }

            private readonly Vector3 _position;
            private readonly Quaternion _rotation;
            private readonly Transform _parent;

            public DefaultUnitVisualFactoryData(Vector3 position, Quaternion rotation, Transform parent)
            {
                _position = position;
                _rotation = rotation;
                _parent = parent;
            }
        }
    }