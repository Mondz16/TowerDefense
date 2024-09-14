using System;
using System.Collections.Generic;
using TowerDefense.Collection;
using TowerDefense.Manager;
using Unity.Collections;
using UnityEngine;

namespace TowerDefense.PathFinding
{
    public class Runner : MonoBehaviour
    {
        private PoolManager _poolManager => PoolManager.Service;
        private GameDataManager _gameDataManager => GameDataManager.Service;
        
        public RunnerData RunnerData => _runnerData;
        
        [SerializeField][ReadOnly]private RunnerData _runnerData;
        
        [SerializeField]
        private List<Node> _path = new List<Node>();

        public Action<GameObject> OnRunnerDisappear { get; set; }

        public void SetRunnerData(RunnerData runnerData)
        {
            _runnerData = new RunnerData(runnerData);
        }
        
        private void LateUpdate()
        {
            if (GameDataManager.Service.IsPlayerDead) return;
            
            if (_path.Count > 0)
            {
                MoveAlongPath();
            }
            else
            {
                Debug.Log($"#{GetType().Name}# Runner -> Reached Target!");
                _gameDataManager.DamagePlayer();
                OnRunnerDisappear?.Invoke(gameObject);
                _poolManager.ReturnObject(gameObject);
            }
        }
        
        private void MoveAlongPath()
        {
            var step = _runnerData.Speed * Time.deltaTime;
            
            transform.position = Vector2.MoveTowards(transform.position, _path[0].transform.position, step);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            if(Vector2.Distance(transform.position, _path[0].transform.position) < 0.00001f)
                _path.RemoveAt(0);
        }

        public void AddPath(List<Node> path)
        {
            _path = new List<Node>(path);
        }

        public void TakeDamage(int damage)
        {
            _runnerData.Health -= damage;
            
            if (_runnerData.Health - 1 <= 0)
            {
                _gameDataManager.OnGainCoins(_runnerData.CoinReward);
                OnRunnerDisappear?.Invoke(gameObject);
                _poolManager.ReturnObject(gameObject);
            }
        }
    }
}