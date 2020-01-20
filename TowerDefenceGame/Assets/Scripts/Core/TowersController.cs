using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public sealed class TowersController : MonoBehaviour
    {
        public static TowersController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<TowersController>();

                return _instance;
            }
        }

        public IList<TowerBase> ActiveTowers
        {
            get { return _towersOnScene.Where(x => x.IsEnabled).ToList(); }
        }

        private static TowersController _instance;

        private readonly IList<TowerBase> _towersOnScene = new List<TowerBase>();
        private readonly IList<TowerBase> _towersForUnregistering = new List<TowerBase>();

        private Coroutine _updatingTowersCoroutine;

        public event Action<TowerBase> TowerSelected;
        public event Action<int> TowerUpgraded; 

        public void StartUpdateTowers()
        {
            StopUpdateTowers();

            _updatingTowersCoroutine = StartCoroutine(UpdateTowers());
        }

        public void StopUpdateTowers()
        {
            if (_updatingTowersCoroutine != null)
            {
                StopCoroutine(_updatingTowersCoroutine);
                _updatingTowersCoroutine = null;
            }
        }

        public void RegisterTower(TowerBase tower)
        {
            if (!_towersOnScene.Contains(tower))
            {
                _towersOnScene.Add(tower);
                
                tower.Selected += OnTowerSelected;
                tower.Upgraded += OnTowerUpgraded;
            }
        }

        private void OnTowerUpgraded(int previewValue)
        {
            if (TowerUpgraded != null)
            {
                TowerUpgraded?.Invoke(previewValue);
            }
        }

        public void UnregisterTower(TowerBase tower)
        {
            if (_towersOnScene.Contains(tower))
            {
                _towersForUnregistering.Add(tower);
            }
        }
        
        private void OnTowerSelected(ICanSelect tower)
        {
            TowerSelected?.Invoke(tower as TowerBase);
        }

        private IEnumerator UpdateTowers()
        {
            while (true)
            {
                foreach (var activeTower in ActiveTowers)
                {
                    activeTower.Update();
                }

                yield return new WaitForEndOfFrame();
                ClearUnitsForUnregistering();
            }
        }

        private void ClearUnitsForUnregistering()
        {
            foreach (var towerBase in _towersForUnregistering)
            {
                if (_towersOnScene.Contains(towerBase))
                    _towersOnScene.Remove(towerBase);
            }

            _towersForUnregistering.Clear();
        }
    }
}
