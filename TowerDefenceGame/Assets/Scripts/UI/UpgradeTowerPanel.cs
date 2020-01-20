using System;
using System.Globalization;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeTowerPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _upgradeButton;
        [SerializeField]
        private TextMeshProUGUI _levelLabel;
        [SerializeField]
        private TextMeshProUGUI _costLabel;

        protected IUpgrader _upgrader;
        protected TowerBase _tower;

        public event Action<ICanUpgrade, IUpgrader> UpgradeClicked;

        public virtual void Initialize(TowerBase tower, IUpgrader towerUpgrader)
        {
            _upgrader = towerUpgrader;
            _tower = tower;

            UpdateViews();
        }

        protected virtual void UpdateViews()
        {
            if (_costLabel != null)
            {
                _costLabel.text = _tower.Cost.ToString(CultureInfo.InvariantCulture);
            }

            if (_levelLabel != null)
            {
                _levelLabel.text = _tower.Level.ToString(CultureInfo.InvariantCulture);
            }
        }
        
        protected virtual void OnEnable()
        {
            if (_upgradeButton != null)
            {
                _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
            }
        }

        private void OnDisable()
        {
            if (_upgradeButton != null)
            {
                _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClick);
            }
        }

        protected virtual void OnUpgradeButtonClick()
        {
            UpgradeClicked?.Invoke(_tower, _upgrader);
            
            UpdateViews();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}