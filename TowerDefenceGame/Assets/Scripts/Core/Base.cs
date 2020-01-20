using System;
using UnityEngine;

namespace Core
{
    public class Base : IAttackTarget
    {
        public float Health
        {
            get { return _model.Health - _inflictedDamage; }
        }

        public float MaxHealth
        {
            get { return _model.Health; }
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

        public Vector3 WorldPosition
        {
            get { return _visual.transform.position; }
        }
    
        public event Action<IAttackTarget, float> Damaged;
        public event Action<IAttackTarget> Destroyed;

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
            
            if (_visual != null)
            {
                _visual.ApplyDamageVisual();
            }
        
            Damaged?.Invoke(this, damage);
        
            if (Health <= 0)
            {
                IsDestroyed = true;
                if (_visual != null)
                {
                    _visual.ApplyDestroyVisual();
                }
            }
        }
    }
}
