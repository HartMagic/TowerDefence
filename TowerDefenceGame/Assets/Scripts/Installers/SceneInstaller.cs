using Core;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Installers
{
    // For ZInject this class can be implemented in installers (MonoInstaller)
    public class SceneInstaller : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField]
        private BaseVisual _baseVisual;
        [SerializeField]
        private BaseSettings _baseSettings;
        
        [Header("Units")]
        [SerializeField]
        private UnitPath[] _unitPaths;
        
        [Space]
        [SerializeField]
        private DefaultUnitVisual _prefab;
        [SerializeField]
        private UnitSettings _unitSettings;
        [SerializeField]
        private UnitSettings _unitUpgradeSettings;
        [SerializeField]
        private UnitWaveSettings _waveSettings;
        
        [Header("Towers")]
        [SerializeField]
        private Transform[] _towersPositions;
        [SerializeField]
        private DefaultTowerVisual _towerPrefab;
        [SerializeField]
        private BulletVisual _bulletPrefab;
        [SerializeField]
        private TowerSettings _towerSettings;
        [SerializeField]
        private TowerSettings _towerUpgradeSettings;

        [Header("Common")]
        [SerializeField]
        private Camera _mainCamera;
        [SerializeField]
        private GraphicRaycaster _graphicRaycaster;

        public Base Base
        {
            get { return _base; }
        }

        public UnitWavesController UnitWavesController
        {
            get { return _unitWavesController; }
        }

        public ISelector TowerSelector
        {
            get { return _towerSelector; }
        }

        public IUpgrader DefaultUnitUpgrader
        {
            get { return _defaultUnitUpgrader; }
        }
        
        protected Base _base;
        private UnitWavesController _unitWavesController;

        protected IUpgrader _defaultUnitUpgrader;
        
        private ISelector _towerSelector;

        public virtual void Install()
        {
            _towerSelector = new TowerSelector(_mainCamera, _graphicRaycaster);
            _defaultUnitUpgrader = new DefaultTowerUpgrader(_towerUpgradeSettings);
            
            InitializeBase();
            InitializeUnits();
            InitializeTowers();
        }

        protected virtual void InitializeBase()
        {
            _base = new Base(_baseVisual, new BaseModel(_baseSettings.Health));
        }

        protected virtual void InitializeUnits()
        {
            var defaultUnitVisualFactory = new DefaultUnitVisual.Factory(_prefab);
            var defaultUnitFactory = new DefaultUnit.Factory(_base);
            var defaultUnitSpawner = new DefaultUnitSpawner(defaultUnitVisualFactory, defaultUnitFactory);
        
            var spawnUnitWaveBehaviour = new DefaultSpawnUnitWaveBehaviour(defaultUnitSpawner, _unitSettings, _unitUpgradeSettings, _unitPaths, transform);
            _unitWavesController = new UnitWavesController(spawnUnitWaveBehaviour, _waveSettings);
        }
        
        protected virtual void InitializeTowers()
        {
            var defaultTowerVisualFactory = new DefaultTowerVisual.Factory(_towerPrefab, _bulletPrefab);
            var towerFactory = new DefaultTower.Factory();
            var model = new DefaultTowerModel(_towerSettings.Damage, _towerSettings.FiringRate, _towerSettings.DetectingDistance, _towerSettings.Cost);
        
            for (var i = 0; i < _towersPositions.Length; i++)
            {
                var towerVisualData = new DefaultTowerVisual.DefaultTowerVisualFactoryData(_towersPositions[i].position, _towersPositions[i].rotation, transform);
                var towerVisual = defaultTowerVisualFactory.Create(towerVisualData);
                var tower = towerFactory.Create(towerVisual, model);
                tower.Reset();
            }
        }
    }
}

