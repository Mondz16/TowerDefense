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
            _runnerData = runnerData;
        }
        
        private void LateUpdate()
        {
            if (_path.Count > 0)
            {
                MoveAlongPath();
            }
            else
            {
                Debug.Log($"#{GetType().Name}# Runner -> Reached Target!");
                OnRunnerDisappear?.Invoke(gameObject);
                _poolManager.ReturnObject(gameObject);
                _gameDataManager.DamagePlayer();
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
    }
}