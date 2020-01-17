using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Settings/TowerSettings", fileName = "TowerSettings")]
    public class TowerSettings : ScriptableObject
    {
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _firingRate;
        [SerializeField]
        private float _detectingDistance;
        [SerializeField]
        private int _cost;

        public float Damage
        {
            get { return _damage; }
        }

        public float FiringRate
        {
            get { return _firingRate; }
        }

        public float DetectingDistance
        {
            get { return _detectingDistance; }
        }

        public int Cost
        {
            get { return _cost; }
        }
    }
}

