using System;
using System.Globalization;
using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DefaultUpgradeTowerPanel : UpgradeTowerPanel
    {
        [SerializeField]
        private TextMeshProUGUI _fromDamageLabel;
        [SerializeField]
        private TextMeshProUGUI _toDamageLabel;

        [SerializeField]
        private TextMeshProUGUI _fromFiringRateLabel;
        [SerializeField]
        private TextMeshProUGUI _toFiringRateLabel;

        [SerializeField]
        private TextMeshProUGUI _fromDistanceLabel;
        [SerializeField]
        private TextMeshProUGUI _toDistanceLabel;

        protected override void UpdateViews()
        {
            base.UpdateViews();
            
            var defaultTower = _tower as DefaultTower;
            var upgradeData = _upgrader.GetUpgradeData(_tower) as DefaultTowerUpgradeData;
            
            if (defaultTower != null && upgradeData != null)
            {
                if (_fromDamageLabel != null)
                {
                    _fromDamageLabel.text = defaultTower.Damage.ToString(CultureInfo.InvariantCulture);
                }

                if (_toDamageLabel != null)
                {
                    _toDamageLabel.text = upgradeData.Damage.ToString(CultureInfo.InvariantCulture);
                }

                if (_fromFiringRateLabel != null)
                {
                    _fromFiringRateLabel.text = defaultTower.FiringRate.ToString(CultureInfo.InvariantCulture);
                }

                if (_toFiringRateLabel != null)
                {
                    _toFiringRateLabel.text = upgradeData.FiringRate.ToString(CultureInfo.InvariantCulture);
                }
                
                if (_fromDistanceLabel != null)
                {
                    _fromDistanceLabel.text = defaultTower.DetectingDistance.ToString(CultureInfo.InvariantCulture);
                }

                if (_toDistanceLabel != null)
                {
                    _toDistanceLabel.text = upgradeData.DetectingDistance.ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }
}

