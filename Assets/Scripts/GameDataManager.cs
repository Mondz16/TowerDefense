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
        public int PlayerHealth => _playerhealth;
        
        [SerializeField]
        private int _playerhealth;

        public void DamagePlayer()
        {
            if (_playerhealth > 1)
            {
                _playerhealth--;
                Debug.Log($"#{GetType().Name}# DamagePlayer -> damage received");
            }
            else
            {
                Debug.Log($"#{GetType().Name}# DamagePlayer -> player is dead");
            }
        }
    }
}