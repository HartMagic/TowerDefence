using System.Collections.Generic;
using UnityEngine;

public class TowerVisual : MonoBehaviour
{
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private Transform _gun;
    [SerializeField]
    private Transform _barrelPivot;

    public Vector3 Forward
    {
        get { return _gun.forward; }
    }

    private IFactory<BulletVisual, BarrelVisualFactoryData> _barrelFactory;

    public void UpdateVisualTarget(Vector3 position)
    {
        if (_head != null)
        {
            var newRotation = Quaternion.LookRotation(position - transform.position).eulerAngles;
            newRotation.x = 0.0f;
            newRotation.z = 0.0f;
            
            _head.rotation = Quaternion.Lerp(_head.rotation, Quaternion.Euler(newRotation), Time.deltaTime * 10.0f);
        }

        if (_gun != null)
        {
            var newRotation = Quaternion.LookRotation(position - transform.position).eulerAngles;
            newRotation.z = 0.0f;
            newRotation.y = 0.0f;
            
            _gun.localRotation = Quaternion.Lerp(_gun.localRotation, Quaternion.Euler(newRotation), Time.deltaTime * 10.0f);
        }
    }

    public void GoToStartRotation()
    {
        
    }

    public void Attack(Vector3 position)
    {
        var data = new BarrelVisualFactoryData(_barrelPivot.position, _barrelPivot.rotation, transform, position);
        _barrelFactory.Create(data);
    }

    public void ResetVisual()
    {
        _head.transform.rotation = Quaternion.identity;
        _gun.transform.rotation = Quaternion.identity;
    }
    
    public sealed class Factory : IFactory<TowerVisual, TowerVisualFactoryData>
    {
        private TowerVisual _prefab;
        private BulletVisual _bulletPrefab;
        
        public Factory(TowerVisual prefab, BulletVisual bulletPrefab)
        {
            _prefab = prefab;
            _bulletPrefab = bulletPrefab;
        }
        
        public TowerVisual Create(TowerVisualFactoryData data)
        {
            var towerVisual = Instantiate(_prefab, data.Position, data.Rotation, data.Parent);
            towerVisual.ResetVisual();
            
            towerVisual._barrelFactory = new BulletVisual.Factory(_bulletPrefab);

            return towerVisual;
        }
    }
}

public sealed class TowerVisualFactoryData
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

    public TowerVisualFactoryData(Vector3 position, Quaternion rotation, Transform parent)
    {
        _position = position;
        _rotation = rotation;
        _parent = parent;
    }
}
