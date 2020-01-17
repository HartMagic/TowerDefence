using Core;
using UnityEngine;

public class DefaultUnit : UnitBase
{
    private float _currentProgress;

    private readonly UnitPath.UnitPathMapper _pathMapper;
    
    private Vector3 _currentPosition;
    private Quaternion _currentRotation;
    
    public DefaultUnit(DefaultUnitVisual visual, DefaultUnitModel model, UnitPath path) : base(visual, model)
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
        
        _currentPosition = Vector3.zero;
        _currentRotation = Quaternion.identity;
        
        if (_visual != null)
        {
            _visual.ResetVisual();
        }
    }

    public override void Update()
    {
        base.Update();

        if (_visual != null)
        {
            ((DefaultUnitVisual)_visual).UpdateVisualTransform(_currentPosition, _currentRotation);
        }
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
    
    public sealed class Factory : IFactory<DefaultUnit, DefaultUnitVisual, DefaultUnitModel, UnitPath>
    {
        private readonly IAttackTarget _target;
        
        public Factory(IAttackTarget target)
        {
            _target = target;
        }
        
        public DefaultUnit Create(DefaultUnitVisual visual, DefaultUnitModel model, UnitPath path)
        {
            var unit = new DefaultUnit(visual, model, path);
            unit.SetAttackTarget(_target);
            
            return unit;
        }
    }
}