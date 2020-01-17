using UnityEngine;

namespace Settings
{
    // Can be extended for another unit types (e.g. some units need a firing speed and damage that deal to towers)
    [CreateAssetMenu(menuName = "Settings/UnitSettings", fileName = "UnitSettings")]
    public class UnitSettings : ScriptableObject
    {
        [SerializeField]
        private float _health;
        [SerializeField]
        private float _damage;
        [SerializeField]
        private int _gold;
        [SerializeField]
        private float _speed;

        public float Health
        {
            get { return _health; }
        }

        public float Damage
        {
            get { return _damage; }
        }

        public float Gold
        {
            get { return _gold; }
        }

        public float Speed
        {
            get { return _speed; }
        }
    }
}
