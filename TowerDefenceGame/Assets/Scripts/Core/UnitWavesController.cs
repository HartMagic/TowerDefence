using System.Collections;
using Settings;
using UnityEngine;

namespace Core
{
    public sealed class UnitWavesController
    {
        public int CurrentWaveIndex
        {
            get { return _currentWaveIndex; }
        }

        private readonly ISpawnUnitWaveBehaviour _spawnUnitWaveBehaviour;
        private readonly UnitWaveSettings _settings;

        private int _currentWaveIndex = 0;

        private Coroutine _spawnCoroutine;
        private Coroutine _spawnWaveCoroutine;

        public UnitWavesController(ISpawnUnitWaveBehaviour behaviour, UnitWaveSettings settings)
        {
            _spawnUnitWaveBehaviour = behaviour;
            _settings = settings;
        }

        public void StartSpawn()
        {
            StopSpawn();

            _spawnCoroutine =
                LevelController.Instance.StartCoroutine(SpawnCoroutine(_settings.TimeBetweenWaves,
                    _settings.UnitCountPerWave));
        }

        public void StopSpawn()
        {
            if (_spawnCoroutine != null)
            {
                LevelController.Instance.StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }

            if (_spawnWaveCoroutine != null)
            {
                LevelController.Instance.StopCoroutine(_spawnWaveCoroutine);
                _spawnWaveCoroutine = null;
            }

            _currentWaveIndex = 0;
        }

        private IEnumerator SpawnCoroutine(float timeBetweenWaves, int unitCountPerWave)
        {
            while (true)
            {
                _currentWaveIndex++;

                var unitCount = Random.Range(_currentWaveIndex, _currentWaveIndex + unitCountPerWave);
                
                _spawnWaveCoroutine = LevelController.Instance.StartCoroutine(
                    _spawnUnitWaveBehaviour.SpawnWave(unitCount, _currentWaveIndex));

                yield return _spawnWaveCoroutine;

                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }
}