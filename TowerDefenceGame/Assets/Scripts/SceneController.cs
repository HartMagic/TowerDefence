using Settings;
using UnityEngine;

public sealed class SceneController : MonoBehaviour
{
    [SerializeField]
    private UnitWavesController _unitWavesController;
    [SerializeField]
    private UnitsController _unitsController = UnitsController.Instance;
    
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

    public Base Base
    {
        get { return _base; }
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
        _base = new Base(_baseVisual, new BaseModel(_gameSettings.Health));
        _base.Destroyed += OnBaseDestroyed;
        
        var defaultUnitVisualFactory = new UnitVisual.Factory(_prefab);
        var defaultUnitFactory = new DefaultUnit.Factory(_base);
        var defaultUnitSpawner = new DefaultUnitSpawner(defaultUnitVisualFactory, defaultUnitFactory);
        
        _spawnUnitWaveBehaviour = new SpawnUnitWaveBehaviour(defaultUnitSpawner, _unitSettings, _unitUpgradeSettings, _unitPaths, transform);
        _unitWavesController = new UnitWavesController(_spawnUnitWaveBehaviour, _waveSettings);
        
        StartGame();
    }

    private void OnBaseDestroyed(IDestroyable obj)
    {
        //EndGame();
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
    }

    public void EndGame()
    {
        if (_unitWavesController != null)
        {
            _unitWavesController.StopSpawn();
        }
        
        if (_unitsController != null)
        {
            _unitsController.StopUpdateUnits();
        }
    }
}
