using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public sealed class UnitsController
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
                    _instance = new UnitsController();

                return _instance;
            }
        }

        private static UnitsController _instance;

        private Coroutine _updatingUnitsCoroutine;

        public void StartUpdateUnits()
        {
            StopUpdateUnits();

            _updatingUnitsCoroutine = SceneController.Instance.StartCoroutine(UpdateUnits());
        }

        public void StopUpdateUnits()
        {
            if (_updatingUnitsCoroutine != null)
            {
                SceneController.Instance.StopCoroutine(_updatingUnitsCoroutine);
                _updatingUnitsCoroutine = null;
            }
        }

        public void RegisterUnit(UnitBase unit)
        {
            if (!_unitsOnScene.Contains(unit))
                _unitsOnScene.Add(unit);
        }

        public void UnregisterUnit(UnitBase unit)
        {
            if (_unitsOnScene.Contains(unit))
            {
                _unitsForUnregistering.Add(unit);
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
