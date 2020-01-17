using System.Collections;
using System.Linq;
using Core;
using Settings;
using UnityEngine;

public class DefaultSpawnUnitWaveBehaviour : ISpawnUnitWaveBehaviour
{
    private UnitPath[] _paths;

    private readonly DefaultUnitSpawner _defaultUnitSpawner;
    private readonly Transform _parent;

    private readonly UnitSettings _unitSettings;
    private readonly UnitSettings _unitUpgradeSettings;
    
    public DefaultSpawnUnitWaveBehaviour(DefaultUnitSpawner spawner, UnitSettings unitSettings, UnitSettings unitUpgradeSettings, UnitPath[] paths, Transform parent)
    {
        _paths = paths;
        _parent = parent;

        _unitSettings = unitSettings;
        _unitUpgradeSettings = unitUpgradeSettings;

        _defaultUnitSpawner = spawner;
    }
    
    public IEnumerator SpawnWave(int unitCount, int currentWaveIndex)
    {
        var unitPerPath = unitCount / _paths.Length;
        var model = GetModelForWave(currentWaveIndex-1, _unitUpgradeSettings);
        
        var random = new System.Random();
        _paths = _paths.OrderBy(x => random.Next()).ToArray();

        for (var i = 0; i < _paths.Length; i++)
        {
            if (i == _paths.Length - 1)
                unitPerPath = unitCount - (unitPerPath * i);

            for (var k = 0; k < unitPerPath; k++)
            {
                var unit = _defaultUnitSpawner.Spawn(model, _paths[i], _parent);
                
                yield return new WaitForSeconds(GetTimeBetweenUnits(model.Speed));
            }
        }
    }

    private DefaultUnitModel GetModelForWave(int currentWaveIndex, UnitSettings unitUpgradeSettings)
    {
        var model = new DefaultUnitModel(_unitSettings.Health + unitUpgradeSettings.Health * currentWaveIndex,
            _unitSettings.Damage + unitUpgradeSettings.Damage * currentWaveIndex, 
            _unitSettings.Gold + unitUpgradeSettings.Gold * currentWaveIndex, 
            _unitSettings.Speed + unitUpgradeSettings.Speed * currentWaveIndex);

        return model;
    }

    private float GetTimeBetweenUnits(float prevUnitSpeed)
    {
        return prevUnitSpeed * 3; // 3 is const, may be that needs another algorithm
    }
}