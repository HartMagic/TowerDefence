using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DisallowMultipleComponent]
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField]
        private Button _tryAgainButton;
        [SerializeField]
        private TextMeshProUGUI _countLabel;

        public event Action RestartButtonClicked; 

        public virtual void Initialize(int unitCount)
        {
            if (_countLabel != null)
            {
                _countLabel.text = unitCount.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void OnEnable()
        {
            if (_tryAgainButton != null)
            {
                _tryAgainButton.onClick.AddListener(OnTryAgainButtonClick);
            }
        }

        private void OnDisable()
        {
            if (_tryAgainButton != null)
            {
                _tryAgainButton.onClick.RemoveListener(OnTryAgainButtonClick);
            }
        }

        private void OnTryAgainButtonClick()
        {
            RestartButtonClicked?.Invoke();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
