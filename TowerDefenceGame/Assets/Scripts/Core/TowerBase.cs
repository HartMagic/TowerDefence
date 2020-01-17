using System.Collections;
using System.Linq;
using UnityEngine;

namespace Core
{
    public abstract class TowerBase : IAttackable
    {
        public float Damage
        {
            get { return _model.Damage; }
        }

        public float DetectingDistance
        {
            get { return _model.DetectingDistance; }
        }

        public IAttackTarget AttackTarget
        {
            get { return _attackTarget; }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            protected set { _isEnabled = value; }
        }

        protected readonly TowerVisual _visual;
        protected readonly TowerModel _model;

        protected IAttackTarget _attackTarget;

        private bool _isEnabled;

        protected TowerBase(TowerVisual visual, TowerModel model)
        {
            _visual = visual;
            _model = model;

            _isEnabled = _visual != null && _model != null;

            TowersController.Instance.RegisterTower(this);
        }

        ~TowerBase()
        {
            TowersController.Instance.UnregisterTower(this);
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

        public virtual void Reset()
        {
            _attackTarget = null;
            _isEnabled = true;
        }

        public virtual void Update()
        {
            if (!IsEnabled)
                return;

            SetAttackTarget(FindTarget());

            if (_attackTarget != null)
            {
                _visual.UpdateVisualTarget(_attackTarget.WorldPosition);
                Attack();
            }
        }

        protected virtual IAttackTarget FindTarget()
        {
            var activeUnits = UnitsController.Instance.ActiveUnits;

            if (_attackTarget != null)
            {
                if (_attackTarget.IsDestroyed ||
                    Vector3.Distance(_visual.transform.position, _attackTarget.WorldPosition) > DetectingDistance)
                    _attackTarget = null;
            }

            var closeTarget = _attackTarget;
            var distance = _attackTarget == null
                ? DetectingDistance
                : Vector3.Distance(_visual.transform.position, _attackTarget.WorldPosition);

            foreach (var activeUnit in activeUnits)
            {
                var newDistance = Vector3.Distance(_visual.transform.position, activeUnit.WorldPosition);
                if (newDistance <= distance)
                {
                    closeTarget = activeUnit;
                    distance = newDistance;
                }
            }

            return closeTarget;
        }
    }
}