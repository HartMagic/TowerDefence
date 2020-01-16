using System.Collections;
using Settings;
using UnityEngine;

public class SpawnUnitWaveBehaviour : ISpawnUnitWaveBehaviour
{
    private readonly UnitPath[] _paths;

    private readonly DefaultUnitSpawner _defaultUnitSpawner;
    private readonly Transform _parent;

    private readonly UnitSettings _unitSettings;
    
    public SpawnUnitWaveBehaviour(DefaultUnitSpawner spawner, UnitSettings unitSettings, UnitPath[] paths, Transform parent)
    {
        _paths = paths;
        _parent = parent;

        _unitSettings = unitSettings;

        _defaultUnitSpawner = spawner;
    }
    
    public IEnumerator SpawnWave(int unitCount, int currentWaveIndex)
    {
        var unitPerPath = unitCount / _paths.Length;

        for (var i = 0; i < _paths.Length; i++)
        {
            if (i == _paths.Length - 1)
                unitPerPath = unitCount - (unitPerPath * i);

            for (var k = 0; k < unitPerPath; k++)
            {
                _defaultUnitSpawner.Spawn(GetModelForWave(currentWaveIndex), _paths[i], _parent);
                
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    private UnitModel GetModelForWave(int currentWaveIndex)
    {
        var model = new UnitModel(_unitSettings.Health, _unitSettings.Damage, _unitSettings.Gold, _unitSettings.Speed);

        return model;
    }
}