using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletVisual : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1.0f;
    
    private Vector3 _target;
    
    private float _currentProgress;

    private void Update()
    {
        Move(Time.deltaTime);
    }

    // TODO:
    private void Move(float t)
    {
        _currentProgress += t * _moveSpeed;

        transform.position += transform.forward * _currentProgress; // linear movement

        if (_currentProgress >= 1.0f)
        {
            _currentProgress = 1.0f;
            gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        _currentProgress = 0.0f;
        _target = Vector3.zero;
    }

    public sealed class Factory : IFactory<BulletVisual, BarrelVisualFactoryData>
    {
        private readonly IList<BulletVisual> _pool = new List<BulletVisual>();

        private readonly BulletVisual _prefab;
        
        public Factory(BulletVisual prefab)
        {
            _prefab = prefab;
        }
        
        public BulletVisual Create(BarrelVisualFactoryData data)
        {
            if (data == null)
                return null;
            
            BulletVisual bullet;

            if (_pool.All(x => x.gameObject.activeSelf))
            {
                bullet = Instantiate(_prefab, data.Position, data.Rotation, data.Parent);
                _pool.Add(bullet);
            }
            else
            {
                bullet = _pool.First(x => !x.gameObject.activeSelf);

                bullet.transform.position = data.Position;
                bullet.transform.rotation = data.Rotation;
                bullet.gameObject.SetActive(true);
            }
            
            bullet.Reset();
            bullet._target = data.Target;

            return bullet;
        }
    }
}

public sealed class BarrelVisualFactoryData
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

    public Vector3 Target
    {
        get { return _target; }
    }
        
    private readonly Vector3 _position;
    private readonly Quaternion _rotation;
        
    private readonly Transform _parent;
    private readonly Vector3 _target;

    public BarrelVisualFactoryData(Vector3 position, Quaternion rotation, Transform parent, Vector3 target)
    {
        _position = position;
        _rotation = rotation;
            
        _parent = parent;
        _target = target;
    }
}
