using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Settings/PlayerStartSettings", fileName = "PlayerStartSettings")]
    public sealed class PlayerSettings : ScriptableObject
    {
        [SerializeField]
        private int _gold;
        [SerializeField]
        private float _health;

        public int Gold
        {
            get { return _gold; }
        }

        public float Health
        {
            get { return _health; }
        }
    }
}