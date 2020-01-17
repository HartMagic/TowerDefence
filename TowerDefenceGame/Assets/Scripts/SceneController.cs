using Settings;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class SceneController : MonoBehaviour
{
    private UnitWavesController _unitWavesController;
    
    private readonly UnitsController _unitsController = UnitsController.Instance;
    private readonly TowersController _towersController = TowersController.Instance;
    
    [Space]
    [SerializeField]
    private UnitWaveSettings _waveSettings;

    [SerializeField]
    private UnitVisual _prefab;
    [SerializeField]
    private UnitSettings _unitSettings;
    [SerializeField]
    private UnitSettings _unitUpgradeSettings;
    
    [SerializeField]
    private UnitPath[] _unitPaths;

    [Space]
    [SerializeField]
    private BaseVisual _baseVisual;
    [SerializeField]
    private GameSettings _gameSettings;

    [Space]
    [SerializeField]
    private Transform[] _towersPositions;
    [SerializeField]
    private TowerVisual _towerPrefab;
    [SerializeField]
    private BulletVisual _bulletPrefab;
    [SerializeField]
    private TowerSettings _towerSettings;

    public Base Base
    {
        get
        {
            if(_base == null)
                _base = new Base(_baseVisual, new BaseModel(_gameSettings.Health));
            
            return _base;
        }
    }
    
    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SceneController>();

            return _instance;
        }
    }
    
    private static SceneController _instance;
    
    private Base _base;
    private ISpawnUnitWaveBehaviour _spawnUnitWaveBehaviour;
    
    private void Start()
    {
        InitializeBase();
        InitializeUnits();
        InitializeTowers();
        
        StartGame();
    }

    // Replace to initializator
    private void InitializeBase()
    {
        Base.Destroyed += OnBaseDestroyed;
    }

    // Replace to initializator
    private void InitializeUnits()
    {
        var defaultUnitVisualFactory = new UnitVisual.Factory(_prefab);
        var defaultUnitFactory = new DefaultUnit.Factory(_base);
        var defaultUnitSpawner = new DefaultUnitSpawner(defaultUnitVisualFactory, defaultUnitFactory);
        
        _spawnUnitWaveBehaviour = new SpawnUnitWaveBehaviour(defaultUnitSpawner, _unitSettings, _unitUpgradeSettings, _unitPaths, transform);
        _unitWavesController = new UnitWavesController(_spawnUnitWaveBehaviour, _waveSettings);
    }

    // Replace to initializator
    private void InitializeTowers()
    {
        var defaultTowerVisualFactory = new TowerVisual.Factory(_towerPrefab, _bulletPrefab);
        var towerFactory = new TowerBase.Factory();
        var model = new TowerModel(_towerSettings.Damage, _towerSettings.FiringRate, _towerSettings.DetectingDistance, _towerSettings.Cost);
        
        for (var i = 0; i < _towersPositions.Length; i++)
        {
            var towerVisualData = new TowerVisualFactoryData(_towersPositions[i].position, _towersPositions[i].rotation, transform);
            var towerVisual = defaultTowerVisualFactory.Create(towerVisualData);
            var tower = towerFactory.Create(towerVisual, model);
            tower.Reset();
        }
    }

    private void OnBaseDestroyed(IAttackTarget obj)
    {
        EndGame();
    }

    public void StartGame()
    {
        if (_unitWavesController != null)
        {
            _unitWavesController.StartSpawn();
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
        if (_unitWavesController != null)
        {
            _unitWavesController.StopSpawn();
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
