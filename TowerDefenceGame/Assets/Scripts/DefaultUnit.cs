using System;
using UnityEngine;

public class DefaultUnit : UnitBase
{
    private float _currentProgress;
    
    private readonly UnitPath _path;
    private readonly UnitPath.UnitPathMapper _pathMapper;

    public event Action<float> TargetReached;
    
    public DefaultUnit(UnitVisual visual, UnitModel model, UnitPath path) : base(visual, model)
    {
        _path = path;
        _pathMapper = new UnitPath.UnitPathMapper(_path);
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
            
            TargetReached?.Invoke(_model.Damage);
        }
    }
    
    public sealed class Factory : IFactory<UnitBase, UnitVisual, UnitModel, UnitPath>
    {
        public UnitBase Create(UnitVisual visual, UnitModel model, UnitPath path)
        {
            return new DefaultUnit(visual, model, path);
        }
    }
}