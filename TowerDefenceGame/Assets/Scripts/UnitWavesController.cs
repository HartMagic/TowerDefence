using System.Collections;
using Settings;
using UnityEngine;

public sealed class UnitWavesController : MonoBehaviour
{
    [SerializeField]
    private UnitWaveSettings _settings;

    [SerializeField]
    private UnitVisual _prefab;
    [SerializeField]
    private UnitSettings _unitSettings;
    
    [SerializeField]
    private UnitPath[] _unitPaths;

    public int CurrentWaveIndex
    {
        get { return _currentWaveIndex; }
    }

    private ISpawnUnitWaveBehaviour _spawnUnitWaveBehaviour;

    private int _currentWaveIndex = 0;
    
    private Coroutine _spawnCoroutine;
    private Coroutine _spawnWaveCoroutine;

    private void Awake()
    {
        var defaultUnitVisualFactory = new UnitVisual.Factory(_prefab);
        var defaultUnitFactory = new DefaultUnit.Factory();
        var defaultUnitSpawner = new DefaultUnitSpawner(defaultUnitVisualFactory, defaultUnitFactory);
        
        _spawnUnitWaveBehaviour = new SpawnUnitWaveBehaviour(defaultUnitSpawner, _unitSettings, _unitPaths, transform);
        
        StartSpawn();
    }

    public void StartSpawn()
    {
        StopSpawn();

        _spawnCoroutine = StartCoroutine(SpawnCoroutine(_settings.TimeBetweenWaves));
    }

    public void StopSpawn()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
        
        if(_spawnWaveCoroutine != null)
        {
            StopCoroutine(_spawnWaveCoroutine);
            _spawnWaveCoroutine = null;
        }

        _currentWaveIndex = 0;
    }

    private IEnumerator SpawnCoroutine(float timeBetweenWaves)
    {
        while (true)
        {
            _currentWaveIndex++;
        
            var unitCount = Random.Range(_currentWaveIndex, _currentWaveIndex + _settings.UnitCountPerWave);
            _spawnWaveCoroutine = StartCoroutine(_spawnUnitWaveBehaviour.SpawnWave(unitCount, _currentWaveIndex));
        
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
}