using Assets.Scripts.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Player
{
    internal class UIController : MonoBehaviour
    {
        [SerializeField]
        private Image _healthIndicator;
        [SerializeField]
        private TextMeshProUGUI _healthNumber;
        [Inject]
        PlayerSignalBus _playerSignalBus;
        [Inject]
        PlayerParameters _playerParameters;
        int _maxHealth;

        [Inject]
        private void Inject()
        {
            _maxHealth = _playerParameters.MaxHealth;
            _playerSignalBus.HealthUpdate += UpdateHealth;
        }

        public void UpdateHealth(int newHealth)
        {
            if (newHealth < 0) 
                newHealth = 0;
            if (newHealth > _maxHealth)
                newHealth = _maxHealth;
            _healthNumber.text = newHealth.ToString();
            _healthIndicator.fillAmount = (float)newHealth / (float)_maxHealth;
        }

        public void OnDestroy()
        {
            _playerSignalBus.HealthUpdate -= UpdateHealth;
        }
    }
}
