using System.Collections;

namespace Core
{
    public interface ISpawnUnitWaveBehaviour
    {
        IEnumerator SpawnWave(int unitCount, int currentWaveIndex);
    }
}
