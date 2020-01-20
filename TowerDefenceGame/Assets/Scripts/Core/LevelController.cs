using Installers;
using UI;
using UnityEngine;

namespace Core
{
    [DisallowMultipleComponent]
    public sealed class LevelController : MonoBehaviour
    {
        [SerializeField]
        private UnitsController _unitsController;
        [SerializeField]
        private TowersController _towersController;

        [SerializeField] 
        private SceneInstaller _sceneInstaller;
        [SerializeField]
        private UIManager _uiManager;

        private int _collectedGold = 0;
        private int _destroyedUnits = 0;

        public static LevelController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<LevelController>();

                return _instance;
            }
        }

        private static LevelController _instance;

        private void Start()
        {
            if (_sceneInstaller != null)
                _sceneInstaller.Install();

            if (_sceneInstaller.Base != null)
            {
                _sceneInstaller.Base.Damaged += OnBaseDamaged;
                _sceneInstaller.Base.Destroyed += OnBaseDestroyed;
                
                if (_uiManager != null)
                {
                    _uiManager.UpdateHealth(_sceneInstaller.Base.Health, _sceneInstaller.Base.MaxHealth);
                }
            }

            if (_unitsController != null)
            {
                _unitsController.UnitDestroyed += OnUnitsControllerUnitDestroyed;
            }
            
            if (_uiManager != null)
            {
                _uiManager.UpdateGold(_collectedGold);
                
                _uiManager.Upgraded += OnUiManagerUpgraded;
            }

            if (_towersController != null)
            {
                _towersController.TowerSelected += OnTowersControllerTowerSelected;
                _towersController.TowerUpgraded += OnTowersControllerTowerUpgraded;
            }

            StartGame();
        }

        private void OnUiManagerUpgraded(ICanUpgrade target, IUpgrader upgrader)
        {
            if (_collectedGold >= target.Cost)
            {
                upgrader.Upgrade(target);
            }
        }

        private void OnTowersControllerTowerUpgraded(int preview)
        {
            _collectedGold -= preview;
            
            if (_uiManager != null)
                _uiManager.UpdateGold(_collectedGold);
        }

        private void OnTowersControllerTowerSelected(TowerBase tower)
        {
            if (_uiManager != null)
            {
                _uiManager.ShowTowerUpgradePanel(tower, _sceneInstaller.DefaultUnitUpgrader);
            }
        }

        private void Update()
        {
            if (_sceneInstaller.TowerSelector != null && _towersController != null)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    var selectResult = _sceneInstaller.TowerSelector.Select(_towersController.ActiveTowers);
                    if (selectResult == null && _uiManager != null)
                    {
                        _uiManager.HideTowerUpgradePanel();
                    }
                }
            }
        }

        private void OnUnitsControllerUnitDestroyed(UnitBase unit)
        {
            _collectedGold += unit.Gold;
            _destroyedUnits++;

            if (_uiManager != null)
            {
                _uiManager.UpdateGold(_collectedGold);
            }
        }

        private void OnBaseDamaged(IAttackTarget baseTarget, float value)
        {
            if (_uiManager != null)
            {
                _uiManager.UpdateHealth(baseTarget.Health, baseTarget.MaxHealth);
            }
        }

        private void OnBaseDestroyed(IAttackTarget obj)
        {
            EndGame();
        }

        public void StartGame()
        {
            if (_sceneInstaller.UnitWavesController != null)
            {
                _sceneInstaller.UnitWavesController.StartSpawn();
            }

            if (_unitsController != null)
            {
                _unitsController.StartUpdateUnits();
            }

            if (_towersController != null)
            {
                _towersController.StartUpdateTowers();
            }
        }

        public void StopGame()
        {
            if (_sceneInstaller.UnitWavesController != null)
            {
                _sceneInstaller.UnitWavesController.StopSpawn();
            }

            if (_unitsController != null)
            {
                _unitsController.StopUpdateUnits();
            }

            if (_towersController != null)
            {
                _towersController.StopUpdateTowers();
            }
        }

        public void EndGame()
        {
            StopGame();
        }
    }
}
