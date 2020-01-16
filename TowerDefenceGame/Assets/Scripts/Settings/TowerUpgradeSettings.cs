using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Settings/TowerUpgradeSettings", fileName = "TowerUpgradeSettings")]
    public sealed class TowerUpgradeSettings : ScriptableObject
    {
        public List<TowerUpgradeField> UpgradeFields
        {
            get { return _upgratedFields; }
        }
        
        [SerializeField]
        private List<TowerUpgradeField> _upgratedFields = new List<TowerUpgradeField>();
    }

    [Serializable]
    public sealed class TowerUpgradeField
    {
        [SerializeField]
        private TowerUpgradeFieldType _type;
        [SerializeField]
        private float _value;

        public float Value
        {
            get { return _value; }
        }
    }

    public enum TowerUpgradeFieldType
    {
        Damage,
        FiringRate,
        DetectingDistance
    }
}

