using System;
using Manic.Services;
using UnityEngine;

namespace TowerDefense.Manager
{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Service
        {
            get
            {
                if (_ == null)
                    _ = Game.Services.Get<GameDataManager>();

                return _;
            }
        }

        private static GameDataManager _;

        public int PlayerCoins => _playerCoins;
        public bool IsPlayerDead => _isPlayerDead;
        
        public static Action OnPlayerDiedEvent { get; set; }
        public static Action<int> OnPlayerHealthUpdatedEvent { get; set; }
        public static Action<int> OnPlayerCoinsUpdatedEvent { get; set; }
        
        [SerializeField]
        private int _playerhealth;
        [SerializeField]
        private int _startingCount = 10;
        [SerializeField]
        private int _playerCoins;
        private bool _isPlayerDead = false;

        private void Start()
        {
            _isPlayerDead = false;
            OnGainCoins(_startingCount);
        }

        public void OnGainCoins(int reward)
        {
            _playerCoins += reward;
            OnPlayerCoinsUpdatedEvent?.Invoke(_playerCoins);
        }
        
        public void OnPurchase(int cost)
        {
            if (_playerCoins == 0) return;
            
            _playerCoins -= cost;
            OnPlayerCoinsUpdatedEvent?.Invoke(_playerCoins);
        }

        public void DamagePlayer()
        {
            _playerhealth -= 1;
            OnPlayerHealthUpdatedEvent?.Invoke(_playerhealth);
            
            if (_playerhealth <= 0)
            {
                _isPlayerDead = true;
                OnPlayerDiedEvent?.Invoke();
                Debug.Log($"#{GetType().Name}# Player is Dead!");
                return;
            }
        }
    }
}