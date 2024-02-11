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

        public static Action<int> OnPlayerHealthUpdatedEvent { get; set; }
        public static Action<int> OnPlayerCoinsUpdatedEvent { get; set; }
        
        [SerializeField]
        private int _playerhealth;
        private int _playerCoins;

        private void Start()
        {
            OnGainCoins(10);
        }

        public void OnGainCoins(int reward)
        {
            _playerCoins += reward;
            OnPlayerCoinsUpdatedEvent?.Invoke(_playerCoins);
        }
        
        public void OnPurchaseDefender(int cost)
        {
            if (_playerCoins == 0) return;
            
            _playerCoins -= cost;
            OnPlayerCoinsUpdatedEvent?.Invoke(_playerCoins);
        }

        public void DamagePlayer()
        {
            _playerhealth -= 1;
            OnPlayerHealthUpdatedEvent?.Invoke(_playerhealth);
            
            if (_playerhealth - 1 <= 0)
            {
                Debug.Log($"#{GetType().Name}# Player is Dead!");
                return;
            }
        }
    }
}