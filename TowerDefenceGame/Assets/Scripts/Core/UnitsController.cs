using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public sealed class UnitsController : MonoBehaviour
    {
        public IList<UnitBase> ActiveUnits
        {
            get { return _unitsOnScene.Where(x => !x.IsDestroyed).ToList(); }
        }

        private readonly IList<UnitBase> _unitsOnScene = new List<UnitBase>();

        private readonly IList<UnitBase> _unitsForUnregistering = new List<UnitBase>();

        public static UnitsController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<UnitsController>();

                return _instance;
            }
        }

        private static UnitsController _instance;

        private Coroutine _updatingUnitsCoroutine;

        public event Action<UnitBase> UnitDestroyed; 

        public void StartUpdateUnits()
        {
            StopUpdateUnits();

            _updatingUnitsCoroutine = StartCoroutine(UpdateUnits());
        }

        public void StopUpdateUnits()
        {
            if (_updatingUnitsCoroutine != null)
            {
                StopCoroutine(_updatingUnitsCoroutine);
                _updatingUnitsCoroutine = null;
            }
        }

        public void RegisterUnit(UnitBase unit)
        {
            if (!_unitsOnScene.Contains(unit))
            {
                _unitsOnScene.Add(unit);
                unit.Destroyed += OnUnitDestroyed;
            }
        }

        public void UnregisterUnit(UnitBase unit)
        {
            if (_unitsOnScene.Contains(unit))
            {
                _unitsForUnregistering.Add(unit);
                unit.Destroyed -= OnUnitDestroyed;
            }
        }
        
        private void OnUnitDestroyed(IAttackTarget obj)
        {
            var unit = obj as UnitBase;
            if (unit != null)
            {
                UnitDestroyed?.Invoke(unit);
            }
        }

        private IEnumerator UpdateUnits()
        {
            while (true)
            {
                foreach (var activeUnit in ActiveUnits)
                {
                    activeUnit.Update();
                }

                yield return new WaitForEndOfFrame();
                ClearUnitsForUnregistering();
            }
        }

        private void ClearUnitsForUnregistering()
        {
            foreach (var baseUnit in _unitsForUnregistering)
            {
                if (_unitsOnScene.Contains(baseUnit))
                    _unitsOnScene.Remove(baseUnit);
            }

            _unitsForUnregistering.Clear();
        }
    }
}
