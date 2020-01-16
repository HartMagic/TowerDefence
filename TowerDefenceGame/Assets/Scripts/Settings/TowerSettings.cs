using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Settings/TowerSettings", fileName = "TowerSettings")]
    public class TowerSettings : ScriptableObject
    {
        [SerializeField]
        protected float _damage;
        [SerializeField]
        protected float _firingRate;
        [SerializeField]
        protected float _detectingDistance;

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
    }
}

