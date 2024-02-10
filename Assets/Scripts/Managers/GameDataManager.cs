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

        [SerializeField]
        private int _playerhealth;

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