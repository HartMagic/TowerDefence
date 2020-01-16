using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Settings/UnitWaveSettings", fileName = "UnitWaveSettings")]
    public sealed class UnitWaveSettings : ScriptableObject
    {
        [SerializeField, Tooltip("In seconds")]
        private float _timeBetweenWaves;
        [SerializeField, Tooltip("[K; K+X], K - wave number, X - unit count per wave")]
        private int _unitCountPerWave;

        public float TimeBetweenWaves
        {
            get { return _timeBetweenWaves; }
        }

        public int UnitCountPerWave
        {
            get { return _unitCountPerWave; }
        }
    }
}

