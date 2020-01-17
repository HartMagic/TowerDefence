using Core;
using UnityEngine;

public class DefaultTower : TowerBase
{
    public float FiringRate
    {
        get { return Model.FiringRate; }
    }
    
    public int Cost
    {
        get { return Model.Cost; }
    }

    private DefaultTowerVisual Visual
    {
        get { return (DefaultTowerVisual) _visual; }
    }

    private DefaultTowerModel Model
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
        if (_attackTarget != null && (Time.time - _previousTime) >= FiringRate &&
            Vector3.Angle(_attackTarget.WorldPosition - _visual.transform.position, Visual.Forward) <= 10.0f)
        {
            _previousTime = Time.time;
            
            _visual.ApplyAttackVisual();
            _attackTarget.ApplyDamage(Damage);
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
