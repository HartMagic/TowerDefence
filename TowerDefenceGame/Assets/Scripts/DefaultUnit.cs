using System;
using UnityEngine;

public class DefaultUnit : UnitBase
{
    private float _currentProgress;

    private readonly UnitPath.UnitPathMapper _pathMapper;
    
    public DefaultUnit(UnitVisual visual, UnitModel model, UnitPath path) : base(visual, model)
    {
        _pathMapper = new UnitPath.UnitPathMapper(path);
    }

    public override void Move(float speed, float t)
    {
        _currentProgress += t * speed;

        CheckProgress();
        
        _currentPosition = _pathMapper.MapPoint(_currentProgress);
        _currentRotation = Quaternion.LookRotation(_pathMapper.MapDirection(_currentProgress));
    }

    public override void Reset()
    {
        base.Reset();
        _currentProgress = 0;
    }

    private void CheckProgress()
    {
        if (_currentProgress >= 1.0f)
        {
            _currentProgress = 1.0f;

            IsDestroyed = true;
            
            Attack();
        }
    }
    
    public sealed class Factory : IFactory<UnitBase, UnitVisual, UnitModel, UnitPath>
    {
        private readonly IAttackTarget _target;
        
        public Factory(IAttackTarget target)
        {
            _target = target;
        }
        
        public UnitBase Create(UnitVisual visual, UnitModel model, UnitPath path)
        {
            var unit = new DefaultUnit(visual, model, path);
            unit.SetAttackTarget(_target);
            
            return unit;
        }
    }
}