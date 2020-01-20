﻿using System;
using System.Globalization;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DisallowMultipleComponent]
    public sealed class UIManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _goldLabel;
        [SerializeField]
        private Slider _healthBar;
        [SerializeField]
        private TextMeshProUGUI _healthLabel;

        [SerializeField]
        private UpgradeTowerPanel _defaultUpgradeTowerPanel;

        public event Action<ICanUpgrade, IUpgrader> Upgraded;

        private void OnEnable()
        {
            if (_defaultUpgradeTowerPanel != null)
            {
                _defaultUpgradeTowerPanel.UpgradeClicked += DefaultUpgradeTowerPanelOnUpgradeClicked;
            }
        }

        private void OnDisable()
        {
            if (_defaultUpgradeTowerPanel != null)
            {
                _defaultUpgradeTowerPanel.UpgradeClicked -= DefaultUpgradeTowerPanelOnUpgradeClicked;
            }
        }

        private void DefaultUpgradeTowerPanelOnUpgradeClicked(ICanUpgrade upgrade, IUpgrader upgrader)
        {
            Upgraded?.Invoke(upgrade, upgrader);
        }

        public void UpdateHealth(float currentHealth, float maxHealth)
        {
            if (_healthBar != null)
            {
                var currentValue = (1.0f / maxHealth) * currentHealth;
                _healthBar.value = Mathf.Clamp01(currentValue);
            }

            if (_healthLabel != null)
            {
                _healthLabel.text = Mathf.Clamp(currentHealth, 0.0f, maxHealth).ToString(CultureInfo.InvariantCulture);
            }
        }

        public void UpdateGold(int currentGold)
        {
            if (_goldLabel != null)
            {
                _goldLabel.text = $"Gold: {currentGold.ToString()}";
            }
        }

        public void ShowTowerUpgradePanel(TowerBase tower, IUpgrader towerUpgrader)
        {
            _defaultUpgradeTowerPanel.Initialize(tower, towerUpgrader);
            _defaultUpgradeTowerPanel.Show();
        }

        public void HideTowerUpgradePanel()
        {
            if (_defaultUpgradeTowerPanel != null)
            {
                _defaultUpgradeTowerPanel.Hide();
            }
        }

        public void ShowEndGamePanel()
        {
            
        }

        public void HideEndGamePanel()
        {
            
        }
    }
}
