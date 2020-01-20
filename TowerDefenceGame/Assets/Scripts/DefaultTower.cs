using Core;
using UnityEngine;

public class DefaultTower : TowerBase
{
    public float FiringRate
    {
        get { return Model.FiringRate; }
    }
    
    private new DefaultTowerVisual Visual
    {
        get { return (DefaultTowerVisual) _visual; }
    }

    private new DefaultTowerModel Model
    {
        get { return (DefaultTowerModel) _model; }
    }
    
    private float _previousTime;

    public DefaultTower(DefaultTowerVisual visual, DefaultTowerModel model) : base(visual, model)
    {
        _previousTime = Time.time;
    }

    public override void Attack()
    {
        if (_attackTarget != null && (Time.time - _previousTime) >= (1.0f/FiringRate) &&
            Vector3.Angle(_attackTarget.WorldPosition - _visual.transform.position, Visual.Forward) <= 10.0f)
        {
            _previousTime = Time.time;
            
            _visual.ApplyAttackVisual();
            _attackTarget.ApplyDamage(Damage);
        }
    }

    public override void Upgrade(IUpgradeData data)
    {
        var towerData = data as DefaultTowerUpgradeData;
        if (towerData != null)
        {
            _model = new DefaultTowerModel(towerData.Damage, towerData.FiringRate, towerData.DetectingDistance, towerData.Cost);
            
            base.Upgrade(data);
        }
    }

    public sealed class Factory : IFactory<DefaultTower, DefaultTowerVisual, DefaultTowerModel>
    {
        public DefaultTower Create(DefaultTowerVisual visual, DefaultTowerModel model)
        {
            return new DefaultTower(visual, model);
        }
    }
}
