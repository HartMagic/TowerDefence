using System;
using UnityEngine;

public class Base : IDestroyable
{
    public float Health
    {
        get { return _model.Health - _inflictedDamage; }
    }

    public bool IsDestroyed
    {
        get { return _isDestroyed; }
        protected set
        {
            if (_isDestroyed != value)
            {
                _isDestroyed = value;

                if (value)
                {
                    Destroyed?.Invoke(this);
                }
            }
        }
    }
    
    public event Action<IDestroyable, float> Damaged;
    public event Action<IDestroyable> Destroyed;

    private readonly BaseVisual _visual;
    private readonly BaseModel _model;

    private bool _isDestroyed;
    
    protected float _inflictedDamage;

    public Base(BaseVisual visual, BaseModel model)
    {
        _visual = visual;
        _model = model;
    }
    
    public void ApplyDamage(float damage)
    {
        _inflictedDamage += damage;
        
        Damaged?.Invoke(this, damage);
        
        if (Health <= 0)
        {
            IsDestroyed = true;
        }
        
        Debug.Log(Health);
    }
}
