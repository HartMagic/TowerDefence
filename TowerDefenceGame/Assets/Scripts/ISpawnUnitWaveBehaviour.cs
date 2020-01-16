using System.Collections;

public interface ISpawnUnitWaveBehaviour
{
    IEnumerator SpawnWave(int unitCount, int currentWaveIndex);
}