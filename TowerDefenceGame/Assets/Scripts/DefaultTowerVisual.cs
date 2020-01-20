using Core;
using UnityEngine;

public class DefaultTowerVisual : TowerVisual
{
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private Transform _gun;
    [SerializeField]
    private Transform _barrelPivot;

    [SerializeField]
    private GameObject _selectedCircle;

    public Vector3 Forward
    {
        get { return _gun.forward; }
    }

    private Vector3 _targetPosition;
    
    private IFactory<BulletVisual, BulletVisual.BarrelVisualFactoryData> _bulletFactory;
    private BulletVisual _bulletPrefab;

    public override void InitializeVisual()
    {
        _bulletFactory = new BulletVisual.Factory(_bulletPrefab); // For ZInject use Bindings and Constructor
    }

    public override void UpdateVisualTarget(Vector3 position)
    {
        _targetPosition = position;
        
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

    public override void UpdateSelectedVisual(bool value)
    {
        if (_selectedCircle != null)
        {
            _selectedCircle.SetActive(value);
        }
    }

    public override void ApplyAttackVisual()
    {
        var data = new BulletVisual.BarrelVisualFactoryData(_barrelPivot.position, _barrelPivot.rotation, null, _targetPosition);
        var bullet = _bulletFactory.Create(data);
    }

    public override void ResetVisual()
    {
        _head.transform.rotation = Quaternion.identity;
        _gun.transform.rotation = Quaternion.identity;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_barrelPivot.position, _barrelPivot.position + Forward * 2);
    }

    public sealed class Factory : IFactory<DefaultTowerVisual, DefaultTowerVisualFactoryData>
    {
        private readonly DefaultTowerVisual _prefab;
        private readonly BulletVisual _bulletPrefab;
        
        public Factory(DefaultTowerVisual prefab, BulletVisual bulletPrefab)
        {
            _prefab = prefab;
            _bulletPrefab = bulletPrefab;
        }
        
        public DefaultTowerVisual Create(DefaultTowerVisualFactoryData data)
        {
            var towerVisual = Instantiate(_prefab, data.Position, data.Rotation, data.Parent);

            towerVisual._bulletPrefab = _bulletPrefab;
            towerVisual.ResetVisual();
            towerVisual.InitializeVisual();

            return towerVisual;
        }
    }
    
    public sealed class DefaultTowerVisualFactoryData
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

        public DefaultTowerVisualFactoryData(Vector3 position, Quaternion rotation, Transform parent)
        {
            _position = position;
            _rotation = rotation;
            _parent = parent;
        }
    }
}