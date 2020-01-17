using System.Collections.Generic;
using System.Linq;
using Settings;
using UnityEngine;

public class UnitVisual : MonoBehaviour
{
    public virtual void UpdateVisualTransform(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public virtual void ResetVisual()
    {
        // Reset any visual states
    }
    
    public sealed class Factory : IFactory<UnitVisual, UnitVisualFactoryData>
    {
        private readonly IList<UnitVisual> _visualsOnScene = new List<UnitVisual>(); // pool of unit visuals (default)

        private readonly UnitVisual _prefab;
        
        public Factory(UnitVisual prefab)
        {
            _prefab = prefab;
        }
        
        public UnitVisual Create(UnitVisualFactoryData data)
        {
            if (data == null)
                return null;
            
            UnitVisual unitVisual;

            if (_visualsOnScene.All(x => x.gameObject.activeSelf))
            {
                unitVisual = Instantiate(_prefab, data.Position, data.Rotation, data.Parent);
                
                _visualsOnScene.Add(unitVisual);
            }
            else
            {
                unitVisual = _visualsOnScene.First(x => !x.gameObject.activeSelf);
                
                unitVisual.transform.position = data.Position;
                unitVisual.transform.rotation = data.Rotation;
                unitVisual.gameObject.SetActive(true);
            }

            return unitVisual;
        }
    }
    
    public sealed class UnitVisualFactoryData
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

        public UnitVisualFactoryData(Vector3 position, Quaternion rotation, Transform parent)
        {
            _position = position;
            _rotation = rotation;
            _parent = parent;
        }
    }
}