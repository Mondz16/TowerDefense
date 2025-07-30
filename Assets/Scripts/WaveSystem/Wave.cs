using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Collection;
using TowerDefense.PathFinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefense.WaveSystem
{
    [CreateAssetMenu(menuName = "New Wave", fileName = "Wave")]
    public class Wave : ScriptableObject
    {
        public float TimeInterval => _timeInterval;
        public int CoinWaveReward => _coinWaveReward;
        
        [SerializeField] private GameDifficulty _difficulty;
        [SerializeField] private float _timeInterval;
        [SerializeField] private int _coinWaveReward;
        [SerializeField] private List<WaveAttribute> _waveAttributeList;

        private RunnerID[] _runnerIds = new []{RunnerID.Hamburger, RunnerID.Fries, RunnerID.PizzaPiece, RunnerID.Soda, RunnerID.Donut, RunnerID.Pizza};        

        public List<WaveAttribute> GetWaveAttributeList()
        {
            return _waveAttributeList;
        }

        public List<WaveRunner> GetAllEnemyWaveCount()
        {
            List<WaveRunner> waveRunner = new List<WaveRunner>();
            foreach (WaveAttribute attribute in _waveAttributeList)
            {
                foreach (RunnerID runnerId in _runnerIds)
                {
                    var runnerList = attribute.WaveRunners.FindAll(x => x.RunnerID == runnerId);
                    int count = 0;
                    foreach (WaveRunner runnerAttribute in runnerList)
                        count += runnerAttribute.EnemyCount;

                    if(count != 0)
                        waveRunner.Add(new WaveRunner(runnerId, count));
                }
            }

            return waveRunner;
        }

        public WaveAttribute GetWaveAttributeListByRandom()
        {
            return _waveAttributeList[Random.Range(0, _waveAttributeList.Count)];
        }
    }

    [Serializable]
    public class WaveAttribute
    {
        public float MinSpawnTime;
        public float MaxSpawnTime;
        public List<WaveRunner> WaveRunners = new List<WaveRunner>();
    }
    
    [Serializable]
    public class WaveRunner
    {
        public RunnerID RunnerID;
        public int EnemyCount;
        public WaveRunner(){}

        public WaveRunner(RunnerID id, int enemyCount)
        {
            RunnerID = id;
            EnemyCount = enemyCount;
        }
    }
}