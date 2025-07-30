using System.Collections;
using System.Collections.Generic;
using ManicStarterPack.Utilties;
using TowerDefense.Collection;
using TowerDefense.WaveSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace TowerDefense.UI
{
    public class EnemyWaveCounterManager : MonoBehaviour
    {
        private RunnerDataCollection _runnerDataCollection => RunnerDataCollection.Service;
        
        [SerializeField] private EnemyCounterUI _enemyCounterUIPrefab;

        private Wave _currentWave;

        public void SetCurrentEnemyWave(Wave wave)
        {
            transform.DeleteChildren();
            _currentWave = wave;
            var allEnemyWaveCount = _currentWave.GetAllEnemyWaveCount();
            foreach (WaveRunner runner in allEnemyWaveCount)
            {
                var runnerData = _runnerDataCollection.GetRunnerDataByID(runner.RunnerID);
                
                var enemyCounterUI = Instantiate(_enemyCounterUIPrefab, transform);
                enemyCounterUI.SetEnemyCounterUI(runnerData.RunnerVisual, runner.EnemyCount, runner.RunnerID);
            }
        }
    }
}