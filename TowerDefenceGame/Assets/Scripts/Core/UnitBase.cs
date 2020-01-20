using System;
using UnityEngine;

namespace Core
{
    public abstract class UnitBase : IMoveable, IAttackTarget, IAttackable
    {
        public float Health
        {
            get { return GetHealth(); }
        }
        
        public float MaxHealth
        {
            get { return _model.Health; }
        }

        public float Damage
        {
            get { return _model.Damage; }
        }

        public int Gold
        {
            get { return _model.Gold; }
        }

        public float Speed
        {
            get { return _model.Speed; }
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
                        _visual.gameObject.SetActive(false);

                        UnitsController.Instance.UnregisterUnit(this);
                    }
                }
            }
        }

        public IAttackTarget AttackTarget
        {
            get { return _attackTarget; }
        }

        public Vector3 WorldPosition
        {
            get { return _visual.transform.position; }
        }

        protected readonly UnitVisual _visual;
        protected readonly UnitModel _model;

        protected float _inflictedDamage;

        private bool _isDestroyed;

        protected IAttackTarget _attackTarget;

        public event Action<IAttackTarget, float> Damaged;
        public event Action<IAttackTarget> Destroyed;

        protected UnitBase(UnitVisual visual, UnitModel model)
        {
            _visual = visual;
            _model = model;

            UnitsController.Instance.RegisterUnit(this);
            
            _visual.InitializeVisual();
        }

        public virtual void Move(float speed, float t)
        {
        }

        public virtual void ApplyDamage(float value)
        {
            _inflictedDamage += value;

            if (_visual != null)
            {
                _visual.ApplyDamageVisual();
            }

            Damaged?.Invoke(this, value);

            if (Health <= 0)
            {
                Destroy();
            }
        }

        public void SetAttackTarget(IAttackTarget target)
        {
            _attackTarget = target;
        }

        public virtual void Attack()
        {
            if (_attackTarget != null)
            {
                _attackTarget.ApplyDamage(Damage);
            }
        }

        public virtual void Destroy()
        {
            if (_visual != null)
            {
                _visual.ApplyDestroyVisual();
            }
            
            Destroyed?.Invoke(this);
            
            IsDestroyed = true;
        }

        public virtual void Reset()
        {
            _inflictedDamage = 0;
            IsDestroyed = false;

            _attackTarget = null;
            
            _visual.ResetVisual();
        }

        /// <summary>
        /// The logic of Unit that perform every frame when the Unit isn't destroyed.
        /// </summary>
        public virtual void Update()
        {
            if (IsDestroyed)
                return;

            Move(Speed, Time.deltaTime);
        }

        protected virtual float GetHealth()
        {
            return _model.Health - _inflictedDamage;
        }
    }
}