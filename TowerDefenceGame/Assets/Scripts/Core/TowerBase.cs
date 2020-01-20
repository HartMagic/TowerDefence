using System;
using UnityEngine;

namespace Core
{
    public abstract class TowerBase : IAttackable, ICanSelect, ICanUpgrade
    {
        public float Damage
        {
            get { return _model.Damage; }
        }

        public float DetectingDistance
        {
            get { return _model.DetectingDistance; }
        }
        
        public int Cost
        {
            get { return _model.Cost; }
        }

        public int Level { get; private set; }

        public IAttackTarget AttackTarget
        {
            get { return _attackTarget; }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            protected set { _isEnabled = value; }
        }

        public Bounds VisualBounds
        {
            get { return _visual.VisualBounds; }
        }

        public TowerModel Model
        {
            get { return _model; }
        }

        public TowerVisual Visual
        {
            get { return _visual; }
        }

        protected readonly TowerVisual _visual;
        protected TowerModel _model;

        protected IAttackTarget _attackTarget;
        
        public event Action<ICanSelect> Selected;
        public event Action<int> Upgraded;

        private bool _isEnabled;

        private int _previewCost;

        protected TowerBase(TowerVisual visual, TowerModel model)
        {
            _visual = visual;
            _model = model;

            _isEnabled = _visual != null && _model != null;
            Level = 1;

            _previewCost = model.Cost;

            TowersController.Instance.RegisterTower(this);
        }

        ~TowerBase()
        {
            TowersController.Instance.UnregisterTower(this);
        }

        public virtual void Upgrade(IUpgradeData data)
        {
            Level++;
            
            Visual.UpdateUpgradeVisual(Level);
            
            if(Upgraded != null)
                Upgraded.Invoke(_previewCost);

            _previewCost = Cost;
        }

        public virtual void Select()
        {
            Visual.UpdateSelectedVisual(true);
            Selected?.Invoke(this);
        }

        public virtual void Unselect()
        {
            Visual.UpdateSelectedVisual(false);
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
            Level = 1;
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